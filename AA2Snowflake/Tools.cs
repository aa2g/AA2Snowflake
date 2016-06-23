using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SB3Utility;
using AA2Install;
using Microsoft.VisualBasic.FileIO;
using System.Drawing;
using System.Xml.Serialization;

namespace AA2Snowflake
{
    public static class Tools
    {
        public static Encoding ShiftJIS => Encoding.GetEncoding(932);

        public static MemoryStream GetStreamFromFile(string file)
        {
            MemoryStream mem = new MemoryStream();
            using (FileStream b = new FileStream(file, FileMode.Open))
                b.CopyTo(mem);
            mem.Position = 0;
            return mem;
        }

        public static void RefreshPPs()
        {
            PP.jg2e00_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e00_00_00.pp");
            PP.jg2e01_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e01_00_00.pp");
            PP.jg2e06_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e06_00_00.pp");
            PP.jg2p01_00_00 = new ppParser(Paths.AA2Play + @"\jg2p01_00_00.pp");
        }

        public static void BackupFile(string file)
        {
            string name = file.Remove(0, file.LastIndexOf('\\'));
            FileSystem.CopyFile(file, Paths.BACKUP + name, UIOption.AllDialogs);
        }

        public static void RestoreFile(string file)
        {
            string name = file.Remove(0, file.LastIndexOf('\\'));
            FileSystem.CopyFile(Paths.BACKUP + name, file, UIOption.AllDialogs);
            RefreshPPs();
        }

        public static void DeleteBackup(string file)
        {
            string name = file.Remove(0, file.LastIndexOf('\\'));
            FileSystem.DeleteFile(Paths.BACKUP + name, UIOption.AllDialogs, RecycleOption.DeletePermanently);
        }

        public static Bitmap LoadTGA(Stream stream)
        {
            Bitmap bit;
            var sett = new ImageMagick.MagickReadSettings()
            {
                Format = ImageMagick.MagickFormat.Tga
            };

            using (var image = new ImageMagick.MagickImage(stream.ToByteArray(), sett))
                bit = image.ToBitmap();

            return bit;
        }

        public static Bitmap LoadTGA(string filename)
        {
            Bitmap bit;
            using (var image = new ImageMagick.MagickImage(filename))
                bit = image.ToBitmap();

            return bit;
        }

        public const double radian = 180 / Math.PI;
        public static float RadiansToDegrees(this float radians)
        {
            return (float)(radians * radian);
        }

        public static float DegreesToRadians(this float degrees)
        {
            return (float)(degrees / radian);
        }

        public static IEnumerable<int> PatternAt(byte[] source, byte[] pattern)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (source.Skip(i).Take(pattern.Length).SequenceEqual(pattern))
                {
                    yield return i;
                }
            }
        }
    }

    public static class PP
    {
        public static ppParser jg2e00_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e00_00_00.pp");
        public static ppParser jg2e01_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e01_00_00.pp");
        public static ppParser jg2e06_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e06_00_00.pp");
        public static ppParser jg2p01_00_00 = new ppParser(Paths.AA2Play + @"\jg2p01_00_00.pp");
    }
}
