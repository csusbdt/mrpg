using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class SetPcMessage : Message
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

        SetPcMessage()
        {
        }

        internal static SetPcMessage Read(BinaryReader binaryReader)
        {
            SetPcMessage message = new SetPcMessage();
            message.entityId = binaryReader.ReadString();
            return message;
        }

        #endregion
    }
}
