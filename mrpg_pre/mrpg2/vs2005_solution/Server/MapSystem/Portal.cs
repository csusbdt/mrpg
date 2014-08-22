using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Portal : WorldEntity
    {
        #region Fields

        Map toMap;
        Vec3f toPosition;
        Vec3f toOrientation;

        #endregion

        #region Properties

        public Map ToMap
        {
            get { return toMap; }
        }

        public Vec3f ToPosition
        {
            get { return toPosition; }
        }

        public Vec3f ToOrientation
        {
            get { return toOrientation; }
        }

        #endregion

        #region Initialization

        public Portal(string entityId, string entityClass, Map map, Vec3f position, Vec3f orientation, Map toMap, Vec3f toPosition, Vec3f toOrientation)
            : base(entityId, entityClass, map, position, orientation)
        {
            this.toMap = toMap;
            this.toPosition = toPosition;
            this.toOrientation = toOrientation;
        }

        #endregion
    }
}
