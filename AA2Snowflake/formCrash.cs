using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AA2Snowflake
{
    public partial class formCrash : Form
    {
        public formCrash(string details, string dumpfile = "")
        {
            InitializeComponent();
            txtDetails.Text = details;
            if (dumpfile != "")
                lsbFiles.Items.Add(dumpfile);
            else
                lsbFiles.Items.Add("Error: There was no dump file produced.");
        }

        public void launchLink(string link)
        {
            System.Diagnostics.Process.Start(link);
        }

        private void linkGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchLink(@"https://github.com/aa2g/AA2Snowflake/issues/new");
        }

        private void linkaa2g_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchLink(@"https://boards.4chan.org/vg/catalog#s=aa2g");
        }

        private void linkHongfire_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            launchLink(@"http://www.hongfire.com/forum/showthread.php/453615-AA2Snowflake-v2-0");
        }
    }
}
