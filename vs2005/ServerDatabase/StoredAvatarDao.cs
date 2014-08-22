using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace ServerDatabase
{
    public class StoredAvatarDao
    {
        const string sqlFindListByUsername =
            "SELECT avatar_id, avatar_class, max_health_points FROM avatar WHERE username = ?username";

        public static List<StoredAvatar> FindListByUsername(string username)
        {
            IDbConnection connection = DatabaseSystem.getConnection();
            connection.Open();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = sqlFindListByUsername;
            command.Prepare();
            IDataParameterCollection parameters = command.Parameters;
            parameters.Add(new MySqlParameter("?username", username));
            IDataReader reader = command.ExecuteReader();
            List<StoredAvatar> avatars = new List<StoredAvatar>();
            while (reader.Read())
            {
                string avatarId = reader.GetString(0);
                string avatarClass = reader.GetString(1);
                float maxHealthPoints = reader.GetFloat(2);
                List<StoredItem> inventory = StoredItemDao.FindListByAvatarId(avatarId);
                StoredAvatar avatar = new StoredAvatar(
                    avatarId,
                    avatarClass,
                    maxHealthPoints,
                    inventory);
                avatars.Add(avatar);
            }
            reader.Close();
            connection.Close();
            return avatars;
        }

    }
}
