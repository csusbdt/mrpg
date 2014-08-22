using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerCommunication
{
    public class InteractRequestMessage : Message
    {
        #region Fields

        string entityId;

        #endregion

        #region Properties

        public string EntityId
        {
            get { return entityId; }
        }

        #endregion

        #region Initialization

        InteractRequestMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            InteractRequestMessage message = new InteractRequestMessage();
            message.entityId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
