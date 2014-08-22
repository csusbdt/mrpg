using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Client
{
    class Camera
    {
        private static Vec3f position = new Vec3f(0, 0, -1);
        private static Vec3f lookat = new Vec3f(0, 0, 0);
        private static Vec3f up = new Vec3f(0, 1, 0);

        public static Vec3f Position
        {
            get { return position; }
            set { position = value; }
        }

        public static Vec3f Lookat
        {
            get { return lookat; }
            set { lookat = value; }
        }

        public static Vec3f Up
        {
            get { return up; }
            set { up = value; }
        }

        public static void Apply()
        {
            Glu.gluLookAt(position.x, position.y, position.z, lookat.x, lookat.y, lookat.z, up.x, up.y, up.z);
        }
    }
}
