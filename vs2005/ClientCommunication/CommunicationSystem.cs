using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Diagnostics;
using CommunicationMessageTypes;

namespace ClientCommunication
{
    public class CommunicationSystem
    {
        #region Fields

        static BooleanSwitch debugSwitch = new BooleanSwitch("ClientCommunication.Debug", "ClientCommunication debug switch");
        static Thread incomingMessagesThread;
        static TcpClient tcpClient;
        static BinaryWriter binaryWriter;
        static BinaryReader binaryReader;
        static List<Message> incomingMessageQueue = new List<Message>();
        static bool isConnected = false;
        internal delegate Message ReadMessageDelegate(BinaryReader binaryReader);
        static Dictionary<string, ReadMessageDelegate> readMessageDelegateDictionary =
            new Dictionary<string, ReadMessageDelegate>();

        #endregion

        #region Properties

        //internal BooleanSwitch DebugSwitch
        //{
        //    get { return debugSwitch; }
        //}

        #endregion

        #region Life Cycle

        static void Init()
        {
            readMessageDelegateDictionary.Add(MessageTypes.LOGIN_SUCCESS, LoginSuccessMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.LOGIN_FAILURE, LoginFailureMessage.Read);

            readMessageDelegateDictionary.Add(MessageTypes.AVATAR_LIST, AvatarListMessage.Read);

            readMessageDelegateDictionary.Add(MessageTypes.SET_MAP, SetMapMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.SET_MANA, SetManaMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.SET_HEALTH, SetHealthMessage.Read);

            readMessageDelegateDictionary.Add(MessageTypes.DIE, DieMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.DELETE_ENTITY, DeleteEntityMessage.Read);

            readMessageDelegateDictionary.Add(MessageTypes.CREATE_MODELED_ENTITY, CreateModeledEntityMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.SET_PC, SetPcMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.ADD_CAPABILITY, AddCapabilityMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.REMOVE_CAPABILITY, RemoveCapabilityMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.ADD_ITEM_TO_INVENTORY, AddItemToInventoryMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.REMOVE_ITEM_FROM_INVENTORY, RemoveItemFromInventoryMessage.Read);

            readMessageDelegateDictionary.Add(MessageTypes.MOVE_ENTITY, MoveEntityMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.CREATE_ACTION, CreateActionMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.STOP_ACTION, StopActionMessage.Read);

            readMessageDelegateDictionary.Add(MessageTypes.CREATE_ITEM_LIST, CreateItemListMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.ADD_ITEM_TO_LIST, AddItemToListMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.REMOVE_ITEM_FROM_LIST, RemoveItemFromListMessage.Read);

        }

        public static void Connect(IPAddress serverAddress, int serverPort)
        {
            // If Init() hasn't been called, then call it.
            if (!readMessageDelegateDictionary.ContainsKey(MessageTypes.LOGIN_SUCCESS))
            {
                Init();
            }
            tcpClient = new TcpClient();
            tcpClient.Connect(serverAddress, serverPort);
            Debug.WriteLineIf(debugSwitch.Enabled, "Client connected to server.", "ClientCommunication: ");
            NetworkStream networkStream = tcpClient.GetStream();
            BufferedStream bufferedStream = new BufferedStream(networkStream, 1024);
            binaryWriter = new BinaryWriter(bufferedStream);
            binaryReader = new BinaryReader(bufferedStream);
            ThreadStart threadStart = new ThreadStart(queueUpIncomingMessages);
            incomingMessagesThread = new Thread(threadStart);
            incomingMessagesThread.Start();
            isConnected = true;
        }

        public static void Update()
        {
            if (binaryWriter != null)
            {
                binaryWriter.Flush();
            }
        }

        public static void Disconnect()
        {
            if (!isConnected)
            {
                return;
            }
            isConnected = false;
            binaryWriter.Close();
            binaryReader.Close();
            tcpClient.Close();
            binaryWriter = null;
            binaryReader = null;
            tcpClient = null;
        }

        #endregion

        #region Outgoing Messages

        // Pre-Login States

        public static void SendLoginMessage(string username, string password)
        {
            binaryWriter.Write(MessageTypes.LOGIN);
            binaryWriter.Write(username);
            binaryWriter.Write(password);
            Debug.WriteLineIf(
                debugSwitch.Enabled, 
                "Login message written to output buffer: " + 
                MessageTypes.LOGIN, 
                "ClientCommunication: ");
        }

        // Pre-Login and Avatar Select States

