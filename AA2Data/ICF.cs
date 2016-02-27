using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AA2Data
{
    public struct Point3F
    {
        public float X;
        public float Y;
        public float Z;

        public Point3F(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
    public struct ICF
    {
        public Point3F Rotation;
        public float Zoom;
        public float FOV;
        public Point3F Position;

        public ICF(Stream icfstream)
        {
            using (BinaryReader b = new BinaryReader(icfstream))
            {
                Rotation = new Point3F(b.ReadSingle(), b.ReadSingle(), b.ReadSingle());
                Zoom = b.ReadSingle();
                FOV = b.ReadSingle();
                Position = new Point3F(b.ReadSingle(), b.ReadSingle(), b.ReadSingle());
            }
        }

        public byte[] Export()
        {
            byte[] output;
            using (MemoryStream mem = new MemoryStream())
            using (BinaryWriter b = new BinaryWriter(mem))
            {
                b.Write(Rotation.X);
                b.Write(Rotation.Y);
                b.Write(Rotation.Z);
                b.Write(Zoom);
                b.Write(FOV);
                b.Write(Position.X);
                b.Write(Position.Y);
                b.Write(Position.Z);
                output = mem.ToByteArray();
            }
            return output;
        }
    }
}
