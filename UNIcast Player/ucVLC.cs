using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace UNIcast_Player
{
    public partial class ucVLC : UserControl
    {
        private Panel transparentPanel;
        private Form frmFullScreen;
        private bool isFullScreen;

        public ucVLC()
        {
            InitializeComponent();
            transparentPanel = new Panel();
            transparentPanel.BackColor = Color.FromArgb(0, 0, 0, 0);
            transparentPanel.Location = Point.Empty;
            transparentPanel.Size = this.Size;
            transparentPanel.DoubleClick += transparentPanel_DoubleClick;
            transparentPanel.MouseMove += transparentPanel_MouseMove;
            this.Controls.Add(transparentPanel);
        }

        void transparentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            HandleMouseMove(Parent.Handle);
        }

        void transparentPanel_DoubleClick(object sender, EventArgs e)
        {
            ToggleFullscreen();
        }

        public void ToggleFullscreen()
        {
            if (!isFullScreen)
            {
                frmFullScreen = new Form();
                frmFullScreen.FormBorderStyle = FormBorderStyle.None;
                frmFullScreen.WindowState = FormWindowState.Maximized;
                frmFullScreen.TopMost = true;
                frmFullScreen.ShowInTaskbar = false;
                frmFullScreen.KeyPreview = true;
                frmFullScreen.KeyDown += delegate(object sender, KeyEventArgs e)
                {
                    switch (e.KeyCode)
                    {
                        case Keys.Enter:
                            ToggleFullscreen();
                            e.Handled = true;
                            break;
                        case Keys.F:
                            ToggleFullscreen();
                            e.Handled = true;
                            break;
                    }
                };
                Point loc = this.Location;
                Size size = this.Size;
                DockStyle dock = this.Dock;
                AnchorStyles anchor = this.Anchor;
                Control parent = this.Parent;

                this.Parent = frmFullScreen;
                this.Location = Point.Empty;
                this.Dock = DockStyle.Fill;
                frmFullScreen.FormClosing += delegate
                {
                    this.Parent = parent;
                    this.Location = loc;
                    this.Size = size;
                    this.Dock = dock;
                    this.Anchor = anchor;
                    parent.Focus();
                };
                frmFullScreen.Show();

                isFullScreen = true;
            }
            else
            {
                frmFullScreen.Close();
                isFullScreen = false;
            }
        }

        private void ucVLC_SizeChanged(object sender, EventArgs e)
        {
            transparentPanel.Size = this.Size;
        }

        public void HandleMouseMove(IntPtr handle)
        {
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HT_CAPTION, 0);
        }
    }
}
