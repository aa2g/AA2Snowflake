namespace AA2Snowflake
{
    partial class formCrash
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formCrash));
            this.button1 = new System.Windows.Forms.Button();
            this.txtDetails = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lsbFiles = new System.Windows.Forms.ListBox();
            this.linkHongfire = new System.Windows.Forms.LinkLabel();
            this.linkaa2g = new System.Windows.Forms.LinkLabel();
            this.linkGithub = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(227, 257);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtDetails
            // 
            this.txtDetails.BackColor = System.Drawing.Color.White;
            this.txtDetails.Location = new System.Drawing.Point(17, 137);
            this.txtDetails.Multiline = true;
            this.txtDetails.Name = "txtDetails";
            this.txtDetails.ReadOnly = true;
            this.txtDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetails.Size = new System.Drawing.Size(494, 114);
            this.txtDetails.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(17, 116);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(494, 18);
            this.label2.TabIndex = 15;
            this.label2.Text = "Details:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lsbFiles
            // 
            this.lsbFiles.FormattingEnabled = true;
            this.lsbFiles.Location = new System.Drawing.Point(120, 82);
            this.lsbFiles.Name = "lsbFiles";
            this.lsbFiles.Size = new System.Drawing.Size(391, 30);
            this.lsbFiles.TabIndex = 14;
            // 
            // linkHongfire
            // 
            this.linkHongfire.AutoSize = true;
            this.linkHongfire.Location = new System.Drawing.Point(423, 51);
            this.linkHongfire.Name = "linkHongfire";
            this.linkHongfire.Size = new System.Drawing.Size(45, 13);
            this.linkHongfire.TabIndex = 13;
            this.linkHongfire.TabStop = true;
            this.linkHongfire.Text = "hongfire";
            this.linkHongfire.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkHongfire_LinkClicked);
            // 
            // linkaa2g
            // 
            this.linkaa2g.AutoSize = true;
            this.linkaa2g.Location = new System.Drawing.Point(374, 51);
            this.linkaa2g.Name = "linkaa2g";
            this.linkaa2g.Size = new System.Drawing.Size(41, 13);
            this.linkaa2g.TabIndex = 12;
            this.linkaa2g.TabStop = true;
            this.linkaa2g.Text = "/aa2g/";
            this.linkaa2g.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkaa2g_LinkClicked);
            // 
            // linkGithub
            // 
            this.linkGithub.AutoSize = true;
            this.linkGithub.Location = new System.Drawing.Point(251, 51);
            this.linkGithub.Name = "linkGithub";
            this.linkGithub.Size = new System.Drawing.Size(63, 13);
            this.linkGithub.TabIndex = 11;
            this.linkGithub.TabStop = true;
            this.linkGithub.Text = "github issue";
            this.linkGithub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGithub_LinkClicked);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(117, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(394, 93);
            this.label1.TabIndex = 9;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AA2Snowflake.Properties.Resources.burn;
            this.pictureBox1.Location = new System.Drawing.Point(8, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(106, 102);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // formCrash
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 289);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDetails);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lsbFiles);
            this.Controls.Add(this.linkHongfire);
            this.Controls.Add(this.linkaa2g);
            this.Controls.Add(this.linkGithub);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formCrash";
            this.Text = "Fatal Error";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtDetails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lsbFiles;
        private System.Windows.Forms.LinkLabel linkHongfire;
        private System.Windows.Forms.LinkLabel linkaa2g;
        private System.Windows.Forms.LinkLabel linkGithub;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
    }
}