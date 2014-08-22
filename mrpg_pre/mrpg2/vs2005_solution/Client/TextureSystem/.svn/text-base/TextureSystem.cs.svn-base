using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class TextureSystem
    {
        static Dictionary<string, Texture> modelTextureDictionary = new Dictionary<string, Texture>();

        public static Dictionary<string, Texture> ModelTextureDictionary
        {
            get { return modelTextureDictionary; }
        }

        public static void Init()
        {
            // In future, this method will load spacifications from XML file.
            Log.Write();

            // dagger texture
            PixelContainer daggerImage = TargaImageLoader.LoadImage("models/test.tga");
            Texture daggerTexture = new Texture(daggerImage);
            daggerImage = null;
            modelTextureDictionary.Add("dagger", daggerTexture);
        }
    }
}
