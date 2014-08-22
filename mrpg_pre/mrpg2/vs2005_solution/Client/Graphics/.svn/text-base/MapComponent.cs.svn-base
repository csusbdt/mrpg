using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;

namespace Client
{
    class MapComponent
    {
        Texture texture;
        Model model;

//        string mapComponentId;
//        string mapComponentClass;

        Vec3f position;
        Vec3f orientation;
        
        //public string MapComponentId
        //{
        //    get { return mapComponentId; }
        //}

        //public string MapComponentClass
        //{
        //    get { return mapComponentClass; }
        //}

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

        public MapComponent(
            Model model,
            Texture texture)
        {
            this.model = model;
            this.texture = texture;
        }

        //MapComponent(
        //    //string mapComponentId, 
        //    //string mapComponentClass, 
        //    Model model,
        //    Texture texture,
        //    Vec3f position,
        //    Vec3f orientation)
        //{
        //    //this.MapComponentId = mapComponentId;
        //    //this.mapComponentClass = mapComponentClass;
        //    this.model = model;
        //    this.texture = texture;
        //    this.position = position;
        //    this.orientation = orientation;
        //}

        public void Draw()
        {
            Gl.glTranslatef(position.x, position.y, position.z);
            Gl.glRotatef(orientation.x, 1, 0, 0);
            Gl.glRotatef(orientation.y, 0, 1, 0);
            Gl.glRotatef(orientation.z, 0, 0, 1);
            texture.Bind();
            model.Draw();
        }
    }
}
