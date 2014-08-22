using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class CreateNpcMessage : Message
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

        CreateNpcMessage()
        {
        }

        public static CreateNpcMessage Read(BinaryReader binaryReader)
        {
            Log.Write();
            CreateNpcMessage createNpcMessage = new CreateNpcMessage();
            createNpcMessage.entityId = binaryReader.ReadString();
            createNpcMessage.entityClass = binaryReader.ReadString();
            createNpcMessage.location = Location.Read(binaryReader);
            return createNpcMessage;
        }

        #endregion
    }
}
