using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using ServerCommunication;
using System.Threading;
using ServerDatabase;

namespace Server
{
    class Program
    {
        #region Fields

        static BooleanSwitch debugSwitch = new BooleanSwitch("Server.DebugSwitch", "Server debug switch");
        static bool shuttingDown = false;

        #endregion

        #region Properties

        public static BooleanSwitch DebugSwitch
        {
            get { return debugSwitch; }
        }

        #endregion

        #region Events

        public delegate void UpdateEventDelegate(TimeSpan dt);
        public static UpdateEventDelegate UpdateEvent;

        #endregion

        #region Initialization

        static void Init()
        {
            Debug.WriteLineIf(debugSwitch.Enabled, "Init()", "Server.Program: ");

            // Initialize database system.
            string databaseServer = System.Configuration.ConfigurationManager.AppSettings["DatabaseServer"];
            string databaseName = System.Configuration.ConfigurationManager.AppSettings["DatabaseName"];
            string databaseUsername = System.Configuration.ConfigurationManager.AppSettings["DatabaseUsername"];
            string databasePassword = System.Configuration.ConfigurationManager.AppSettings["DatabasePassword"];
            DatabaseSystem.Init(databaseServer, databaseName, databaseUsername, databasePassword);

            // Initialize communication system.
            string serverPortString = System.Configuration.ConfigurationManager.AppSettings["ServerPort"];
            string maximumNumberOfConnectionsString = System.Configuration.ConfigurationManager.AppSettings["ServerMaximumConnections"];
            int serverPort = int.Parse(serverPortString);
            int maximumNumberOfConnections = int.Parse(maximumNumberOfConnectionsString);
            CommunicationSystem.Init(
                serverPort,
                maximumNumberOfConnections,
                CreateCommunicationChannelEventHandler);
        }

        static void Shutdown()
        {
            CommunicationSystem.Shutdown();
        }

        #endregion

        #region Communication Channel Maintenance

        public static void CreateCommunicationChannelEventHandler(CommunicationChannel communicationChannel)
        {
            new PreLoginState(communicationChannel);
        }

        #endregion

        #region Main

        static void Main(string[] args)
        {
            TextWriterTraceListener consoleOutputListener = new TextWriterTraceListener(Console.Out);
            Trace.Listeners.Add(consoleOutputListener);
            Trace.WriteLine("Server started.");
            try
            {
                Init();
                Loop();
                Shutdown();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
                Trace.WriteLine(e.StackTrace);
                Console.ReadLine();
            }
            Trace.WriteLine("Server terminated.");
            Console.ReadLine();
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
                if (dt != TimeSpan.Zero)
                {
                    if (UpdateEvent != null)
                    {
                        UpdateEvent(dt);
                    }
                    CommunicationSystem.Update();
                }
                GC.Collect();
                Thread.Sleep(0);  // This results in better performance on Giang's laptop.
            }
        }

        #endregion
    }
}
