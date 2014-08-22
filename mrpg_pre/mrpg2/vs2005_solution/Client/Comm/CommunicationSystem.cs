using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.IO;

namespace Client
{
    class CommunicationSystem
    {
        #region Fields

        static Thread incomingMessagesThread;
        static TcpClient tcpClient;
        static BinaryWriter binaryWriter;
        static BinaryReader binaryReader;
        static List<Message> incomingMessageQueue = new List<Message>();
        static bool isConnected = false;

        #endregion

        #region Life Cycle

        public static void Connect()
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(Configuration.ServerAddress, Configuration.ServerPort);
            NetworkStream networkStream = tcpClient.GetStream();
            BufferedStream bufferedStream = new BufferedStream(networkStream);
            binaryWriter = new BinaryWriter(bufferedStream);
            binaryReader = new BinaryReader(bufferedStream);
            ThreadStart threadStart = new ThreadStart(queueUpIncomingMessages);
            incomingMessagesThread = new Thread(threadStart);
            incomingMessagesThread.Start();
            isConnected = true;
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
            binaryWriter.Flush();
        }

        // Pre-Login and Avatar Select States

        public static void SendLogoutMessage()
        {
            binaryWriter.Write("logout");
            binaryWriter.Flush();
        }

        // Avatar Select State

        public static void SendAvatarSelectMessage(string avatarName)
        {
            binaryWriter.Write("avatar_select");
            binaryWriter.Write(avatarName);
            binaryWriter.Flush();
        }

        // Game Play State

        public static void SendExitGamePlayMessage()
        {
            binaryWriter.Write("exit");
            binaryWriter.Flush();
        }

        #endregion

        #region Incoming Messages

        public static Message GetNextMessage()
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
            //            Log.Write("Popping message: " + message.GetType().Name);
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
            //            Log.Write("Message received of type " + messageType);
            switch (messageType)
            {
                // pre-login state
                case "login_success": return LoginSuccessMessage.Read(binaryReader);
                case "login_failure": return LoginFailureMessage.Read(binaryReader);

                // avatar selection state
                case "avatar_list": return AvatarListMessage.Read(binaryReader);

                // game play state
                case "set_map": return SetMapMessage.Read(binaryReader);
                case "create_pc": return CreatePcMessage.Read(binaryReader);
                case "create_npc": return CreateNpcMessage.Read(binaryReader);
                //case "create_container": return CreateContainerMessage.Read(binaryReader);
                //case "create_portal": return CreatePortalMessage.Read(binaryReader);
                //case "create_inventory_item": return CreateEntityMessage.Read(binaryReader);
                //case "create_capability": return CreateCapabilityMessage.Read(binaryReader);

                case "delete_entity": return DeleteEntityMessage.Read(binaryReader);
                case "move_entity": return MoveEntityMessage.Read(binaryReader);
                //case "invoke_capability": return MoveEntityMessage.Read(binaryReader);
                default: throw new Exception("Invalid message from server.");
            }
        }

        #endregion
    }
}
