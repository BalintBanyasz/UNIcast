using LibVLC;
using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using ExtensionMethods;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using CommonLib;

namespace UNIcast_Player
{
    public partial class frmUNIcastPlayer : MetroForm
    {
        private const string IniFile = "settings.ini";
        private const string Section = "UNIcastPlayerConfiguration";
        private const string KeyStream = "StreamAddress";
        private const string KeyMessage = "MessageAddress";
        private const string KeyHWA = "HardwareAcceleration";

        private static readonly Address DefaultStreamAddress = new Address("224.5.6.7", 2000);
        private static readonly Address DefaultMessageAddress = new Address("224.5.6.7", 2001);

        private const bool DefaultHWA = false;

        private static readonly Size DefaultClientSize = new Size(686, 500);
        private static readonly Size CropModeMinimumSize = new Size(400, 400);

        private const float WhRatio = (float)16 / 9;

        VlcInstance instance;
        private VlcMediaPlayer player;
        VlcMedia media;

        MulticastClient multicastClient;
        Point currentCursorPosition;
        Rectangle currentCaretPosition;
        Point lastCursorPosition;
        Rectangle lastCaretPosition;
        bool isTyping;

        private Address streamAddress;
        private Address messageAddress;
        private bool enableHWA;

        private int margin;

        private volatile bool isExiting;

        private CropMode cropMode;

        private LibVlc.EventCallbackDelegate stoppedDelegate;
        private LibVlc.EventCallbackDelegate playingDelegate;

        private frmStats stats;

        private enum CropMode
        {
            /// <summary>
            /// Display the whole image.
            /// </summary>
            NoCropping,
            /// <summary>
            /// Show only the area around the cursor.
            /// </summary>
            CropToContent
        }

        public frmUNIcastPlayer()
        {
            InitializeComponent();

            // Load settings
            IniFile ini = new IniFile(Path.Combine(Environment.CurrentDirectory, IniFile));
            string str = ini.IniReadValue(Section, KeyStream);
            if (!Address.TryParse(str, out streamAddress))
            {
                streamAddress = DefaultStreamAddress;
                ini.IniWriteValue(Section, KeyStream, streamAddress.ToString());
            }
            str = ini.IniReadValue(Section, KeyMessage);
            if (!Address.TryParse(str, out messageAddress))
            {
                messageAddress = DefaultMessageAddress;
                ini.IniWriteValue(Section, KeyMessage, messageAddress.ToString());
            }
            str = ini.IniReadValue(Section, KeyHWA);
            if (!Boolean.TryParse(str, out enableHWA))
            {
                enableHWA = DefaultHWA;
                ini.IniWriteValue(Section, KeyHWA, enableHWA.ToString());
            }

            Debug.WriteLine("streamAddress: " + streamAddress);
            Debug.WriteLine("messageAddress: " + messageAddress);
            Debug.WriteLine("HWA: " + enableHWA);

            // Last supported VLC version: VLC 2.0.8 "Twoflower"
            string[] args = new string[] {
                "-I", "dummy", "--ignore-config", 

                 // don't display file path
                "--no-video-title-show",
                
                // keep showing logo 
                // http://stackoverflow.com/questions/15992874/logo-appears-for-only-a-second-and-then-disappears
                "--sub-filter=logo", 

                // Caching value for network resources, in milliseconds.
                "--network-caching=60",
                
                // C:\Program Files (x86)\VideoLAN\VLC\plugins"
                @"--plugin-path=VLC_PLUGIN_PATH" 
            };

            // Enable hardware acceleration based on settings
            if (enableHWA)
            {
                List<string> argsList = args.ToList();
                argsList.Add("--ffmpeg-hw");
                args = argsList.ToArray();
            }
            else
            {
                ibtnHWA.Visible = false;
            }

            instance = new VlcInstance(args);
            player = null;

            cropMode = CropMode.NoCropping;

            margin = this.Width - ucVLC.Width;

            this.MinimumSize = DefaultClientSize;

            Debug.WriteLine(LibVlc.GetLibVlcVersion());

            media = new VlcMedia(instance, String.Format("udp://@{0}", streamAddress));
            player = new VlcMediaPlayer(media);

            stoppedDelegate = new LibVlc.EventCallbackDelegate(MediaPlayerStopped);
            playingDelegate = new LibVlc.EventCallbackDelegate(MediaPlayerPlaying);

            player.EventAttach(LibVlc.libvlc_event_e.libvlc_MediaStateChanged, stoppedDelegate);
            player.EventAttach(LibVlc.libvlc_event_e.libvlc_MediaPlayerStopped, stoppedDelegate);
            player.EventAttach(LibVlc.libvlc_event_e.libvlc_MediaPlayerPlaying, playingDelegate);

            player.Drawable = ucVLC.Handle;
            //ucVLC.BringToFront();
            player.Play();

            multicastClient = new MulticastClient(messageAddress.IP, messageAddress.Port);
            multicastClient.OnInputPositionReceived += multicastClient_OnInputPositionReceived;
            multicastClient.BeginReceive();
        }

