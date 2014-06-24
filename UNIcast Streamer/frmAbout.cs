using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIcast_Streamer
{
    public partial class frmAbout : Form
    {
        private const string URL_DECKLINK = "http://www.blackmagicdesign.com/";
        private const string URL_FFMPEG = "http://www.ffmpeg.org/";
        private const string LIC_PATH =  @"\licences\";
        private const string LIC_FFMPEG = "lgpl-2.1.txt";

        public frmAbout(string deckLinkAPIVersion, string ffmpegVersion)
        {
            InitializeComponent();
            lblVersion.Text += Application.ProductVersion;
            lblDeckLinkAPI.Text = String.Format(lblDeckLinkAPI.Text, deckLinkAPIVersion);
            lblFFmpeg.Text = String.Format(lblFFmpeg.Text, ffmpegVersion);

            llblDeckLinkUrl.Links.Add(0, 0, URL_DECKLINK);
            llblFFmpegUrl.Links.Add(0, 0, URL_FFMPEG);
            llblFFmpegLic.Links.Add(0, 0, Application.StartupPath + LIC_PATH + LIC_FFMPEG);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(e.Link.LinkData != null)
                System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
