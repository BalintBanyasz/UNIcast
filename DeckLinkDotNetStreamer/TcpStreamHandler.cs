using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace DeckLinkDotNetStreamer
{
    /// <summary>
    /// Module created to replace the non-working part of the DeckLink Streaming SDK.
    /// Uses Blackmagic's internal localhost tcp communication to obtain and manage the Mpeg2Ts stream.
    /// Unofficial 'workaround' written by Balint Banyasz.
    /// </summary>
    class TcpStreamHandler : IDisposable
    {
        private const string Localhost = "localhost";
        private const int BmdPort = 13823;
        private const int TsPacketSize = 188;
        private const string BmdCmdNotify = "notify\n";
        private const string BmdCmdStart = "start -id {0}\n";
        private const string BmdCmdReceive = "receive -id {0} -transport tcp\n";
        private const string BmdPtrnStart = "OK: start -id ([0-9])";
        private const string BmdPtrnArrived = "arrived: ([0-9]) (0x[0-9a-fA-F]+) ([0-9a-zA-Z ]+)";

        private int deviceId;
        private bool isReceiving;

        private TcpClient tcpClient;
        private NetworkStream networkStream;

        public event EventHandler<Mpeg2TsReceivedEventArgs> Mpeg2TsReceived;

        public class Mpeg2TsReceivedEventArgs : EventArgs
        {
            public NetworkStream Mpeg2Ts;
        }

        public TcpStreamHandler()
        {
            isReceiving = false;
        }

        /// <summary>
        /// Queries the given command and returns the response as a Match.
        /// </summary>
        /// <param name="command">The command to query.</param>
        /// <param name="pattern">The response pattern.</param>
        /// <returns>Returns the response as a Match.</returns>
        /// <exception cref="SocketException">Socket error.</exception>
        private static Match QueryForMatch(string command, string pattern)
        {
            using (TcpClient tcpClient = new TcpClient(Localhost, BmdPort))
            using (NetworkStream networkStream = tcpClient.GetStream())
            {
                // Encode the data string into a byte array.
                byte[] dataToSend = Encoding.ASCII.GetBytes(command);

                // Send the data through the socket.
                networkStream.Write(dataToSend, 0, dataToSend.Length);
                networkStream.Flush();

                // Receive the response.
                byte[] bytes = new byte[256];
                int bytesRec = networkStream.Read(bytes, 0, bytes.Length);
                string response = Encoding.ASCII.GetString(bytes, 0, bytesRec);
                return Regex.Match(response, pattern);
            }
        }

        /// <summary>
        /// Tries to obtain the Device Id.
        /// </summary>
        /// <param name="deviceId">When this method returns, contains the Device Id in integer format, if the query succeeded, or zero if it failed.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool GetDeviceId(out int deviceId)
        {
            try
            {
                Match match = QueryForMatch(BmdCmdNotify, BmdPtrnArrived);
                if (match.Success)
                {
                    int result;
                    if (Int32.TryParse(match.Groups[1].Value, out result))
                    {
                        deviceId = result;
                        return true;
                    }
                }
            }
            catch (SocketException se)
            {
                Debug.WriteLine(se.ToString());
            }
            deviceId = 0;
            return false;
        }

        /// <summary>
        /// Tries to obtain the Device Id.
        /// </summary>
        /// <returns>Returns true on success, false on failure.</returns>
        public bool GetDeviceId()
        {
            try
            {
                Match match = QueryForMatch(BmdCmdNotify, BmdPtrnArrived);
                if (match.Success)
                {
                    if (Int32.TryParse(match.Groups[1].Value, out deviceId))
                    {
                        return true;
                    }
                }
            }
            catch (SocketException se)
            {
                Debug.WriteLine(se.ToString());
            }
            return false;
        }

        /// <summary>
        /// Tries to start recording.
        /// </summary>
        /// <param name="deviceId">The Id of the device to start.</param>
        /// <returns>Returns true on success, false on failure.</returns>
        public static bool Start(int deviceId)
        {
            Match match = QueryForMatch(String.Format(BmdCmdStart, deviceId), BmdPtrnStart);
            if (match.Success)
            {
                if (Int32.TryParse(match.Groups[1].Value, out deviceId))
                {
                    return true;
                }
            }
            return false;
        }

        public void StartReceiving()
        {
            this.isReceiving = true;
            if (Start(deviceId))
            {
                GetStream();
            }
            else
            {
                // TODO: handle failure
            }
        }

        public void StopReceiving()
        {
            this.isReceiving = false;
        }

        /// <summary>
        /// Tries to obtain the Mpeg2Ts stream.
        /// </summary>
        /// <returns>Returns true on success, false on failure.</returns>
        private bool GetStream()
        {
            try
            {
                tcpClient = new TcpClient(Localhost, BmdPort);
                networkStream = tcpClient.GetStream();
                byte[] dataToSend = Encoding.ASCII.GetBytes(String.Format(BmdCmdReceive, deviceId));

                // Register for MPEG2TS stream
                networkStream.Write(dataToSend, 0, dataToSend.Length);
                networkStream.Flush();

                // Fire event 
                OnMpeg2TsReceived(new Mpeg2TsReceivedEventArgs() { Mpeg2Ts = networkStream });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error reading stream");
                Debug.WriteLine(ex.ToString());
                Dispose();
                return false;
            }
            return true;
        }

        protected virtual void OnMpeg2TsReceived(Mpeg2TsReceivedEventArgs e)
        {
            EventHandler<Mpeg2TsReceivedEventArgs> handler = Mpeg2TsReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void Dispose()
        {
            if (networkStream != null)
            {
                networkStream.Close();
                networkStream = null;
            }

            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient = null;
            }
        }
    }
}
