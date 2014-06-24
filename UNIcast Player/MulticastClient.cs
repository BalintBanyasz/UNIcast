using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UNIcast_Player
{
    public class MulticastClient
    {
        private const int MessageHeaderLength = 9;

        public delegate void InputPositionReceivedEventHandler(object sender, InputEventArgs e);
        public event InputPositionReceivedEventHandler OnInputPositionReceived;

        private UdpClient client;
        private UdpState state;

        private long lastTimeStamp;

        private volatile bool isRunning;

        public class InputEventArgs : System.EventArgs
        {
            public Point cursorPosition;
            public Rectangle caretPosition;
            public Point screenSize;
        }

        public class UdpState
        {
            public UdpClient c;
            public IPEndPoint e;
        }

        public MulticastClient (string multicastIP, int port)
	    {
            client = new UdpClient();

            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, port);

            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            client.ExclusiveAddressUse = false;

            client.Client.Bind(localEp);

            IPAddress multicastaddress = IPAddress.Parse(multicastIP);
            client.JoinMulticastGroup(multicastaddress);

            state = new UdpState();
            state.e = localEp;
            state.c = client;
	    }

        public void BeginReceive()
        {
            isRunning = true;

            // Start async receiving
            client.BeginReceive(new AsyncCallback(ReceiveCallback), state);
        }

        public void EndReceive()
        {
            isRunning = false;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            UdpClient c = (UdpClient)((UdpState)ar.AsyncState).c;
            IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).e;

            if (c == null || e == null)
                return;

            Byte[] receiveBytes = c.EndReceive(ar, ref e);

            if (isRunning)
            {
                // Restart listening for udp data packages
                c.BeginReceive(new AsyncCallback(ReceiveCallback), ar.AsyncState);
            }
            else
            {
                client.Close();
                return;
            }

            // Process message
            ProcessMessage(receiveBytes);
        }

        private void ProcessMessage(Byte[] receiveBytes)
        {
            if (receiveBytes.Count() < MessageHeaderLength)
            {
                return;
            }

            long timeStamp = BitConverter.ToInt64(receiveBytes, 0);

            if (timeStamp <= lastTimeStamp)
            {
                Debug.WriteLine("UDP packet out of order");
                return;
            }

            lastTimeStamp = timeStamp;

            byte messageType = receiveBytes[8];

            switch (messageType)
            {
                // Cursor position
                case 0:

                    // Check length
                    if (receiveBytes.Count() != MessageHeaderLength + 8 * 4)
                    {
                        return;
                    }

                    int[] values = new int[8];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = BitConverter.ToInt32(receiveBytes, MessageHeaderLength + i * 4);
                    }

                    InputEventArgs eventArgs = new InputEventArgs();
                    eventArgs.cursorPosition = new Point(values[0], values[1]);
                    eventArgs.caretPosition = new Rectangle(values[2], values[3], values[4], values[5]);
                    eventArgs.screenSize = new Point(values[6], values[7]);
                    if (OnInputPositionReceived != null)
                        OnInputPositionReceived(this, eventArgs);

                    break;
                default:
                    break;
            }
        }
    }
}
