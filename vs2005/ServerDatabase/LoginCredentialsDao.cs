using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace ServerDatabase
{
    public class LoginCredentialsDao
    {
        private const string sqlFindByUsername =
            "SELECT password FROM login_credentials " +
            "WHERE username = ?username";

        public static LoginCredentials FindByUsername(string username)
        {
            IDbConnection connection = DatabaseSystem.getConnection();
            connection.Open();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = sqlFindByUsername;
            command.Prepare();
            IDataParameterCollection parameters = command.Parameters;
            parameters.Add(new MySqlParameter("?username", username));
            IDataReader reader = command.ExecuteReader();
            bool found = reader.Read();
            if (!found)
            {
                return null;
            }
            string password = reader.GetString(0);
            reader.Close();
            connection.Close();
            LoginCredentials loginCredentials = new LoginCredentials(username, password);
            return loginCredentials;
        }
    }
}
