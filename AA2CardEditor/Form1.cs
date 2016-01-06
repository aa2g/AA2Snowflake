using System;
using System.Reflection;
using System.Windows.Forms;

namespace AA2CardEditor
{
    public partial class Form1 : Form
    {
        private static readonly string ApplicationName = GetApplicationName();

        private AA2Card card = new AA2Card();

        public Form1()
        {
            InitializeComponent();
            UpdateWindowState();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenCardFile();
            UpdateWindowState();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCardFile();
            UpdateWindowState();
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            OpenCardFile();
            UpdateWindowState();
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveCardFile();
            UpdateWindowState();
        }

        private void saveAsToolStripButton_Click(object sender, EventArgs e)
        {
            SaveAsCardFile();
            UpdateWindowState();
        }

        private void replaceCardFaceToolStripButton_Click(object sender, EventArgs e)
        {
            ReplaceCardFace();
            UpdateWindowState();
        }

        private static string GetApplicationName()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type type = typeof(AssemblyProductAttribute);
            object[] customAttributes = assembly.GetCustomAttributes(type, false);
            return ((AssemblyProductAttribute)customAttributes[0]).Product;
        }

        private void OpenCardFile()
        {
            cardOpenFileDialog.FileName = "";
            if (cardOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    card.ReadCardFile(cardOpenFileDialog.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show("The card file could not be opened.");
                }
            }
        }

        private void SaveCardFile()
        {
            try
            {
                card.Save();
            }
            catch (Exception e)
            {
                MessageBox.Show("The card file could not be saved.");
            }
        }

        private void SaveAsCardFile()
        {
            saveFileDialog.FileName = "";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    card.Save(saveFileDialog.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show("The card file could not be saved.");
                }
            }
        }

        private void ReplaceCardFace()
        {
            faceOpenFileDialog.FileName = "";
            if (faceOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    card.ReplaceFaceImage(faceOpenFileDialog.FileName);
                }
                catch (Exception e)
                {
                    MessageBox.Show("The card face image could not be replaced.");
                }
            }
        }
        
        private void UpdateWindowState()
        {
            UpdateWindowCaption();
            UpdateToolstripState();
            UpdateCardFaceView();
        }

        private void UpdateWindowCaption()
        {
            Text = ApplicationName;
            if (card.FileName != "")
            {
                if (card.IsDirty)
                {
                    Text = card.FileName + "* - " + Text;
                }
                else
                {
                    Text = card.FileName + " - " + Text;
                }
            }
        }

        private void UpdateToolstripState()
        {
            if (card.FileName != "")
            {
                saveToolStripButton.Enabled = true;
                saveAsToolStripButton.Enabled = true;
                replaceCardFaceToolStripButton.Enabled = true;
            }
            else
            {
                saveToolStripButton.Enabled = false;
                saveAsToolStripButton.Enabled = false;
                replaceCardFaceToolStripButton.Enabled = false;
            }
        }

        private void UpdateCardFaceView()
        {
            cardFacePictureBox.Image = card.FaceImage;
        }
    }
}
