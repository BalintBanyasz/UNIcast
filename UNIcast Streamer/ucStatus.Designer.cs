namespace UNIcast_Streamer
{
    partial class ucStatus
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucStatus));
            this.picStatusDevice = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.picStatusHdmi = new System.Windows.Forms.PictureBox();
            this.btnNext = new MetroFramework.Controls.MetroButton();
            this.lblStatusDevice = new MetroFramework.Controls.MetroLabel();
            this.lblStatusHdmi = new MetroFramework.Controls.MetroLabel();
            this.lblStatusMain = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picStatusDevice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStatusHdmi)).BeginInit();
            this.SuspendLayout();
            // 
            // picStatusDevice
            // 
            this.picStatusDevice.BackColor = System.Drawing.Color.White;
            this.picStatusDevice.Image = global::UNIcast_Streamer.Properties.Resources.ico_check;
            resources.ApplyResources(this.picStatusDevice, "picStatusDevice");
            this.picStatusDevice.Name = "picStatusDevice";
            this.picStatusDevice.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // picStatusHdmi
            // 
            this.picStatusHdmi.BackColor = System.Drawing.Color.White;
            this.picStatusHdmi.Image = global::UNIcast_Streamer.Properties.Resources.ico_error;
            resources.ApplyResources(this.picStatusHdmi, "picStatusHdmi");
            this.picStatusHdmi.Name = "picStatusHdmi";
            this.picStatusHdmi.TabStop = false;
            // 
            // btnNext
            // 
            resources.ApplyResources(this.btnNext, "btnNext");
            this.btnNext.Name = "btnNext";
            this.btnNext.Style = MetroFramework.MetroColorStyle.Red;
            // 
            // lblStatusDevice
            // 
            resources.ApplyResources(this.lblStatusDevice, "lblStatusDevice");
            this.lblStatusDevice.Name = "lblStatusDevice";
            // 
            // lblStatusHdmi
            // 
            resources.ApplyResources(this.lblStatusHdmi, "lblStatusHdmi");
            this.lblStatusHdmi.Name = "lblStatusHdmi";
            // 
            // lblStatusMain
            // 
            this.lblStatusMain.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.lblStatusMain, "lblStatusMain");
            this.lblStatusMain.Name = "lblStatusMain";
            // 
            // ucStatus
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblStatusMain);
            this.Controls.Add(this.lblStatusHdmi);
            this.Controls.Add(this.lblStatusDevice);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.picStatusHdmi);
            this.Controls.Add(this.picStatusDevice);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "ucStatus";
            ((System.ComponentModel.ISupportInitialize)(this.picStatusDevice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStatusHdmi)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        public MetroFramework.Controls.MetroButton btnNext;
        public System.Windows.Forms.PictureBox picStatusDevice;
        public System.Windows.Forms.PictureBox picStatusHdmi;
        public MetroFramework.Controls.MetroLabel lblStatusDevice;
        public MetroFramework.Controls.MetroLabel lblStatusHdmi;
        public System.Windows.Forms.Label lblStatusMain;
    }
}
