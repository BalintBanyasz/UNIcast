using CommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Globalization;
using ExtensionMethods;
using System.Resources;
using System.Reflection;
using UNIcast_Streamer.res;
using UNIcast_Streamer;
using MetroFramework;
using MetroFramework.Forms;
using DeckLinkDotNetStreamer;
using System.DirectoryServices.AccountManagement;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml.Linq;

namespace UNIcast_Streamer
{
    public partial class frmUNIcastStreamer : MetroForm, IDeckLinkStreamerCallback
    {

        private const string IniFile = "settings.ini";
        private const string Section = "UNIcastStreamerConfiguration";
        private const string KeyStream = "StreamAddress";
        private const string KeyMessage = "MessageAddress";

        private static readonly Address DefaultStreamAddress = new Address("224.5.6.7", 2000);
        private static readonly Address DefaultMessageAddress = new Address("224.5.6.7", 2001);

        private const int TTL = 3;

        // Configuration values
        private Address streamAddress;
        private Address messageAddress;

        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);

        private bool isStreaming;

        private string deckLinkAPIVersion;

        private FFmpeg ffmpeg;
        private string ffmpegVersion;
        private PerformanceMonitor performanceMonitor;
        private PipeHandler pipeHandler;
        private InputMonitor inputMonitor;
        private MulticastServer messageServer;
        private MulticastServer streamServer;

        private System.Windows.Forms.Timer timer;
        private DateTime timeStarted;
        private TimeSpan duration;
        private int timeMode;

        private DeckLinkStreamer deckLinkStreamer;
        private List<EncodingModeEntry> encodingPresets;
        private string deviceName;
        private string videoFormat;

        private int bitrate = 2000;

        private frmMessenger messenger;

        private string filePath;
        FileStream fs = null;
        BinaryWriter bw = null;

        bool shouldRecord;

