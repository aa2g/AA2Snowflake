using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AA2Data;

namespace AA2Snowflake
{
    public partial class formInfo : Form
    {
        public AA2Card card;

        public formInfo()
        {
            InitializeComponent();
        }

        public formInfo(AA2Card c)
        {
            InitializeComponent();
            card = c;
            updateInformation();
        }

        public void updateInformation()
        {
            txtLastName.Text = card.data.PROFILE_FAMILY_NAME;
            txtFirstName.Text = card.data.PROFILE_FIRST_NAME;
            chkRainbow.Checked = card.data.RAINBOW_CARD;
            cmbGender.SelectedIndex = card.data.PROFILE_GENDER;
            cmbPersonality.SelectedIndex = card.data.PROFILE_PERSONALITY_ID;
            txtBio.Text = card.data.PROFILE_BIO;
        }

        private void txtBio_TextChanged(object sender, EventArgs e)
        {
            if (card != null)
                card.data.PROFILE_BIO = txtBio.Text;
            lblBio.Text = "Bio: (" + Tools.ShiftJIS.GetByteCount(txtBio.Text.Replace("\0", "")) + " / 480 bytes used)";
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {
            if (card != null)
                card.data.PROFILE_FAMILY_NAME = txtLastName.Text;
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {
            if (card != null)
                card.data.PROFILE_FIRST_NAME = txtFirstName.Text;
        }

        private void cmbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (card != null)
                card.data.PROFILE_GENDER = (byte)cmbGender.SelectedIndex;
        }

        private void cmbPersonality_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (card != null)
                card.data.PROFILE_PERSONALITY_ID = (byte)cmbPersonality.SelectedIndex;
        }

        private void chkRainbow_CheckedChanged(object sender, EventArgs e)
        {
            if (card != null)
                card.data.RAINBOW_CARD = chkRainbow.Checked;
        }
    }
}
