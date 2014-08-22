using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class PixelContainer
    {
        private int width;
        private int height;
        private int bytesPerPixel;
        private byte[] data;

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int BytesPerPixel
        {
            get { return bytesPerPixel; }
            set { bytesPerPixel = value; }
        }

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }
    }
}
