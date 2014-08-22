using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    class Avatar
    {
        #region Fields

        string avatarId;
        string avatarClass;
        Vec3f position;
        Vec3f orientation;
        float healthPoints;
        List<Entity> inventory;

        #endregion

        #region Properties

        public string AvatarId
        {
            get { return avatarId; }
        }

        public string AvatarClass
        {
            get { return avatarClass; }
        }

        public float HealthPoints
        {
            get { return healthPoints; }
        }

        public Vec3f Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vec3f Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        public List<Entity> Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }

        #endregion

        #region Initialization

        public Avatar(
            string avatarId, 
            string avatarClass,
            Vec3f position, 
            Vec3f orientation,
            float healthPoints,
            List<Entity> inventory)
        {
            this.avatarId = avatarId;
            this.avatarClass = avatarClass;
            this.position = position;
            this.orientation = orientation;
            this.healthPoints = healthPoints;
            this.inventory = inventory;
        }

        #endregion

        //#region Communications

        //public void Write(BinaryWriter binaryWriter)
        //{
        //    Log.Write(this);
        //    binaryWriter.Write(avatarId);
        //    binaryWriter.Write(avatarClass);
        //    position.Write(binaryWriter);
        //    orientation.Write(binaryWriter);
        //    binaryWriter.Write(healthPoints);
        //    binaryWriter.Write(inventory.Count);
        //    foreach (Entity entity in inventory)
        //    {
        //        entity.Write(binaryWriter);
        //    }
        //}

        //#endregion
    }
}
