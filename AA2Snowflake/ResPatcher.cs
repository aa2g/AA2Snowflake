using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace AA2Snowflake
{
    public static class ResPatcher
    {
        //thanks to aa2mpatcher anon
#warning change static byte values to dynamic sizes
        public static readonly byte[][] res1 = new byte[][] {
                new byte[] {0xb0, 0x04, 0x00, 0x00},
                new byte[] {0x60, 0x09, 0x00, 0x00},
                new byte[] {0x10, 0x0e, 0x00, 0x00}
            };
        public static readonly byte[][] res2 = new byte[][] {
                new byte[] {0x20, 0x03, 0x00, 0x00},
                new byte[] {0x40, 0x06, 0x00, 0x00},
                new byte[] {0x60, 0x09, 0x00, 0x00}
            };
        public static readonly byte[][] bg = new byte[][] {
                new byte[] {0x00, 0x00, 0x48, 0x44, 0x00, 0x00, 0x96, 0x44},
                new byte[] {0x00, 0x00, 0xc8, 0x44, 0x00, 0x00, 0x16, 0x45},
                new byte[] {0x00, 0x00, 0x16, 0x45, 0x00, 0x00, 0x61, 0x45}
            };
        public static readonly byte[][] sticker = new byte[][] {
                new byte[] {0x00, 0x00, 0x78, 0x43, 0x00, 0x00, 0x60, 0x43},
                new byte[] {0x00, 0x00, 0xf8, 0x43, 0x00, 0x00, 0xe0, 0x43},
                new byte[] {0x00, 0x00, 0x3a, 0x44, 0x00, 0x00, 0x28, 0x44}
            };
        public static readonly byte[][] n1x = new byte[][] {
                new byte[] {0x00, 0x00, 0x5c, 0x44},
                new byte[] {0x00, 0x00, 0xeb, 0x44},
                new byte[] {0x00, 0x00, 0x34, 0x45}
            };
        public static readonly byte[][] n1y = new byte[][] {
                new byte[] {0x00, 0x00, 0x80, 0x42},
                new byte[] {0x00, 0x00, 0x80, 0x42},
                new byte[] {0x00, 0x00, 0x00, 0x43}
            };
        public static readonly byte[][] n2x = new byte[][] {
                new byte[] {0x00, 0x00, 0x7a, 0x44},
                new byte[] {0x00, 0x00, 0xfa, 0x44},
                new byte[] {0x00, 0x80, 0x3b, 0x45}
            };
        public static readonly byte[][] n2y = new byte[][] {
                new byte[] {0x00, 0x00, 0x3c, 0x43},
                new byte[] {0x00, 0x00, 0x3c, 0x43},
                new byte[] {0x00, 0x00, 0xbc, 0x43}
            };

        public static readonly byte[] bug = new byte[]
        {
            0x68, 0x00, 0x02, 0x00, 0x00, 0x8D, 0x54, 0x24, 0x0C, 0x52, 0x56, 0xFF
        };
        public static readonly byte[] bugfix = new byte[]
        {
            0x68, 0x00, 0x01, 0x00, 0x00, 0x8D, 0x54, 0x24, 0x0C, 0x52, 0x56, 0xFF
        };

        public static void PatchResolution(Stream stream, Size cardres, RenderMode render, bool patchBug = false)
        {
            if (!IsCompatible(stream))
                return;
            int choice = (int)render;

            if (patchBug)
            {
                stream.Position = Tools.PatternAt(stream.ToByteArray(), bug).First();
                stream.Write(bugfix, 0, bugfix.Length);
            }

            switch (GetExeSignature(stream))
            {
                case ExeSignature.v142FP:
                    //no harm in doing this, so might as well make sure it's done
                    //to not have to deal with it when patching resolutions
                    stream.Position = 0x8cda;
                    stream.Write(new byte[] { 0x90, 0x90 }, 0, 2);

                    //patch card output resolution
                    stream.Position = 0x00123440 + 0x841;
                    stream.Write(BitConverter.GetBytes((uint)cardres.Height), 0, 4);
                    stream.Position = 0x00123445 + 0x841;
                    stream.Write(BitConverter.GetBytes((uint)cardres.Width), 0, 4);
                    
                    //first res
                    stream.Position = 0x00122b6a + 0x6f0;
                    stream.Write(res1[choice], 0, 4);
                    stream.Position++;
                    stream.Write(res2[choice], 0, 4);
                    //second res
                    stream.Position = 0x00122bb1 + 0x6f0;
                    stream.Write(res1[choice], 0, 4);
                    stream.Position++;
                    stream.Write(res2[choice], 0, 4);
                    //bg (float)
                    stream.Position = 0x0030f044 + 0x49F0;
                    stream.Write(bg[choice], 0, 8);
                    //personality sticker (float)
                    stream.Position = 0x0030f03c + 0x49F0;
                    stream.Write(sticker[choice], 0, 8);
                    //1st name x (float)
                    stream.Position = 0x0030f084 + 0x49ec;
                    stream.Write(n1x[choice], 0, 4);
                    //1st name y (float)
                    stream.Position = 0x0030ec70 + 0x49d0;
                    stream.Write(n1y[choice], 0, 4);
                    //2nd name x (float)
                    stream.Position = 0x0030ea94 + 0x49cc;
                    stream.Write(n2x[choice], 0, 4);
                    //2nd name y (float)
                    stream.Position = 0x0030f080 + 0x49ec;
                    stream.Write(n2y[choice], 0, 4);
                    break;
                default:
                    //no harm in doing this, so might as well make sure it's done
                    //to not have to deal with it when patching resolutions
                    stream.Position = 0x8cda;
                    stream.Write(new byte[] { 0x90, 0x90 }, 0, 2);

                    //patch card output resolution
                    stream.Position = 0x00123440;
                    stream.Write(BitConverter.GetBytes((uint)cardres.Height), 0, 4);
                    stream.Position = 0x00123445;
                    stream.Write(BitConverter.GetBytes((uint)cardres.Width), 0, 4);

                    //first res
                    stream.Position = 0x00122b6a;
                    stream.Write(res1[choice], 0, 4);
                    stream.Position++;
                    stream.Write(res2[choice], 0, 4);
                    //second res
                    stream.Position = 0x00122bb1;
                    stream.Write(res1[choice], 0, 4);
                    stream.Position++;
                    stream.Write(res2[choice], 0, 4);
                    //bg (float)
                    stream.Position = 0x0030f044;
                    stream.Write(bg[choice], 0, 8);
                    //personality sticker (float)
                    stream.Position = 0x0030f03c;
                    stream.Write(sticker[choice], 0, 8);
                    //1st name x (float)
                    stream.Position = 0x0030f084;
                    stream.Write(n1x[choice], 0, 4);
                    //1st name y (float)
                    stream.Position = 0x0030ec70;
                    stream.Write(n1y[choice], 0, 4);
                    //2nd name x (float)
                    stream.Position = 0x0030ea94;
                    stream.Write(n2x[choice], 0, 4);
                    //2nd name y (float)
                    stream.Position = 0x0030f080;
                    stream.Write(n2y[choice], 0, 4);
                    break;
            }
        }

        public static Size GetCardSize(Stream stream)
        {
            byte[] b = new byte[4];
            uint CardWidth, CardHeight;
            switch (GetExeSignature(stream))
            {
                case ExeSignature.v142FP:
                    stream.Position = 0x00123445 + 0x841;
                    stream.Read(b, 0, 4);
                    CardWidth = BitConverter.ToUInt32(b, 0);
                    stream.Position = 0x00123440 + 0x841;
                    stream.Read(b, 0, 4);
                    CardHeight = BitConverter.ToUInt32(b, 0);
                    return new Size((int)CardWidth, (int)CardHeight);
                default:
                    stream.Position = 0x00123445;
                    stream.Read(b, 0, 4);
                    CardWidth = BitConverter.ToUInt32(b, 0);
                    stream.Position = 0x00123440;
                    stream.Read(b, 0, 4);
                    CardHeight = BitConverter.ToUInt32(b, 0);
                    return new Size((int)CardWidth, (int)CardHeight);
            }
        }

        public static RenderMode GetCardRenderResolution(Stream stream)
        {
            List<byte[]> res = new List<byte[]>(res1);
            byte[] b = new byte[4];
            switch (GetExeSignature(stream))
            {
                case ExeSignature.v142FP:
                    stream.Position = 0x00122b6a + 0x6f0;
                    stream.Read(b, 0, 4);
                    switch (res.FindIndex(e => ByteArrayEqual(e, b)))
                    {
                        case 0: return RenderMode.r1200x800;
                        case 1: return RenderMode.r2400x1600;
                        case 2: return RenderMode.r3600x2400;
                        default: return RenderMode.rUnknown;
                    }
                default:
                    stream.Position = 0x00122b6a;
                    stream.Read(b, 0, 4);
                    switch (res.FindIndex(e => ByteArrayEqual(e, b)))
                    {
                        case 0: return RenderMode.r1200x800;
                        case 1: return RenderMode.r2400x1600;
                        case 2: return RenderMode.r3600x2400;
                        default: return RenderMode.rUnknown;
                    }
            }
        }

        private static bool ByteArrayEqual(byte[] a, byte[] b)
        {
            //for some reason this is the fastest way without p/invoke
            if (a.Length != b.Length) return false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }

        public static bool IsCompatible(Stream stream)
        {
            if (GetExeSignature(stream) != ExeSignature.Unknown)
                return true;

            byte[] check = new byte[8];
            byte[] valid;

            //res unlock signature
            stream.Position = 0x8cd2;
            stream.Read(check, 0, 8);
            valid = new byte[] { 0x10, 0x56, 0x8b, 0xf1, 0x80, 0x7e, 0x32, 0x01 };
            if (!ByteArrayEqual(check, valid)) { return false; }

            //card size signature
            stream.Position = 0x00123430;
            valid = new byte[] { 0xe6, 0x77, 0x00, 0x8d, 0x44, 0x24, 0x24, 0x50 };
            stream.Read(check, 0, 8);
            if (!ByteArrayEqual(check, valid)) { return false; }

            stream.Position = 0x00122b6a;
            stream.Read(check, 0, 4);
            if (!res1.Contains(check)) { return false; }
            stream.Position++;
            stream.Read(check, 0, 4);
            if (!res2.Contains(check)) { return false; }
            //second res
            stream.Position = 0x00122bb1;
            stream.Read(check, 0, 4);
            if (!res1.Contains(check)) { return false; }
            stream.Position++;
            stream.Read(check, 0, 4);
            if (!res2.Contains(check)) { return false; }
            //bg (float)
            stream.Position = 0x0030f044;
            stream.Read(check, 0, 8);
            if (!bg.Contains(check)) { return false; }
            //personality sticker (float)
            stream.Position = 0x0030f03c;
            stream.Read(check, 0, 8);
            if (!sticker.Contains(check)) { return false; }
            //1st name x (float)
            stream.Position = 0x0030f084;
            stream.Read(check, 0, 4);
            if (!n1x.Contains(check)) { return false; }
            //1st name y (float)
            stream.Position = 0x0030ec70;
            stream.Read(check, 0, 4);
            if (!n1y.Contains(check)) { return false; }
            //2nd name x (float)
            stream.Position = 0x0030ea94;
            stream.Read(check, 0, 4);
            if (!n2x.Contains(check)) { return false; }
            //2nd name y (float)
            stream.Position = 0x0030f080;
            stream.Read(check, 0, 4);
            if (!n2y.Contains(check)) { return false; }

            return true;
        }

        public static UInt32 CalculateSignature(Stream stream)
        {
            byte[] raw = new byte[512];
            stream.Position = 0;
            stream.Read(raw, 0, 512);
            return DamienG.Security.Cryptography.Crc32.Compute(raw);
        }

        public static string GetSignature(Stream stream)
        {
            var crc = CalculateSignature(stream);
            if (CRCs.ContainsKey(crc))
                return CRCs[crc] + " (0x" + crc.ToString("X5") + ")";
            else
                return "Unknown (0x" + crc.ToString("X5") + ")";
        }

        public static ExeSignature GetExeSignature(Stream stream)
        {
            var crc = CalculateSignature(stream);
            if (CRCs.ContainsKey(crc))
                return (ExeSignature)crc;
            else
                return ExeSignature.Unknown;
        }

        public static readonly Dictionary<UInt32, string> CRCs = new Dictionary<uint, string>
        {
            {0xAC02B960, "AA2Edit v1.0" },
            {0x4C14DD7C, "AA2Edit v1.0.1 FP v142" }
        };

        public enum ExeSignature: uint
        {
            v1 = 0xAC02B960,
            v142FP = 0x4C14DD7C,
            Unknown = 0x00000000
        }

        public enum RenderMode
        {
            rUnknown = -1,
            r1200x800 = 0,
            r2400x1600 = 1,
            r3600x2400 = 2
        }
    }
}
