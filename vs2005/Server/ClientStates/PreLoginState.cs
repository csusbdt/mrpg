using System;
using System.Collections.Generic;
using System.Text;
using ServerCommunication;
using System.Diagnostics;
using ServerDatabase;

namespace Server
{
    class PreLoginState
    {
        #region Fields

        CommunicationChannel communicationChannel;

        #endregion

        #region Initialization

        public PreLoginState(CommunicationChannel communicationChannel)
        {
            Debug.WriteLineIf(Program.DebugSwitch.Enabled, "PreLoginState()", "Server.PreLoginState: ");
            this.communicationChannel = communicationChannel;
            Program.UpdateEvent += Update;
        }

        #endregion

        #region Update

        public void Update(TimeSpan dt)
        {
            // The only valid message from the client is the login message.
            Message message = communicationChannel.GetNextReceivedMessage();
            if (message == null)
            {
                return;
            }
            LoginMessage loginMessage = message as LoginMessage;
            if (loginMessage == null)
            {
                throw new Exception("Invalid client command.");
            }
            string username = loginMessage.Username;
            LoginCredentials loginCredentials = LoginCredentialsDao.FindByUsername(username);
            if (loginCredentials == null ||
                !loginCredentials.Password.Equals(loginMessage.Password))
            {
//                Log.Write(this, "Login credentials rejected.");
                communicationChannel.SendLoginFailureMessage();
      //          return;
            }
            else
            {
//                Log.Write(this, "Login credentials accepted.");
                communicationChannel.SendLoginSuccessMessage();
                communicationChannel.Username = username;
    //            AvatarSelectionState avatarSelectionState = new AvatarSelectionState(client);
    //            client.ClientState = avatarSelectionState;
    //            avatarSelectionState.Activate();
        //        return;
            }
        }

        #endregion
    }
}
