using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    class ExitGamePlayMessage : Message
    {
        private ExitGamePlayMessage()
        {
        }

        public static Message Read(BinaryReader binaryReader)
        {
            Log.Write();
            return new ExitGamePlayMessage();
        }
    }
}
