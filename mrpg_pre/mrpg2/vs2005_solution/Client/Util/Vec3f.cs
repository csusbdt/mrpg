using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Client
{
    public struct Vec3f
    {
        public float x;
        public float y;
        public float z;

        public Vec3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public void Normalize()
        {
            float len = (float)Math.Sqrt(x * x + y * y + z * z);
            x /= len;
            y /= len;
            z /= len;
        }

        public override bool Equals(object o)
        {
            if (o is Vec3f)
            {
                Vec3f vec = (Vec3f)o;
                return x == vec.x && y == vec.y && z == vec.z;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            throw new Exception("Vec3f should not be used as a key in a hash table because it is mutable.");
            //return base.GetHashCode();
        }

        public static bool operator !=(Vec3f a, Vec3f b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public static Vec3f operator -(Vec3f a, Vec3f b)
        {
            return new Vec3f(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public Vec3f CrossProduct(Vec3f v)
        {
            return new Vec3f(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);
        }

        public static bool operator ==(Vec3f a, Vec3f b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static Vec3f Read(BinaryReader binaryReader)
        {
            Log.Write();
            Vec3f vec3f = new Vec3f();
            vec3f.x = binaryReader.ReadSingle();
            vec3f.y = binaryReader.ReadSingle();
            vec3f.z = binaryReader.ReadSingle();
            return vec3f;
        }

        public void Write(BinaryWriter binaryWriter)
        {
            binaryWriter.Write(x);
            binaryWriter.Write(y);
            binaryWriter.Write(z);
        }
    }
}
