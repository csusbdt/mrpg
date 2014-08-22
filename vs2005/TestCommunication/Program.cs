using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;

namespace TestCommunication
{
    class Program
    {
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

        #region main

        static void Main(string[] args)
        {
            // Configure debug output.
            TextWriterTraceListener consoleOutputListener = new TextWriterTraceListener(Console.Out);
            Trace.Listeners.Add(consoleOutputListener);

            // Start server in separate thread.
            ThreadStart threadStart = new ThreadStart(Server.Start);
            new Thread(threadStart).Start();

            // Run client in main thread.
            Client.Start();
        }

        #endregion
    }
}
