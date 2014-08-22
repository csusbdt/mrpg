using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Tao.OpenGl;

namespace Client
{
    class ModeledEntity : Entity
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

        public ModeledEntity(string entityId, string entityClass, Vec3f position, Vec3f orientation)
            : base(entityId, entityClass)
        {
            this.position = position;
            this.orientation = orientation;
            if (!TextureSystem.ModelTextureDictionary.ContainsKey(entityClass))
            {
                throw new Exception("Entity class " + entityClass + " not in model texture dictionary");
            }
            if (!ModelSystem.ModelDictionary.ContainsKey(entityClass))
            {
                throw new Exception("Entity class " + entityClass + " not in model dictionary");
            }
            texture = TextureSystem.ModelTextureDictionary[entityClass];
            model = ModelSystem.ModelDictionary[entityClass];
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
    }
}