        public static void SendLogoutMessage()
        {
            binaryWriter.Write(MessageTypes.LOGOUT);
            Debug.WriteLineIf(
                debugSwitch.Enabled,
                "Logout message written to output buffer: " +
                MessageTypes.LOGOUT,
                "ClientCommunication: ");
        }

        // Avatar Select State

        public static void SendSelectAvatarMessage(string avatarName)
        {
            binaryWriter.Write(MessageTypes.SELECT_AVATAR);
            binaryWriter.Write(avatarName);
            Debug.WriteLineIf(
                debugSwitch.Enabled,
                "Select avatar message written to output buffer: " +
                MessageTypes.SELECT_AVATAR,
                "ClientCommunication: ");
        }

        // Game Play State

        public static void SendMoveRequestMessage(
            float x,
            float y,
            float z,
            float rx,
            float ry,
            float rz)
        {
            binaryWriter.Write(MessageTypes.MOVE_REQUEST);
            binaryWriter.Write(x);
            binaryWriter.Write(y);
            binaryWriter.Write(z);
            binaryWriter.Write(rx);
            binaryWriter.Write(ry);
            binaryWriter.Write(rz);
            Debug.WriteLineIf(
                debugSwitch.Enabled,
                "Move request message written to output buffer: " +
                MessageTypes.MOVE_REQUEST,
                "ClientCommunication: ");
        }

        public static void SendActionRequestMessage(string targetId, string capability)
        {
            binaryWriter.Write(MessageTypes.ACTION_REQUEST);
            binaryWriter.Write(targetId);
            binaryWriter.Write(capability);
            Debug.WriteLineIf(
                debugSwitch.Enabled,
                "Action request message written to output buffer: " +
                MessageTypes.ACTION_REQUEST,
                "ClientCommunication: ");
        }

        public static void SendStopActionRequestMessage(string actionId)
        {
            binaryWriter.Write(MessageTypes.STOP_ACTION_REQUEST);
            binaryWriter.Write(actionId);
            Debug.WriteLineIf(
                debugSwitch.Enabled,
                "Stop action message written to output buffer: " +
                MessageTypes.STOP_ACTION_REQUEST,
                "ClientCommunication: ");
        }

        public static void SendInteractRequestMessage(string targetId)
        {
            binaryWriter.Write(MessageTypes.INTERACT_REQUEST);
            binaryWriter.Write(targetId);
            Debug.WriteLineIf(
                debugSwitch.Enabled,
                "Interact request message written to output buffer: " +
                MessageTypes.INTERACT_REQUEST,
                "ClientCommunication: ");
        }

        public static void SendAcquireItemRequestMessage(string itemId)
        {
            binaryWriter.Write(MessageTypes.ACQUIRE_ITEM_REQUEST);
            binaryWriter.Write(itemId);
            Debug.WriteLineIf(
                debugSwitch.Enabled,
                "Acquire item message written to output buffer: " +
                MessageTypes.ACQUIRE_ITEM_REQUEST,
                "ClientCommunication: ");
        }

        public static void SendExitGameMessage()
        {
            binaryWriter.Write(MessageTypes.EXIT_GAME);
            Debug.WriteLineIf(
                debugSwitch.Enabled,
                "Exit message written to output buffer: " +
                MessageTypes.EXIT_GAME,
                "ClientCommunication: ");
        }

        #endregion

        #region Incoming Messages

        // Pop message from incoming message queue.
        // This method is called in the game loop.
        public static Message GetNextReceivedMessage()
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

        static void queueUpIncomingMessages()
        {
            while (isConnected)
            {
                Message message = null;
                try
                {
                    message = ReadMessage();
                }
                catch (SocketException se)
                {
                    if (!isConnected)
                    {
                        // Main thread closed socket; don't log exception.
                        return;
                    }
                    throw se;
                }
                catch (IOException ioe)
                {
                    if (!isConnected)
                    {
                        // Main thread closed socket; don't log exception.
                        return;
                    }
                    throw ioe;
                }
                lock (incomingMessageQueue)
                {
                    incomingMessageQueue.Add(message);
                }
            }
        }

        static Message ReadMessage()
        {
            string messageType = binaryReader.ReadString();
            Debug.WriteLineIf(debugSwitch.Enabled, "Message received of type " + messageType, "ClientCommunication: ");
            ReadMessageDelegate readMessageDelegate;
            try
            {
                readMessageDelegate = readMessageDelegateDictionary[messageType];
            }
            catch (KeyNotFoundException)
            {
                throw new Exception("Received unknown message type from server: " + messageType);
            }
            return readMessageDelegate(binaryReader);
        }

        #endregion
    }
}
