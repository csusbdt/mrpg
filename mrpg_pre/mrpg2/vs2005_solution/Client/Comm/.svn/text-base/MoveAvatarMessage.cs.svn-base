using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class MoveAvatarMessage : Message
    {
        Vec3f position;
        Vec3f orientation;

        public Vec3f Position
        {
            get { return position; }
        }

        public Vec3f Orientation
        {
            get { return orientation; }
        }

        MoveAvatarMessage()
        {
        }

        public static Message Read(BinaryReader binaryReader)
        {
            Log.Write();
            MoveAvatarMessage moveAvatarMessage = new MoveAvatarMessage();
            moveAvatarMessage.position = Vec3f.Read(binaryReader);
            moveAvatarMessage.orientation = Vec3f.Read(binaryReader);
            return moveAvatarMessage;
        }
    }
}
