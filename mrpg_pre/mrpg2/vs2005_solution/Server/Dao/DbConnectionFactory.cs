using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace Server
{
    class DbConnectionFactory
    {
        private static string connectionString;

        private DbConnectionFactory()
        {
        }

        public static void Init()
        {
            if (Configuration.DbServer == null)
            {
                throw new LogicError();
            }
            connectionString =
                "Server=" + Configuration.DbServer + ";" +
                "Database=" + Configuration.DbName + ";" +
                "uid=" + Configuration.DbUid + ";" +
                "pwd=" + Configuration.DbPwd + ";" +
                "Pooling=" + Configuration.DbPooling + "";
        }

        public static IDbConnection getConnection()
        {
            IDbConnection dbConnection;
            dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }
    }
}
