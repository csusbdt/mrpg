using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Item : Entity
    {
        #region Initialization

        public Item(string entityId, string entityClass)
            : base(entityId, entityClass)
        {
        }

        #endregion
    }
}
