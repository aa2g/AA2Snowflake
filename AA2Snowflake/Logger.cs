using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Snowflake
{
    public static class Logger
    {
        public static List<String> Log = new List<string>();

        public static void WriteLine(string item)
        {
            Log.Add(item);
        }

        public static string Export()
        {
            return string.Join("\r\n", Log.ToArray());
        }
    }
}
