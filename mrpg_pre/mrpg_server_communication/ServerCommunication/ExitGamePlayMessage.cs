using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server.Communication
{
    public class ExitGamePlayMessage : Message
    {
        #region Initialization

        private ExitGamePlayMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            return new ExitGamePlayMessage();
        }

        #endregion
    }
}
