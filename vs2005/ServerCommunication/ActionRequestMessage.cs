using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerCommunication
{
    public class ActionRequestMessage : Message
    {
        #region Fields

        string targetId;
        string capability;

        #endregion

        #region Properties

        public string TargetId
        {
            get { return targetId; }
        }

        public string Capability
        {
            get { return capability; }
        }

        #endregion

        #region Initialization

        ActionRequestMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            ActionRequestMessage message = new ActionRequestMessage();
            message.targetId = binaryReader.ReadString();
            message.capability = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
