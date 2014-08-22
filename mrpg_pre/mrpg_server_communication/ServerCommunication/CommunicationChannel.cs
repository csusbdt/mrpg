using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Server.Communication
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
            binaryWriter.Write("login_success");
        }

        public void SendLoginFailureMessage()
        {
            binaryWriter.Write("login_failure");
        }

        // Avatar Selection State

        public void SendAvatarListMessage(List<Avatar> avatarList)
        {
            binaryWriter.Write("avatar_list");
            binaryWriter.Write(avatarList.Count);
            foreach (Avatar avatar in avatarList)
            {
                binaryWriter.Write(avatar.AvatarId);
                binaryWriter.Write(avatar.AvatarClass);
                binaryWriter.Write(avatar.Level);
            }
        }

        // Game Play State

        public void SendSetMapMessage(string mapId)
        {
            binaryWriter.Write("set_map");
            binaryWriter.Write(mapId);
        }

        public void SendCreateEntityMessage(string entityId, string entityClass)
        {
            binaryWriter.Write("create_entity");
            binaryWriter.Write(entityId);
            binaryWriter.Write(entityClass);
        }

        public void SendCreatePcMessage(
            string entityId,
            string entityClass,
            float x,
            float y,
            float z,
            float rx,
            float ry,
            float rz,
            List<string> capabilities,
            List<string> inventory)
        {
            binaryWriter.Write("create_pc");
            binaryWriter.Write(entityId);
            binaryWriter.Write(entityClass);
            binaryWriter.Write(x);
            binaryWriter.Write(y);
            binaryWriter.Write(z);
            binaryWriter.Write(rx);
            binaryWriter.Write(ry);
            binaryWriter.Write(rz);
            binaryWriter.Write(capabilities.Count);
            foreach (string capability in capabilities)
            {
                binaryWriter.Write(capability);
            }
            binaryWriter.Write(inventory.Count);
            foreach (string item in inventory)
            {
                binaryWriter.Write(item);
            }
        }

        public void SendCreateNpcMessage(
            string entityId,
            string entityClass,
            float x,
            float y,
            float z,
            float rx,
            float ry,
            float rz)
        {
            binaryWriter.Write("create_npc");
            binaryWriter.Write(entityId);
            binaryWriter.Write(entityClass);
            binaryWriter.Write(x);
            binaryWriter.Write(y);
            binaryWriter.Write(z);
            binaryWriter.Write(rx);
            binaryWriter.Write(ry);
            binaryWriter.Write(rz);
        }

        public void SendCreateContainerMessage(
            string entityId,
            string entityClass,
            List<string> items)
        {
            binaryWriter.Write("create_container");
            binaryWriter.Write(entityId);
            binaryWriter.Write(entityClass);
            binaryWriter.Write(items.Count);
            foreach (string item in items)
            {
                binaryWriter.Write(item);
            }
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
            binaryWriter.Write("create_modeled_entity");
            binaryWriter.Write(entityId);
            binaryWriter.Write(entityClass);
            binaryWriter.Write(x);
            binaryWriter.Write(y);
            binaryWriter.Write(z);
            binaryWriter.Write(rx);
            binaryWriter.Write(ry);
            binaryWriter.Write(rz);
        }

        public void SendDeleteEntityMessage(string entityId)
        {
            binaryWriter.Write("delete_entity");
            binaryWriter.Write(entityId);
            binaryWriter.Flush();
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
            binaryWriter.Write("move_entity");
            binaryWriter.Write(entityId);
            binaryWriter.Write(x);
            binaryWriter.Write(y);
            binaryWriter.Write(z);
            binaryWriter.Write(rx);
            binaryWriter.Write(ry);
            binaryWriter.Write(rz);
        }

        public void SendInvokeCapabilityMessage(string entityId, string capability, string targetId)
        {
            binaryWriter.Write("invoke_capability");
            binaryWriter.Write(entityId);
            binaryWriter.Write(capability);
            binaryWriter.Write(targetId);
        }

        public void SendRevokeCapabilityMessage(string entityId, string capability, string targetId)
        {
            binaryWriter.Write("revoke_capability");
            binaryWriter.Write(entityId);
            binaryWriter.Write(capability);
            binaryWriter.Write(targetId);
        }

        public void SendSetManaMessage(string entityId, float mana)
        {
            binaryWriter.Write("set_mana");
            binaryWriter.Write(entityId);
            binaryWriter.Write(mana);
        }

        public void SendSetHealthMessage(string entityId, float health)
        {
            binaryWriter.Write("set_health");
            binaryWriter.Write(entityId);
            binaryWriter.Write(health);
        }

        public void SendDieMessage(string entityId)
        {
            binaryWriter.Write("die");
            binaryWriter.Write(entityId);
        }

        public void SendDeleteMessage(string entityId)
        {
            binaryWriter.Write("delete");
            binaryWriter.Write(entityId);
        }

        #endregion
    }
}
