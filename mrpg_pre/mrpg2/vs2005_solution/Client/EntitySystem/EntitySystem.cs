using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class EntitySystem
    {
        #region Fields

        static Dictionary<string, Entity> entityDictionary = new Dictionary<string, Entity>();
        delegate Entity CreateEntityDelegate(CreateModeledEntityMessage createEntityMessage);
        static Dictionary<string, CreateEntityDelegate> createEntityDelegateDictionary = new Dictionary<string, CreateEntityDelegate>();

        #endregion

        #region Initialization

        public static void Init()
        {
//            createEntityDelegateDictionary.Add("dagger", CreateModeledEntity);
        }

        #endregion

        #region Delete

        public static void DeleteEntity(string entityId)
        {
            Entity entity = entityDictionary[entityId];
            GraphicsSystem.Entities.Remove(entity);
            entityDictionary.Remove(entityId);
        }

        #endregion

        #region Create

        public static Entity CreateEntity(CreateEntityMessage createEntityMessage)
        {
            Entity entity = createEntityMessage.CreateEntity();
            //Entity entity = createEntityDelegateDictionary[createEntityMessage.EntityClass](createEntityMessage);
            entityDictionary.Add(entity.EntityId, entity);
            return entity;
        }

        //static Entity CreateModeledEntity(CreateModeledEntityMessage createModeledEntityMessage)
        //{
        //    ModeledEntity modeledEntity = createModeledEntityMessage.CreateModeledEntity();
        //    return modeledEntity;
        //}

        #endregion

    }
}
