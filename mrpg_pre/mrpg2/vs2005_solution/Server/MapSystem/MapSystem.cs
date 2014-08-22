using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class MapSystem
    {
        static Dictionary<string, Map> mapDictionary = new Dictionary<string, Map>();

        public static Dictionary<string, Map> MapDictionary
        {
            get { return mapDictionary; }
        }

        public static void Init()
        {
            Map homeMap = buildHomeMap();
            mapDictionary.Add(homeMap.MapId, homeMap);
        }

        public static Dictionary<string, Map> FindAll()
        {
            return mapDictionary;
        }

        static Map buildHomeMap()
        {
            Map map = new Map("home");
            WorldItem dagger0 = new WorldItem("dagger0", "dagger", map, new Vec3f(0, 0, 0), new Vec3f());
            map.AddMapEntity(dagger0);
            return map;
        }
    }
}
