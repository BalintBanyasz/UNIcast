using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UNIcast_Streamer
{
    class MulticastServer
    {
        // Multicast IP addresses are within the Class D range of
        // 224.0.0.0-239.255.255.255 
        //public string multicastIP = "224.5.6.7";
        //public int port = 5000;
        private Socket s;

        public MulticastServer(string multicastIP, int port, int ttl)
        {
            try
            {
                s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                IPAddress ip = IPAddress.Parse(multicastIP);

                s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip));

                s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, ttl);

                IPEndPoint ipep = new IPEndPoint(ip, port);
                s.Connect(ipep);

                Debug.WriteLine("Connected");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void Disconnect()
        {
            if (s.Connected)
                s.Close();
        }

        public void SendBytes(byte[] buf)
        {
            if (s.Connected)
            {
                try
                {
                    s.Send(buf, buf.Length, SocketFlags.None);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }

        public void SendMessage(byte[] message, bool enryption = false)
        {

            if (!enryption)
            {
                // Use UTF8.GetString(Byte[]) method to decode it
                //byte[] buffer = Encoding.UTF8.GetBytes(message);
                SendBytes(message);
            }
        }
    }
}
