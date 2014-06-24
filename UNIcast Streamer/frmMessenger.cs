
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UNIcast_Streamer
{
    public partial class frmMessenger : MetroForm
    {
        public frmMessenger()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (metroTextBox.Text.Length == 0)
            {
                return;
            }

            metroTextBox.Clear();
        }

        private void frmMessenger_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
