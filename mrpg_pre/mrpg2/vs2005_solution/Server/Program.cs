using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Server.Communication;

namespace Server
{
    class Program
    {
        #region Fields

        //static TcpListener tcpListener;
        //static int numberOfPlayers = 0;
        static bool shuttingDown = false;
        //static List<Client> clientList = new List<Client>();
        //static List<Client> deadClients = new List<Client>();
        static Dictionary<string, Map> mapDictionary = new Dictionary<string, Map>();

        #endregion

        #region Properties

        #endregion

        #region Events

        public delegate void UpdateEventDelegate(TimeSpan dt);
        public static UpdateEventDelegate UpdateEvent;

        #endregion

        #region Main, Init, Shutdown

        static void Main(string[] args)
        {
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Log.Write();
            try
            {
                Init();
                Loop();
                Shutdown();
            }
            catch (Exception e)
            {
                Log.Write(e.Message + "\n" + e.StackTrace);
                Console.ReadLine();
            }
        }

        static void Init()
        {
            Configuration.Init();
            DbConnectionFactory.Init();
            MapSystem.Init();
            mapDictionary = MapSystem.FindAll();
            //StartTcpListening();
            CommunicationSystem.Init(10008, 8, 
        }

        static void Shutdown()
        {
            foreach (Client client in clientList)
            {
                client.Shutdown();
            }
        }

        #endregion

        #region Loop

        static void Loop()
        {
            DateTime previousTime = DateTime.Now;
            while (!shuttingDown)
            {
                DateTime currentTime = DateTime.Now;
                TimeSpan dt = currentTime - previousTime;
                previousTime = currentTime;
                ProcessIncomingConnectionRequests();
                if (dt != TimeSpan.Zero)
                {
                    if (UpdateEvent != null)
                    {
                        UpdateEvent(dt);
                    }
                    //UpdateClients(dt);
                    CommunicationSystem.Update();
                }
                GC.Collect();
                Thread.Sleep(0);  // This results in better performance on Giang's laptop.
            }
        }

        static void UpdateClients(TimeSpan dt)
        {
            foreach (Client client in clientList)
            {
                try
                {
                    client.Update(dt);
                }
                catch (SocketException e)
                {
                    Log.Write(e.Message);
                    client.Dead = true;
                }
                if (client.Dead)
                {
                    deadClients.Add(client);
                }
            }
            foreach (Client deadClient in deadClients)
            {
                clientList.Remove(deadClient);
            }
            deadClients.Clear();
        }

        #endregion

        #region TCP Connection Requests

        static void StartTcpListening()
        {
            IPHostEntry ipHostEntry = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostEntry.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Configuration.Port);
            tcpListener = new TcpListener(localEndPoint);
            tcpListener.Start();
        }

        static void ProcessIncomingConnectionRequests()
        {
            while (tcpListener.Pending() && numberOfPlayers < Configuration.MaximumNumberOfPlayers)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                Log.Write("TCP connection accepted.");
                Client client = new Client(tcpClient);
                clientList.Add(client);
                ++numberOfPlayers;
                Log.Write("New player connection accepted.");
            }
        }

        #endregion
    }
}
