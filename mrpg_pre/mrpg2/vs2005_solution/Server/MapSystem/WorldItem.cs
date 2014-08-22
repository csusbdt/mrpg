using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class WorldItem : WorldEntity
    {
        #region Initialization

        public WorldItem(string entityId, string entityClass, Map map, Vec3f position, Vec3f orientation)
            : base(entityId, entityClass, map, position, orientation)
        {
        }

        #endregion
    }
}
