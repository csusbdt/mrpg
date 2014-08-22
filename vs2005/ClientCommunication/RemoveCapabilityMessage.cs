using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class RemoveCapabilityMessage : Message
    {
        #region Fields

        string capability;

        #endregion

        #region Properties

        public string Capability
        {
            get { return capability; }
        }

        #endregion

        #region Initialization

        RemoveCapabilityMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            RemoveCapabilityMessage message = new RemoveCapabilityMessage();
            message.capability = binaryReader.ReadString();
            return message;
        }

        #endregion    
    }
}
