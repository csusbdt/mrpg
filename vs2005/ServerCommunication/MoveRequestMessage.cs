using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerCommunication
{
    public class MoveRequestMessage : Message
    {
        #region Fields

        float x;
        float y;
        float z;
        float rx;
        float ry;
        float rz;

        #endregion

        #region Properties

        public float X
        {
            get { return x; }
        }

        public float Y
        {
            get { return y; }
        }

        public float Z
        {
            get { return z; }
        }

        public float Rx
        {
            get { return rx; }
        }

        public float Ry
        {
            get { return ry; }
        }

        public float Rz
        {
            get { return rz; }
        }

        #endregion

        #region Initialization

        MoveRequestMessage()
        {
        }

        internal static MoveRequestMessage Read(BinaryReader binaryReader)
        {
            MoveRequestMessage message = new MoveRequestMessage();
            message.x = binaryReader.ReadSingle();
            message.y = binaryReader.ReadSingle();
            message.z = binaryReader.ReadSingle();
            message.rx = binaryReader.ReadSingle();
            message.ry = binaryReader.ReadSingle();
            message.rz = binaryReader.ReadSingle();
            return message;
        }

        #endregion
    }
}
