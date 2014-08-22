using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class RemoveItemFromListMessage : Message
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

        RemoveItemFromListMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            RemoveItemFromListMessage message = new RemoveItemFromListMessage();
            message.entityId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
