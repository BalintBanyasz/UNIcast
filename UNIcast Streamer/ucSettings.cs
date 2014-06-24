using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Threading;
using System.DirectoryServices.AccountManagement;
using System.Globalization;
using System.Diagnostics;
using System.IO;

namespace UNIcast_Streamer
{
    public partial class ucSettings : UserControl
    {

        public ucSettings()
        {
            InitializeComponent();
            this.txtOutputFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            if (Debugger.IsAttached)
            {
                txtLecturer.Text = "Teszt Elek";
                txtSubject.Text = "Szoftvertechnológia I.";
            }

            // Debug.WriteLine(UserPrincipal.Current.DisplayName);
        }

        private void chkRecord_CheckedChanged(object sender, EventArgs e)
        {
            pnlRecord.Enabled = chkRecord.Checked;
            CheckOutputFolderSet();
        }

        private void btnChooseOutputFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = txtOutputFolder.Text;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = fbd.SelectedPath;
                CheckOutputFolderSet();
            }
        }

        private void CheckOutputFolderSet()
        {
            btnStart.Enabled = !chkRecord.Checked || txtOutputFolder.Text.Length != 0;
        }
    }
}
