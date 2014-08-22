using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;

namespace Server
{
    class Client
    {
        #region Fields

        bool dead = false;
        string username;
        TcpClient tcpClient;
        Thread queueUpIncomingMessagesThread;
        BinaryWriter binaryWriter;
        BinaryReader binaryReader;
        List<Message> incomingMessageQueue = new List<Message>();
        ClientState clientState = null;
        Avatar avatar;

        #endregion

        #region Properties

        public ClientState ClientState
        {
            get { return clientState; }
            set { clientState = value; }
        }

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

        public Avatar Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }

        //public Map Map
        //{
        //    get { return map; }
        //    set { map = value; }
        //}

        #endregion

        #region Life Cycle

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            NetworkStream networkStream = tcpClient.GetStream();
            BufferedStream bufferedStream = new BufferedStream(networkStream);
            binaryWriter = new BinaryWriter(bufferedStream);
            binaryReader = new BinaryReader(bufferedStream);
            ThreadStart threadStart = new ThreadStart(QueueUpIncomingMessages);
            queueUpIncomingMessagesThread = new Thread(threadStart);
            queueUpIncomingMessagesThread.Start();
            clientState = new PreLoginState(this);
        }

        public void Shutdown()
        {
            dead = true;
            tcpClient.Close();
        }

        public void Update(TimeSpan dt)
        {
            clientState.Update(dt);
            //ClientState newState = clientState.Update(dt);
            //if (newState != null)
            //{
            //    newState.Activate();
            //    clientState = newState;
            //}
        }

        #endregion

        #region Incoming Messages

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

        Message ReadMessage()
        {
            string messageType = binaryReader.ReadString();
            Log.Write(this, "Message received of type " + messageType);
            switch (messageType)
            {
                case "login": return LoginMessage.Read(binaryReader);
                case "logout": return LogoutMessage.Read(binaryReader);
                case "avatar_select": return AvatarSelectMessage.Read(binaryReader);
                case "exit": return ExitGamePlayMessage.Read(binaryReader);
                default: throw new Exception("Invalid message from client.");
            }
        }

        public Message GetNextMessage()
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

        #endregion

        #region Outgoing Messages (Pre-Login State)

        public void SendSuccessMessage()
        {
            Log.Write(this);
            binaryWriter.Write("login_success");
            binaryWriter.Flush();
        }

        public void SendFailureMessage()
        {
            Log.Write(this);
            binaryWriter.Write("login_failure");
            binaryWriter.Flush();
        }

        #endregion

        #region Outgoing Messages (Avatar Selection State)

        public void SendAvatarListMessage(List<Avatar> avatarList)
        {
            Log.Write(this);
            binaryWriter.Write("avatar_list");
            binaryWriter.Write(avatarList.Count);
            foreach (Avatar avatar in avatarList)
            {
                binaryWriter.Write(avatar.AvatarId);
                binaryWriter.Write(avatar.AvatarClass);
            }
            binaryWriter.Flush();
        }

        #endregion

        #region Outgoing Messages (Game Play State)

        public void SendSetMapMessage(string mapId)
        {
            Log.Write(this);
            binaryWriter.Write("set_map");
            binaryWriter.Write(mapId);
            binaryWriter.Flush();
        }

        public void SendCreatePcMessage(Pc pc)
        {
            Log.Write(this);
            binaryWriter.Write("create_pc");
            binaryWriter.Write(pc.EntityId);
            binaryWriter.Write(pc.EntityClass);
            pc.Position.Write(binaryWriter);
            pc.Orientation.Write(binaryWriter);
            binaryWriter.Flush();
        }

        public void SendCreateNpcMessage(Npc npc)
        {
            Log.Write(this);
            binaryWriter.Write("create_npc");
            binaryWriter.Write(npc.EntityId);
            binaryWriter.Write(npc.EntityClass);
            npc.Position.Write(binaryWriter);
            npc.Orientation.Write(binaryWriter);
            binaryWriter.Flush();
        }

        public void SendDeleteEntityMessage(string entityId)
        {
            Log.Write(this);
            binaryWriter.Write("delete_entity");
            binaryWriter.Write(entityId);
            binaryWriter.Flush();
        }

        public void SendMoveEntityMessage(Entity entity, Vec3f position, Vec3f orientation)
        {
            Log.Write(this);
            binaryWriter.Write("move_entity");
            binaryWriter.Write(entity.EntityId);
            position.Write(binaryWriter);
            orientation.Write(binaryWriter);
            binaryWriter.Flush();
        }

        #endregion
    }
}
