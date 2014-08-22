using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerCommunication
{
    public class ExitGameMessage : Message
    {
        #region Initialization

        private ExitGameMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            return new ExitGameMessage();
        }

        #endregion
    }
}
