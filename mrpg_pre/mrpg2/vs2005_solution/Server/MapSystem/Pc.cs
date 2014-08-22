using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class Pc : Entity
    {
        #region Fields

        Client client;
        Map map;
        Vec3f position;
        Vec3f orientation;

        #endregion

        #region Properties

        public Vec3f Position
        {
            get { return position; }
        }

        public Vec3f Orientation
        {
            get { return orientation; }
        }

        #endregion

        #region Initialization

        public Pc(string entityId, string entityClass, Map map, Vec3f position, Vec3f orientation, Client client)
            : base(entityId, entityClass)
        {
            this.map = map;
            this.position = position;
            this.orientation = orientation;
            this.client = client;
        }

        #endregion

        #region Pc Creation

        public static Pc CreatePc(Client client)
        {
            // Determine initial location.
            Map map = MapSystem.MapDictionary["home"];
            Vec3f position = new Vec3f(0, 0, -400);
            Vec3f orientation = new Vec3f(0, 0, 0);

            // Inform the client of the map and all entities in the map.
            // This must be done before the pc is added to the map, so we don't 
            // send a message to create the player's own pc.  (I'd like to remove this restriction.)
            map.SendMapInitializationMessages(client);
            client.SendMoveAvatarMessage(position, orientation);

            // Create an entity for the pc and add to the map.
            Pc pc = new Pc(GenerateUniqueId(), "mage", map, position, orientation, client);
            map.AddMapEntity(pc);
            return pc;
        }

        public static void DestroyPc(Pc pc)
        {
            pc.map.RemoveMapEntity(pc);
        }

        #endregion
    }
}
