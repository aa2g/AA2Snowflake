using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Data
{
    public static class Extensions
    {
        public static byte[] ToByteArray(this Stream str)
        {
            str.Position = 0;
            byte[] buffer = new byte[str.Length];
            str.Read(buffer, 0, (int)str.Length);
            return buffer;
        }
    }
}
