using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class LoginFailureMessage : Message
    {
        private LoginFailureMessage()
        {
        }

        public override bool IsGamePlayMessage
        {
            get { return false; }
        }

        public static LoginFailureMessage Read(BinaryReader binaryReader)
        {
            return new LoginFailureMessage();
        }
    }
}
