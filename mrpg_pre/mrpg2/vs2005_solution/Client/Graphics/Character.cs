using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    class Character : Entity
    {
        #region Fields

        Vec3f position;
        Vec3f orientation;
        Texture texture;
        Model model;

        #endregion

        #region Properties

        public Vec3f Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vec3f Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        #endregion

        #region Initialization

        Character()
        {
        }

        #endregion

        #region Draw

        public void Draw()
        {
            Gl.glTranslatef(position.x, position.y, position.z);
            Gl.glRotatef(orientation.x, 1, 0, 0);
            Gl.glRotatef(orientation.y, 0, 1, 0);
            Gl.glRotatef(orientation.z, 0, 0, 1);
            texture.Bind();
            model.Draw();
        }

        #endregion

        #region Read

        public static Character Read(BinaryReader binaryReader)
        {
            Log.Write();
            Character character = new Character();
            character.entityId = binaryReader.ReadString();
            character.entityClass = binaryReader.ReadString();
            character.position = Vec3f.Read(binaryReader);
            character.orientation = Vec3f.Read(binaryReader);
            if (!TextureSystem.ModelTextureDictionary.ContainsKey(character.entityClass))
            {
                throw new Exception("Entity class " + character.entityClass + " not in model texture dictionary");
            }
            if (!ModelSystem.ModelDictionary.ContainsKey(character.entityClass))
            {
                throw new Exception("Entity class " + character.entityClass + " not in model dictionary");
            }
            character.texture = TextureSystem.ModelTextureDictionary[character.entityClass];
            character.model = ModelSystem.ModelDictionary[character.entityClass];
            return character;
        }

        #endregion

    }
}
