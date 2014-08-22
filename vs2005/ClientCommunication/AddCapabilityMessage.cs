using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class AddCapabilityMessage : Message
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

        AddCapabilityMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            AddCapabilityMessage message = new AddCapabilityMessage();
            message.capability = binaryReader.ReadString();
            return message;
        }

        #endregion    
    }
}
