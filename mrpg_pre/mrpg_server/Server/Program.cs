using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Server.Communication;
using System.Threading;

namespace Server
{
    class Program
    {
        #region Fields

        static BooleanSwitch mainSwitch = new BooleanSwitch("MainSwitch", "Server.Main()");
        static bool shuttingDown = false;
        static List<Client> clients = new List<Client>();

        #endregion

        #region Properties

        public static int ServerPort
        {
            get { return 10008; }
        }

        public static int MaximumNumberOfCommunicationChannels
        {
            get { return 8; }
        }

        #endregion

        #region Events

        public delegate void UpdateEventDelegate(TimeSpan dt);
        public static UpdateEventDelegate UpdateEvent;

        #endregion

        #region Initialization

        public static void CreateCommunicationChannelEventHandler(CommunicationChannel communicationChannel)
        {
            clients.Add(new Client(communicationChannel));
        }

        static void Init()
        {
            CommunicationSystem.Init(
                ServerPort,
                MaximumNumberOfCommunicationChannels,
                CreateCommunicationChannelEventHandler);
            Trace.WriteLineIf(mainSwitch.Enabled, "Init()", "Program");
        }

        #endregion

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

        static void Shutdown()
        {
            CommunicationSystem.Shutdown();
        }
    }
}
