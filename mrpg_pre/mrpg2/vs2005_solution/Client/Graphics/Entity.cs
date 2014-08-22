using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using System.IO;

namespace Client
{
    class Entity
    {
        #region

        protected string entityId;
        protected string entityClass;

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
    }
}
