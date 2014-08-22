using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Client
{
    class Map
    {
        #region Fields

        static List<IDisposable> disposables = new List<IDisposable>();
        static Dictionary<string, MapComponent> mapComponentDictionary = new Dictionary<string,MapComponent>();

        #endregion

        #region Properties

        #endregion

        #region Life Cycle

        public static void Load(string mapId)
        {
            Log.Write();
            Unload();
            loadHomeMap(); // ignore mapId for now
        }

        public static void Unload()
        {
            Log.Write();
            foreach (IDisposable disposable in disposables)
            {
                disposable.Dispose();
            }
            mapComponentDictionary.Clear();
        }

        private static void loadHomeMap()
        {
            PixelContainer image = TargaImageLoader.LoadImage("maps/level.tga");
            Texture texture = new Texture(image);
            disposables.Add(texture);
            InMemoryMeshCollection inMemoryMeshCollection = ThreeDSModelLoader.Load("maps/level.3ds");
            MeshCollection meshCollection = new MeshCollection(inMemoryMeshCollection);
            Model model = new Model(meshCollection);
            disposables.Add(meshCollection);
            MapComponent mapComponent = new MapComponent(model, texture);
            mapComponentDictionary.Add("home", mapComponent);
        }

        #endregion

        #region Draw

        public static void Draw()
        {
            foreach (MapComponent mapComponent in mapComponentDictionary.Values)
            {
                Gl.glPushMatrix();
                mapComponent.Draw();
                Gl.glPushMatrix();
            }
        }

        #endregion
    }
}
