using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Configuration
    {
        #region Fields

        static string dbServer;
        static string dbName;
        static string dbUid;
        static string dbPwd;
        static string dbPooling;
        const int port = 10008;
        const int maximumNumberOfPlayers = 8;

        #endregion

        #region Properties

        public static string DbServer
        {
            get { return dbServer; }
        }

        public static string DbName
        {
            get { return dbName; }
        }

        public static string DbUid
        {
            get { return dbUid; }
        }

        public static string DbPwd
        {
            get { return dbPwd; }
        }

        public static string DbPooling
        {
            get { return dbPooling; }
        }

        public static int Port
        {
            get { return port; }
        }

        public static int MaximumNumberOfPlayers
        {
            get { return maximumNumberOfPlayers; }
        }

        #endregion

        #region Initialization

        public static void Init()
        {
            Log.Write();
            dbServer = "localhost";
            dbName = "rpg";
            dbUid = "gamer";
            dbPwd = "gamer";
            dbPooling = "true";
        }

        #endregion
    }
}
