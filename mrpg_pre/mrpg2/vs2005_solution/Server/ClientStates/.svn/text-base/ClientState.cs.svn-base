using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    abstract class ClientState
    {
        protected Client client;

        public ClientState(Client client)
        {
            this.client = client;
        }

        public abstract void Update(TimeSpan dt);
        public abstract void Activate();
    }
}
