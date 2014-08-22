using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace Client
{
    class Configuration
    {
        #region Fields

        static bool fullScreen;
        static int horizontalPixels = 640;
        static int verticalPixels = 480;
        static string title = "CSUSB MRPG";
        static int serverPort = 10008;
        static IPAddress serverAddress = IPAddress.Parse("127.0.0.1");
        static Dictionary<string, string> entityClassModelDictionary = new Dictionary<string, string>();
        static Dictionary<string, string> entityClassTextureDictionary = new Dictionary<string, string>();

        #endregion

        #region Properties

        public static bool FullScreen
        {
            get { return fullScreen; }
        }

        public static int HorizontalPixels
        {
            get { return horizontalPixels; }
        }

        public static int VerticalPixels
        {
            get { return verticalPixels; }
        }

        public static string Title
        {
            get { return title; }
        }

        public static int ServerPort
        {
            get { return serverPort; }
        }

        public static IPAddress ServerAddress
        {
            get { return serverAddress; }
        }

        public static Dictionary<string, string> EntityClassModelDictionary
        {
            get { return entityClassModelDictionary; }
        }

        public static Dictionary<string, string> EntityClassTextureDictionary
        {
            get { return entityClassTextureDictionary; }
        }

        #endregion

        #region Init

        public static void Init()
        {
            fullScreen = false;
            entityClassModelDictionary.Add("dagger", "models/test.tga");
            entityClassTextureDictionary.Add("dagger", "models/test.3ds");
        }

        #endregion
    }
}
