using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server.Communication
{
    public class RevokeCapabilityRequestMessage : Message
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

        RevokeCapabilityRequestMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            RevokeCapabilityRequestMessage message = new RevokeCapabilityRequestMessage();
            message.capability = binaryReader.ReadString();
            message.targetId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
