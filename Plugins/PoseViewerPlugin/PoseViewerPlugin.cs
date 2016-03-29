using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginLoader;
using System.Windows.Forms;

namespace PoseViewerPlugin
{
    public class PoseViewerPlugin : IPlugin
    {
        public string Name => "Pose Viewer";

        public Version Version => new Version("0.9.0.0");

        public PluginType Type => PluginType.MenuStripButton;

        public object Payload => new MenuStripMethod(Name, () =>
        {
            var form = new formViewer();
            form.Show();
        });
    }
}
