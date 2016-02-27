namespace AA2Snowflake
{
    partial class formInfo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formInfo));
            this.label1 = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbGender = new System.Windows.Forms.ComboBox();
            this.cmbPersonality = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chkRainbow = new System.Windows.Forms.CheckBox();
            this.lblBio = new System.Windows.Forms.Label();
            this.txtBio = new System.Windows.Forms.TextBox();
            this.btnCloth = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Name:";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(78, 36);
            this.txtFirstName.MaxLength = 130;
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(194, 20);
            this.txtFirstName.TabIndex = 1;
            this.txtFirstName.TextChanged += new System.EventHandler(this.txtFirstName_TextChanged);
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(78, 12);
            this.txtLastName.MaxLength = 130;
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(194, 20);
            this.txtLastName.TabIndex = 3;
            this.txtLastName.TextChanged += new System.EventHandler(this.txtLastName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Last Name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Gender:";
            // 
            // cmbGender
            // 
            this.cmbGender.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGender.FormattingEnabled = true;
            this.cmbGender.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.cmbGender.Location = new System.Drawing.Point(78, 60);
            this.cmbGender.Name = "cmbGender";
            this.cmbGender.Size = new System.Drawing.Size(98, 21);
            this.cmbGender.TabIndex = 5;
            this.cmbGender.SelectedIndexChanged += new System.EventHandler(this.cmbGender_SelectedIndexChanged);
            // 
            // cmbPersonality
            // 
            this.cmbPersonality.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPersonality.FormattingEnabled = true;
            this.cmbPersonality.Items.AddRange(new object[] {
            "(00) Lively",
            "(01) Delicate",
            "(02) Cheerful",
            "(03) Quiet",
            "(04) Playful",
            "(05) Frisky",
            "(06) Kind",
            "(07) Joyful",
            "(08) Ordinary",
            "(09) Irritated",
            "(10) Harsh",
            "(11) Sweet",
            "(12) Creepy",
            "(13) Reserved",
            "(14) Dignified",
            "(15) Aloof",
            "(16) Smart",
            "(17) Genuine",
            "(18) Mature",
            "(19) Lazy",
            "(20) Manly",
            "(21) Gentle",
            "(22) Positive",
            "(23) Otaku",
            "(24) Savage",
            "(25) Cadet",
            "(26) Caring",
            "(27) Schemer",
            "(28) Carefree",
            "(29) Warm"});
            this.cmbPersonality.Location = new System.Drawing.Point(78, 87);
            this.cmbPersonality.Name = "cmbPersonality";
            this.cmbPersonality.Size = new System.Drawing.Size(98, 21);
            this.cmbPersonality.TabIndex = 7;
            this.cmbPersonality.SelectedIndexChanged += new System.EventHandler(this.cmbPersonality_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Personality:";
            // 
            // chkRainbow
            // 
            this.chkRainbow.AutoSize = true;
            this.chkRainbow.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkRainbow.Location = new System.Drawing.Point(198, 62);
            this.chkRainbow.Name = "chkRainbow";
            this.chkRainbow.Size = new System.Drawing.Size(74, 17);
            this.chkRainbow.TabIndex = 8;
            this.chkRainbow.Text = "Rainbow?";
            this.chkRainbow.UseVisualStyleBackColor = true;
            this.chkRainbow.CheckedChanged += new System.EventHandler(this.chkRainbow_CheckedChanged);
            // 
            // lblBio
            // 
            this.lblBio.AutoSize = true;
            this.lblBio.Location = new System.Drawing.Point(12, 118);
            this.lblBio.Name = "lblBio";
            this.lblBio.Size = new System.Drawing.Size(123, 13);
            this.lblBio.TabIndex = 9;
            this.lblBio.Text = "Bio: (0 / 480 bytes used)";
            // 
            // txtBio
            // 
            this.txtBio.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtBio.Location = new System.Drawing.Point(0, 134);
            this.txtBio.MaxLength = 480;
            this.txtBio.Multiline = true;
            this.txtBio.Name = "txtBio";
            this.txtBio.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBio.Size = new System.Drawing.Size(284, 127);
            this.txtBio.TabIndex = 10;
            this.txtBio.TextChanged += new System.EventHandler(this.txtBio_TextChanged);
            // 
            // btnCloth
            // 
            this.btnCloth.Location = new System.Drawing.Point(197, 85);
            this.btnCloth.Name = "btnCloth";
            this.btnCloth.Size = new System.Drawing.Size(75, 23);
            this.btnCloth.TabIndex = 11;
            this.btnCloth.Text = "Load .cloth";
            this.btnCloth.UseVisualStyleBackColor = true;
            this.btnCloth.Click += new System.EventHandler(this.btnCloth_Click);
            // 
            // formInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnCloth);
            this.Controls.Add(this.txtBio);
            this.Controls.Add(this.lblBio);
            this.Controls.Add(this.chkRainbow);
            this.Controls.Add(this.cmbPersonality);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbGender);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "formInfo";
            this.Text = "Character Information";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formInfo_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblBio;
        public System.Windows.Forms.TextBox txtFirstName;
        public System.Windows.Forms.TextBox txtLastName;
        public System.Windows.Forms.ComboBox cmbGender;
        public System.Windows.Forms.ComboBox cmbPersonality;
        public System.Windows.Forms.CheckBox chkRainbow;
        public System.Windows.Forms.TextBox txtBio;
        private System.Windows.Forms.Button btnCloth;
    }
}