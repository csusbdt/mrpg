using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class MeshCollection : IDisposable
    {
        #region Fields

        private Dictionary<string, Mesh> meshes = new Dictionary<string, Mesh>();

        #endregion

        #region Properties

        public Dictionary<string, Mesh> Meshes
        {
            get { return meshes; }
        }

        #endregion

        #region Initialization

        public MeshCollection(InMemoryMeshCollection inMemoryMeshCollection)
        {
            foreach (KeyValuePair<string, InMemoryMesh> inMemoryMesh in inMemoryMeshCollection.InMemoryMeshes)
            {
                Mesh mesh = new Mesh(inMemoryMesh.Value);
                meshes.Add(mesh.Name, mesh);
            }
        }

        private bool disposed = false;

        public void Dispose()
        {
            if (!disposed && !Program.ShuttingDown)
            {
                foreach (KeyValuePair<string, Mesh> mesh in meshes)
                {
                    mesh.Value.Dispose();
                }
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~MeshCollection()
        {
            if (!disposed && !Program.ShuttingDown)
            {
                //throw new Exception("Mesh collection was not disposed before garbage collected.");
            }
        }


        #endregion

        #region Operations

        //public void AddInMemoryMesh(InMemoryMesh inMemoryMesh)
        //{
        //    inMemoryMeshes.Add(inMemoryMesh);
        //}

        #endregion

    }
}
