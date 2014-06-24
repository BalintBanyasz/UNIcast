namespace UNIcast_Streamer
{
    partial class frmMessenger
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
            this.btnSend = new MetroFramework.Controls.MetroButton();
            this.metroTextBox = new MetroFramework.Controls.MetroTextBox();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSend.Location = new System.Drawing.Point(106, 274);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(113, 33);
            this.btnSend.Style = MetroFramework.MetroColorStyle.Red;
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // metroTextBox
            // 
            this.metroTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroTextBox.Location = new System.Drawing.Point(24, 63);
            this.metroTextBox.Multiline = true;
            this.metroTextBox.Name = "metroTextBox";
            this.metroTextBox.Size = new System.Drawing.Size(278, 193);
            this.metroTextBox.Style = MetroFramework.MetroColorStyle.Red;
            this.metroTextBox.TabIndex = 1;
            // 
            // frmMessenger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 330);
            this.Controls.Add(this.metroTextBox);
            this.Controls.Add(this.btnSend);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(325, 330);
            this.Name = "frmMessenger";
            this.Style = MetroFramework.MetroColorStyle.Red;
            this.Text = "Messenger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMessenger_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroButton btnSend;
        private MetroFramework.Controls.MetroTextBox metroTextBox;
    }
}