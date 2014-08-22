using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class InvokeCapabilityMessage : Message
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

        InvokeCapabilityMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            InvokeCapabilityMessage message = new InvokeCapabilityMessage();
            message.entityId = binaryReader.ReadString();
            message.capability = binaryReader.ReadString();
            message.targetId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
