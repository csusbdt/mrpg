using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class AddItemToListMessage : Message
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

        AddItemToListMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            AddItemToListMessage message = new AddItemToListMessage();
            message.entityId = binaryReader.ReadString();
            message.entityClass = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
