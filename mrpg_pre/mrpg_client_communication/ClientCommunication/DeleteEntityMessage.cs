using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class DeleteEntityMessage : Message
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

        DeleteEntityMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            DeleteEntityMessage message = new DeleteEntityMessage();
            message.entityId = binaryReader.ReadString();
            return message;
        }

        #endregion    
    }
}
