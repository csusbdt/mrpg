using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    interface ClientState
    {
        void Activate();
        void Update(TimeSpan dt);
    }
}
