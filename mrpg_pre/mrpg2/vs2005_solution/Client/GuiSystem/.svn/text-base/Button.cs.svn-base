using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Client
{
    class Button : GuiComponent
    {
        //        private Texture texture;
        //        private Color color = new Color(1, 0, 0, 0.5f);
        //        private Label label;
        private ScreenCoordinate lowerLeftCorner;
        private ScreenCoordinate upperRightCorner;
        //private uint width;
        //private uint height;
        //private bool hasFocus = false;
        //private float red = 1;
        //private float green = 0;
        //private float blue = 0;
        //private float alpha = 1;

        private Texture texture;
        public delegate void ClickHandler();
        private ClickHandler clickHandler;
        private bool enabled = true;
        private bool clicked = false;

        public bool Clicked
        {
            set { clicked = value; }
            get { return clicked; }
        }
        public bool Enabled
        {
            set { enabled = value; }
            get { return enabled; }
        }
        public Texture Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public ClickHandler ClickEventHandler
        {
            get { return clickHandler; }
        }

        public int Width
        {
            set
            {
                upperRightCorner.x = lowerLeftCorner.x + value;
                //if (label != null)
                //{
                //    label.Width = value;
                //}
            }
        }

        public int Height
        {
            set
            {
                upperRightCorner.y = lowerLeftCorner.y + value;
                //if (label != null)
                //{
                //    label.Height = value;
                //}
            }
        }

        // new code
        public Button(ScreenCoordinate lowerLeftCorner, ScreenCoordinate upperRightCorner, Texture texture, ClickHandler clickHandler)
        {
            this.lowerLeftCorner = lowerLeftCorner;
            this.upperRightCorner = upperRightCorner;
            this.Texture = texture;
            this.clickHandler = clickHandler;
        }

        //public Button(ScreenCoordinate lowerLeftCorner, ScreenCoordinate upperRightCorner)
        //{
        //    this.lowerLeftCorner = lowerLeftCorner;
        //    this.upperRightCorner = upperRightCorner;
        //}

        //public Button(ScreenCoordinate lowerLeftCorner, ScreenCoordinate upperRightCorner, string buttonText)
        //{
        //    this.lowerLeftCorner = lowerLeftCorner;
        //    this.upperRightCorner = upperRightCorner;
        //    this.label = new Label(lowerLeftCorner, upperRightCorner, buttonText);
        //}

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
            //Gl.glLoadIdentity();
            //if (hasFocus)
            //{
            //    Gl.glColor4f(red, green, blue, alpha / 2);
            //}
            //else
            //{
            //    Gl.glColor4f(red, green, blue, alpha);
            //}
            ////if (texture != null)
            ////{
            //    drawTexture();
            ////}

            //if (label != null)
            //{
            //    label.draw();
            //}
            //            Gl.glColor4f(1, 1, 1, 1);
            texture.Bind();
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

            if (this.clicked)
            {
                Gl.glBegin(Gl.GL_LINE_LOOP);
                {
                    Gl.glVertex2i((int)lowerLeftCorner.x-1, (int)lowerLeftCorner.y-1);
                    Gl.glVertex2i((int)upperRightCorner.x+1, (int)lowerLeftCorner.y-1);
                    Gl.glVertex2i((int)upperRightCorner.x+1, (int)upperRightCorner.y+1);
                    Gl.glVertex2i((int)lowerLeftCorner.x-1, (int)upperRightCorner.y+1);
                }
                Gl.glEnd();
            }


        }

        public void Update(TimeSpan dt)
        {
        }
    }
}
