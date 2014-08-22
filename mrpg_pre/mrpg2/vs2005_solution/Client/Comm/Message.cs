using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    abstract class Message
    {
        public void Write(BinaryWriter binaryWriter)
        {
            throw new LogicError();
        }

        public virtual bool IsGamePlayMessage
        {
            get { return true; }
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
