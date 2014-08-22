using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class CreateEntityMessage : Message
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

        CreateEntityMessage()
        {
        }

        internal static CreateEntityMessage Read(BinaryReader binaryReader)
        {
            CreateEntityMessage message = new CreateEntityMessage();
            message.entityId = binaryReader.ReadString();
            message.entityClass = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
