using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerDatabase
{
    public class StoredAvatar
    {
        #region Fields

        string avatarId;
        string avatarClass;
        float healthPoints;
        List<StoredItem> inventory;

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

        public List<StoredItem> Inventory
        {
            get { return inventory; }
            set { inventory = value; }
        }

        #endregion

        #region Initialization

        public StoredAvatar(
            string avatarId, 
            string avatarClass,
            float healthPoints,
            List<StoredItem> inventory)
        {
            this.avatarId = avatarId;
            this.avatarClass = avatarClass;
            this.healthPoints = healthPoints;
            this.inventory = inventory;
        }

        #endregion
    }
}
