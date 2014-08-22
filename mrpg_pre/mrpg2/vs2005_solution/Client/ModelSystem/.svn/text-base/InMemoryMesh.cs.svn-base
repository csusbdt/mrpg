using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class InMemoryMesh
    {
        private string name;
        private Vec3f[] vertices;
        private Vec3us[] faces;
        private Vec2f[] uvCoordinates;
        private Vec3f[] translationMatrix;
        private Vec3f[] normals;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Vec3f[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        public Vec3us[] Faces
        {
            get { return faces; }
            set { faces = value; }
        }

        public Vec2f[] UvCoordinates
        {
            get { return uvCoordinates; }
            set { uvCoordinates = value; }
        }

        public Vec3f[] TranslationMatrix
        {
            get { return translationMatrix; }
            set { translationMatrix = value; }
        }

        public Vec3f[] Normals
        {
            get { return normals; }
            set { normals = value; }
        }

        public void BuildNormals()
        {
            // Compute a normal for each face.
            Vec3f[] faceNormals = new Vec3f[faces.Length];
            for (int k = 0; k < faceNormals.Length; k++)
            {
                Vec3us face = faces[k];
                int vertexIndexA = face.x;
                int vertexIndexB = face.y;
                int vertexIndexC = face.z;
                Vec3f vertexA = vertices[vertexIndexA];
                Vec3f vertexB = vertices[vertexIndexB];
                Vec3f vertexC = vertices[vertexIndexC];
                Vec3f n1 = vertexA - vertexC;
                Vec3f n2 = vertexB - vertexA;
                faceNormals[k] = n1.CrossProduct(n2);
            }
            normals = new Vec3f[vertices.Length];
            for (int i = 0; i < normals.Length; i++)
            {
                Vec3f vertex = vertices[i];
                float x = vertex.x;
                float y = vertex.y;
                float z = vertex.z;
                Vec3f normal = new Vec3f();
                for (int k = 0; k < faces.Length; k++)
                {
                    Vec3us face = faces[k];
                    int vertexIndexA = face.x;
                    int vertexIndexB = face.y;
                    int vertexIndexC = face.z;
                    Vec3f vertexA = vertices[vertexIndexA];
                    Vec3f vertexB = vertices[vertexIndexB];
                    Vec3f vertexC = vertices[vertexIndexC];
                    if (i == vertexIndexA ||
                        vertex == vertexA ||
                        i == vertexIndexB ||
                        vertex == vertexB ||
                        i == vertexIndexC ||
                        vertex == vertexC)
                    {
                        normal.x += faceNormals[k].x;
                        normal.y += faceNormals[k].y;
                        normal.z += faceNormals[k].z;
                    }
                }
                normal.Normalize();
                normals[i] = normal;
            }
        }
    }
}
