using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SB3Utility;
using AA2Install;
using Microsoft.VisualBasic.FileIO;

namespace AA2Snowflake
{
    public static class Tools
    {
        public static Encoding ShiftJIS => Encoding.GetEncoding(932);

        public static MemoryStream GetStreamFromSubfile(IWriteFile iw)
        {
            MemoryStream mem = new MemoryStream();
            iw.WriteTo(mem);
            mem.Position = 0;
            return mem;
        }

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
            PP.jg2e00_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e00_00_00.pp", new ppFormat_AA2());
            PP.jg2e01_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e01_00_00.pp", new ppFormat_AA2());
            PP.jg2e06_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e06_00_00.pp", new ppFormat_AA2());
            PP.jg2p01_00_00 = new ppParser(Paths.AA2Play + @"\jg2p01_00_00.pp", new ppFormat_AA2());
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

        public static byte[] GetBytesFromStream(Stream str)
        {
            str.Position = 0;
            byte[] buffer = new byte[str.Length];
            str.Read(buffer, 0, (int)str.Length);
            return buffer;
        }

        public static MemSubfile ManipulateLst(IWriteFile lst, int column, string replacement)
        {
            byte[] buffer;
            using (MemoryStream mem = GetStreamFromSubfile(lst))
                buffer = GetBytesFromStream(mem);

            string slst = ShiftJIS.GetString(buffer);

            StringBuilder str = new StringBuilder();
            foreach (string line in slst.Split(new string[] { "\r\n" }, StringSplitOptions.None))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] set = line.Split('\t');
                set[column - 1] = replacement;

                str.Append(set.Aggregate((i, j) => i + "\t" + j) + "\r\n");
            }

            return new MemSubfile(new MemoryStream(ShiftJIS.GetBytes(str.ToString())), lst.Name);
        }
    }

    public static class PP
    {
        public static ppParser jg2e00_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e00_00_00.pp", new ppFormat_AA2());
        public static ppParser jg2e01_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e01_00_00.pp", new ppFormat_AA2());
        public static ppParser jg2e06_00_00 = new ppParser(Paths.AA2Edit + @"\jg2e06_00_00.pp", new ppFormat_AA2());
        public static ppParser jg2p01_00_00 = new ppParser(Paths.AA2Play + @"\jg2p01_00_00.pp", new ppFormat_AA2());
    }
}
