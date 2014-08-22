using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Client
{
    class Text : GuiComponent
    {
        protected ScreenCoordinate lowerLeftCorner;
        protected ScreenCoordinate upperRightCorner;

        private Texture texture;
        protected String textValue;
        private RGB textColor;
        private bool isPassword = false;
        private int trimToLength = -1;

        public bool IsPassword
        {
            get { return isPassword; }
            set { isPassword = value; }
        }

        public int TrimToLength
        {
            get { return trimToLength; }
            set { trimToLength = value; }
        }

        public RGB TextColor
        {
            set
            {
                textColor = value;
            }
            get { return textColor; }
        }

        public String TextValue
        {
            set
            {
                textValue = value;
            }
            get { return textValue; }
        }
        private Font font = new Font("fonts/FreeSans.ttf", 16);
        public Texture Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Text(ScreenCoordinate lowerLeftCorner, ScreenCoordinate upperRightCorner, Texture texture, String text)
        {
            this.lowerLeftCorner = lowerLeftCorner;
            this.upperRightCorner = upperRightCorner;
            this.Texture = texture;
            this.textValue = text;
        }

        public virtual void draw()
        {
            if (textColor == null)
            {
                Gl.glColor4f(1, 1, 1, 1);
            }
            else
            {
                Gl.glColor4f(textColor.Rf, textColor.Gf, textColor.Bf, 1);
            }
            if (this.isPassword)
            {
                String dummyPassword = "";
                if (textValue != null && textValue.Length > 0)
                {
                    for (int i = 0; i < textValue.Length; i++)
                    {
                        dummyPassword += "*";
                    }
                    if (trimToLength != -1 && dummyPassword.Length > trimToLength)
                    {
                        font.Print(lowerLeftCorner.x,
                                  lowerLeftCorner.y + 2,
                                  dummyPassword.Substring(dummyPassword.Length - trimToLength));
                    }
                    else
                    {
                        font.Print(lowerLeftCorner.x, lowerLeftCorner.y + 2, dummyPassword);
                    }
                }
            }
            else
            {
                if (trimToLength != -1 && textValue.Length > trimToLength)
                {
                    font.Print(lowerLeftCorner.x,
                              lowerLeftCorner.y + 2,
                              textValue.Substring(textValue.Length - trimToLength));
                }
                else
                {
                    font.Print(lowerLeftCorner.x, lowerLeftCorner.y + 2, textValue);
                }

            }

            Gl.glColor4f(1, 1, 1, 1);
        }

        public virtual bool IsInside(ScreenCoordinate point)
        {
            return false;
        }

        public virtual void Update(TimeSpan dt)
        {
        }
    }
}
