using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Client
{
    class Texture : IDisposable
    {
        #region Fields

        private int[] textureName = new int[1];
        private int width;
        private int height;

        #endregion

        #region Properties

        public int Width
        {
            get { return width; }
        }

        public int Height
        {
            get { return height; }
        }

        #endregion

        #region Initialization

        public Texture(PixelContainer image)
        {
            Gl.glGenTextures(1, textureName);
            Gl.glEnable(Gl.GL_TEXTURE_2D);
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureName[0]);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MIN_FILTER, Gl.GL_LINEAR_MIPMAP_NEAREST);
            Gl.glTexParameteri(Gl.GL_TEXTURE_2D, Gl.GL_TEXTURE_MAG_FILTER, Gl.GL_LINEAR_MIPMAP_LINEAR);
            width = image.Width;
            height = image.Height;
            int bytesPerPixel = image.BytesPerPixel;
            byte[] data = image.Data;
            int result = 0;
            if (bytesPerPixel == 1)
            {
                result = Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 1, width, height, Gl.GL_LUMINANCE, Gl.GL_UNSIGNED_BYTE, data);
            }
            else if (bytesPerPixel == 3)
            {
                result = Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 3, width, height, Gl.GL_RGB, Gl.GL_UNSIGNED_BYTE, data);
            }
            else if (bytesPerPixel == 4)
            {
                result = Glu.gluBuild2DMipmaps(Gl.GL_TEXTURE_2D, 4, width, height, Gl.GL_RGBA, Gl.GL_UNSIGNED_BYTE, data);
            }
            else
            {
                throw new Exception("Logic error");
            }
            if (result != 0)
            {
                string errorString = Glu.gluErrorString(result);
                throw new Exception(errorString);
            }
        }

        private bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                Gl.glDeleteTextures(1, textureName);
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~Texture()
        {
            if (!disposed && !Program.ShuttingDown)
            {
                throw new Exception("Texture object was not disposed before garbage collected.");
            }
        }

        #endregion

        #region Bind

        public void Bind()
        {
            Gl.glBindTexture(Gl.GL_TEXTURE_2D, textureName[0]);
        }

        #endregion
    }
}
