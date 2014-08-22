using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class CreateContainerMessage : Message
    {
        #region Fields

        string entityId;
        string entityClass;
        List<string> items = new List<string>();

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

        public List<string> Items
        {
            get { return items; }
        }

        #endregion

        #region Initialization

        CreateContainerMessage()
        {
        }

        internal static CreateContainerMessage Read(BinaryReader binaryReader)
        {
            CreateContainerMessage message = new CreateContainerMessage();
            message.entityId = binaryReader.ReadString();
            message.entityClass = binaryReader.ReadString();
            int numberOfItems = binaryReader.ReadInt32();
            for (int i = 0; i < numberOfItems; ++i)
            {
                string item = binaryReader.ReadString();
                message.items.Add(item);
            }
            return message;
        }

        #endregion
    }
}
