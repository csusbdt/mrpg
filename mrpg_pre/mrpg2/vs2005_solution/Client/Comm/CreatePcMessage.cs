using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class CreatePcMessage : Message
    {
        #region Fields

        string entityId;
        string entityClass;
        Location location;

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

        public Location Location
        {
            get { return location; }
        }

        #endregion

        #region Initialization

        CreatePcMessage()
        {
        }

        public static CreatePcMessage Read(BinaryReader binaryReader)
        {
            Log.Write();
            CreatePcMessage createPcMessage = new CreatePcMessage();
            createPcMessage.entityId = binaryReader.ReadString();
            createPcMessage.entityClass = binaryReader.ReadString();
            createPcMessage.location = Location.Read(binaryReader);
            return createPcMessage;
        }

        #endregion
    }
}
