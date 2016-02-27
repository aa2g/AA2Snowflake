using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginLoader;
using System.Windows.Forms;
using AA2Data;
using System.IO;

namespace BatchClothPlugin
{
    public class BatchClothPlugin : IPlugin
    {
        public string Name => "Batch Cloth Loader";

        public Version Version => new Version("1.0.0.0");

        public PluginType Type => PluginType.MenuStripButton;

        public object Payload => new MenuStripMethod(Name, () =>
                                               {
                                                   string[] filenames;
                                                   using (OpenFileDialog file = new OpenFileDialog())
                                                   {
                                                       file.Filter = "AA2 Card files (*.png)|*.png";
                                                       file.Multiselect = true;

                                                       if (file.ShowDialog() != DialogResult.OK ||
                                                            file.FileNames.Length < 1)
                                                           return;

                                                       filenames = file.FileNames;
                                                   }

                                                   AA2Cloth cloth;
                                                   
                                                   using (OpenFileDialog file = new OpenFileDialog())
                                                   {
                                                       file.Filter = "Cloth file (*.cloth)|*.cloth";
                                                       file.Multiselect = false;

                                                       if (file.ShowDialog() != DialogResult.OK)
                                                           return;

                                                       cloth = new AA2Cloth(File.ReadAllBytes(file.FileName));
                                                   }

                                                   foreach (string path in filenames)
                                                   {
                                                       AA2Card card = new AA2Card(File.ReadAllBytes(path));
                                                       card.data.CLOTH_UNIFORM = cloth;
                                                       File.WriteAllBytes(path, card.raw);
                                                   }

                                                   MessageBox.Show("Done!");
                                               });
    }
}
