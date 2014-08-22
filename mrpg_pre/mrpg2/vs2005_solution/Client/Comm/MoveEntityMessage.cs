using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class MoveEntityMessage : Message
    {
        string entityId;
        Vec3f position;
        Vec3f orientation;

        public String EntityId
        {
            get { return entityId; }
        }

        public Vec3f Position
        {
            get { return position; }
        }

        public Vec3f Orientation
        {
            get { return orientation; }
        }

        MoveEntityMessage()
        {
        }

        public static Message Read(BinaryReader binaryReader)
        {
            Log.Write();
            MoveEntityMessage moveEntityMessage = new MoveEntityMessage();
            moveEntityMessage.entityId = binaryReader.ReadString();
            moveEntityMessage.position = Vec3f.Read(binaryReader);
            moveEntityMessage.orientation = Vec3f.Read(binaryReader);
            return moveEntityMessage;
        }
    }
}
