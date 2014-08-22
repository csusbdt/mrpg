using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class CreateItemListMessage : Message
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

        CreateItemListMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            CreateItemListMessage message = new CreateItemListMessage();
            message.entityId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
