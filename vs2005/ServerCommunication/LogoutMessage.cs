using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerCommunication
{
    public class LogoutMessage : Message
    {
        #region Initialization

        private LogoutMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            return new LogoutMessage();
        }

        #endregion
    }
}
