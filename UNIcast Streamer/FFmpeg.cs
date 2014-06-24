using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UNIcast_Streamer
{
    class FFmpeg
    {
        private const string RelativePath = "/ffmpeg/ffmpeg.exe";
        private const string ArgVersion = "version";
        private const string PtrnVersion = @"ffmpeg version ([^ ]*)(.*?)built on (\w+ \w+ \w+)";
        private const string ArgUdpStream = @"-re -f mpegts -analyzeduration 800000 -fpsprobesize 10 -i \\.\pipe\{0} -vcodec copy -an -copyts -metadata service_provider=""UNIcast"" -metadata service_name=""UNIcast Stream"" -f mpegts udp://{1}"; //-re -f mpegts -analyzeduration 800000 -fpsprobesize 10 -i \\.\pipe\{0} -vcodec copy -acodec copy -metadata service_provider=""UNIcast"" -metadata service_name=""UNIcast Stream"" -f mpegts udp://{1}
        private const string PtrnUdpStream = @"frame=\s*(?<frame>\d+) fps=\s*(?<fps>\d+(\.\d+)?) q=\s*(?<q>[\+\-]?\d+(\.\d+)?) size=\s*(?<size>\d+)kB time=(?<time>(\d\d):(\d\d):(\d\d(\.\d\d)?)) bitrate=\s*(?<bitrate>\d+(\.\d+)?)kbits";
        
        private string exePath;
        public bool isAvailable;

        private Process proc;

        /// <summary>
        /// Event Handler for FFmpeg log
        /// </summary>
        public delegate void LogDataReceivedEventHandler(object sender, LogDataEventArgs e);
        public event LogDataReceivedEventHandler OnLogDataReceived;

        public class LogDataEventArgs : System.EventArgs
        {
            public int frame;
            public float fps;
            public float q;
            public int size;
            public TimeSpan time;
            public float bitrate;
        }

        public FFmpeg()
        {
            isAvailable = false;
            exePath = System.Windows.Forms.Application.StartupPath + RelativePath;
            if (File.Exists(exePath))
            {
                isAvailable = true;
                Debug.WriteLine("FFmpeg found");
            }
            else
                Debug.WriteLine("FFmpeg not found");
        }

        private string RunQuery(string args)
        {
            if (!isAvailable)
                return string.Empty;

            ProcessStartInfo psi = new ProcessStartInfo(exePath, args) 
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            string output = null; 
            StreamReader sr = null;

            try
            {
                Process proc = Process.Start(psi);
                proc.WaitForExit();

                sr = proc.StandardError;
                output = sr.ReadToEnd();

                proc.Close();
            }
            catch (Exception)
            {
                output = string.Empty;
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                }
            }
            return output;
        }

        private void RunProcess(string args)
        {
            if (!isAvailable)
                return;

            ProcessStartInfo psi = new ProcessStartInfo(exePath, args)
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                RedirectStandardError = true,
                RedirectStandardInput = true
            };

            try
            {
                proc = new Process() { StartInfo = psi, EnableRaisingEvents = true };
                proc.ErrorDataReceived += ErrorDataReceived;
                proc.Exited += Exited;
                proc.Start();
                Debug.WriteLine("FFmpeg started");
                proc.BeginErrorReadLine();
            }
            catch (Exception)
            {
                Debug.WriteLine("FFmpeg proc error");
            }
        }

        void Exited(object sender, EventArgs e)
        {
            //proc.CancelOutputRead();
		    proc.Close();
            proc.Dispose();
            proc = null;
            Debug.WriteLine("FFmpeg exited");
        }

        // FFmpeg uses stderr for output
        void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!String.IsNullOrEmpty(e.Data))
            {
                Debug.WriteLine("FFmpeg: " + e.Data);
                var matches = Regex.Match(e.Data, PtrnUdpStream);
                if (matches.Success)
                {
                    LogDataEventArgs eventArgs = new LogDataEventArgs();
                    Int32.TryParse(matches.Groups["frame"].Value, out eventArgs.frame);
                    float.TryParse(matches.Groups["fps"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,  out eventArgs.fps);
                    float.TryParse(matches.Groups["q"].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out eventArgs.q);
                    Int32.TryParse(matches.Groups["size"].Value, out eventArgs.size);
                    TimeSpan.TryParse(matches.Groups["time"].Value, out eventArgs.time);
                    float.TryParse(matches.Groups["bitrate"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out eventArgs.bitrate);
                    if (OnLogDataReceived != null)
                    {
                        OnLogDataReceived(this, eventArgs);
                    } 
                }
            }            
        }

        /// <summary>
        /// Gets the version and build information of FFmpeg.
        /// </summary>
        public string GetVersion()
        {
            string version;
            version = RunQuery(ArgVersion);
            if(version != string.Empty)
            {
                var matches = Regex.Match(version, PtrnVersion, RegexOptions.Singleline);
                if (matches.Length >= 3)
                {
                    version = String.Format("{0} ({1})", matches.Groups[1].Value, matches.Groups[3].Value);
                    Debug.WriteLine("FFmpeg version: " + version);
                }
            }
            return version;
        }

        /// <summary>
        /// Starts streaming data from a named pipe to a given IP adress in UDP packets using an FFmpeg process.
        /// </summary>
        /// <para><param name="pipe">The named pipe used to send the packets to FFmpeg.</param></para> 
        /// <param name="adress">The target IP adress and port.</param>
        public void StartStreaming(string pipeName="UNIcast.ts", string adress="236.1.0.1:2000")
        {
            RunProcess(String.Format(ArgUdpStream, pipeName, adress));
            Debug.WriteLine("FFmpeg streaming started");
        }

        /// <summary>
        /// Stops the FFmpeg streaming process
        /// </summary>
        public void StopProcess()
        {
            if (proc != null)
                proc.StandardInput.WriteLine("q");
        }
    }
}
