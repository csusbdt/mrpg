using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    class LogoutMessage : Message
    {
        private LogoutMessage()
        {
        }

        public static Message Read(BinaryReader binaryReader)
        {
            Log.Write();
            return new LogoutMessage();
        }
    }
}
