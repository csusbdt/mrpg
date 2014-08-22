using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class LoginSuccessMessage : Message
    {
        private LoginSuccessMessage()
        {
        }

        public override bool IsGamePlayMessage
        {
            get { return false; }
        }

        public static LoginSuccessMessage Read(BinaryReader binaryReader)
        {
            return new LoginSuccessMessage();
        }
    }
}
