using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class AddItemToInventoryMessage : Message
    {
        #region Fields

        string entityId;
        string entityClass;

        #endregion

        #region Properties

        public string EntityId
        {
            get { return entityId; }
        }

        public string EntityClass
        {
            get { return entityClass; }
        }

        #endregion

        #region Initialization

        AddItemToInventoryMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            AddItemToInventoryMessage message = new AddItemToInventoryMessage();
            message.entityId = binaryReader.ReadString();
            message.entityClass = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
