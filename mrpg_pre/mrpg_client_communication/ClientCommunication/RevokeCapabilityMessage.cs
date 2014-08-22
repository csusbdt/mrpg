using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class RevokeCapabilityMessage : Message
    {
        #region Fields

        string entityId;
        string capability;
        string targetId;

        #endregion

        #region Properties

        public string EntityId
        {
            get { return entityId; }
        }

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

        RevokeCapabilityMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            RevokeCapabilityMessage message = new RevokeCapabilityMessage();
            message.entityId = binaryReader.ReadString();
            message.capability = binaryReader.ReadString();
            message.targetId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
