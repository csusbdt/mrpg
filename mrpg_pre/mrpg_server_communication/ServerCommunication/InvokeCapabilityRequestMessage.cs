using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server.Communication
{
    public class InvokeCapabilityRequestMessage : Message
    {
        #region Fields

        string capability;
        string targetId;

        #endregion

        #region Properties

        public string Capability
        {
            get { return capability; }
        }

        public string TargetId
        {
            get { return targetId; }
        }

        #endregion

        #region Initialization

        InvokeCapabilityRequestMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            InvokeCapabilityRequestMessage message = new InvokeCapabilityRequestMessage();
            message.capability = binaryReader.ReadString();
            message.targetId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
