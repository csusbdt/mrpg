using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace Client.Communication
{
    public class CommunicationSystem
    {
        #region Fields

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

        #region Life Cycle

        // Init must be called at start up.
        public static void Init()
        {
            readMessageDelegateDictionary.Add("login_success", LoginSuccessMessage.Read);
            readMessageDelegateDictionary.Add("login_failure", LoginFailureMessage.Read);
            readMessageDelegateDictionary.Add("avatar_list", AvatarListMessage.Read);
            readMessageDelegateDictionary.Add("set_map", SetMapMessage.Read);
            readMessageDelegateDictionary.Add("create_entity", CreateEntityMessage.Read);
            readMessageDelegateDictionary.Add("create_pc", CreatePcMessage.Read);
            readMessageDelegateDictionary.Add("create_npc", CreateNpcMessage.Read);
            readMessageDelegateDictionary.Add("move_entity", MoveEntityMessage.Read);
            readMessageDelegateDictionary.Add("invoke_capability", InvokeCapabilityMessage.Read);
            readMessageDelegateDictionary.Add("revoke_capability", RevokeCapabilityMessage.Read);
            readMessageDelegateDictionary.Add("create_container", CreateContainerMessage.Read);
            readMessageDelegateDictionary.Add("create_modeled_entity", CreateModeledEntityMessage.Read);
            readMessageDelegateDictionary.Add("set_mana", SetManaMessage.Read);
            readMessageDelegateDictionary.Add("set_health", SetHealthMessage.Read);
            readMessageDelegateDictionary.Add("die", DieMessage.Read);
            readMessageDelegateDictionary.Add("delete", DeleteEntityMessage.Read);
        }

        public static void Connect(IPAddress serverAddress, int serverPort)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(serverAddress, serverPort);
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
            binaryWriter.Write("login");
            binaryWriter.Write(username);
            binaryWriter.Write(password);
        }

        // Pre-Login and Avatar Select States

        public static void SendLogoutMessage()
        {
            binaryWriter.Write("logout");
        }

        // Avatar Select State

        public static void SendAvatarSelectMessage(string avatarName)
        {
            binaryWriter.Write("avatar_select");
            binaryWriter.Write(avatarName);
        }

        // Game Play State

        public static void SendMovePcMessage(
            float x,
            float y,
            float z,
            float rx,
            float ry,
            float rz)
        {
            binaryWriter.Write("move_pc");
            binaryWriter.Write(x);
            binaryWriter.Write(y);
            binaryWriter.Write(z);
            binaryWriter.Write(rx);
            binaryWriter.Write(ry);
            binaryWriter.Write(rz);
        }

        public static void SendInvokeCapabilityRequestMessage(string capability, string targetId)
        {
            binaryWriter.Write("invoke_capability_request");
            binaryWriter.Write(capability);
            binaryWriter.Write(targetId);
        }

        public static void SendRevokeCapabilityRequestMessage(string capability, string targetId)
        {
            binaryWriter.Write("revoke_capability_request");
            binaryWriter.Write(capability);
            binaryWriter.Write(targetId);
        }

        public static void SendExitGamePlayMessage()
        {
            binaryWriter.Write("exit");
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
            ReadMessageDelegate readMessageDelegate = readMessageDelegateDictionary[messageType];
            if (readMessageDelegate == null)
            {
                throw new Exception("Invalid message from client.");
            }
            return readMessageDelegate(binaryReader);
        }

        #endregion
    }
}
