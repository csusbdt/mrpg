using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class SetManaMessage : Message
    {
        #region Fields

        string entityId;
        float mana;

        #endregion

        #region Properties

        public string EntityId
        {
            get { return entityId; }
        }

        public float Mana
        {
            get { return mana; }
        }

        #endregion

        #region Initialization

        SetManaMessage()
        {
        }

        internal static SetManaMessage Read(BinaryReader binaryReader)
        {
            SetManaMessage message = new SetManaMessage();
            message.entityId = binaryReader.ReadString();
            message.mana = binaryReader.ReadSingle();
            return message;
        }

        #endregion
    }
}