        private frmWizard wizard;


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SHOWME)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                bool top = TopMost;
                TopMost = true;
                TopMost = top;
            }
            base.WndProc(ref m);
        }

        public frmUNIcastStreamer()
        {
            InitializeComponent();

            // Clear sample texts
            lblSubject.Text = String.Empty;
            lblLecturer.Text = String.Empty;
            lblDate.Text = String.Empty;

            // Fix screen position bug
            this.StartPosition = FormStartPosition.CenterScreen;

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

            // Load DeckLink API
            Thread deckLinkStreamerThread = new Thread(() =>
            {
                deckLinkStreamer = new DeckLinkStreamer();

                // Set callback
                deckLinkStreamer.SetCallback(this);

                // Initialise API
                if (!deckLinkStreamer.TryInitializeAPI())
                {
                    deckLinkStreamer.SetCallback(null);
                    MessageBox.Show(strings.errorDeckLinkDriver, strings.error);
                    Environment.Exit(1);
                }

                deckLinkAPIVersion = deckLinkStreamer.DeckLinkAPIVersion;
                lblDeckLinkVersion.InvokeIfRequired(c => c.Text = deckLinkAPIVersion);
            });
            deckLinkStreamerThread.SetApartmentState(ApartmentState.MTA);
            deckLinkStreamerThread.IsBackground = true;
            deckLinkStreamerThread.Start();

            // Initialise variables, load settings
            duration = new TimeSpan(1, 30, 00);
            timeMode = 0;
            timer = new System.Windows.Forms.Timer();
            timer.Tick += timer_Tick;       
            
            tabControl.SelectedIndex = 0;

            progressRing.ProgressPercentage = 0;

            isStreaming = false;

            streamServer = new MulticastServer(streamAddress.IP, streamAddress.Port, TTL);
            messageServer = new MulticastServer(messageAddress.IP, messageAddress.Port, TTL);
            Thread inputMonitorThread = new Thread(() =>
            {
                inputMonitor = new InputMonitor();
                inputMonitor.InputPositionReceived += inputMonitor_InputPositionReceived;
                inputMonitor.Start();
            });
            inputMonitorThread.IsBackground = true;
            inputMonitorThread.Start();

            ffmpeg = new FFmpeg();
            if (ffmpeg.isAvailable)
            {
                //ffmpeg.OnLogDataReceived += ffmpeg_LogDataReceived;
                ffmpegVersion = ffmpeg.GetVersion();
                lblFFmpegVersion.Text = ffmpegVersion;
            }

            pipeHandler = new PipeHandler();
            pipeHandler.ClientConnected += pipeHandler_ClientConnected;
            pipeHandler.ClientDisconnected += pipeHandler_ClientDisconnected;
            //else
            //{
            //    MessageBox.Show(strings.errorFFmpeg, strings.error);
            //    Environment.Exit(1);
            //}

            //performanceMonitor = new PerformanceMonitor(new string[] { "ffmpeg", "BMDStreamingServer", Process.GetCurrentProcess().ProcessName });
            //performanceMonitor.PerfValuesReceived += performanceMonitor_PerfValuesReceived;
            //performanceMonitor.StartMonitoring();

            lblVersion.Text += Application.ProductVersion;

            progressRing.Enabled = true;

            messenger = new frmMessenger();
        }

        private void ShowWizard()
        {
            wizard = new frmWizard(deviceName, videoFormat);
            if (wizard.ShowDialog() == DialogResult.OK)
            {
                UNIcastMeta meta = wizard.Meta;

                this.lblLecturer.Text = meta.Lecturer;
                this.lblSubject.Text = meta.Subject;
                this.ratings.SelectedItem = meta.Media.Quality;
                this.bitrate = meta.Media.Quality * 1000;
                this.lblDate.Text = String.Format("{0} {1}", meta.DateTime.ToShortDateString(), meta.DateTime.ToShortTimeString());
                this.shouldRecord = wizard.ShouldRecord;
                string dateTimeString = meta.DateTime.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                if (shouldRecord)
                {
                    this.filePath = Path.Combine(wizard.OutputFolder, String.Concat(dateTimeString, ".ts"));
                    Utils.WriteMetaXml(meta, Path.Combine(wizard.OutputFolder, String.Concat(dateTimeString, ".xml")));
                }

                Start();
            }
            else
            {
                Application.Exit();
            }
        }

        void pipeHandler_ClientDisconnected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void pipeHandler_ClientConnected(object sender, EventArgs e)
        {
            // Signal that the connection has been made
            connectDone.Set();
        }

        void inputMonitor_InputPositionReceived(object sender, InputMonitor.InputEventArgs e)
        {
            int[] values = new int[] {
                e.cursorPosition.X, 
                e.cursorPosition.Y, 
                e.caretPosition.Left, 
                e.caretPosition.Top, 
                e.caretPosition.Width, 
                e.caretPosition.Height, 
                Screen.PrimaryScreen.Bounds.Width, 
                Screen.PrimaryScreen.Bounds.Height};
            byte[] message = new byte[8 + 1 + 8 * 4];

            // Header
            // TimeStamp
            Buffer.BlockCopy(BitConverter.GetBytes(DateTime.UtcNow.ToUnixTimestamp()), 0, message, 0, 8);

            // MessageType
            message[8] = 0;

            // Payload
            for (int i = 0; i < values.Length; i++)
			{
                Buffer.BlockCopy(BitConverter.GetBytes(values[i]), 0, message, 9 + i * 4, 4);
			}
            messageServer.SendMessage(message);
        }

        void performanceMonitor_PerfValuesReceived(object sender, PerformanceMonitor.PerfValuesEventArgs e)
        {
            switch (e.processName)
            {
                case "ffmpeg":
                    lblFFmpegCPU.InvokeIfRequired(c => c.Text = String.Format("{0:0}%", e.cpuPercentage));
                    lblFFmpegRAM.InvokeIfRequired(c => c.Text = String.Format("{0} K", e.ramK.ToString("N0")));
                    break;
                case "BMDStreamingServer":
                    lblBMDStreamingServerCPU.InvokeIfRequired(c => c.Text = String.Format("{0:0}%", e.cpuPercentage));
                    lblBMDStreamingServerRAM.InvokeIfRequired(c => c.Text = String.Format("{0} K", e.ramK.ToString("N0")));
                    break;
                case "UNIcast Recorder":
                    lblUNIcastCPU.InvokeIfRequired(c => c.Text = String.Format("{0:0}%", e.cpuPercentage));
                    lblUNIcastRAM.InvokeIfRequired(c => c.Text = String.Format("{0} K", e.ramK.ToString("N0")));
                    break;
                // Debug
                case "UNIcast Recorder.vshost":
                    lblUNIcastCPU.InvokeIfRequired(c => c.Text = String.Format("{0:0}%", e.cpuPercentage));
                    lblUNIcastRAM.InvokeIfRequired(c => c.Text = String.Format("{0} K", e.ramK.ToString("N0")));
                    break;
                default:
                    break;
            }
        }

        void ffmpeg_LogDataReceived(object sender, FFmpeg.LogDataEventArgs e)
        {
            lblStatsFrame.InvokeIfRequired(c => c.Text = e.frame.ToString());
            lblStatsFps.InvokeIfRequired(c => c.Text = e.fps.ToString());
            lblStatsSize.InvokeIfRequired(c => c.Text = e.size.ToString("N0"));
            lblStatsTime.InvokeIfRequired(c => c.Text = String.Format("{0:hh\\:mm\\:ss\\.ff}", e.time));
            lblStatsBitrate.InvokeIfRequired(c => c.Text = e.bitrate.ToString("N1"));
        }

        private void Start()
        {
            // Create named pipe
            //pipeHandler.CreateNamedPipeServer("UNIcast.ts");

            // Connect FFmpeg
            //ffmpeg.StartStreaming("UNIcast.ts", StreamAdress);

            // Wait for FFmpeg to connect to the named pipe
            //connectDone.WaitOne();

            // Try to initialize FileStream and BinaryWriter with the given path
            if (shouldRecord)
            {
                try
                {
                    fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                    bw = new BinaryWriter(fs);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Failed to initialize FileStream and BinaryWriter with the given path.");
                    shouldRecord = false;
                }
            }

            Debug.WriteLine(DateTime.Now + " - Start time");
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                if (encodingPresets == null)
                    return;

                // Start streaming
                // Use 'Native (Progressive)' encoding preset with a custom bitrate
                deckLinkStreamer.StartStreaming(encodingPresets.ElementAt(2), bitrate);
            });

            isStreaming = true;
            lblTime.Enabled = true;
            timer.Enabled = true;
            timeStarted = DateTime.Now;
        }

        private void Stop()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(delegate
            {
                deckLinkStreamer.StopStreaming();
            });

            isStreaming = false;
            lblTime.Text = "00:00:00.00";
            lblTime.Enabled = false;
            timer.Enabled = false;
            progressRing.ProgressPercentage = 0;

            ffmpeg.StopProcess();
            connectDone.Reset();
        }

        #region UI event handlers
        private void frmUNIcastServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isStreaming)
            {
                Stop();
            }

            if (deckLinkStreamer != null)
            {
                deckLinkStreamer.SetCallback(null);
            }

            if (inputMonitor != null)
            {
                inputMonitor.Stop();
            }
            if (performanceMonitor != null)
                performanceMonitor.StopMonitoring();
        }

        private void lblTime_Click(object sender, EventArgs e)
        {
            if (timeMode == 2)
                timeMode = 0;
            else
                timeMode++;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            TimeSpan t = DateTime.Now.Subtract(timeStarted);
            progressRing.ProgressPercentage = (int)(t.TotalSeconds * 100.0 / duration.TotalSeconds);
            TimeSpan timeRemaining = duration.Subtract(t);
            switch (timeMode)
            {
                case 0:
                    lblTime.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", (int)t.TotalHours, t.Minutes, t.Seconds, t.Milliseconds / 10); //t.ToString(@"hh\:mm\:ss\.ff");
                    break;
                case 1:
                    lblTime.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}/{4:00}:{5:00}:{6:00}", (int)t.TotalHours, t.Minutes, t.Seconds, t.Milliseconds / 10, (int)duration.TotalHours, duration.Minutes, duration.Seconds);
                    break;
                case 2:
                    lblTime.Text = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", (int)timeRemaining.TotalHours, timeRemaining.Minutes, timeRemaining.Seconds, t.Milliseconds / 10);
                    break;
                default:
                    break;
            }
            if (t > duration)
            {
                timer.Stop();
                return;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }
        #endregion

        #region IDeckLinkStreamerCallback interface implementation
        public void DeviceArrived(string deviceName, DisplayModeEntry inputMode, List<DisplayModeEntry> displayModes, List<EncodingModeEntry> encodingPresets)
        {
            Debug.WriteLine("Device Arrived: " + deviceName);

            lblDevice.InvokeIfRequired(c => c.Text = deviceName);
            progressRing.InvokeIfRequired(c =>
            {
                ((RecordButton)c).ProgressPercentage = 0;
                c.Text = strings.record;
                c.Enabled = true;
            });
            this.deviceName = deviceName;
            OnStatusChanged(new StatusChangedEventArgs(deviceName, inputMode.ToString()));
        }

        public void DeviceRemoved()
        {
            Debug.WriteLine("Device Removed");

            picStatus.InvokeIfRequired(c => c.Image = UNIcast_Streamer.Properties.Resources.ico_idle);
            lblStatus.InvokeIfRequired(c => c.Text = strings.statusIdle); //
            lblDevice.InvokeIfRequired(c => c.Text = strings.statusNoDevice);
            lblTime.InvokeIfRequired(c => c.Text = "00:00:00:00");
            lblStatsTime.InvokeIfRequired(c => c.Text = "00:00:00");
            progressRing.InvokeIfRequired(c =>
            {
                ((RecordButton)c).ProgressPercentage = 0;
                c.Text = strings.record;
                c.Enabled = false;
            });

            this.deviceName = null;
            OnStatusChanged(new StatusChangedEventArgs(null, null));
        }

        public void DeviceModeChanged(DeckLinkAPI._BMDStreamingDeviceMode mode)
        {
            string status;
            Image statusImage;
            switch (mode)
            {
                case DeckLinkAPI._BMDStreamingDeviceMode.bmdStreamingDeviceEncoding:
                    status = strings.statusEncoding;
                    statusImage = UNIcast_Streamer.Properties.Resources.ico_encoding;
                    break;
                case DeckLinkAPI._BMDStreamingDeviceMode.bmdStreamingDeviceIdle:
                    status = strings.statusIdle;
                    statusImage = UNIcast_Streamer.Properties.Resources.ico_idle;
                    break;
                case DeckLinkAPI._BMDStreamingDeviceMode.bmdStreamingDeviceStopping:
                    status = strings.statusStopping;
                    statusImage = UNIcast_Streamer.Properties.Resources.ico_stopping;
                    break;
                case DeckLinkAPI._BMDStreamingDeviceMode.bmdStreamingDeviceUnknown:
                    status = strings.statusIdle;
                    statusImage = UNIcast_Streamer.Properties.Resources.ico_idle;
                    break;
                default:
                    status = strings.statusIdle;
                    statusImage = UNIcast_Streamer.Properties.Resources.ico_idle;
                    break;
            }
            lblStatus.InvokeIfRequired(c => c.Text = status);
            picStatus.InvokeIfRequired(c => c.Image = statusImage);
            Debug.WriteLine("Device Mode: " + status);
        }

        public void InputModeChanged(DisplayModeEntry inputMode, List<EncodingModeEntry> encodingPresets)
        {
            if (inputMode.displayMode == null)
            {
                Debug.WriteLine("No Input");
                // TODO: handle no input
                this.videoFormat = null;
                OnStatusChanged(new StatusChangedEventArgs(lblDevice.Text, null));
                return;
            }

            this.encodingPresets = encodingPresets;

            Debug.WriteLine("Input Mode: " + inputMode.ToString());
            Debug.WriteLine("Supported Encoding Presets:");
            foreach (EncodingModeEntry enodingMode in encodingPresets)
            {
                Debug.WriteLine(enodingMode.ToString());
            }

            this.videoFormat = inputMode.ToString();
            OnStatusChanged(new StatusChangedEventArgs(deviceName, videoFormat));
        }

        public void VideoInputConnectorChanged(DeckLinkAPI._BMDVideoConnection videoConnection)
        {
            string connector;
            switch (videoConnection)
            {
                case DeckLinkAPI._BMDVideoConnection.bmdVideoConnectionComponent:
                    connector = "Component";
                    break;
                case DeckLinkAPI._BMDVideoConnection.bmdVideoConnectionComposite:
                    connector = "Composite";
                    break;
                case DeckLinkAPI._BMDVideoConnection.bmdVideoConnectionHDMI:
                    connector = "HDMI";
                    break;
                case DeckLinkAPI._BMDVideoConnection.bmdVideoConnectionOpticalSDI:
                    connector = "Optical SDI";
                    break;
                case DeckLinkAPI._BMDVideoConnection.bmdVideoConnectionSDI:
                    connector = "SDI";
                    break;
                case DeckLinkAPI._BMDVideoConnection.bmdVideoConnectionSVideo:
                    connector = "S-Video";
                    break;
                default:
                    connector = "Unknown";
                    break;
            }
            Debug.WriteLine("Video Input Connector: " + connector);
        }

        public void VideoInputConnectorScanningChanged(bool isScanning)
        {
            Debug.WriteLine("Video Input Connector Scanning Status: " + (isScanning ? "scanning" : "scanning stopped"));
        }

        public void Mpeg2TsReceived(System.Net.Sockets.NetworkStream mpeg2Ts)
        {
            Debug.WriteLine(DateTime.Now + " - Stream received time");
            Debug.WriteLine("Latency: " + DateTime.Now.Subtract(timeStarted).TotalSeconds + " sec");

            try
            {
                //using (BinaryWriter bw = new BinaryWriter(File.Open(@"D:\Stream\testts.ts", FileMode.Append)))
                //using (BinaryWriter writer = new BinaryWriter(pipeHandler.Pipe))
                using (mpeg2Ts)
                {
                    // Start reading stream
                    byte[] buf = new byte[DeckLinkStreamer.TsPacketSize];
                    int bytesRead;
                    while (isStreaming && (bytesRead = mpeg2Ts.Read(buf, 0, buf.Length)) > 0)
                    {
                        //bw.Write(buf);
                        //writer.Write(buf);
                        // Write to local file

                        // Send packet via multicast
                        streamServer.SendBytes(buf);

                        // Record to local file
                        if (shouldRecord)
                            bw.Write(buf);
                    }
                    // Stream stopped
                    // Close binary writer and file stream
                    bw.Close();
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error writing stream");
                Debug.WriteLine(ex.ToString());
            }
        }
        #endregion

        private void frmUNIcastStreamer_Shown(object sender, EventArgs e)
        {
            ShowWizard();
        }

        protected virtual void OnStatusChanged(StatusChangedEventArgs e)
        {
            if (wizard == null)
            {
               ShowWizard();
            }
            else
            {
                wizard.ShowUpdatedStatusScreen(e.DeviceName, e.VideoFormat);
            }
        }

        private void progressRing_Click(object sender, EventArgs e)
        {
            if(!isStreaming)
                ShowWizard();
        }
    }

    public class StatusChangedEventArgs : EventArgs
    {
        public string DeviceName { get; private set; }
        public string VideoFormat { get; private set; }

        public StatusChangedEventArgs(string deviceName, string videoFormat)
        {
            DeviceName = deviceName;
            VideoFormat = videoFormat;
        }
    }
}
