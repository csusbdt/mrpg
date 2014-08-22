using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using CommunicationMessageTypes;
using System.Diagnostics;

namespace ServerCommunication
{
    public class CommunicationChannel
    {
        #region Fields

        bool dead = false;
        string username;
        TcpClient tcpClient;
        Thread queueUpIncomingMessagesThread;
        BinaryWriter binaryWriter;
        BinaryReader binaryReader;
        List<Message> incomingMessageQueue = new List<Message>();

        #endregion

        #region Properties

        public bool Dead
        {
            get { return dead; }
            set { dead = value; if (value == true) tcpClient.Close(); }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        #endregion

        #region Initialization

        internal CommunicationChannel(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            NetworkStream networkStream = tcpClient.GetStream();
            BufferedStream bufferedStream = new BufferedStream(networkStream, 1024);
            binaryWriter = new BinaryWriter(bufferedStream);
            binaryReader = new BinaryReader(bufferedStream);
            ThreadStart threadStart = new ThreadStart(QueueUpIncomingMessages);
            queueUpIncomingMessagesThread = new Thread(threadStart);
            queueUpIncomingMessagesThread.Start();
        }

        public void Shutdown()
        {
            dead = true;
            tcpClient.Close();
        }

        #endregion

        #region Update

        public void Update()
        {
            if (binaryWriter != null)
            {
                binaryWriter.Flush();
            }
        }

        #endregion

        #region Incoming Messages

        // Call this method from the main thread to pop a message from the
        // incoming message queue.  This method returns null when the queue is empty.
        public Message GetNextReceivedMessage()
        {
            if (incomingMessageQueue.Count == 0)
            {
                return null;
            }
            Message message = null;
            lock (incomingMessageQueue)
            {
                message = incomingMessageQueue[0];
                incomingMessageQueue.RemoveAt(0);
            }
            return message;
        }

        void QueueUpIncomingMessages()
        {
            while (true)
            {
                Message message = null;
                try
                {
                    message = ReadMessage();
                }
                catch (SocketException)
                {
                    dead = true;
                    tcpClient.Close();
                    return;
                }
                catch (IOException)
                {
                    dead = true;
                    tcpClient.Close();
                    return;
                }
                lock (incomingMessageQueue)
                {
                    incomingMessageQueue.Add(message);
                }
            }
        }

        internal Message ReadMessage()
        {
            string messageType = binaryReader.ReadString();
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Message received of type " + messageType,
                "ServerCommunication: ");
            CommunicationSystem.ReadMessageDelegate readMessageDelegate =
                CommunicationSystem.readMessageDelegateDictionary[messageType];
            if (readMessageDelegate == null)
            {
                throw new Exception("Invalid message from client.");
            }
            return readMessageDelegate(binaryReader);
        }

        #endregion

        #region Outgoing Messages 

        // Pre-Login State

        public void SendLoginSuccessMessage()
        {
            binaryWriter.Write(MessageTypes.LOGIN_SUCCESS);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Login success message written to output buffer:" + 
                MessageTypes.LOGIN_SUCCESS,
                "ServerCommunication: ");
        }

