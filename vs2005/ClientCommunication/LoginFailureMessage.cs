using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class LoginFailureMessage : Message
    {
        #region Initialization

        LoginFailureMessage()
        {
        }

        internal static LoginFailureMessage Read(BinaryReader binaryReader)
        {
            LoginFailureMessage message = new LoginFailureMessage();
            return message;
        }

        #endregion
   }
}
