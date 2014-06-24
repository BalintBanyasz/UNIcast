

using MetroFramework.Controls;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using UNIcast_Streamer.res;

namespace UNIcast_Streamer
{
    public partial class frmWizard : MetroForm
    {
        // UserControls
        private ucStatus status;
        private ucSettings settings;

        // Properties
        public bool ShouldRecord { get { return settings.chkRecord.Checked;  } }
        public string OutputFolder { get { return settings.txtOutputFolder.Text; } }

        public UNIcastMeta Meta
        {
            get
            {
                DateTime now = DateTime.Now;
                UNIcastMeta meta = new UNIcastMeta();
                meta.DateTime = now;
                meta.Lecturer = settings.txtLecturer.Text;
                meta.Subject = settings.txtSubject.Text;
                meta.Media = new Media(String.Concat(now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture), ".ts"), "video/MP2T", settings.ratings.SelectedItem);
                meta.Title = settings.txtTitle.Text;
                meta.Description = settings.txtDescription.Text;
                meta.Tags = new List<string>();
                meta.Privacy = settings.radPublic.Checked ? Privacy.Public : (settings.radPrivate.Checked ? Privacy.Private : Privacy.Unlisted);
                foreach (string tag in settings.txtTags.Text.Split(','))
                {
                    meta.Tags.Add(tag.Trim());
                }
                return meta;
            }
        }

        public frmWizard(string deviceName, string videoFormat)
        {
            InitializeComponent();

            // Instantiate the User Controls
            status = new ucStatus();
            settings = new ucSettings();
            ShowUpdatedStatusScreen(deviceName, videoFormat);
        }

        public void ShowUpdatedStatusScreen(string deviceName, string videoFormat)
        {
            Debug.WriteLine("status changed");

            status.btnNext.Click += btnNext_Click;
            status.lblStatusMain.Text = strings.statusScreenNoSource;
            if (deviceName == null)
            {
                status.lblStatusMain.Text = strings.statusScreenNoDevice;
                status.picStatusDevice.Image = Properties.Resources.ico_error;
                status.lblStatusDevice.Text = String.Empty;
            }
            else
            {
                status.picStatusDevice.Image = Properties.Resources.ico_check;
                status.lblStatusDevice.Text = deviceName;
            }
            if (videoFormat == null)
            {
                status.picStatusHdmi.Image = Properties.Resources.ico_error;
                status.lblStatusHdmi.Text = String.Empty;
            }
            else
            {
                status.lblStatusMain.Text = strings.statusScreenReady;
                status.picStatusHdmi.Image = Properties.Resources.ico_check;
                status.lblStatusHdmi.Text = videoFormat;
            }

            status.btnNext.Enabled = deviceName != null && videoFormat != null;

            metroPanel1.Controls.Clear();
            metroPanel1.Controls.Add(status);
        }

        void btnNext_Click(object sender, EventArgs e)
        {
            metroPanel1.Controls.Clear();
            metroPanel1.Controls.Add(settings);
        }

        private void flowLayoutPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void flowLayoutPanel_DragDrop(object sender, DragEventArgs e)
        {
            FlowLayoutPanel flowLayoutPanel = (FlowLayoutPanel)sender;
            MetroButton data = (MetroButton)e.Data.GetData(typeof(MetroButton));
            Point p = flowLayoutPanel.PointToClient(new Point(e.X, e.Y));
            var item = flowLayoutPanel.GetChildAtPoint(p);
            int index = flowLayoutPanel.Controls.GetChildIndex(item, false);
            Debug.WriteLine(index);
            Debug.WriteLine(flowLayoutPanel.Controls.Count);

            // Hold place for 'Add new' button
            if (index == flowLayoutPanel.Controls.Count - 1)
            {
                index--;
            }
            else if (index == -1)
            {
                index = flowLayoutPanel.Controls.Count - 2;
            }

            flowLayoutPanel.Controls.SetChildIndex(data, index);
            flowLayoutPanel.Invalidate();
        }

        private void btn_MouseDown(object sender, MouseEventArgs e)
        {
            ((Button)sender).DoDragDrop(((Button)sender), DragDropEffects.Move);
        }
    }
}
