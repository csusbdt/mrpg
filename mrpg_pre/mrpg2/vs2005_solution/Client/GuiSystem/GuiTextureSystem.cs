using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class GuiTextureSystem
    {
        static Dictionary<string, Texture> idTextureDictionary = new Dictionary<string, Texture>();
        static Dictionary<string, string> idFilenameDictionary = new Dictionary<string, string>();

        public static void Init()
        {
            idFilenameDictionary.Add("logoutButton", "gui/logoutButtonTexture.tga");
            idFilenameDictionary.Add("loginButton", "gui/loginButtonTexture.tga");
            idFilenameDictionary.Add("usernameText", "gui/usernameTextTexture.tga");
            idFilenameDictionary.Add("passwordText", "gui/passwordTextTexture.tga");
            idFilenameDictionary.Add("exitButton",   "gui/exitButtonTexture.tga");
            idFilenameDictionary.Add("okButton",     "gui/okButtonTexture.tga");
            idFilenameDictionary.Add("finishedButton", "gui/finishedButtonTexture.tga");
            idFilenameDictionary.Add("mageIcon",     "gui/mageIconTexture.tga");
        }

        public static Texture GetTextureById(string textureId)
        {
            string textureFilename = idFilenameDictionary[textureId];
            if (idTextureDictionary.ContainsKey(textureId))
            {
                return idTextureDictionary[textureId];
            }
            PixelContainer pixelContainer = TargaImageLoader.LoadImage(textureFilename);
            Texture texture = new Texture(pixelContainer);
            idTextureDictionary.Add(textureId, texture);
            return texture;
        }

        public static void UnloadAllTextures()
        {
            foreach (Texture texture in idTextureDictionary.Values)
            {
                texture.Dispose();
            }
            idTextureDictionary.Clear();
        }
    }
}
