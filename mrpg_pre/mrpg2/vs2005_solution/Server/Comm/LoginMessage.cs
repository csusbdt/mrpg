using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    class LoginMessage : Message
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

        #endregion

        #region Read

        public static Message Read(BinaryReader binaryReader)
        {
            Log.Write();
            LoginMessage loginMessage = new LoginMessage();
            loginMessage.username = binaryReader.ReadString();
            loginMessage.password = binaryReader.ReadString();
            return loginMessage;
        }

        #endregion
    }
}
