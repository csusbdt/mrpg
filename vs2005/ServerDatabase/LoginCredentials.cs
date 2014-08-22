using System;
using System.Collections.Generic;
using System.Text;

namespace ServerDatabase
{
    public class LoginCredentials
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

        public LoginCredentials(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        #endregion

    }
}
