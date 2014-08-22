using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class SetHealthMessage : Message
    {
        #region Fields

        string entityId;
        float health;

        #endregion

        #region Properties

        public string EntityId
        {
            get { return entityId; }
        }

        public float Health
        {
            get { return health; }
        }

        #endregion

        #region Initialization

        SetHealthMessage()
        {
        }

        internal static SetHealthMessage Read(BinaryReader binaryReader)
        {
            SetHealthMessage message = new SetHealthMessage();
            message.entityId = binaryReader.ReadString();
            message.health = binaryReader.ReadSingle();
            return message;
        }

        #endregion    
    }
}
