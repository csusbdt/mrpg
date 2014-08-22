using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Tao.OpenGl;

namespace Client
{
    class Mesh : IDisposable
    {
        #region Fields

        private string name;
        private int displayListName;

        #endregion

        #region Initialization

        public Mesh(InMemoryMesh inMemoryMesh)
        {
            name = inMemoryMesh.Name;
            displayListName = Gl.glGenLists(1);
            Gl.glNewList(displayListName, Gl.GL_COMPILE);
            Gl.glEnableClientState(Gl.GL_VERTEX_ARRAY);
            Gl.glEnableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
            if (inMemoryMesh.Normals != null)
            {
                Gl.glEnableClientState(Gl.GL_NORMAL_ARRAY);
            }
            Gl.glVertexPointer(3, Gl.GL_FLOAT, 0, inMemoryMesh.Vertices);
            Gl.glTexCoordPointer(2, Gl.GL_FLOAT, 0, inMemoryMesh.UvCoordinates);
            if (inMemoryMesh.Normals != null)
            {
                Gl.glNormalPointer(Gl.GL_FLOAT, 0, inMemoryMesh.Normals);
            }
            Gl.glDrawElements(Gl.GL_TRIANGLES, inMemoryMesh.Faces.Length * 3, Gl.GL_UNSIGNED_SHORT, inMemoryMesh.Faces);
            Gl.glDisableClientState(Gl.GL_VERTEX_ARRAY);
            Gl.glDisableClientState(Gl.GL_TEXTURE_COORD_ARRAY);
            if (inMemoryMesh.Normals != null)
            {
                Gl.glDisableClientState(Gl.GL_NORMAL_ARRAY);
            }
            Gl.glEndList();
        }

        private bool disposed = false;

        public void Dispose()
        {
            if (!disposed)
            {
                Gl.glDeleteLists(displayListName, 1);
                disposed = true;
                GC.SuppressFinalize(this);
            }
        }

        ~Mesh()
        {
            if (!disposed && !Program.ShuttingDown)
            {
                //throw new Exception("Mesh was not disposed before garbage collected.");
            }
        }

        #endregion

        public void Draw()
        {
            Gl.glCallList(displayListName);
        }

        public string Name
        {
            get { return name; }
        }
    }
}
