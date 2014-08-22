using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace ServerDatabase
{
    public class StoredItemDao
    {
        private const string sqlFindListByAvatarId =
            "SELECT entity_id, entity_class FROM inventory " +
            "WHERE avatar_id = ?avatar_id";

        public static List<StoredItem> FindListByAvatarId(string avatarId)
        {
            List<StoredItem> entities = new List<StoredItem>();
            IDbConnection connection = DatabaseSystem.getConnection();
            connection.Open();
            IDbCommand command = connection.CreateCommand();
            command.CommandText = sqlFindListByAvatarId;
            command.Prepare();
            IDataParameterCollection parameters = command.Parameters;
            parameters.Add(new MySqlParameter("?avatar_id", avatarId));
            IDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string entityId = reader.GetString(0);
                string entityClass = reader.GetString(1);
                StoredItem entity = new StoredItem(entityId, entityClass);
                entities.Add(entity);
            }
            reader.Close();
            connection.Close();
            return entities;
        }
    }
}
