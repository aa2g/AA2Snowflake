using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PoseViewerPlugin
{
    public partial class formViewer : Form
    {
        int Maximum = 0;

        public formViewer()
        {
            InitializeComponent();
        }

        private void formViewer_Load(object sender, EventArgs e)
        {
            cmbCategory.SelectedIndex = 0;
        }

        private void formViewer_Resize(object sender, EventArgs e)
        {
            imgPose.Size = new Size(this.Size.Width - 18, this.Size.Height - 91);
        }

        private void LoadImage(string Filename)
        {
            string location = System.Reflection.Assembly.GetAssembly(typeof(PoseViewerPlugin)).Location;
            string path = location.Remove(location.LastIndexOf('\\')) + "\\" + Filename;

            if (imgPose.Image != null)
                imgPose.Image.Dispose();

            imgPose.Image = Image.FromFile(path);
        }

        public void UpdateImage()
        {
            switch (cmbCategory.SelectedIndex)
            {
                case 0: //Poses
                    numIndex.Maximum = Maximum = 187;
                    LoadImage(@"\Previews\Body Positions " + numIndex.Value + ".jpg");
                    break;
                case 1: //Eye Open/Shut
                    numIndex.Maximum = Maximum = 8;
                    LoadImage(@"\Previews\Eyes Open-Shut " + numIndex.Value + ".jpg");
                    break;
                case 2: //Eyebrows
                    numIndex.Maximum = Maximum = 6;
                    LoadImage(@"\Previews\Eyebrows " + numIndex.Value + ".jpg");
                    break;
            }
        }

        private void numIndex_ValueChanged(object sender, EventArgs e)
        {
            UpdateImage();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            numIndex.Value = 0;
            UpdateImage();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            numIndex.Value = Math.Max(0, numIndex.Value - 1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            numIndex.Value = Math.Min(Maximum, numIndex.Value + 1);
        }
        private void formViewer_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    numIndex.Value = Math.Max(0, numIndex.Value - 1);
                    break;
                case Keys.Right:
                    numIndex.Value = Math.Max(Maximum, numIndex.Value + 1);
                    break;
            }
        }
    }
}
