using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace Server
{
    class AvatarDao
    {
        const string sqlFindListByUsername =
            "SELECT avatar_id, avatar_class, max_health_points FROM avatar WHERE username = ?username";

        public static List<Avatar> FindListByUsername(string username)
        {
            IDbConnection connection = DbConnectionFactory.getConnection();
            connection.Open();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = sqlFindListByUsername;
            command.Prepare();
            IDataParameterCollection parameters = command.Parameters;
            parameters.Add(new MySqlParameter("?username", username));
            IDataReader reader = command.ExecuteReader();
            List<Avatar> avatars = new List<Avatar>();
            while (reader.Read())
            {
                string avatarId = reader.GetString(0);
                string avatarClass = reader.GetString(1);
                float maxHealthPoints = reader.GetFloat(2);
                List<Entity> inventory = InventoryDao.FindListByAvatarId(avatarId);
                Avatar avatar = new Avatar(
                    avatarId,
                    avatarClass,
                    new Vec3f(),
                    new Vec3f(),
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
