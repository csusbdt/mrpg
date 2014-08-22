using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class DieMessage : Message
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

        DieMessage()
        {
        }

        internal static Message Read(BinaryReader binaryReader)
        {
            DieMessage message = new DieMessage();
            message.entityId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
