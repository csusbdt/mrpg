using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class RGB
    {
        private byte ri = 255;
        private byte gi = 255;
        private byte bi = 255;

        private float rf = 1.0f;
        private float gf = 1.0f;
        private float bf = 1.0f;

        public RGB(byte r, byte g, byte b)
        {
            this.Ri = r;
            this.Gi = g;
            this.Bi = b;
        }

        public RGB(float r, float g, float b)
        {
            this.Rf = r;
            this.Gf = g;
            this.Bf = b;
        }
        public byte Ri
        {
            set
            {
                if (value < 0 || value > 255)
                    value = 0;

                ri = value;
                rf = (float)ri / 255;
            }
            get { return ri; }
        }

        public byte Gi
        {
            set
            {
                if (value < 0 || value > 255)
                    value = 0;

                gi = value;
                gf = (float)gi / 255;
            }
            get { return gi; }
        }

        public byte Bi
        {
            set
            {
                if (value < 0 || value > 255)
                    value = 0;

                bi = value;
                bf = (float)bi / 255;
            }
            get { return bi; }
        }

        public float Rf
        {
            set
            {
                if (value < 0.0f || value > 1.0f)
                    value = 0.0f;

                rf = value;
                ri = (byte)(rf * 255);
            }
            get { return rf; }
        }

        public float Gf
        {
            set
            {
                if (value < 0.0f || value > 1.0f)
                    value = 0.0f;

                gf = value;
                gi = (byte)(gf * 255);
            }
            get { return gf; }
        }

        public float Bf
        {
            set
            {
                if (value < 0.0f || value > 1.0f)
                    value = 0.0f;

                bf = value;
                bi = (byte)(bf * 255);
            }
            get { return bf; }
        }
    }
}
