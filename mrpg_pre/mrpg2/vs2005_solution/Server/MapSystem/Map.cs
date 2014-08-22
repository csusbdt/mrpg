using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Server
{
    class Map
    {
        #region Fields

        string mapId;
        List<Pc> pcs = new List<Pc>();
        List<WorldItem> worldItems = new List<WorldItem>();

        #endregion

        #region Properties

        public string MapId
        {
            get { return mapId; }
        }

        public List<WorldItem> WorldItems
        {
            get { return worldItems; }
        }

        public List<Pc> Pcs
        {
            get { return pcs; }
        }

        #endregion

        #region Events

        public delegate void UpdateEventDelegate(TimeSpan dt);
        public UpdateEventDelegate UpdateEvent;

        // Propogate update event to downstream listeners.
        public void UpdateEventHandler(TimeSpan dt)
        {
            UpdateEvent(dt);
        }

        #endregion

        #region Initialization

        public Map(string mapId)
        {
            this.mapId = mapId;
        }

        #endregion

        #region Communication

        public void SendMapInitializationMessages(Client client)
        {
            client.SendSetMapMessage(mapId);
            foreach (Pc pc in pcs)
            {
                client.SendCreatePcMessage(pc);
            }
        }

        #endregion

        #region Entity Management

        public void AddMapEntity(WorldEntity mapEntity)
        {
            if (mapEntity is Pc)
            {
                pcs.Add((Pc) mapEntity);
                if (pcs.Count == 1)
                {
                    Program.UpdateEvent += UpdateEventHandler;
                }
            }
            else if (mapEntity is WorldItem)
            {
                worldItems.Add((WorldItem) mapEntity);
            }
        }

        public void RemoveMapEntity(WorldEntity mapEntity)
        {
            if (mapEntity is Pc)
            {
                pcs.Remove((Pc)mapEntity);
                if (pcs.Count == 0)
                {
                    Program.UpdateEvent -= UpdateEventHandler;
                }
            }
            else if (mapEntity is WorldItem)
            {
                worldItems.Remove((WorldItem)mapEntity);
            }
        }

        #endregion
    }
}
