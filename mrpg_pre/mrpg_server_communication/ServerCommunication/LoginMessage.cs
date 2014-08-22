using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server.Communication
{
    public class LoginMessage : Message
    {
        #region Fields

        private string username;
        private string password;

        #endregion

        #region Properties

        public string Username
        {
            get { return username; }
        }

        public string Password
        {
            get { return password; }
        }

        #endregion

        #region Initialization

        private LoginMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            LoginMessage loginMessage = new LoginMessage();
            loginMessage.username = binaryReader.ReadString();
            loginMessage.password = binaryReader.ReadString();
            return loginMessage;
        }

        #endregion
    }
}
