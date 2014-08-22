using System;
using System.Collections.Generic;
using System.Text;
using Server;
using ServerCommunication;

namespace ServerPreLoginState
{
    public class PreLoginState //: ClientState
    {
        protected Client client;
        protected CommunicationChannel communicationChannel;

        //public ClientState(Client client)
        //{
        //    this.client = client;
        //    communicationChannel = client.CommunicationChannel;
        //}

        //public abstract void Update(TimeSpan dt);
        //public abstract void Activate(Dictionary<string, object> parameters);

        //public PreLoginState(Client client)
        //    : base(client)
        //{
        //}
    }
}