        public void SendLoginFailureMessage()
        {
            binaryWriter.Write(MessageTypes.LOGIN_FAILURE);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Login failure message written to output buffer:" +
                MessageTypes.LOGIN_FAILURE,
                "ServerCommunication: ");
        }

        // Avatar Selection State

        public void SendAvatarListMessage(List<Avatar> avatarList)
        {
            binaryWriter.Write(MessageTypes.AVATAR_LIST);
            binaryWriter.Write(avatarList.Count);
            foreach (Avatar avatar in avatarList)
            {
                binaryWriter.Write(avatar.AvatarId);
                binaryWriter.Write(avatar.AvatarClass);
                binaryWriter.Write(avatar.Level);
            }
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Avatar list message written to output buffer:" +
                MessageTypes.AVATAR_LIST,
                "ServerCommunication: ");
        }

        // Game Play State

        public void SendSetMapMessage(string mapId)
        {
            binaryWriter.Write(MessageTypes.SET_MAP);
            binaryWriter.Write(mapId);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Set map message written to output buffer:" +
                MessageTypes.SET_MAP,
                "ServerCommunication: ");
        }

        // Use to tell client which entity is his pc (avatar).
        public void SendSetPcMessage(string pcId)
        {
            binaryWriter.Write(MessageTypes.SET_PC);
            binaryWriter.Write(pcId);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Set pc message written to output buffer:" +
                MessageTypes.SET_PC,
                "ServerCommunication: ");
        }

        public void SendAddCapabilityMessage(string capability)
        {
            binaryWriter.Write(MessageTypes.ADD_CAPABILITY);
            binaryWriter.Write(capability);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Add capability message written to output buffer:" +
                MessageTypes.ADD_CAPABILITY,
                "ServerCommunication: ");
        }

        public void SendRemoveCapabilityMessage(string capability)
        {
            binaryWriter.Write(MessageTypes.REMOVE_CAPABILITY);
            binaryWriter.Write(capability);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Remove capability written to output buffer:" +
                MessageTypes.REMOVE_CAPABILITY,
                "ServerCommunication: ");
        }

        public void SendAddItemToInventoryMessage(string entityId, string entityClass)
        {
            binaryWriter.Write(MessageTypes.ADD_ITEM_TO_INVENTORY);
            binaryWriter.Write(entityId);
            binaryWriter.Write(entityClass);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Add item to inventory written to output buffer:" +
                MessageTypes.ADD_ITEM_TO_INVENTORY,
                "ServerCommunication: ");
        }

        public void SendRemoveItemFromInventoryMessage(string entityId)
        {
            binaryWriter.Write(MessageTypes.REMOVE_ITEM_FROM_INVENTORY);
            binaryWriter.Write(entityId);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Remove item from inventory messsge written to output buffer:" +
                MessageTypes.REMOVE_ITEM_FROM_INVENTORY,
                "ServerCommunication: ");
        }

        public void SendCreateModeledEntityMessage(
            string entityId,
            string entityClass,
            float x,
            float y,
            float z,
            float rx,
            float ry,
            float rz)
        {
            binaryWriter.Write(MessageTypes.CREATE_MODELED_ENTITY);
            binaryWriter.Write(entityId);
            binaryWriter.Write(entityClass);
            binaryWriter.Write(x);
            binaryWriter.Write(y);
            binaryWriter.Write(z);
            binaryWriter.Write(rx);
            binaryWriter.Write(ry);
            binaryWriter.Write(rz);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Create modeled entity written to output buffer:" +
                MessageTypes.CREATE_MODELED_ENTITY,
                "ServerCommunication: ");
        }

        public void SendDeleteEntityMessage(string entityId)
        {
            binaryWriter.Write(MessageTypes.DELETE_ENTITY);
            binaryWriter.Write(entityId);
            binaryWriter.Flush();
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Delete entity written to output buffer:" +
                MessageTypes.DELETE_ENTITY,
                "ServerCommunication: ");
        }

        public void SendMoveEntityMessage(
            string entityId,
            float x,
            float y,
            float z,
            float rx,
            float ry,
            float rz)
        {
            binaryWriter.Write(MessageTypes.MOVE_ENTITY);
            binaryWriter.Write(entityId);
            binaryWriter.Write(x);
            binaryWriter.Write(y);
            binaryWriter.Write(z);
            binaryWriter.Write(rx);
            binaryWriter.Write(ry);
            binaryWriter.Write(rz);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Move entity message written to output buffer:" +
                MessageTypes.MOVE_ENTITY,
                "ServerCommunication: ");
        }

        public void SendCreateItemListMessage(string entityId)
        {
            binaryWriter.Write(MessageTypes.CREATE_ITEM_LIST);
            binaryWriter.Write(entityId);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Create item message written to output buffer:" +
                MessageTypes.CREATE_ITEM_LIST,
                "ServerCommunication: ");
        }

        public void SendAddItemToListMessage(string entityId, string entityClass)
        {
            binaryWriter.Write(MessageTypes.ADD_ITEM_TO_LIST);
            binaryWriter.Write(entityId);
            binaryWriter.Write(entityClass);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Add item to list message written to output buffer:" +
                MessageTypes.ADD_ITEM_TO_LIST,
                "ServerCommunication: ");
        }

        public void SendRemoveItemFromMessage(string entityId)
        {
            binaryWriter.Write(MessageTypes.REMOVE_ITEM_FROM_LIST);
            binaryWriter.Write(entityId);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Remove item from list message written to output buffer:" +
                MessageTypes.REMOVE_ITEM_FROM_LIST,
                "ServerCommunication: ");
        }

        // Used to invoke both instantaneous and continuous capabilities.
        public void SendCreateActionMessage(
            string actionId, 
            string sourceId, 
            string targetId,
            string capability) 
        {
            binaryWriter.Write(MessageTypes.CREATE_ACTION);
            binaryWriter.Write(actionId);
            binaryWriter.Write(sourceId);
            binaryWriter.Write(targetId);
            binaryWriter.Write(capability);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Create action written to output buffer:" +
                MessageTypes.CREATE_ACTION,
                "ServerCommunication: ");
        }

        public void SendStopActionMessage(string actionId)
        {
            binaryWriter.Write(MessageTypes.STOP_ACTION);
            binaryWriter.Write(actionId);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Stop action message written to output buffer:" +
                MessageTypes.STOP_ACTION,
                "ServerCommunication: ");
        }

        public void SendSetManaMessage(string entityId, float mana)
        {
            binaryWriter.Write(MessageTypes.SET_MANA);
            binaryWriter.Write(entityId);
            binaryWriter.Write(mana);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Set mana written to output buffer:" +
                MessageTypes.SET_MANA,
                "ServerCommunication: ");
        }

        public void SendSetHealthMessage(string entityId, float health)
        {
            binaryWriter.Write(MessageTypes.SET_HEALTH);
            binaryWriter.Write(entityId);
            binaryWriter.Write(health);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Set health written to output buffer:" +
                MessageTypes.SET_HEALTH,
                "ServerCommunication: ");
        }

        public void SendDieMessage(string entityId)
        {
            binaryWriter.Write(MessageTypes.DIE);
            binaryWriter.Write(entityId);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Die message written to output buffer:" +
                MessageTypes.DIE,
                "ServerCommunication: ");
        }

        public void SendDeleteMessage(string entityId)
        {
            binaryWriter.Write(MessageTypes.DELETE_ENTITY);
            binaryWriter.Write(entityId);
            Debug.WriteLineIf(
                CommunicationSystem.DebugSwitch.Enabled,
                "Delete message written to output buffer:" +
                MessageTypes.DELETE_ENTITY,
                "ServerCommunication: ");
        }

        #endregion
    }
}
