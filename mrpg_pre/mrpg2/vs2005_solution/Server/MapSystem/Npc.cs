using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Npc : Entity
    {
        #region Fields

        Map map;
        Vec3f position;
        Vec3f orientation;

        #endregion

        #region Properties

        public Vec3f Position
        {
            get { return position; }
        }

        public Vec3f Orientation
        {
            get { return orientation; }
        }

        #endregion

        #region Initialization

        public Npc(string entityId, string entityClass, Map map, Vec3f position, Vec3f orientation)
            : base(entityId, entityClass)
        {
            this.map = map;
            this.position = position;
            this.orientation = orientation;
        }

        #endregion
    }
}