        // Handle resize
        protected override void WndProc(ref Message m)
        {

            if (m.Msg == NativeMethods.WM_MOVING || m.Msg == NativeMethods.WM_SIZING)
            {
                if (cropMode == CropMode.CropToContent)
                {
                    return;
                }

                // Center logo
                picLogo.Left = ucVLC.Left + (ucVLC.Width - picLogo.Width) / 2;
                picLogo.Top = ucVLC.Top + (ucVLC.Height - picLogo.Height) / 2;

                // Keep the aspect ratio locked for the user control
                NativeMethods.RECT rc = (NativeMethods.RECT)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.RECT));
                int w = rc.Right - rc.Left;
                int h = rc.Bottom - rc.Top;
                int z = w > h ? w : h;
                rc.Bottom = rc.Top + h;
                rc.Right = rc.Left + (int)(WhRatio * ucVLC.Height) + margin;
                Marshal.StructureToPtr(rc, m.LParam, false);
                m.Result = (IntPtr)1;
                return;
            }
            base.WndProc(ref m);
        }

        public void MediaPlayerPlaying(IntPtr userdata)
        {
            Console.WriteLine("Playing");
            picLogo.InvokeIfRequired(c => c.Visible = false);
        }

        public void MediaPlayerStopped(IntPtr userdata)
        {
            Console.WriteLine("Stopped");
        }

        private void setTop(bool enable)
        {
            this.TopMost = enable;
        }

        void multicastClient_OnInputPositionReceived(object sender, MulticastClient.InputEventArgs e)
        {
            if (player == null || isExiting)
            {
                return;
            }

            Point center;
            currentCursorPosition = e.cursorPosition;
            currentCaretPosition = e.caretPosition;
            if (!isTyping)
            {
                if (currentCaretPosition != lastCaretPosition)
                {
                    isTyping = true;
                }
            }
            else
            {
                if (currentCursorPosition != lastCursorPosition)
                {
                    isTyping = false;
                }
            }
            lastCursorPosition = currentCursorPosition;
            lastCaretPosition = currentCaretPosition;

            center = isTyping ? new Point(currentCaretPosition.Left + currentCaretPosition.Width / 2, currentCaretPosition.Top + currentCaretPosition.Height /2) : currentCursorPosition;

            Size videoResolution = player.GetSize();
            int x = (int)((float)center.X / e.screenSize.X * videoResolution.Width);
            int y = (int)((float)center.Y / e.screenSize.Y * videoResolution.Height);
            

            if (cropMode == CropMode.NoCropping)
            {
                player.SetLogoInt((uint)LibVlc.libvlc_video_logo_option_t.libvlc_logo_enable, 1);
                player.SetLogoString("cursor_highlight.png");
                player.SetLogoInt((uint)LibVlc.libvlc_video_logo_option_t.libvlc_logo_x, x);
                player.SetLogoInt((uint)LibVlc.libvlc_video_logo_option_t.libvlc_logo_y, y);
                player.SetLogoInt((uint)LibVlc.libvlc_video_logo_option_t.libvlc_logo_repeat, -1);
                player.SetLogoInt((uint)LibVlc.libvlc_video_logo_option_t.libvlc_logo_opacity, 255); // 0-255
                return;
            }

            int x0, y0;

            // Keep cropping rectangle within bounds
            if (x - ucVLC.Width / 2 < 0)
            {
                x0 = 0;
            }
            else if (x + ucVLC.Width / 2 > videoResolution.Width)
            {
                x0 = videoResolution.Width - ucVLC.Width;
            }
            else
            {
                x0 = x - ucVLC.Width / 2;
            }

            if (y - ucVLC.Height / 2 < 0)
            {
                y0 = 0;
            }
            else if (y + ucVLC.Height / 2 > videoResolution.Height)
            {
                y0 = videoResolution.Height - ucVLC.Height;
            }
            else
            {
                y0 = y - ucVLC.Height / 2;
            }

            player.SetCropGeometry(x0, y0, ucVLC.Width, ucVLC.Height);
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    ucVLC.ToggleFullscreen();
                    break;
                case Keys.F:
                    ucVLC.ToggleFullscreen();
                    break;
            }
            return true;
        }

        private void btnStats_Click(object sender, EventArgs e)
        {
            if (media == null)
            {
                return;
            }

            if ((stats == null) || (stats.IsDisposed))
            {
                stats = new frmStats(media);  
            }

            stats.Show();
        }

        private void frmUNIcastPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (multicastClient != null)
                multicastClient.EndReceive();
            isExiting = true;
            if (media != null) media.Dispose();
            if (player != null) player.Dispose();
            instance.Dispose();
        }

        private void tglCrop_Toggle(object sender, EventArgs e)
        {
            if (tglCrop.IsChecked)
            {
                cropMode = CropMode.CropToContent;
                this.MinimumSize = CropModeMinimumSize;
                player.SetLogoInt((uint)LibVlc.libvlc_video_logo_option_t.libvlc_logo_enable, 0);
            }else
	        {
                cropMode = CropMode.NoCropping;
                this.MinimumSize = DefaultClientSize;
                if (this.Height < MinimumSize.Height)
                {
                    this.ClientSize = MinimumSize;
                }
                else
                {
                    this.Width = (int)(WhRatio * ucVLC.Height) + margin;
                }
                player.UnSetCropGeometry();
	        }
        }
    }
}
