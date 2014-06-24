namespace UNIcast_Player
{
    partial class frmUNIcastPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUNIcastPlayer));
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.tglCrop = new UNIcast_Streamer.PictureToggle();
            this.ibtnHWA = new UNIcast_Streamer.ImageButton();
            this.ibtnAbout = new UNIcast_Streamer.ImageButton();
            this.ibtnStats = new UNIcast_Streamer.ImageButton();
            this.ibtnChat = new UNIcast_Streamer.ImageButton();
            this.ucVLC = new UNIcast_Player.ucVLC();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picLogo.Image = global::UNIcast_Player.Properties.Resources._256;
            this.picLogo.Location = new System.Drawing.Point(216, 116);
            this.picLogo.MaximumSize = new System.Drawing.Size(256, 256);
            this.picLogo.MinimumSize = new System.Drawing.Size(256, 256);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(256, 256);
            this.picLogo.TabIndex = 8;
            this.picLogo.TabStop = false;
            // 
            // tglCrop
            // 
            this.tglCrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.tglCrop.BackColor = System.Drawing.Color.Transparent;
            this.tglCrop.Image = global::UNIcast_Player.Properties.Resources.ico_crop;
            this.tglCrop.IsChecked = false;
            this.tglCrop.Location = new System.Drawing.Point(23, 440);
            this.tglCrop.MouseOverImage = global::UNIcast_Player.Properties.Resources.ico_crop_m;
            this.tglCrop.Name = "tglCrop";
            this.tglCrop.Size = new System.Drawing.Size(45, 45);
            this.tglCrop.TabIndex = 14;
            this.tglCrop.ToggleImage = global::UNIcast_Player.Properties.Resources.ico_crop_m;
            this.tglCrop.Toggle += new System.EventHandler(this.tglCrop_Toggle);
            // 
            // ibtnHWA
            // 
            this.ibtnHWA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ibtnHWA.BackColor = System.Drawing.Color.Transparent;
            this.ibtnHWA.ClickedImage = null;
            this.ibtnHWA.DisabledImage = null;
            this.ibtnHWA.Image = global::UNIcast_Player.Properties.Resources.ico_hwa;
            this.ibtnHWA.Location = new System.Drawing.Point(617, 457);
            this.ibtnHWA.MouseOverImage = null;
            this.ibtnHWA.Name = "ibtnHWA";
            this.ibtnHWA.Size = new System.Drawing.Size(35, 14);
            this.ibtnHWA.TabIndex = 13;
            // 
            // ibtnAbout
            // 
            this.ibtnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ibtnAbout.BackColor = System.Drawing.Color.Transparent;
            this.ibtnAbout.ClickedImage = null;
            this.ibtnAbout.DisabledImage = null;
            this.ibtnAbout.Image = ((System.Drawing.Image)(resources.GetObject("ibtnAbout.Image")));
            this.ibtnAbout.Location = new System.Drawing.Point(216, 440);
            this.ibtnAbout.MouseOverImage = ((System.Drawing.Image)(resources.GetObject("ibtnAbout.MouseOverImage")));
            this.ibtnAbout.Name = "ibtnAbout";
            this.ibtnAbout.Size = new System.Drawing.Size(45, 45);
            this.ibtnAbout.TabIndex = 12;
            this.ibtnAbout.Text = "imageButton4";
            // 
            // ibtnStats
            // 
            this.ibtnStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ibtnStats.BackColor = System.Drawing.Color.Transparent;
            this.ibtnStats.ClickedImage = null;
            this.ibtnStats.DisabledImage = null;
            this.ibtnStats.Image = ((System.Drawing.Image)(resources.GetObject("ibtnStats.Image")));
            this.ibtnStats.Location = new System.Drawing.Point(152, 440);
            this.ibtnStats.MouseOverImage = ((System.Drawing.Image)(resources.GetObject("ibtnStats.MouseOverImage")));
            this.ibtnStats.Name = "ibtnStats";
            this.ibtnStats.Size = new System.Drawing.Size(45, 45);
            this.ibtnStats.TabIndex = 11;
            this.ibtnStats.Text = "ibtnPerformance";
            this.ibtnStats.Click += new System.EventHandler(this.btnStats_Click);
            // 
            // ibtnChat
            // 
            this.ibtnChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ibtnChat.BackColor = System.Drawing.Color.Transparent;
            this.ibtnChat.ClickedImage = null;
            this.ibtnChat.DisabledImage = null;
            this.ibtnChat.Image = ((System.Drawing.Image)(resources.GetObject("ibtnChat.Image")));
            this.ibtnChat.Location = new System.Drawing.Point(88, 440);
            this.ibtnChat.MouseOverImage = ((System.Drawing.Image)(resources.GetObject("ibtnChat.MouseOverImage")));
            this.ibtnChat.Name = "ibtnChat";
            this.ibtnChat.Size = new System.Drawing.Size(45, 45);
            this.ibtnChat.TabIndex = 10;
            // 
            // ucVLC
            // 
            this.ucVLC.AllowDrop = true;
            this.ucVLC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucVLC.BackColor = System.Drawing.Color.Black;
            this.ucVLC.Location = new System.Drawing.Point(24, 64);
            this.ucVLC.Name = "ucVLC";
            this.ucVLC.Size = new System.Drawing.Size(640, 360);
            this.ucVLC.TabIndex = 6;
            // 
            // frmUNIcastPlayer
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 500);
            this.Controls.Add(this.tglCrop);
            this.Controls.Add(this.ibtnHWA);
            this.Controls.Add(this.ibtnAbout);
            this.Controls.Add(this.ibtnStats);
            this.Controls.Add(this.ibtnChat);
            this.Controls.Add(this.picLogo);
            this.Controls.Add(this.ucVLC);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmUNIcastPlayer";
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "UNIcast Player";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUNIcastPlayer_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ucVLC ucVLC;
        private System.Windows.Forms.PictureBox picLogo;
        private UNIcast_Streamer.ImageButton ibtnChat;
        private UNIcast_Streamer.ImageButton ibtnStats;
        private UNIcast_Streamer.ImageButton ibtnAbout;
        private UNIcast_Streamer.ImageButton ibtnHWA;
        private UNIcast_Streamer.PictureToggle tglCrop;
    }
}

