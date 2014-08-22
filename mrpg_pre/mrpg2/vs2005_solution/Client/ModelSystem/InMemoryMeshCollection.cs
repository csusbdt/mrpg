using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class InMemoryMeshCollection
    {
        #region Fields

        private Dictionary<string, InMemoryMesh> inMemoryMeshes = new Dictionary<string, InMemoryMesh>();

        #endregion

        #region Properties

        public Dictionary<string, InMemoryMesh> InMemoryMeshes
        {
            get { return inMemoryMeshes; }
        }

        #endregion
    }
}
