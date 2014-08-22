using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class ModelSystem
    {
        static Dictionary<string, Model> modelDictionary = new Dictionary<string, Model>();

        public static Dictionary<string, Model> ModelDictionary
        {
            get { return modelDictionary; }
        }

        public static void Init()
        {
            // In future, this method will load spacifications from XML file.
            Log.Write();

            // dagger model
            InMemoryMeshCollection daggerInMemoryMeshCollection = ThreeDSModelLoader.Load("models/test.3ds");
            MeshCollection daggerMeshCollection = new MeshCollection(daggerInMemoryMeshCollection);
            Model daggerModel = new Model(daggerMeshCollection);
            modelDictionary.Add("dagger", daggerModel);
        }
    }
}
