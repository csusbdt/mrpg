using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace ServerDatabase
{
    public class DatabaseSystem
    {
        #region Fields

        static string connectionString;

        #endregion

        #region Initialization

        DatabaseSystem()
        {
        }

        public static void Init(
            string databaseServer, 
            string databaseName, 
            string databaseUsername, 
            string databasePassword)
        {
            connectionString =
                "Server=" + databaseServer + ";" +
                "Database=" + databaseName + ";" +
                "uid=" + databaseUsername + ";" +
                "pwd=" + databasePassword + ";" +
                "Pooling=true";
        }

        #endregion

        #region Connection Factory

        internal static IDbConnection getConnection()
        {
            IDbConnection dbConnection;
            dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        #endregion
    }
}
