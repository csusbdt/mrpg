using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerCommunication
{
    public class AcquireItemRequestMessage : Message
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

        AcquireItemRequestMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            AcquireItemRequestMessage message = new AcquireItemRequestMessage();
            message.entityId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
