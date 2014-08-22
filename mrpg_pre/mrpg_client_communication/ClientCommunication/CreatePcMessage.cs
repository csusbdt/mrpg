using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client.Communication
{
    public class CreatePcMessage : Message
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
        List<string> capabilities = new List<string>();
        List<string> inventory = new List<string>();

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

        public List<string> Capabilities
        {
            get { return capabilities; }
        }

        public List<string> Inventory
        {
            get { return inventory; }
        }

        #endregion

        #region Initialization

        CreatePcMessage()
        {
        }

        internal static CreatePcMessage Read(BinaryReader binaryReader)
        {
            CreatePcMessage message = new CreatePcMessage();
            message.entityId = binaryReader.ReadString();
            message.entityClass = binaryReader.ReadString();
            message.x = binaryReader.ReadSingle();
            message.y = binaryReader.ReadSingle();
            message.z = binaryReader.ReadSingle();
            message.rx = binaryReader.ReadSingle();
            message.ry = binaryReader.ReadSingle();
            message.rz = binaryReader.ReadSingle();
            int numberOfCapabilities = binaryReader.ReadInt32();
            for (int i = 0; i < numberOfCapabilities; ++i)
            {
                string capability = binaryReader.ReadString();
                message.capabilities.Add(capability);
            }
            int numberOfItems = binaryReader.ReadInt32();
            for (int i = 0; i < numberOfItems; ++i)
            {
                string item = binaryReader.ReadString();
                message.inventory.Add(item);
            }
            return message;
        }

        #endregion
    }
}
