using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using System.IO;

namespace Client
{
    class Avatar
    {
        string avatarId;
        string avatarClass;
        //Vec3f position;
        //Vec3f orientation;
        //float healthPoints;
        //List<Entity> inventory;
        //float speed = 30;

        public string AvatarId
        {
            get { return avatarId; }
        }

        public string AvatarClass
        {
            get { return avatarClass; }
        }

        //public float HealthPoints
        //{
        //    get { return healthPoints; }
        //}

        //public Vec3f Position
        //{
        //    get { return position; }
        //    set { position = value; }
        //}

        //public Vec3f Orientation
        //{
        //    get { return orientation; }
        //    set { orientation = value; }
        //}

        //public List<Entity> Inventory
        //{
        //    get { return inventory; }
        //    set { inventory = value; }
        //}

        //public float Speed
        //{
        //    get { return speed; }
        //    set { speed = value; }
        //}

        //public void Draw()
        //{
        //    Gl.glTranslatef(position.x, position.y, position.z);
        //    Gl.glRotatef(orientation.x, 1, 0, 0);
        //    Gl.glRotatef(orientation.y, 0, 1, 0);
        //    Gl.glRotatef(orientation.z, 0, 0, 1);
        //    //            texture.Bind();
        //    //            model.Draw();
        //}

        //public void Update(TimeSpan dt)
        //{
        //}

        //public void MoveTo(Vec3f position, Vec3f orientation)
        //{
        //    this.position = position;
        //    this.orientation = orientation;
        //    Camera.Position = position;
        //}
    }
}
