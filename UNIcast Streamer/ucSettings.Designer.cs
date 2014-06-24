namespace UNIcast_Streamer
{
    partial class ucSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSettings));
            this.ratings = new UNIcast_Streamer.Ratings();
            this.pnlRecord = new MetroFramework.Controls.MetroPanel();
            this.btnChooseOutputFolder = new MetroFramework.Controls.MetroButton();
            this.txtOutputFolder = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel8 = new MetroFramework.Controls.MetroLabel();
            this.txtTags = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
            this.txtDescription = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.txtTitle = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
            this.radPrivate = new MetroFramework.Controls.MetroRadioButton();
            this.radUnlisted = new MetroFramework.Controls.MetroRadioButton();
            this.radPublic = new MetroFramework.Controls.MetroRadioButton();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroButton2 = new MetroFramework.Controls.MetroButton();
            this.metroButton1 = new MetroFramework.Controls.MetroButton();
            this.txtLecturer = new MetroFramework.Controls.MetroTextBox();
            this.txtSubject = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.chkRecord = new MetroFramework.Controls.MetroCheckBox();
            this.btnStart = new MetroFramework.Controls.MetroButton();
            this.pnlRecord.SuspendLayout();
            this.SuspendLayout();
            // 
            // ratings
            // 
            this.ratings.BackColor = System.Drawing.Color.Transparent;
            this.ratings.BottomMargin = 2;
            this.ratings.EmptyColor = System.Drawing.Color.Silver;
            this.ratings.EmptyImage = null;
            this.ratings.FilledImage = null;
            this.ratings.ForeColor = System.Drawing.Color.Gray;
            this.ratings.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(78)))), ((int)(((byte)(114)))));
            this.ratings.HoverImage = null;
            this.ratings.ImageCount = 5;
            this.ratings.ImageSpacing = 8;
            this.ratings.ImageToDraw = 1;
            this.ratings.LeftMargin = 4;
            resources.ApplyResources(this.ratings, "ratings");
            this.ratings.Name = "ratings";
            this.ratings.RightMargin = 4;
            this.ratings.SelectedColor = System.Drawing.Color.Red;
            this.ratings.SelectedItem = 3;
            this.ratings.Style = MetroFramework.MetroColorStyle.Red;
            this.ratings.TopMargin = 2;
            // 
            // pnlRecord
            // 
            this.pnlRecord.Controls.Add(this.btnChooseOutputFolder);
            this.pnlRecord.Controls.Add(this.txtOutputFolder);
            this.pnlRecord.Controls.Add(this.metroLabel8);
            this.pnlRecord.Controls.Add(this.txtTags);
            this.pnlRecord.Controls.Add(this.metroLabel7);
            this.pnlRecord.Controls.Add(this.txtDescription);
            this.pnlRecord.Controls.Add(this.metroLabel5);
            this.pnlRecord.Controls.Add(this.txtTitle);
            this.pnlRecord.Controls.Add(this.metroLabel4);
            this.pnlRecord.Controls.Add(this.metroLabel6);
            this.pnlRecord.Controls.Add(this.radPrivate);
            this.pnlRecord.Controls.Add(this.radUnlisted);
            this.pnlRecord.Controls.Add(this.radPublic);
            resources.ApplyResources(this.pnlRecord, "pnlRecord");
            this.pnlRecord.HorizontalScrollbarBarColor = true;
            this.pnlRecord.HorizontalScrollbarHighlightOnWheel = false;
            this.pnlRecord.HorizontalScrollbarSize = 10;
            this.pnlRecord.Name = "pnlRecord";
            this.pnlRecord.VerticalScrollbarBarColor = true;
            this.pnlRecord.VerticalScrollbarHighlightOnWheel = false;
            this.pnlRecord.VerticalScrollbarSize = 10;
            // 
            // btnChooseOutputFolder
            // 
            resources.ApplyResources(this.btnChooseOutputFolder, "btnChooseOutputFolder");
            this.btnChooseOutputFolder.Name = "btnChooseOutputFolder";
            this.btnChooseOutputFolder.Click += new System.EventHandler(this.btnChooseOutputFolder_Click);
            // 
            // txtOutputFolder
            // 
            resources.ApplyResources(this.txtOutputFolder, "txtOutputFolder");
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Style = MetroFramework.MetroColorStyle.Red;
            this.txtOutputFolder.TabStop = false;
            // 
            // metroLabel8
            // 
            resources.ApplyResources(this.metroLabel8, "metroLabel8");
            this.metroLabel8.Name = "metroLabel8";
            // 
            // txtTags
            // 
            resources.ApplyResources(this.txtTags, "txtTags");
            this.txtTags.Multiline = true;
            this.txtTags.Name = "txtTags";
            this.txtTags.Style = MetroFramework.MetroColorStyle.Red;
            // 
            // metroLabel7
            // 
            resources.ApplyResources(this.metroLabel7, "metroLabel7");
            this.metroLabel7.Name = "metroLabel7";
            // 
            // txtDescription
            // 
            resources.ApplyResources(this.txtDescription, "txtDescription");
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Style = MetroFramework.MetroColorStyle.Red;
            // 
            // metroLabel5
            // 
            resources.ApplyResources(this.metroLabel5, "metroLabel5");
            this.metroLabel5.Name = "metroLabel5";
            // 
            // txtTitle
            // 
            resources.ApplyResources(this.txtTitle, "txtTitle");
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Style = MetroFramework.MetroColorStyle.Red;
            // 
            // metroLabel4
            // 
            resources.ApplyResources(this.metroLabel4, "metroLabel4");
            this.metroLabel4.Name = "metroLabel4";
            // 
            // metroLabel6
            // 
            resources.ApplyResources(this.metroLabel6, "metroLabel6");
            this.metroLabel6.Name = "metroLabel6";
            // 
            // radPrivate
            // 
            resources.ApplyResources(this.radPrivate, "radPrivate");
            this.radPrivate.Name = "radPrivate";
            this.radPrivate.Style = MetroFramework.MetroColorStyle.Red;
            this.radPrivate.TabStop = true;
            this.radPrivate.Tag = "";
            this.radPrivate.UseVisualStyleBackColor = true;
            // 
            // radUnlisted
            // 
            resources.ApplyResources(this.radUnlisted, "radUnlisted");
            this.radUnlisted.Name = "radUnlisted";
            this.radUnlisted.Style = MetroFramework.MetroColorStyle.Red;
            this.radUnlisted.TabStop = true;
            this.radUnlisted.Tag = "";
            this.radUnlisted.UseVisualStyleBackColor = true;
            // 
            // radPublic
            // 
            resources.ApplyResources(this.radPublic, "radPublic");
            this.radPublic.Checked = true;
            this.radPublic.Name = "radPublic";
            this.radPublic.Style = MetroFramework.MetroColorStyle.Red;
            this.radPublic.TabStop = true;
            this.radPublic.Tag = "";
            this.radPublic.UseVisualStyleBackColor = true;
            // 
            // metroLabel3
            // 
            resources.ApplyResources(this.metroLabel3, "metroLabel3");
            this.metroLabel3.Name = "metroLabel3";
            // 
            // metroButton2
            // 
            resources.ApplyResources(this.metroButton2, "metroButton2");
            this.metroButton2.Name = "metroButton2";
            // 
            // metroButton1
            // 
            resources.ApplyResources(this.metroButton1, "metroButton1");
            this.metroButton1.Name = "metroButton1";
            // 
            // txtLecturer
            // 
            resources.ApplyResources(this.txtLecturer, "txtLecturer");
            this.txtLecturer.Name = "txtLecturer";
            this.txtLecturer.Style = MetroFramework.MetroColorStyle.Red;
            // 
            // txtSubject
            // 
            resources.ApplyResources(this.txtSubject, "txtSubject");
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Style = MetroFramework.MetroColorStyle.Red;
            // 
            // metroLabel2
            // 
            resources.ApplyResources(this.metroLabel2, "metroLabel2");
            this.metroLabel2.Name = "metroLabel2";
            // 
            // metroLabel1
            // 
            resources.ApplyResources(this.metroLabel1, "metroLabel1");
            this.metroLabel1.Name = "metroLabel1";
            // 
            // chkRecord
            // 
            resources.ApplyResources(this.chkRecord, "chkRecord");
            this.chkRecord.Name = "chkRecord";
            this.chkRecord.Style = MetroFramework.MetroColorStyle.Red;
            this.chkRecord.UseVisualStyleBackColor = true;
            this.chkRecord.CheckedChanged += new System.EventHandler(this.chkRecord_CheckedChanged);
            // 
            // btnStart
            // 
            resources.ApplyResources(this.btnStart, "btnStart");
            this.btnStart.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnStart.Name = "btnStart";
            this.btnStart.Style = MetroFramework.MetroColorStyle.Red;
            // 
            // ucSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.ratings);
            this.Controls.Add(this.pnlRecord);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroButton2);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.txtLecturer);
            this.Controls.Add(this.txtSubject);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.chkRecord);
            this.Name = "ucSettings";
            this.pnlRecord.ResumeLayout(false);
            this.pnlRecord.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public Ratings ratings;
        private MetroFramework.Controls.MetroPanel pnlRecord;
        private MetroFramework.Controls.MetroButton btnChooseOutputFolder;
        public MetroFramework.Controls.MetroTextBox txtOutputFolder;
        private MetroFramework.Controls.MetroLabel metroLabel8;
        public MetroFramework.Controls.MetroTextBox txtTags;
        private MetroFramework.Controls.MetroLabel metroLabel7;
        public MetroFramework.Controls.MetroTextBox txtDescription;
        private MetroFramework.Controls.MetroLabel metroLabel5;
        public MetroFramework.Controls.MetroTextBox txtTitle;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel6;
        public MetroFramework.Controls.MetroRadioButton radPrivate;
        public MetroFramework.Controls.MetroRadioButton radUnlisted;
        public MetroFramework.Controls.MetroRadioButton radPublic;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroButton metroButton2;
        private MetroFramework.Controls.MetroButton metroButton1;
        public MetroFramework.Controls.MetroTextBox txtLecturer;
        public MetroFramework.Controls.MetroTextBox txtSubject;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        public MetroFramework.Controls.MetroCheckBox chkRecord;
        private MetroFramework.Controls.MetroButton btnStart;
    }
}
