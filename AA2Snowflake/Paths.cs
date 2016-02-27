using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA2Install
{
    public static class Paths
    {
        /// <summary>
        /// AA2Play data install location.
        /// </summary>
        public static string AA2Play
        {
            get
            {
                object dir = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\illusion\AA2Play", "INSTALLDIR", "NULL");
                return dir + @"\data";
            }
        }
        /// <summary>
        /// AA2Edit data install location.
        /// </summary>
        public static string AA2Edit
        {
            get
            {
                object dir = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\illusion\AA2Edit", "INSTALLDIR", "NULL");
                return dir + @"\data";
            }
        }
        public static string BACKUP => Environment.CurrentDirectory + @"\backup";
        public static string PLUGINS => Environment.CurrentDirectory + @"\plugins";
    }
}
