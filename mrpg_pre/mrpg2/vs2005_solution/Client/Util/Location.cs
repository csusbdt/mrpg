using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class Location
    {
        #region Fields

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

        public Location(Vec3f position, Vec3f orientation)
        {
        }

        public static Location Read(BinaryReader binaryReader)
        {
            Log.Write();
            Location location = new Location();
            location.position = Vec3f.Read(binaryReader);
            location.orientation = Vec3f.Read(binaryReader);
            return location;
        }

        #endregion
    }
}
