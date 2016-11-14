using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using PNGNet;

namespace AA2Data
{
    public class AA2Card
    {
        public byte[] raw
        {
            get
            {
                byte[] buffer = new byte[_image.Length + Offset];
                using (MemoryStream mem = new MemoryStream()) //_image.Length + Offset
                using (BinaryWriter bw = new BinaryWriter(mem))
                {
                    //AA2Unlimited chunk
                    using (MemoryStream ms = new MemoryStream(_image))
                    using (MemoryStream ex = new MemoryStream())
                    {
                        var img = new PNGImage(ms);

                        bool containsChunk = img.Chunks.Any(x => x.Type == "aaUd");

                        if (!containsChunk && AA2UChunk != null)
                        {
                            img.Chunks.Insert(img.Chunks.FindIndex(x => x.Type == "IEND"), AA2UChunk);
                        }
                        else if (AA2UChunk != null)
                        {
                            int index = img.Chunks.FindIndex(x => x.Type == "aaUd");
                            img.Chunks.RemoveAt(index);
                            img.Chunks.Insert(index, AA2UChunk);
                        }
                        
                        img.Write(ex, false);

                        _image = ex.ToArray();
                    }

                    bw.Write(_image);
                    bw.Write(data.raw);
                    bw.Write(RosterLength);
                    bw.Write(_RosterImage);
                    bw.Write(Offset);

                    buffer = mem.ToArray();
                }
                return buffer;
            }
            set
            {
                using (MemoryStream mem = new MemoryStream(value))
                using (BinaryReader br = new BinaryReader(mem))
                {
                    br.BaseStream.Seek(-4, SeekOrigin.End);
                    int offset = br.ReadInt32();
                    br.BaseStream.Seek(0, SeekOrigin.Begin);
                    
                    _image = br.ReadBytes((int)br.BaseStream.Length - offset);

                    //AA2Unlimited chunk
                    using (MemoryStream ms = new MemoryStream(_image))
                    {
                        var img = new PNGImage(ms);

                        AA2UChunk = img.Chunks.DefaultIfEmpty(null).FirstOrDefault(x => x.Type == "aaUd");
                    }

                    data.raw = br.ReadBytes(3011);
                    int length = br.ReadInt32();
                    _RosterImage = br.ReadBytes(length);
                }
            }
        }
        
        private Chunk AA2UChunk = null;

        private byte[] _image;
        public Image Image
        {
            get
            {
                return Image.FromStream(new MemoryStream(_image));
            }
            set
            {
                using (MemoryStream str = new MemoryStream())
                {
                    value.Save(str, ImageFormat.Png);
                    str.Position = 0;
                    _image = new byte[str.Length];
                    str.Read(_image, 0, (int)str.Length);
                }

            }
        }
        public AA2Data data = new AA2Data();
        public Int32 RosterLength => _RosterImage.Length;

        private byte[] _RosterImage;
        public Image RosterImage
        {
            get
            {
                return Image.FromStream(new MemoryStream(_RosterImage));
            }
            set
            {
                using (MemoryStream str = new MemoryStream())
                {
                    value.Save(str, ImageFormat.Png);
                    str.Position = 0;
                    _RosterImage = new byte[str.Length];
                    str.Read(_RosterImage, 0, (int)str.Length);
                }
            }
        }

        public Int32 Offset => data.dataLength + 4 + _RosterImage.Length + 4;

        public AA2Card(byte[] data)
        {
            raw = data;
        }
    }
}
