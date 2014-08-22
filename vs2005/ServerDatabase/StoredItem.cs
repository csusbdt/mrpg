using System;
using System.Collections.Generic;
using System.Text;

namespace ServerDatabase
{
    public class StoredItem
    {
        #region Fields

        string entityId;
        string entityClass;

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

        #endregion

        #region Initialization

        public StoredItem(string entityId, string entityClass)
        {
            this.entityId = entityId;
            this.entityClass = entityClass;
        }

        #endregion
    }
}
