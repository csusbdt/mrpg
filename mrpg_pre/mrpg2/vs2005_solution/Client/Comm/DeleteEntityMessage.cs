using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class DeleteEntityMessage : Message
    {
        Entity entity;

        public Entity Entity
        {
            get { return entity; }
        }

        DeleteEntityMessage()
        {
        }

        public static DeleteEntityMessage Read(BinaryReader binaryReader)
        {
            Log.Write();
            DeleteEntityMessage deleteEntityMessage = new DeleteEntityMessage();
            string entityId = binaryReader.ReadString();

//            deleteEntityMessage.entity = Entity.Read(binaryReader);
            return deleteEntityMessage;
        }
    }
}
