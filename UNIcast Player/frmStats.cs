using LibVLC;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using ExtensionMethods;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace UNIcast_Player
{
    public partial class frmStats : MetroForm
    {
        VlcMedia media;
        private bool isRunning;

        public frmStats(VlcMedia media)
        {
            InitializeComponent();
            this.media = media;


            isRunning = true;
            Thread thread = new Thread(GetStats);
            thread.IsBackground = true;
            thread.Start();
        }

        public void GetStats()
        {
            while (isRunning)
            {
                LibVLC.LibVlc.libvlc_media_stats_t stats = media.GetStats();
                lblAudioDecoded.InvokeIfRequired(c => c.Text = stats.i_decoded_audio.ToString());
                lblAudioPlayed.InvokeIfRequired(c => c.Text = stats.i_played_abuffers.ToString());
                lblAudioLost.InvokeIfRequired(c => c.Text = stats.i_lost_abuffers.ToString());
                lblVideoDecoded.InvokeIfRequired(c => c.Text = stats.i_decoded_video.ToString());
                lblVideoDisplayed.InvokeIfRequired(c => c.Text = stats.i_displayed_pictures.ToString());
                lblVideoLost.InvokeIfRequired(c => c.Text = stats.i_lost_pictures.ToString());
                lblMediaSize.InvokeIfRequired(c => c.Text = (stats.i_read_bytes / 1024).ToString());
                lblInputBitrate.InvokeIfRequired(c => c.Text = (stats.f_input_bitrate * 8000).ToString("0"));
                lblDemuxedSize.InvokeIfRequired(c => c.Text = (stats.i_demux_read_bytes / 1024).ToString());
                lblContentBitrate.InvokeIfRequired(c => c.Text = (stats.f_demux_bitrate * 8000).ToString("0"));
                lblDiscarded.InvokeIfRequired(c => c.Text = stats.i_demux_corrupted.ToString());
                lblDropped.InvokeIfRequired(c => c.Text = stats.i_demux_discontinuity.ToString());
                Thread.Sleep(1000);
            }
        }

        private void frmStats_FormClosing(object sender, FormClosingEventArgs e)
        {
            isRunning = false;
        }
    }
}
