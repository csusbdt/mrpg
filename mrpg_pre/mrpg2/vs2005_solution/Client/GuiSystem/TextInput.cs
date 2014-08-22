using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using Tao.Sdl;

namespace Client
{
    class TextInput : Text
    {
        public delegate void ActionHandler();
        //private ActionHandler actionHandler;
        private bool focused = false;
        private RGB backgroundColor = new RGB(0.9f, 0.9f, 0.9f);
        private bool showCursor = false;


        public TextInput(ScreenCoordinate lowerLeftCorner, ScreenCoordinate upperRightCorner, Texture texture, String text)
            : base(lowerLeftCorner, upperRightCorner, texture, text)
        {
            base.TextColor = new RGB(0, 0, 0);
        }

        public bool Focused
        {
            get { return focused; }
            set { focused = value; }
        }
        public override void draw()
        {

            //draw the cursor, transparent quad 
            Gl.glDisable(Gl.GL_TEXTURE_2D);
            Gl.glColor4f(0.3f, 0.3f, 0.3f, 1);

            Gl.glBegin(Gl.GL_LINE_LOOP);
            {
                Gl.glVertex2i((int)lowerLeftCorner.x - 2, (int)lowerLeftCorner.y - 1);
                Gl.glVertex2i((int)upperRightCorner.x, (int)lowerLeftCorner.y - 1);
                Gl.glVertex2i((int)upperRightCorner.x, (int)upperRightCorner.y + 1);
                Gl.glVertex2i((int)lowerLeftCorner.x - 2, (int)upperRightCorner.y + 1);
            }
            Gl.glEnd();

            if (showCursor)
            {
                Gl.glColor4f(1, 0, 0, 1);
                Gl.glBegin(Gl.GL_LINE_LOOP);
                {
                    Gl.glVertex2i((int)lowerLeftCorner.x - 2, (int)lowerLeftCorner.y - 1);
                    Gl.glVertex2i((int)upperRightCorner.x, (int)lowerLeftCorner.y - 1);
                    Gl.glVertex2i((int)upperRightCorner.x, (int)upperRightCorner.y + 1);
                    Gl.glVertex2i((int)lowerLeftCorner.x - 2, (int)upperRightCorner.y + 1);
                }
                Gl.glEnd();
            }

            Gl.glColor4f(backgroundColor.Rf, backgroundColor.Gf, backgroundColor.Bf, 1);

            Gl.glBegin(Gl.GL_QUADS);
            {
                Gl.glVertex2i((int)lowerLeftCorner.x, (int)lowerLeftCorner.y);
                Gl.glVertex2i((int)upperRightCorner.x, (int)lowerLeftCorner.y);
                Gl.glVertex2i((int)upperRightCorner.x, (int)upperRightCorner.y);
                Gl.glVertex2i((int)lowerLeftCorner.x, (int)upperRightCorner.y);
            }
            Gl.glEnd();

            Gl.glEnable(Gl.GL_TEXTURE_2D);

            base.draw();

            Gl.glColor4f(1, 1, 1, 1);
        }

        public override bool IsInside(ScreenCoordinate point)
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

        public void focusHandler()
        {
            Log.Write("Focusing Text input");
            focused = true;
            showCursor = true;
            backgroundColor.Rf = 1.0f;
            backgroundColor.Gf = 1.0f;
            backgroundColor.Bf = 1.0f;
        }

        public void blurHandler()
        {
            Log.Write("Blur Text input");
            focused = false;
            showCursor = false;
            backgroundColor.Rf = 0.9f;
            backgroundColor.Gf = 0.9f;
            backgroundColor.Bf = 0.9f;
        }
        public void modifyTextHandler(Sdl.SDL_keysym key)
        {
            //Need more work to filter out unwanted input characters.                       
            switch (key.sym)
            {
                case Sdl.SDLK_BACKSPACE:
                    if (textValue.Length > 0)
                    {
                        textValue = textValue.Substring(0, textValue.Length - 1);
                    }
                    else
                    {
                        //No more character to delete.
                    }
                    break;
                default:
                    if (key.sym < 0x80)
                    {
                        char c = (char)key.sym;
                        if ((key.mod & Sdl.KMOD_SHIFT) == 1)//Capital letter
                        {
                            c = (char)(key.sym + ('A' - 'a'));
                            Log.Write("c = " + c);
                        }

                        textValue = textValue + c;
                    }
                    break;
            }
        }

        public override void Update(TimeSpan dt)
        {
            if (focused)
            {
                DateTime now = DateTime.Now;
                showCursor = ((int)now.Second % 2 == 0);
            }
        }
    }
}
