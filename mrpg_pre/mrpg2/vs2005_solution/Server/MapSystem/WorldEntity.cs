using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class WorldEntity : Entity
    {
        #region Fields

        protected Map map;
        protected Vec3f position;
        protected Vec3f orientation;

        #endregion

        #region Properties

        public virtual Map Map
        {
            get { return map; }
            set
            {
                if (map != null)
                {
                    map.RemoveMapEntity(this);
                }
                map = value;
                map.AddMapEntity(this);
            }
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

        #endregion

        #region Initialization

        public WorldEntity(string entityId, string entityClass, Map map, Vec3f position, Vec3f orientation)
            : base(entityId, entityClass)
        {
            this.map = map;
            this.position = position;
            this.orientation = orientation;
        }

        #endregion
    }
}
