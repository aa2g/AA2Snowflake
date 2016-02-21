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
        /// 7Zip standalone location.
        /// </summary>
        public static string _7Za
        {
            get
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    return Environment.CurrentDirectory + @"\tools\7z\x64\7za.exe";
                } 
                else 
                {
                    return Environment.CurrentDirectory + @"\tools\7z\x86\7za.exe";
                }
            }
        }
        /// <summary>
        /// AA2Play data install location.
        /// </summary>
        public static string AA2Play
        {
            get
            {
                /*if (bool.Parse(Configuration.ReadSetting("AA2PLAY") ?? "False"))
                {
                    return Configuration.ReadSetting("AA2PLAY_Path");
                } 
                else 
                {*/
                    object dir = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\illusion\AA2Play", "INSTALLDIR", "NULL");
                    return dir + @"\data";
                //}
            }
        }
        /// <summary>
        /// AA2Edit data install location.
        /// </summary>
        public static string AA2Edit
        {
            get
            {
                /*if (bool.Parse(Configuration.ReadSetting("AA2EDIT") ?? "False"))
                {
                    return Configuration.ReadSetting("AA2EDIT_Path");
                } 
                else 
                {*/
                    object dir = Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\illusion\AA2Edit", "INSTALLDIR", "NULL");
                    return dir + @"\data";
                //}
            }
        }
        public static string Nature => Environment.CurrentDirectory + @"\nature";
        public static string BACKUP => Environment.CurrentDirectory + @"\backup";

    }
}
