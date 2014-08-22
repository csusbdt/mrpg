using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClientCommunication
{
    public class CreateModeledEntityMessage : Message
    {
        #region Fields

        string entityId;
        string entityClass;
        float x;
        float y;
        float z;
        float rx;
        float ry;
        float rz;

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

        CreateModeledEntityMessage()
        {
        }

        internal static CreateModeledEntityMessage Read(BinaryReader binaryReader)
        {
            CreateModeledEntityMessage message = new CreateModeledEntityMessage();
            message.entityId = binaryReader.ReadString();
            message.entityClass = binaryReader.ReadString();
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
