﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SB3Utility;
using AA2Install;
using AA2Data;
using System.Drawing.Imaging;
using AA2Snowflake.Personalities;
using System.Text.RegularExpressions;
using PluginLoader;

namespace AA2Snowflake
{
    public partial class formMain : Form
    {
#warning use DevIL.NET instead of this shitty tga class
#warning add blush values
#warning Add personality name english translations patch

        SortedDictionary<int, IPersonality> Personalities;
        List<IPlugin> Plugins = new List<IPlugin>();
        #region Form
        public formLoad load = new formLoad();
        public formMain()
        {
            InitializeComponent();
        }

        public void ShowLoadingForm()
        {
            load.Show(this);

            load.Location = new Point(this.Location.X + (this.Width / 2) - (load.Width / 2), this.Location.Y + (this.Height / 2) - (load.Height / 2));
            this.Enabled = false;
        }

        public void HideLoadingForm()
        {
            Tools.RefreshPPs();
            load.Hide();
            this.Activate();
            this.Enabled = true;
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(Paths.PLUGINS))
                Directory.CreateDirectory(Paths.PLUGINS);

#if RELEASE
            if (!(Directory.Exists(Paths.AA2Play) && Directory.Exists(Paths.AA2Edit)))
            {
                MessageBox.Show("You don't seem to have AA2Play and/or AA2Edit (properly) installed.\nPlease install it, or use the registry fixer in AA2Install (if you've already installed it).", "AA2 not installed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
#endif

            //Version info in window title
            this.Text = "AA2Snowflake v" + formAbout.AssemblyVersion;

            //Initialisers
            Personalities = new SortedDictionary<int, IPersonality>(PersonalityFactory.GetAllPersonalities());
            info = new formInfo();

            Plugins.AddRange(PluginLoader.PluginLoader.LoadAllDLLs(Paths.PLUGINS + "\\"));

            foreach (IPlugin plugin in Plugins)
            {
                switch (plugin.Type)
                {
                    case PluginType.StartupScript:
                        Method script = (Method)plugin.Payload;

                        script.Invoke();
                        break;
                    case PluginType.MenuStripButton:
                        MenuStripMethod strip = (MenuStripMethod)plugin.Payload;

                        ToolStripMenuItem button = new ToolStripMenuItem();
                        button.Text = strip.Text;
                        button.AutoSize = true;
                        button.Click += new EventHandler((s, ev) =>
                        {
                            strip.Method.Invoke();
                        });

                        pluginsToolStripMenuItem.DropDownItems.Add(button);
                        break;
                    case PluginType.UserControl:
                        UserControlMethod control = (UserControlMethod)plugin.Payload;

                        TabPage page = new TabPage(control.Text);
                        page.Controls.Add(control.Method);
                        page.Controls[0].Dock = DockStyle.Fill;

                        tabControl1.TabPages.Add(page);
                        break;
                }
            }

            cmbBackground.SelectedIndex = 0;
            cmbBorder.SelectedIndex = 0;
            cmbCharacter.SelectedIndex = 0;
            cmbRoster.SelectedIndex = 0;
            cmbHeight33.SelectedIndex = 0;
            cmbMode33.SelectedIndex = 0;
            
            cmbPersonality32.Items.Clear();
            cmbPersonality33.Items.Clear();
            cmbPersonality34.Items.Clear();
            foreach (IPersonality p in Personalities.Values)
            {
                cmbPersonality32.Items.Add("(" + p.Slot.ToString("00") + ") " + p.Name);
                cmbPersonality33.Items.Add("(" + p.Slot.ToString("00") + ") " + p.Name);
                cmbPersonality34.Items.Add("(" + p.Slot.ToString("00") + ") " + p.Name);
            }
            cmbPersonality32.SelectedIndex = 0;
            cmbPersonality33.SelectedIndex = 0;
            cmbPersonality34.SelectedIndex = 0;

            UpdateWindowState();
        }
        #endregion
        #region Menu Bar
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void snowflakeGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/aa2g/AA2Snowflake/wiki/Snowflake-Guide-v3");
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (formAbout about = new formAbout())
                about.ShowDialog();
        }

        private void loadedPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> items = Plugins.Select(p => p.Name + " (" + p.Version.ToString() + ")").ToList();

            MessageBox.Show(items.Aggregate((i, j) => i + "\n" + j));
        }
        #endregion

        #region Background
        string backgroundpath;

