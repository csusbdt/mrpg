using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class RemoveItemFromInventoryMessage : Message
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

        RemoveItemFromInventoryMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            RemoveItemFromInventoryMessage message = new RemoveItemFromInventoryMessage();
            message.entityId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
