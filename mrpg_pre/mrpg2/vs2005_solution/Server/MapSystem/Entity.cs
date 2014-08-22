using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    class Entity
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

        public Entity(string entityId, string entityClass)
        {
            this.entityId = entityId;
            this.entityClass = entityClass;
        }

        #endregion

        #region Id Generation

        static int nextId = 0;

        protected static string GenerateUniqueId()
        {
            ++nextId;
            return "" + nextId;
        }

        #endregion
    }
}
