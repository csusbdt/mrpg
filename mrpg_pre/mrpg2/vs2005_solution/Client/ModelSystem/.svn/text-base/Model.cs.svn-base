using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class Model
    {
        #region Fields

        Dictionary<string, Mesh> meshes;

        #endregion

        #region Properties

        public Dictionary<string, Mesh> Meshes
        {
            get { return meshes; }
        }

        #endregion

        #region Initialization

        public Model(MeshCollection meshCollection)
        {
            this.meshes = meshCollection.Meshes;
        }

        #endregion

        #region Draw

        public void Draw()
        {
            foreach (KeyValuePair<string, Mesh> mesh in meshes)
            {
                mesh.Value.Draw();
            }
        }

        #endregion
    }
}
