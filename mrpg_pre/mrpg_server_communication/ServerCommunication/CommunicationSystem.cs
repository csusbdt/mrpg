using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Server.Communication
{
    public class CommunicationSystem
    {
        #region Public Fields

        public delegate void CreateCommunicationChannelEventDelegate(CommunicationChannel channel);
        public static event CreateCommunicationChannelEventDelegate CreateCommunicationChannelEvent;

        #endregion

        #region Non-Public Fields

        static TcpListener tcpListener;
        static int maximumNumberOfCommunicationChannels;
        static int numberOfPlayers = 0;
        static List<CommunicationChannel> clientList = new List<CommunicationChannel>();
        static List<CommunicationChannel> deadClients = new List<CommunicationChannel>();
        internal delegate Message ReadMessageDelegate(BinaryReader binaryReader);
        internal static Dictionary<string, ReadMessageDelegate> readMessageDelegateDictionary =
            new Dictionary<string, ReadMessageDelegate>();

        #endregion

        #region Properties

        internal static Dictionary<string, ReadMessageDelegate> ReadMessageDelegateDictionary
        {
            get { return readMessageDelegateDictionary; }
        }

        #endregion

        #region Initialization

        // Init must be called at start up.
        public static void Init(
            int listenPort, 
            int maximumNumberOfCommunicationChannels,
            CreateCommunicationChannelEventDelegate createCommunicationChannelEventDelegate)
        {
            Debug.WriteLine("Server.Communication.CommunicationSystem.Init()");
            BuildReadMessageDelegateDictionary();
            CommunicationSystem.maximumNumberOfCommunicationChannels = maximumNumberOfCommunicationChannels;
            CreateCommunicationChannelEvent += createCommunicationChannelEventDelegate;
            StartTcpListening(listenPort);
        }

        static void BuildReadMessageDelegateDictionary()
        {
            readMessageDelegateDictionary.Add("login", LoginMessage.Read);
            readMessageDelegateDictionary.Add("logout", LogoutMessage.Read);
            readMessageDelegateDictionary.Add("avatar_select", AvatarSelectMessage.Read);
            readMessageDelegateDictionary.Add("move_pc", MovePcMessage.Read);
            readMessageDelegateDictionary.Add("invoke_capability_request", InvokeCapabilityRequestMessage.Read);
            readMessageDelegateDictionary.Add("revoke_capability_request", RevokeCapabilityRequestMessage.Read);
            readMessageDelegateDictionary.Add("exit", ExitGamePlayMessage.Read);
        }

        static void StartTcpListening(int port)
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            tcpListener = new TcpListener(localEndPoint);
            tcpListener.Start();
        }

        /// <summary>
        /// Call this when terminating.
        /// </summary>
        public static void Shutdown()
        {
            foreach (CommunicationChannel client in clientList)
            {
                client.Shutdown();
            }
        }

        #endregion

        #region Update

        // This method should be called regulary to process incoming connection requests.
        public static void Update()
        {
            ProcessIncomingConnectionRequests();
            UpdateCommunicationChannels();
            ProcessDeadClients();
        }

        static void ProcessIncomingConnectionRequests()
        {
            while (tcpListener.Pending() && numberOfPlayers < maximumNumberOfCommunicationChannels)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                CommunicationChannel client = new CommunicationChannel(tcpClient);
                clientList.Add(client);
                ++numberOfPlayers;
                CreateCommunicationChannelEvent(client);
            }
        }

        static void UpdateCommunicationChannels()
        {
            foreach (CommunicationChannel communicationChannel in clientList)
            {
                communicationChannel.Update();
            }
        }

        static void ProcessDeadClients()
        {
            foreach (CommunicationChannel client in clientList)
            {
                if (client.Dead)
                {
                    deadClients.Add(client);
                }
            }
            foreach (CommunicationChannel deadClient in deadClients)
            {
                clientList.Remove(deadClient);
            }
            deadClients.Clear();
        }

        #endregion
    }
}
