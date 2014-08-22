using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class CreateActionMessage : Message
    {
        #region Fields

        string actionId;
        string sourceId;
        string targetId;
        string capability;

        #endregion

        #region Properties

        public string ActionId
        {
            get { return actionId; }
        }

        public string SourceId
        {
            get { return sourceId; }
        }

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

        CreateActionMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            CreateActionMessage message = new CreateActionMessage();
            message.actionId = binaryReader.ReadString();
            message.sourceId = binaryReader.ReadString();
            message.targetId = binaryReader.ReadString();
            message.capability = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
