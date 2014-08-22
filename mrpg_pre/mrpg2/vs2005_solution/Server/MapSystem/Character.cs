using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Character : WorldEntity
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Initialization

        public Character(string entityId, string entityClass, Map map, Vec3f position, Vec3f orientation)
            : base(entityId, entityClass, map, position, orientation)
        {
        }

        #endregion
    }
}
