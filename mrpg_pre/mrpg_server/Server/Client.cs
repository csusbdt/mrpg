using System;
using System.Collections.Generic;
using System.Text;
using Server.Communication;

namespace Server
{
    class Client
    {
        CommunicationChannel communicationChannel;

        public Client(CommunicationChannel communicationChannel)
        {
            this.communicationChannel = communicationChannel;
        }
    }
}
