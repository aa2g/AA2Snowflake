using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginLoader
{
    public enum PluginType
    {
        StartupScript,
        MenuStripButton,
        UserControl
    }

    public delegate void Method();

    public struct MenuStripMethod
    {
        public string Text;
        public Method Method;
    }

    public struct UserControlMethod
    {
        public string Text;
        public UserControl Method;
    }

    public interface IPlugin
    {
        string Name { get; }
        Version Version { get; }
        PluginType Type { get; }
        object Payload { get; }
    }
}
