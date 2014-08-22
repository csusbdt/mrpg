using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    struct Vec2f
    {
        public float x;
        public float y;

        public Vec2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void Normalize()
        {
            float len = (float)Math.Sqrt(x * x + y * y);
            x /= len;
            y /= len;
        }
    }
}
