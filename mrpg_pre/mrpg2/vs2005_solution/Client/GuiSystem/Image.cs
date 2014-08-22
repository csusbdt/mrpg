using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Client
{
    class Image : GuiComponent
    {
        private Texture texture;
        private ScreenCoordinate lowerLeftCorner;
        private ScreenCoordinate upperRightCorner;

        public Image(Texture texture, ScreenCoordinate lowerLeftCorner)
        {
            this.texture = texture;
            this.lowerLeftCorner = lowerLeftCorner;
            upperRightCorner = new ScreenCoordinate();
            upperRightCorner.x = lowerLeftCorner.x + texture.Width;
            upperRightCorner.y = lowerLeftCorner.y + texture.Height;
        }

        public bool IsInside(ScreenCoordinate point)
        {
            if (point.x >= lowerLeftCorner.x &&
                point.x <= upperRightCorner.x &&
                point.y >= lowerLeftCorner.y &&
                point.y <= upperRightCorner.y)
            {
                return true;
            }
            return false;
        }

        public void draw()
        {
            //            Gl.glLoadIdentity();
            //            Gl.glColor4f(1, 1, 1, 1);
            texture.Bind();
            //      Gl.glEnable(Gl.GL_BLEND);
            Gl.glBegin(Gl.GL_QUADS);
            {
                Gl.glTexCoord2f(0, 0);
                Gl.glVertex2i((int)lowerLeftCorner.x, (int)lowerLeftCorner.y);
                Gl.glTexCoord2f(1, 0);
                Gl.glVertex2i((int)upperRightCorner.x, (int)lowerLeftCorner.y);
                Gl.glTexCoord2f(1, 1);
                Gl.glVertex2i((int)upperRightCorner.x, (int)upperRightCorner.y);
                Gl.glTexCoord2f(0, 1);
                Gl.glVertex2i((int)lowerLeftCorner.x, (int)upperRightCorner.y);
            }
            Gl.glEnd();
        }

        public void Update(TimeSpan dt)
        {
        }
    }
}
