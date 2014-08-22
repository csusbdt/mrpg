using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class PreLoginState : ClientState
    {
//        Client client;

        public PreLoginState(Client client) : base(client)
        {
        }

        //public PreLoginState(Client client)
        //{
        //    this.client = client;
        //    Log.Write();
        //}

        public override void Activate()
        {
            Log.Write();
        }

        public override void Update(TimeSpan dt)
        {
            Message message = client.GetNextMessage();
            if (message == null)
            {
                return;
            }
            LoginMessage loginMessage = message as LoginMessage;
            if (loginMessage == null)
            {
                throw new Exception("Invalid client command.");
            }
            Log.Write(this);
            string username = loginMessage.Username;
            LoginCredentials loginCredentials = LoginCredentialsDao.FindByUsername(username);
            if (loginCredentials == null ||
                !loginCredentials.Password.Equals(loginMessage.Password))
            {
                Log.Write(this, "Login credentials rejected.");
                client.SendFailureMessage();
                return;
            }
            else
            {
                Log.Write(this, "Login credentials accepted.");
                client.SendSuccessMessage();
                client.Username = username;
                AvatarSelectionState avatarSelectionState = new AvatarSelectionState(client);
                client.ClientState = avatarSelectionState;
                avatarSelectionState.Activate();
                return;
            }
        }
    }
}
