using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class ThreeDSModelLoader
    {
        private static BinaryReader binaryReader;
        private static List<InMemoryMesh> inMemoryMeshes = new List<InMemoryMesh>();
        private static InMemoryMesh currentMesh;

        public static InMemoryMeshCollection Load(string filename)
        {
            InMemoryMeshCollection inMemoryMeshCollection = new InMemoryMeshCollection();
            FileStream fileStream = File.OpenRead(filename);
            binaryReader = new BinaryReader(fileStream);
            ParseChunk();
            for (int i = 0; i < inMemoryMeshes.Count; ++i)
            {
                inMemoryMeshes[i].BuildNormals();
                string meshName = inMemoryMeshes[i].Name;
                if (inMemoryMeshCollection.InMemoryMeshes.ContainsKey(meshName))
                {
                    meshName += "" + new Random().Next();
                    inMemoryMeshes[i].Name = meshName;
                }
                inMemoryMeshCollection.InMemoryMeshes.Add(inMemoryMeshes[i].Name, inMemoryMeshes[i]);
            }
            return inMemoryMeshCollection;
        }

        /*
         * The first element of a chunk is a chunk id (2-byte unsigned int).
         * The second element of a chunk is the chunk size in bytes (4-byte unsigned int).
         * 
         * The chunk Id and chunk size values make up the chunk header.
         * 
         * The third element of a chunk is a data section whose definition is chunk dependent.
         * Some chunks do not have anything in the data section.
         * 
         * The fourth element of a chunk is a sequence of zero or more subchunks.
         *
         * The first Chunk in a 3DS file is the main chunk. I believe that for our purposes,
         * we can discard anything that might follow the main chunk.
         * 
         * We read the file by calling the ParseChunk method to parse the main chunk.
         * The method will recursively call itself to process all chunks under the main chunk.
         * 
         * We don't need to extract information from each chunk, so we read through
         * unused chunks.
         */

        private const UInt16 MAIN_CHUNK_ID = 0x4D4D;
        private const UInt16 EDITOR_CHUNK_ID = 0x3D3D;
        private const UInt16 OBJECT_CHUNK_ID = 0x4000;
        private const UInt16 TRIANGULAR_MESH_CHUNK_ID = 0x4100;
        private const UInt16 VERTEX_LIST_CHUNK_ID = 0x4110;
        private const UInt16 FACE_LIST_CHUNK_ID = 0x4120;
        private const UInt16 TEXTURE_MAPPING_CHUNK_ID = 0x4140;
        private const UInt16 TRANSLATION_MATRIX_CHUNK_ID = 0x4160;

        // Returns chunk size.
        private static UInt32 ParseChunk()
        {
            // Read until (bytesParsed == chunkSize)
            UInt32 bytesParsed = 0;

            // Parse chunk header.
            UInt16 chunkId = binaryReader.ReadUInt16();
            bytesParsed += 2;
            UInt32 chunkSize = binaryReader.ReadUInt32();
            bytesParsed += 4;

            // If I am an unused chunk, then skip over data section and subchunks.
            if (chunkId != MAIN_CHUNK_ID &&
                chunkId != EDITOR_CHUNK_ID &&
                chunkId != OBJECT_CHUNK_ID &&
                chunkId != TRIANGULAR_MESH_CHUNK_ID &&
                chunkId != VERTEX_LIST_CHUNK_ID &&
                chunkId != FACE_LIST_CHUNK_ID &&
                chunkId != TEXTURE_MAPPING_CHUNK_ID &&
                chunkId != TRANSLATION_MATRIX_CHUNK_ID)
            {
                binaryReader.ReadBytes((int)(chunkSize - bytesParsed));
                return chunkSize;
            }

            // Parse data section.
            if (chunkId == OBJECT_CHUNK_ID)
            {
                currentMesh = new InMemoryMesh();
                inMemoryMeshes.Add(currentMesh);
                string objectName = ParseZeroTerminatedString();
                bytesParsed += (UInt32) objectName.Length + 1;
                currentMesh.Name = objectName;
            }
            else if (chunkId == VERTEX_LIST_CHUNK_ID)
            {
                UInt16 numberOfVertices = binaryReader.ReadUInt16();
                bytesParsed += 2;
                Vec3f[] vertices = new Vec3f[numberOfVertices];
                for (int i = 0; i < numberOfVertices; ++i)
                {
                    vertices[i].x = binaryReader.ReadSingle();
                    vertices[i].y = binaryReader.ReadSingle();
                    vertices[i].z = binaryReader.ReadSingle();
                    bytesParsed += 12;
                }
                currentMesh.Vertices = vertices;
            }
            else if (chunkId == FACE_LIST_CHUNK_ID)
            {
                // For each face, store first 3 values, discard the fourth.
                UInt16 numberOfFaces = binaryReader.ReadUInt16();
                bytesParsed += 2;
                Vec3us[] faces = new Vec3us[numberOfFaces];
                for (int i = 0; i < numberOfFaces; ++i)
                {
                    faces[i].x = binaryReader.ReadUInt16();
                    faces[i].y = binaryReader.ReadUInt16();
                    faces[i].z = binaryReader.ReadUInt16();
                    binaryReader.ReadUInt16();  // Discard this ushort.
                    bytesParsed += 8;
                }
                currentMesh.Faces = faces;
            }
            else if (chunkId == TEXTURE_MAPPING_CHUNK_ID) 
            {
                UInt16 numberOfUVCoordinates = binaryReader.ReadUInt16();
                bytesParsed += 2;
                Vec2f[] uvCoordinates = new Vec2f[numberOfUVCoordinates];
                for (int i = 0; i < numberOfUVCoordinates; ++i)
                {
                    uvCoordinates[i] = new Vec2f();
                    uvCoordinates[i].x = binaryReader.ReadSingle();
                    uvCoordinates[i].y = binaryReader.ReadSingle();
                    bytesParsed += 8;
                }
                currentMesh.UvCoordinates = uvCoordinates;
            }
            else if (chunkId == TRANSLATION_MATRIX_CHUNK_ID)
            {
                Vec3f[] translationMatrix = new Vec3f[4];
                for (int i = 0; i < 4; ++i)
                {
                    translationMatrix[i] = new Vec3f();
                    translationMatrix[i].x = binaryReader.ReadSingle();
                    translationMatrix[i].y = binaryReader.ReadSingle();
                    translationMatrix[i].z = binaryReader.ReadSingle();
                    bytesParsed += 12;
                }
            }
            else
            {
                // No other chunk types of interest have data sections.
            }

            // Parse subchunks.
            while (bytesParsed < chunkSize)
            {
                bytesParsed += ParseChunk();
            }
            return chunkSize;
        }

        private static string ParseZeroTerminatedString()
        {
            MemoryStream memoryStream = new MemoryStream();
            while (true)
            {
                byte asciiChar = binaryReader.ReadByte();
                if (asciiChar == 0)
                {
                    break;
                }
                memoryStream.WriteByte(asciiChar);
            }
            Encoding asciiEncoding = Encoding.ASCII;
            char[] unicodeChars = asciiEncoding.GetChars(memoryStream.ToArray());
            return new string(unicodeChars);
        }
    }
}


