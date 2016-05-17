using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace AA2Data
{
    public static partial class Tools
    {
        public static byte[] ExTransform(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = (byte)((buffer[i] ^ 0xFF) & 0xFF);
            return buffer;
        }

        public static Encoding ShiftJIS => Encoding.GetEncoding(932);
    }

    class AA2Reader : IDisposable
    {
        private BinaryReader br;

        public AA2Reader(Stream stream)
        {
            br = new BinaryReader(stream);
        }

        public AA2Reader(byte[] card)
        {
            br = new BinaryReader(new MemoryStream(card));
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            br.BaseStream.Seek(offset, origin);
        }

        public bool ReadBool()
        {
            if (br.ReadByte() == 0)
                return false;
            else
                return true;
        }

        public byte ReadByte() => br.ReadByte();

        public Int16 ReadInt16() => br.ReadInt16();

        public Int32 ReadInt32() => br.ReadInt32();

        public Color ReadColor() => Color.FromArgb(br.ReadInt32());

        public string ReadString(int length) => Tools.ShiftJIS.GetString(br.ReadBytes(length));

        public string ReadStringEx(int length) => Tools.ShiftJIS.GetString(Tools.ExTransform(br.ReadBytes(length)));

        public void Dispose()
        {
            br.Dispose();
        }
    }

    class AA2Writer : IDisposable
    {
        private BinaryWriter bw;

        public static implicit operator byte[] (AA2Writer x) => x.GetBytes();
        public static implicit operator Stream (AA2Writer x) => x.bw.BaseStream;
        public static implicit operator AA2Writer(Stream x) => new AA2Writer(x);

        public AA2Writer(Stream stream)
        {
            bw = new BinaryWriter(stream);
        }

        public AA2Writer()
        {
            bw = new BinaryWriter(new MemoryStream());
        }

        public byte[] GetBytes()
        {
            long pos = bw.BaseStream.Position;
            bw.BaseStream.Position = 0;
            byte[] buffer = new byte[bw.BaseStream.Length];
            bw.BaseStream.Read(buffer, 0, (int)bw.BaseStream.Length);
            bw.BaseStream.Position = pos;
            return buffer;
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            bw.BaseStream.Seek(offset, origin);
        }

        public void WriteBool(bool value)
        {
            if (value)
                bw.Write((byte)1);
            else
                bw.Write((byte)0);
        }

        public void WriteByte(byte value) => bw.Write(value);

        public void WriteInt16(Int16 value) => bw.Write(value);

        public void WriteInt32(Int32 value) => bw.Write(value);

        public void WriteColor(Color value) => bw.Write(value.ToArgb());

        public void WriteString(string value, int length)
        {
            byte[] buffer = new byte[length];
            byte[] str = Tools.ShiftJIS.GetBytes(value);
            for (int i = 0; i < str.Length; i++)
                buffer[i] = str[i];
            bw.Write(buffer);
        }

        public void WriteStringEx(string value, int length)
        {
            byte[] buffer = new byte[length];
            byte[] str = Tools.ShiftJIS.GetBytes(value);
            for (int i = 0; i < str.Length; i++)
                buffer[i] = str[i];
            buffer = Tools.ExTransform(buffer);
            bw.Write(buffer);
        }

        public void Dispose()
        {
            bw.Dispose();
        }
    }
}
