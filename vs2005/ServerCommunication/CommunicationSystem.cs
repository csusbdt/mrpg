using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using System.Net;
using CommunicationMessageTypes;

namespace ServerCommunication
{
    public class CommunicationSystem
    {
        #region Fields

        static BooleanSwitch debugSwitch = new BooleanSwitch("ServerCommunication.Debug", "ServerCommunication debug switch");
        static BooleanSwitch traceSwitch = new BooleanSwitch("ServerCommunication.Trace", "ServerCommunication trace switch");
        public delegate void CreateCommunicationChannelEventDelegate(CommunicationChannel channel);
        public static event CreateCommunicationChannelEventDelegate CreateCommunicationChannelEvent;
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

        internal static BooleanSwitch DebugSwitch
        {
            get { return debugSwitch; }
        }

        internal static BooleanSwitch TraceSwitch
        {
            get { return traceSwitch; }
        }

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
            BuildReadMessageDelegateDictionary();
            CommunicationSystem.maximumNumberOfCommunicationChannels = maximumNumberOfCommunicationChannels;
            CreateCommunicationChannelEvent += createCommunicationChannelEventDelegate;
            StartTcpListening(listenPort);
            Debug.WriteLineIf(debugSwitch.Enabled, "Server accepting connections.", "ServerCommunication: ");
        }

        static void BuildReadMessageDelegateDictionary()
        {
            readMessageDelegateDictionary.Add(MessageTypes.LOGIN, LoginMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.LOGOUT, LogoutMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.SELECT_AVATAR, SelectAvatarMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.MOVE_REQUEST, MoveRequestMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.ACTION_REQUEST, ActionRequestMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.STOP_ACTION_REQUEST, StopActionRequestMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.EXIT_GAME, ExitGameMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.INTERACT_REQUEST, InteractRequestMessage.Read);
            readMessageDelegateDictionary.Add(MessageTypes.ACQUIRE_ITEM_REQUEST, AcquireItemRequestMessage.Read);
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
            if (!tcpListener.Pending())
            {
                return;
            }
            if (numberOfPlayers >= maximumNumberOfCommunicationChannels)
            {
                Trace.WriteLineIf(
                    traceSwitch.Enabled, 
                    "Connection request ignored because maximum number of players reached.", 
                    "ServerCommunication: ");
                return;
            }
            TcpClient tcpClient = tcpListener.AcceptTcpClient();
            Trace.WriteLineIf(
                traceSwitch.Enabled,
                "Connection request accepted.",
                "ServerCommunication: ");
            CommunicationChannel client = new CommunicationChannel(tcpClient);
            clientList.Add(client);
            ++numberOfPlayers;
            CreateCommunicationChannelEvent(client);
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