// Old solution follows, whcih is based on defining an internal class.

        //// To ignore a chunk, create an instance of Chunk and call its parse method.
        //internal class Chunk
        //{
        //    private const UInt16 MAIN_CHUNK_ID = 0x4D4D;
        //    private const UInt16 EDITOR_CHUNK_ID = 0x3D3D;
        //    private const UInt16 OBJECT_CHUNK_ID = 0x4000;
        //    private const UInt16 TRIANGULAR_MESH_CHUNK_ID = 0x4100;
        //    private const UInt16 VERTEX_LIST_CHUNK_ID = 0x4110;
        //    private const UInt16 FACE_LIST_CHUNK_ID = 0x4120;
        //    private const UInt16 TEXTURE_MAPPING_CHUNK_ID = 0x4140;
        //    private const UInt16 TRANSLATION_MATRIX_CHUNK_ID = 0x4160;

        //    public UInt16 chunkId;
        //    public UInt32 chunkSize;
        //    private UInt32 bytesParsed = 0;

        //    public void Parse()
        //    {
        //        chunkId = binaryReader.ReadUInt16();
        //        bytesParsed += 2;
        //        chunkSize = binaryReader.ReadUInt32();
        //        bytesParsed += 4;
        //        if (chunkId == MAIN_CHUNK_ID ||
        //            chunkId == EDITOR_CHUNK_ID ||
        //            chunkId == OBJECT_CHUNK_ID ||
        //            chunkId == TRIANGULAR_MESH_CHUNK_ID ||
        //            chunkId == VERTEX_LIST_CHUNK_ID ||
        //            chunkId == FACE_LIST_CHUNK_ID ||
        //            chunkId == TEXTURE_MAPPING_CHUNK_ID ||
        //            chunkId == TRANSLATION_MATRIX_CHUNK_ID)
        //        {
        //            // Process known chunks.
        //            ReadDataSection();
        //            ReadSubChunks();
        //        }
        //        else
        //        {
        //            // Read through unknown chunks.
        //            binaryReader.ReadBytes(chunkSize - bytesParsed);
        //            bytesParsed = chunkSize;
        //        }
        //    }

        //    private void ReadDataSection()
        //    {
        //        switch (chunkId)
        //        {
        //            case OBJECT_CHUNK_ID:
        //                currentMesh = new Mesh();
        //                meshes.Add(currentMesh);
        //                string objectName = ParseZeroTerminatedString();
        //                currentMesh.Name = objectName;
        //                break;
        //            case VERTEX_LIST_CHUNK_ID:
        //                UInt16 numberOfVertices = binaryReader.ReadUInt16();
        //                bytesParsed += 2;
        //                float[] vertices = new float[numberOfVertices * 3];
        //                for (int i = 0; i < numberOfVertices; ++i)
        //                {
        //                    vertices[i * 3 + 0] = binaryReader.ReadSingle();
        //                    vertices[i * 3 + 1] = binaryReader.ReadSingle();
        //                    vertices[i * 3 + 2] = binaryReader.ReadSingle();
        //                    bytesParsed += 9;
        //                }
        //                currentMesh.Vertices = vertices;
        //                break;
        //            case FACE_LIST_CHUNK_ID:
        //                // For each face, store first 3 values, discard the fourth.
        //                UInt16 numberOfFaces = binaryReader.ReadUInt16();
        //                bytesParsed += 2;
        //                UInt16[] faces = new UInt16[numberOfFaces * 3];
        //                for (int i = 0; i < numberOfFaces; ++i)
        //                {
        //                    faces[i * 3 + 0] = binaryReader.ReadUInt16();
        //                    faces[i * 3 + 1] = binaryReader.ReadUInt16();
        //                    faces[i * 3 + 2] = binaryReader.ReadUInt16();
        //                    binaryReader.ReadUInt16();
        //                    bytesParsed += 8;
        //                }
        //                currentMesh.Faces = faces;
        //                break;
        //            case TEXTURE_MAPPING_CHUNK_ID:
        //                UInt16 numberOfUVCoordinates = binaryReader.ReadUInt16();
        //                bytesParsed += 2;
        //                float[] uvCoordinates = new float[numberOfUVCoordinates * 2];
        //                for (int i = 0; i < numberOfUVCoordinates; ++i)
        //                {
        //                    uvCoordinates[i * 2 + 0] = binaryReader.ReadSingle();
        //                    uvCoordinates[i * 2 + 1] = binaryReader.ReadSingle();
        //                    bytesParsed += 8;
        //                }
        //                currentMesh.UvCoordinates = uvCoordinates;
        //                break;
        //            case TRANSLATION_MATRIX_CHUNK_ID:
        //                float[] translationMatrix = new float[3 * 4];
        //                for (int i = 0; i < 4; ++i)
        //                {
        //                    translationMatrix[i * 3 + 0] = binaryReader.ReadSingle();
        //                    translationMatrix[i * 3 + 1] = binaryReader.ReadSingle();
        //                    translationMatrix[i * 3 + 2] = binaryReader.ReadSingle();
        //                    bytesParsed += 12;
        //                }
        //                break;
        //        }
        //    }

        //    private void ReadSubChunks()
        //    {
        //        while (bytesParsed < chunkSize)
        //        {
        //            Chunk subChunk = new Chunk();
        //            subChunk.Parse();
        //            bytesParsed += subChunk.chunkSize;
        //        }
        //    }

        //    protected string ParseZeroTerminatedString()
        //    {
        //        MemoryStream memoryStream = new MemoryStream();
        //        while (true)
        //        {
        //            byte asciiChar = binaryReader.ReadByte();
        //            ++bytesParsed;
        //            if (asciiChar == 0)
        //            {
        //                break;
        //            }
        //            memoryStream.WriteByte(asciiChar);
        //        }
        //        Encoding asciiEncoding = Encoding.ASCII;
        //        char[] unicodeChars = asciiEncoding.GetChars(memoryStream.ToArray());
        //        return new string(unicodeChars);
        //    }
        //}
