using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class TargaImageLoader
    {
        private const int HEADER_SIZE = 18;
        private const int COLOR_MAP_TYPE_NONE = 0;
        private const int IMAGE_TYPE_UNCOMPRESSED_BGR = 2;

        internal struct TgaHeader
        {
            public byte imageIdLength;
            public byte colorMapType;
            public byte imageType;
            public byte colorMap1; // color palettes not supported in this code
            public byte colorMap2;
            public byte colorMap3;
            public byte colorMap4;
            public byte colorMap5;
            public short xOriginOfImage;
            public short yOriginOfImage;
            public short width;
            public short height;
            public byte bitsPerPixel;
            public byte imageDescriptor;

            public static TgaHeader Read(BinaryReader binaryReader)
            {
                TgaHeader tgaHeader = new TgaHeader();
                tgaHeader.imageIdLength = binaryReader.ReadByte();
                tgaHeader.colorMapType = binaryReader.ReadByte();
                tgaHeader.imageType = binaryReader.ReadByte();
                tgaHeader.colorMap1 = binaryReader.ReadByte();
                tgaHeader.colorMap2 = binaryReader.ReadByte();
                tgaHeader.colorMap3 = binaryReader.ReadByte();
                tgaHeader.colorMap4 = binaryReader.ReadByte();
                tgaHeader.colorMap5 = binaryReader.ReadByte();
                tgaHeader.xOriginOfImage = binaryReader.ReadInt16();
                tgaHeader.yOriginOfImage = binaryReader.ReadInt16();
                tgaHeader.width = binaryReader.ReadInt16();
                tgaHeader.height = binaryReader.ReadInt16();
                tgaHeader.bitsPerPixel = binaryReader.ReadByte();
                tgaHeader.imageDescriptor = binaryReader.ReadByte();
                return tgaHeader;
            }
        }

        public static PixelContainer LoadImage(string filename)
        {
            PixelContainer image = new PixelContainer();
            FileStream fileStream = File.OpenRead(filename);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            TgaHeader header = TgaHeader.Read(binaryReader);
            if (header.colorMapType != COLOR_MAP_TYPE_NONE)
            {
                throw new Exception("Only raw RGB targa files supported.");
            }
            if (header.imageType != IMAGE_TYPE_UNCOMPRESSED_BGR)
            {
                throw new Exception("Only uncompressed unmapped targa files supported.");
            }
            if (header.bitsPerPixel != 8 && header.bitsPerPixel != 24 && header.bitsPerPixel != 32)
            {
                throw new Exception("Bits per pixel in targa file must be either 8, 24 or 32.");
            }
            // Skip over image id field.
            binaryReader.ReadBytes(header.imageIdLength);

            // Read pixel data.
            int bytesPerPixel = header.bitsPerPixel / 8;
            int numberOfPixels = header.width * header.height;
            int numberOfBytes = numberOfPixels * bytesPerPixel;
            byte[] data = binaryReader.ReadBytes(numberOfBytes);
            binaryReader.Close();

            // Convert BGR into RGB.
            if (bytesPerPixel == 3 || bytesPerPixel == 4)
            {
                for (int pixelIndex = 0; pixelIndex < numberOfPixels; ++pixelIndex)
                {
                    int i = pixelIndex * bytesPerPixel;
                    byte blue = data[i];
                    byte red = data[i + 2];
                    data[i] = red;
                    data[i + 2] = blue;
                }
            }

            image.Height = header.height;
            image.Width = header.width;
            image.BytesPerPixel = bytesPerPixel;
            image.Data = data;
            return image;
        }

    }
}