        private void cmbBackground_SelectedIndexChanged(object sender, EventArgs e)
        {
            backgroundpath = null;
            
            using (var mem = PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_01_0" + cmbBackground.SelectedIndex.ToString() + ".bmp").ToStream())
            {
                if (imgBackground.Image != null)
                    imgBackground.Image.Dispose();

                imgBackground.Image = Image.FromStream(mem);
            }
        }

        private void btnLoadBG_Click(object sender, EventArgs e)
        {
            if (cmbBackground.SelectedIndex < 0)
                return;

            using (var file = new OpenFileDialog())
            {
                file.Filter = "Bitmap files (*.bmp)|*.bmp";
                file.Multiselect = false;
                
                if (file.ShowDialog() != DialogResult.Cancel)
                {
                    backgroundpath = file.FileName;
                    if (imgBackground.Image != null)
                        imgBackground.Image.Dispose();
                    
                    imgBackground.Image = Image.FromStream(Tools.GetStreamFromFile(file.FileName));
                }
            }
        }

        private void btnSaveBG_Click(object sender, EventArgs e)
        {
            if (!File.Exists(backgroundpath))
                return;
            
            var index = PP.jg2e06_00_00.Subfiles.IndexOf(PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_01_0" + cmbBackground.SelectedIndex.ToString() + ".bmp"));
            var sub = new Subfile(backgroundpath);
            sub.Name = "sp_04_01_0" + cmbBackground.SelectedIndex.ToString() + ".bmp";
            PP.jg2e06_00_00.Subfiles[index] = sub;
            var back = PP.jg2e06_00_00.WriteArchive(PP.jg2e06_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbBackground_SelectedIndexChanged(null, null);
        }

        private void btnRestoreBG_Click(object sender, EventArgs e)
        {
            if (cmbBackground.SelectedIndex < 0)
                return;

            var index = PP.jg2e06_00_00.Subfiles.IndexOf(PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_01_0" + cmbBackground.SelectedIndex.ToString() + ".bmp"));
            var sub = new Subfile(Paths.BACKUP + @"\sp_04_01_0" + cmbBackground.SelectedIndex.ToString() + ".bmp");
            PP.jg2e06_00_00.Subfiles[index] = sub;
            var back = PP.jg2e06_00_00.WriteArchive(PP.jg2e06_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbBackground_SelectedIndexChanged(null, null);
        }

        private void btnRestoreAllBG_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                var index = PP.jg2e06_00_00.Subfiles.IndexOf(PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_01_0" + i.ToString() + ".bmp"));
                var sub = new Subfile(Paths.BACKUP + @"\sp_04_01_0" + i.ToString() + ".bmp");
                PP.jg2e06_00_00.Subfiles[index] = sub;
            }
            var back = PP.jg2e06_00_00.WriteArchive(PP.jg2e06_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbBackground_SelectedIndexChanged(null, null);
        }
        #endregion
        #region Roster Background
        string rosterpath;

        private void cmbRoster_SelectedIndexChanged(object sender, EventArgs e)
        {
            rosterpath = null;

            using (var mem = PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_03_0" + cmbRoster.SelectedIndex.ToString() + ".bmp").ToStream())
            {
                if (imgRosterBackground.Image != null)
                    imgRosterBackground.Image.Dispose();

                imgRosterBackground.Image = Image.FromStream(mem);
            }
        }

        private void btnRosterLoad_Click(object sender, EventArgs e)
        {
            if (cmbRoster.SelectedIndex < 0)
                return;

            using (var file = new OpenFileDialog())
            {
                file.Filter = "Bitmap files (*.bmp)|*.bmp";
                file.Multiselect = false;

                if (file.ShowDialog() != DialogResult.Cancel)
                {
                    rosterpath = file.FileName;
                    if (imgRosterBackground.Image != null)
                        imgRosterBackground.Image.Dispose();

                    imgRosterBackground.Image = Image.FromStream(Tools.GetStreamFromFile(file.FileName));
                }
            }
        }

        private void btnRosterSave_Click(object sender, EventArgs e)
        {
            if (!File.Exists(rosterpath))
                return;

            var index = PP.jg2e06_00_00.Subfiles.IndexOf(PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_03_0" + cmbRoster.SelectedIndex.ToString() + ".bmp"));
            var sub = new Subfile(rosterpath);
            sub.Name = "sp_04_03_0" + cmbRoster.SelectedIndex.ToString() + ".bmp";
            PP.jg2e06_00_00.Subfiles[index] = sub;
            var back = PP.jg2e06_00_00.WriteArchive(PP.jg2e06_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbRoster_SelectedIndexChanged(null, null);
        }

        private void btnRosterRestore_Click(object sender, EventArgs e)
        {
            if (cmbRoster.SelectedIndex < 0)
                return;

            var index = PP.jg2e06_00_00.Subfiles.IndexOf(PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_03_0" + cmbRoster.SelectedIndex.ToString() + ".bmp"));
            var sub = new Subfile(Paths.BACKUP + @"\sp_04_03_0" + cmbRoster.SelectedIndex.ToString() + ".bmp");
            PP.jg2e06_00_00.Subfiles[index] = sub;
            var back = PP.jg2e06_00_00.WriteArchive(PP.jg2e06_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbRoster_SelectedIndexChanged(null, null);
        }

        private void btnRosterRestoreAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                var index = PP.jg2e06_00_00.Subfiles.IndexOf(PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_03_0" + i.ToString() + ".bmp"));
                var sub = new Subfile(Paths.BACKUP + @"\sp_04_03_0" + i.ToString() + ".bmp");
                PP.jg2e06_00_00.Subfiles[index] = sub;
            }
            var back = PP.jg2e06_00_00.WriteArchive(PP.jg2e06_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbRoster_SelectedIndexChanged(null, null);
        }
        #endregion
        #region Border
        string borderpath;
        bool render = false;

        private void cmbBorder_SelectedIndexChanged(object sender, EventArgs e)
        {
            borderpath = null;

            if (render)
                using (var mem = PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_02_0" + cmbBorder.SelectedIndex.ToString() + ".tga").ToStream())
                {
                    if (imgBorder.Image != null)
                        imgBorder.Image.Dispose();

                    imgBorder.Image = Tools.LoadTGA(mem);
                }
        }

        private void chkRenderBorder_CheckedChanged(object sender, EventArgs e)
        {
            render = chkRenderBorder.Checked;
        }

        private void btnLoadBorder_Click(object sender, EventArgs e)
        {
            if (cmbBorder.SelectedIndex < 0)
                return;

            using (var file = new OpenFileDialog())
            {
                file.Filter = "TGA image (*.tga)|*.tga";
                file.Multiselect = false;

                if (file.ShowDialog() != DialogResult.Cancel)
                {
                    borderpath = file.FileName;
                    if (render)
                    {
                        if (imgBorder.Image != null)
                            imgBorder.Image.Dispose();

                        imgBorder.Image = Tools.LoadTGA(file.FileName);
                    }
                }
            }
        }

        private void btnSetBorderBlank_Click(object sender, EventArgs e)
        {
            borderpath = Paths.BACKUP + @"\border_blank.tga";
            if (render)
            {
                if (imgBorder.Image != null)
                    imgBorder.Image.Dispose();

                imgBorder.Image = Tools.LoadTGA(borderpath);
            }
        }

        private void btnSaveBorder_Click(object sender, EventArgs e)
        {
            if (!File.Exists(borderpath))
                return;

            var index = PP.jg2e06_00_00.Subfiles.IndexOf(PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_02_0" + cmbBorder.SelectedIndex.ToString() + ".tga"));
            var sub = new Subfile(borderpath);
            sub.Name = "sp_04_02_0" + cmbBorder.SelectedIndex.ToString() + ".tga";
            PP.jg2e06_00_00.Subfiles[index] = sub;
            var back = PP.jg2e06_00_00.WriteArchive(PP.jg2e06_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbBorder_SelectedIndexChanged(null, null);
        }

        private void btnRestoreBorder_Click(object sender, EventArgs e)
        {
            if (cmbBorder.SelectedIndex < 0)
                return;

            var index = PP.jg2e06_00_00.Subfiles.IndexOf(PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_02_0" + cmbBorder.SelectedIndex.ToString() + ".tga"));
            var sub = new Subfile(Paths.BACKUP + @"\sp_04_02_0" + cmbBorder.SelectedIndex.ToString() + ".tga");
            PP.jg2e06_00_00.Subfiles[index] = sub;
            var back = PP.jg2e06_00_00.WriteArchive(PP.jg2e06_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbBorder_SelectedIndexChanged(null, null);
        }

        private void btnRestoreAllBorder_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                var index = PP.jg2e06_00_00.Subfiles.IndexOf(PP.jg2e06_00_00.Subfiles.First(pp => pp.Name == "sp_04_02_0" + i.ToString() + ".tga"));
                var sub = new Subfile(Paths.BACKUP + @"\sp_04_02_0" + i.ToString() + ".tga");
                PP.jg2e06_00_00.Subfiles[index] = sub;
            }
            var back = PP.jg2e06_00_00.WriteArchive(PP.jg2e06_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbBorder_SelectedIndexChanged(null, null);
        }
        #endregion
        #region Clothes
        string chrpath;

        private void cmbCharacter_SelectedIndexChanged(object sender, EventArgs e)
        {
            chrpath = null;

            using (var mem = PP.jg2e01_00_00.Subfiles.First(pp => pp.Name == "def0" + cmbCharacter.SelectedIndex.ToString() + ".png").ToStream())
            {
                if (imgCharacter.Image != null)
                    imgCharacter.Image.Dispose();

                imgCharacter.Image = Image.FromStream(mem);
            }
        }

        private void btnLoadCHR_Click(object sender, EventArgs e)
        {
            if (cmbCharacter.SelectedIndex < 0)
                return;

            using (var file = new OpenFileDialog())
            {
                file.Filter = "AA2 Card files (*.png)|*.png";
                file.Multiselect = false;

                if (file.ShowDialog() != DialogResult.Cancel)
                {
                    chrpath = file.FileName;
                    if (imgCharacter.Image != null)
                        imgCharacter.Image.Dispose();

                    imgCharacter.Image = Image.FromStream(Tools.GetStreamFromFile(file.FileName));
                }
            }
        }

        private void btnSaveCHR_Click(object sender, EventArgs e)
        {
            if (!File.Exists(chrpath))
                return;

            var index = PP.jg2e01_00_00.Subfiles.IndexOf(PP.jg2e01_00_00.Subfiles.First(pp => pp.Name == "def0" + cmbCharacter.SelectedIndex.ToString() + ".png"));
            var sub = new Subfile(chrpath);
            sub.Name = "def0" + cmbCharacter.SelectedIndex.ToString() + ".png";
            PP.jg2e01_00_00.Subfiles[index] = sub;
            var back = PP.jg2e01_00_00.WriteArchive(PP.jg2e01_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbCharacter_SelectedIndexChanged(null, null);
        }

        private void btnRestoreCHR_Click(object sender, EventArgs e)
        {
            if (cmbCharacter.SelectedIndex < 0)
                return;

            var index = PP.jg2e01_00_00.Subfiles.IndexOf(PP.jg2e01_00_00.Subfiles.First(pp => pp.Name == "def0" + cmbCharacter.SelectedIndex.ToString() + ".png"));
            var sub = new Subfile(Paths.BACKUP + @"\def0" + cmbCharacter.SelectedIndex.ToString() + ".png");
            PP.jg2e01_00_00.Subfiles[index] = sub;
            var back = PP.jg2e01_00_00.WriteArchive(PP.jg2e01_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbCharacter_SelectedIndexChanged(null, null);
        }

        private void btnRestoreAllCHR_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 2; i++)
            {
                var index = PP.jg2e01_00_00.Subfiles.IndexOf(PP.jg2e01_00_00.Subfiles.First(pp => pp.Name == "def0" + i.ToString() + ".png"));
                var sub = new Subfile(Paths.BACKUP + @"\def0" + i.ToString() + ".png");
                PP.jg2e01_00_00.Subfiles[index] = sub;
            }
            var back = PP.jg2e01_00_00.WriteArchive(PP.jg2e01_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            cmbCharacter_SelectedIndexChanged(null, null);
        }
        #endregion
        #region Poses
        #region 3.1
        private void btnBackup31_Click(object sender, EventArgs e)
        {
            Tools.BackupFile(Paths.AA2Edit + @"\jg2e01_00_00.pp");
        }

        private void btnRestore31_Click(object sender, EventArgs e)
        {
            Tools.RestoreFile(Paths.AA2Edit + @"\jg2e01_00_00.pp");
        }

        private void btnMove31_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you absolutely certain you want to move poses? Have you made a backup yet?", "Anti-fuckup dialog", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            int index;
            foreach (IWriteFile iw in PP.jg2p01_00_00.Subfiles)
            {
                if (iw.Name.StartsWith("HAK"))
                    iw.Name = "HAE" + iw.Name.Remove(0, 3);
                if (iw.Name.StartsWith("HSK"))
                    iw.Name = "HSE" + iw.Name.Remove(0, 3);

                index = PP.jg2e01_00_00.Subfiles.FindIndex(pp => pp.Name == iw.Name);
                if (index < 0)
                    PP.jg2e01_00_00.Subfiles.Add(iw);
                else
                    PP.jg2e01_00_00.Subfiles[index] = iw;
            }

            var back = PP.jg2e01_00_00.WriteArchive(PP.jg2e01_00_00.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }

            //This part was indicated as in snowflake guide v2 but I'm starting to question whether or not it's necessary

            /*index = PP.jg2e00_00_00.Subfiles.IndexOf(PP.jg2e00_00_00.Subfiles.First(pp => pp.Name == "jg2e_00_01_00_00.lst"));

            var lst = LSTFactory.LoadLST(PP.jg2e00_00_00.Subfiles[index], LSTMode.Default);
            lst.AA2EditPose 
            var sub = Tools.ManipulateLst(, 4, "51");
            sub.Name = "jg2e_00_01_00_00.lst";
            PP.jg2e00_00_00.Subfiles[index] = sub;
            back = PP.jg2e00_00_00.WriteArchive(PP.jg2e00_00_00.FilePath, false);
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }*/
            HideLoadingForm();
            MessageBox.Show("Finished!");
        }

        private void btnDelete31_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete your backup?", "Anti-fuckup dialog", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            Tools.DeleteBackup(Paths.AA2Edit + @"\jg2e01_00_00.pp");
        }
        #endregion
        #region 3.2
        public void GenerateLSTBackups()
        {
            SerializableDictionary<string, byte[]> backup = new SerializableDictionary<string, byte[]>();
            foreach (IPersonality personality in Personalities.Values)
                using (MemoryStream ms = new MemoryStream())
                {
                    personality.GetLst().WriteTo(ms);
                    backup[personality.LSTLocation] = ms.ToByteArray();
                }
            File.WriteAllText(Paths.BACKUP + "\\lstbackup.xml", backup.SerializeObject());
        }

        private void btnSet32_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Paths.BACKUP + "\\lstbackup.xml"))
                if (MessageBox.Show("You haven't created a backup, so restoration is not possible.\nAre you sure you want to continue?", "Unable to restore", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                    return;

            IPersonality personality = Personalities.ElementAt(cmbPersonality32.SelectedIndex).Value; //i've rewritten this to change only 1 personality since you don't want to rewrite 5gb of files everytime you change poses
            ppParser pp = personality.GetLstPP();
            IWriteFile sub = personality.GetLst();
            int index = pp.Subfiles.FindIndex(x => x.Name == sub.Name);

            var lst = LSTFactory.LoadLST(personality);

            if (chkPose32.Checked)
                lst.AA2EditPose = (int)numPose32.Value;
            if (chkEyebrow32.Checked)
                lst.AA2EditEyebrow = (int)numEyebrow32.Value;
            if (chkEyeOS32.Checked)
                lst.AA2EditEye = (int)numEye32.Value;
            if (chkEye32.Checked)
                lst.AA2EditEyeOS = (int)numEyeOS32.Value;
            if (chkMouth32.Checked)
                lst.AA2EditMouth = (int)numMouth32.Value;

            //lst.WriteValue(1, "1");

            //sub = Tools.ManipulateLst(sub, 4, "51");
            //sub.Name = "jg2e_00_01_00_00.lst";
            pp.Subfiles[index] = lst.ToSubfile(sub.Name);
            var back = pp.WriteArchive(pp.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
        }

        private void btnBackupAll32_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to do this? You may be overwriting an already existing backup.", "AA2Snowflake", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            BackgroundWorker back = new BackgroundWorker();
            back.DoWork += (o, ev) =>
            {
                GenerateLSTBackups();
            };
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
        }

        private void btnRestore32_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Paths.BACKUP + "\\lstbackup.xml"))
            {
                MessageBox.Show("You haven't created a backup, so restoration is not possible.", "Unable to restore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SerializableDictionary<string, byte[]> backup = File.ReadAllText(Paths.BACKUP + "\\lstbackup.xml").DeserializeObject<SerializableDictionary<string, byte[]>>();
            
            IPersonality personality = Personalities.ElementAt(cmbPersonality32.SelectedIndex).Value;
            ppParser pp = personality.GetLstPP();
            IWriteFile sub = personality.GetLst();
            var index = pp.Subfiles.IndexOf(pp.Subfiles.First(iw => iw.Name == sub.Name));
            sub = new MemSubfile(backup[personality.LSTLocation], sub.Name);
            pp.Subfiles[index] = sub;
            var back = pp.WriteArchive(pp.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
        }

        private void chkPose32_CheckedChanged(object sender, EventArgs e)
        {
            numPose32.Enabled = chkPose32.Checked;
        }

        private void chkEyebrow32_CheckedChanged(object sender, EventArgs e)
        {
            numEyebrow32.Enabled = chkEyebrow32.Checked;
        }

        private void chkEyeOS32_CheckedChanged(object sender, EventArgs e)
        {
            numEyeOS32.Enabled = chkEyeOS32.Checked;
        }

        private void chkEye32_CheckedChanged(object sender, EventArgs e)
        {
            numEye32.Enabled = chkEye32.Checked;
        }

        private void chkMouth32_CheckedChanged(object sender, EventArgs e)
        {
            numMouth32.Enabled = chkMouth32.Checked;
        }
        #endregion
        #region 3.3
        public void GenerateICFBackups()
        {
            SerializableDictionary<string, byte[]> backup = new SerializableDictionary<string, byte[]>();
            Regex regex = new Regex(@"e\d{2}_\d{2}_\d{2}\.[Ii][Cc][Ff]");
            foreach (IPersonality personality in Personalities.Values)
                foreach (IWriteFile icf in personality.GetIcfPP().Subfiles.Where(iw => iw.Name.ToLower().EndsWith(".icf")))
                    if (regex.IsMatch(icf.Name))
                        using (MemoryStream ms = new MemoryStream())
                        {
                            icf.WriteTo(ms);
                            backup[icf.Name.ToLower()] = ms.ToByteArray();
                        }
            File.WriteAllText(Paths.BACKUP + "\\icfbackup.xml", backup.SerializeObject());
        }

        public void LoadICF()
        {
            if (cmbHeight33.SelectedIndex < 0 || cmbMode33.SelectedIndex < 0 || cmbPersonality33.SelectedIndex < 0)
                return;

            IPersonality personality = Personalities.ElementAt(cmbPersonality33.SelectedIndex).Value;
            Logger.WriteLine("[LoadICF]");
            Logger.WriteLine(personality.Name + " : " + personality.ID);
            int height = personality.Gender == Gender.Female ? cmbHeight33.SelectedIndex : cmbHeight33.SelectedIndex + 1;
            string name = "e" + cmbMode33.SelectedIndex.ToString("00") + "_" + personality.Slot.ToString("00") + "_" + height.ToString("00") + ".ICF";
            Logger.WriteLine(name);
            ppParser pp = personality.GetIcfPP();
            Logger.WriteLine(pp.FilePath);
            Logger.WriteLine("Contains " + pp.Subfiles.Count);
            Logger.WriteLine(string.Join("\r\n", pp.Subfiles.Where(p => p.Name.ToLower().EndsWith("icf"))));
            IWriteFile sub = pp.Subfiles.First(iw => iw.Name.ToLower() == name.ToLower());
            ICF icf;
            using (MemoryStream mem = sub.ToStream())
                icf = new ICF(mem);

            txtRotX.Text = icf.Rotation.X.RadiansToDegrees().ToString();
            txtRotY.Text = icf.Rotation.Y.RadiansToDegrees().ToString();
            txtRotZ.Text = icf.Rotation.Z.RadiansToDegrees().ToString();
            txtZoom.Text = icf.Zoom.ToString();
            txtFOV.Text = icf.FOV.ToString();
            txtPosX.Text = icf.Position.X.ToString();
            txtPosY.Text = icf.Position.Y.ToString();
            txtPosZ.Text = icf.Position.Z.ToString();
        }

        private void cmbPersonality33_SelectedIndexChanged(object sender, EventArgs e)
        {
            IPersonality personality = Personalities.ElementAt(cmbPersonality33.SelectedIndex).Value;
            cmbHeight33.Items.Clear();
            if (personality.Gender == Gender.Female)
            {
                cmbHeight33.Items.Add("(00) Short");
                cmbHeight33.Items.Add("(01) Normal");
                cmbHeight33.Items.Add("(02) Tall");
                cmbHeight33.SelectedIndex = 0;
            }
            else
            {
                cmbHeight33.Items.Add("(01) Delicate/Standard");
                cmbHeight33.Items.Add("(02) Tall/Fat");
                cmbHeight33.SelectedIndex = 1;
            }
            LoadICF();
        }

        private void cmbHeight33_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadICF();
        }

        private void cmbMode33_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadICF();
        }

        private void btnSet33_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Paths.BACKUP + "\\icfbackup.xml"))
                if (MessageBox.Show("You haven't created a backup, so restoration is not possible.\nAre you sure you want to continue?", "Unable to restore", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                    return;

            IPersonality personality = Personalities.ElementAt(cmbPersonality33.SelectedIndex).Value;
            Logger.WriteLine("[3.3 Set]");
            Logger.WriteLine(personality.Name);
            int height = personality.Gender == Gender.Female ? cmbHeight33.SelectedIndex : cmbHeight33.SelectedIndex + 1;
            string name = "e" + cmbMode33.SelectedIndex.ToString("00") + "_" + personality.Slot.ToString("00") + "_" + height.ToString("00") + ".ICF";
            Logger.WriteLine(name);
            ppParser pp = personality.GetIcfPP();
            Logger.WriteLine(pp.FilePath);
            Logger.WriteLine("Contains " + pp.Subfiles.Count);
            Logger.WriteLine(string.Join("\r\n", pp.Subfiles.Where(p => p.Name.ToLower().EndsWith("icf"))));
            IWriteFile sub = pp.Subfiles.First(iw => iw.Name.ToLower() == name.ToLower());
            int index = pp.Subfiles.IndexOf(sub);

            var icf = new ICF();
            try
            {
                icf.Rotation.X = float.Parse(txtRotX.Text).DegreesToRadians();
                icf.Rotation.Y = float.Parse(txtRotY.Text).DegreesToRadians();
                icf.Rotation.Z = float.Parse(txtRotZ.Text).DegreesToRadians();
                icf.Zoom = float.Parse(txtZoom.Text);
                icf.FOV = float.Parse(txtFOV.Text);
                icf.Position.X = float.Parse(txtPosX.Text);
                icf.Position.Y = float.Parse(txtPosY.Text);
                icf.Position.Z = float.Parse(txtPosZ.Text);
            }
            catch (Exception ex) when (ex is FormatException || ex is ArgumentNullException)
            {
                MessageBox.Show("Error: One or more of the values are not valid number(s).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            sub = new MemSubfile(icf.Export(), sub.Name);
            pp.Subfiles[index] = sub;
            var back = pp.WriteArchive(pp.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
        }

        private void btnRestore33_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Paths.BACKUP + "\\icfbackup.xml"))
            {
                MessageBox.Show("You haven't created a backup, so restoration is not possible.", "Unable to restore", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SerializableDictionary<string, byte[]> backup = File.ReadAllText(Paths.BACKUP + "\\icfbackup.xml").DeserializeObject<SerializableDictionary<string, byte[]>>();

            IPersonality personality = Personalities.ElementAt(cmbPersonality33.SelectedIndex).Value;
            ppParser pp = personality.GetIcfPP();
            string name = "e" + cmbMode33.SelectedIndex.ToString("00") + "_" + personality.Slot.ToString("00") + "_" + cmbHeight33.SelectedIndex.ToString("00") + ".ICF";
            var sub = pp.Subfiles.First(iw => iw.Name.ToLower() == name.ToLower());
            var index = pp.Subfiles.IndexOf(pp.Subfiles.First(iw => iw.Name.ToLower() == name.ToLower()));
            sub = new MemSubfile(backup[name.ToLower()], name);
            pp.Subfiles[index] = sub;
            var back = pp.WriteArchive(pp.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
            LoadICF();
        }

        private void btnBackupAll33_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to do this? You may be overwriting an already existing backup.", "AA2Snowflake", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            BackgroundWorker back = new BackgroundWorker();
            back.DoWork += (o, ev) =>
            {
                GenerateICFBackups();
            };
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
        }
        #endregion
        #region 3.4
        public void GenerateTGABackups()
        {
            Regex regex = new Regex(@"sp_04_00_\d{2}\.[Tt][Gg][Aa]");
            foreach (IPersonality personality in Personalities.Values)
                if (personality.Custom)
                    foreach (IWriteFile tga in personality.GetIcfPP().Subfiles.Where(iw => iw.Name.ToLower().EndsWith(".tga")))
                        if (regex.IsMatch(tga.Name))
                            using (MemoryStream ms = new MemoryStream())
                            {
                                tga.WriteTo(ms);
                                File.WriteAllBytes(Paths.NATURE + "\\" + tga.Name, ms.ToByteArray());
                            }
        }
        private void btnBackupAll34_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to do this? You may be overwriting an already existing backup.", "AA2Snowflake", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            BackgroundWorker back = new BackgroundWorker();
            back.DoWork += (o, ev) =>
            {
                GenerateTGABackups();
            };
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
        }

        private void btnSetBlank34_Click(object sender, EventArgs e)
        {
            IPersonality personality = Personalities.ElementAt(cmbPersonality34.SelectedIndex).Value;

            string name = "sp_04_00_" + personality.Slot.ToString("00") + ".tga";

            if (!File.Exists(Paths.NATURE + "\\" + name))
                if (MessageBox.Show("You haven't created a backup (for this personality), so restoration is not possible.\nAre you sure you want to continue?", "Unable to restore", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                    return;

            ppParser pp = personality.GetIcfPP();
            if (!personality.Custom)
                pp = PP.jg2e06_00_00;

            IWriteFile sub = pp.Subfiles.First(iw => iw.Name.ToLower() == name.ToLower());
            int index = pp.Subfiles.IndexOf(sub);

            sub = new Subfile(Paths.BACKUP + "\\personality_blank.tga", sub.Name);
            pp.Subfiles[index] = sub;
            var back = pp.WriteArchive(pp.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
        }

        private void btnRestore34_Click(object sender, EventArgs e)
        {
            IPersonality personality = Personalities.ElementAt(cmbPersonality34.SelectedIndex).Value;

            string name = "sp_04_00_" + personality.Slot.ToString("00") + ".tga";

            if (!File.Exists(Paths.NATURE + "\\" + name))
            {
                MessageBox.Show("You haven't created a backup (for this personality), so restoration is not possible.", "Unable to restore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ppParser pp = personality.GetIcfPP();
            if (!personality.Custom)
                pp = PP.jg2e06_00_00;

            IWriteFile sub = pp.Subfiles.First(iw => iw.Name.ToLower() == name.ToLower());
            int index = pp.Subfiles.IndexOf(sub);

            sub = new Subfile(Paths.NATURE + "\\" + name, sub.Name);
            pp.Subfiles[index] = sub;
            var back = pp.WriteArchive(pp.FilePath, false);
            ShowLoadingForm();
            back.RunWorkerAsync();
            while (back.IsBusy)
            {
                Application.DoEvents();
            }
            HideLoadingForm();
            MessageBox.Show("Finished!");
        }
        #endregion
        #endregion
        #region Card Face
        private formInfo info;
        private string cardpath;
        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            using (var file = new OpenFileDialog())
            {
                file.Filter = "AA2 Card files (*.png)|*.png";
                file.Multiselect = false;
                if (file.ShowDialog() == DialogResult.OK)
                {
                    info.card = new AA2Card(File.ReadAllBytes(file.FileName));
                    info.updateInformation();
                    info.Show();
                    cardpath = file.FileName;
                    UpdateWindowState();
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if (cardpath != null && info.card != null)
            {
                bool tryAgain = false;
                do
                {
                    tryAgain = false;
                    try
                    {
                        File.WriteAllBytes(cardpath, info.card.raw);
                        MessageBox.Show("Saved!");
                    }
                    catch (IOException)
                    {
                        var result = MessageBox.Show("Cannot save the card to it's original position!\nIs the card currently being accessed/viewed by AA2Edit/another program?", "Cannot save", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                        if (result == DialogResult.Retry)
                            tryAgain = true;
                    }
                } while (tryAgain);
            }
            UpdateWindowState();
        }

        private void saveAsToolStripButton_Click(object sender, EventArgs e)
        {
            if (cardpath != null && info.card != null)
                using (var file = new SaveFileDialog())
                {
                    file.Filter = "AA2 Card files (*.png)|*.png";
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(file.FileName, info.card.raw);
                        cardpath = file.FileName;
                        MessageBox.Show("Saved!");
                        UpdateWindowState();
                    }
                }
        }

        private void replaceCardFaceToolStripButton_Click(object sender, EventArgs e)
        {
            if (cardpath != null && info.card != null)
                using (var file = new OpenFileDialog())
                {
                    file.Filter = "PNG (*.png)|*.png";
                    file.Multiselect = false;
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            var tempcard = new AA2Card(File.ReadAllBytes(file.FileName));
                            info.card.Image = tempcard.Image;
                        }
                        catch
                        {
                            info.card.Image = Image.FromFile(file.FileName);
                        }
                        UpdateWindowState();
                    }
                }
        }

        private void UpdateWindowState()
        {
            if (File.Exists(cardpath))
            {
                saveToolStripButton.Enabled = true;
                saveAsToolStripButton.Enabled = true;
                replaceCardFaceToolStripButton.Enabled = true;
                replaceCardRosterToolStripButton.Enabled = true;
                replaceCardRosterFromCardToolStripButton.Enabled = true;
            }
            else
            {
                saveToolStripButton.Enabled = false;
                saveAsToolStripButton.Enabled = false;
                replaceCardFaceToolStripButton.Enabled = false;
                replaceCardRosterToolStripButton.Enabled = false;
                replaceCardRosterFromCardToolStripButton.Enabled = false;
            }
            Size size;
            if (info.card != null)
            {
                imgRoster.Image = info.card.RosterImage;
                imgCard.Image = info.card.Image;
                size = info.card.Image.Size;
            }
            else
                size = new Size(0, 0);

            lblDimensions.Text = "[" + size.Width + ", " + size.Height + "]";
        }

        private void replaceCardRosterToolStripButton_Click(object sender, EventArgs e)
        {
            if (cardpath != null && info.card != null)
                using (var file = new OpenFileDialog())
                {
                    file.Filter = "PNG (*.png)|*.png";
                    file.Multiselect = false;
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        info.card.RosterImage = Image.FromFile(file.FileName);
                    }
                }
            UpdateWindowState();
        }

        private void replaceCardRosterFromCardToolStripButton_Click(object sender, EventArgs e)
        {
            if (cardpath != null && info.card != null)
                using (var file = new OpenFileDialog())
                {
                    file.Filter = "PNG (*.png)|*.png";
                    file.Multiselect = false;
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        var tempcard = new AA2Card(File.ReadAllBytes(file.FileName));
                        info.card.RosterImage = tempcard.RosterImage;
                    }
                }
            UpdateWindowState();
        }
        #endregion

        #region Patcher
        private Size cardsize = new Size(200, 300);
        private string exefile;

        private void btnPatcherLoad_Click(object sender, EventArgs e)
        {
            using (var file = new OpenFileDialog())
            {
                file.Filter = "Executable file (*.exe)|*.exe";
                file.Multiselect = false;
                if (file.ShowDialog() == DialogResult.OK)
                    exefile = file.FileName;
                else
                    return;
            }
            
            using (var exe = new FileStream(exefile, FileMode.Open))
            {
                lblPatcherSignature.Text = "Detected signature: " + ResPatcher.GetSignature(exe);
                lblPatcherCompatible.Text = "Is compatible? " + (ResPatcher.IsCompatible(exe) ? "Yes" : "No");
                var size = ResPatcher.GetCardSize(exe);
                lblPatcherCurrentCardSize.Text = "Current card size: " + size.Width.ToString() + "x" + size.Height.ToString();
                lblPatcherCurrentRenderMode.Text = "Current render mode: " + Enum.GetName(typeof(ResPatcher.RenderMode), ResPatcher.GetCardRenderResolution(exe)).Remove(0, 1);
            }
        }

        private void trkCardSize_Scroll(object sender, EventArgs e)
        {
            cardsize = new Size(200 + (trkCardSize.Value * 60), 300 + (trkCardSize.Value * 90));
            lblPatcherOutputSize.Text = "Output card size: " + cardsize.Width.ToString() + "x" + cardsize.Height.ToString();
        }

        private void btnPatch_Click(object sender, EventArgs e)
        {
            if (File.Exists(exefile))
            {
                using (var exe = new FileStream(exefile, FileMode.Open))
                    if (ResPatcher.IsCompatible(exe))
                        if (radio1x.Checked)
                            ResPatcher.PatchResolution(exe, cardsize, ResPatcher.RenderMode.r1200x800, chkPatcherBug.Checked);
                        else if (radio2x.Checked)
                            ResPatcher.PatchResolution(exe, cardsize, ResPatcher.RenderMode.r2400x1600, chkPatcherBug.Checked);
                        else if (radio3x.Checked)
                            ResPatcher.PatchResolution(exe, cardsize, ResPatcher.RenderMode.r3600x2400, chkPatcherBug.Checked);

                MessageBox.Show("Finished!");
            }
        }
        #endregion
    }
}
