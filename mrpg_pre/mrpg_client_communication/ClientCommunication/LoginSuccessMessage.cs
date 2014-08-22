using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class LoginSuccessMessage : Message
    {
        #region Initialization

        LoginSuccessMessage()
        {
        }

        internal static LoginSuccessMessage Read(BinaryReader binaryReader)
        {
            LoginSuccessMessage message = new LoginSuccessMessage();
            return message;
        }

        #endregion
    }
}
