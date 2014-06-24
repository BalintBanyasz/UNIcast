using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UNIcast_Streamer
{
    public class PipeHandler
    {
        private const int TsPacketSize = 188;

        private NamedPipeServerStream _pipe;
        private bool _isConnected;

        // Events
        public event EventHandler ClientConnected;
        public event EventHandler ClientDisconnected;

        // Constructor
        public PipeHandler()
        {
            _isConnected = false;
        }

        // Properties
        public NamedPipeServerStream Pipe { get { return _pipe; } }

        public bool IsConnected { get { return _isConnected; } }

        /// <summary>
        /// Creates a named pipe server stream.
        /// </summary>
        /// <param name="pipeName">The name of the pipe.</param>
        public void CreateNamedPipeServer(string pipeName="UNIcast.ts")
        {
            _pipe = new NamedPipeServerStream("UNIcast.ts", PipeDirection.Out, 100, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, TsPacketSize, TsPacketSize);

            Debug.WriteLine("Pipe created {0}", _pipe.GetHashCode());

            _pipe.BeginWaitForConnection(WaitForConnectionCallBack, _pipe);
        }

        private void WaitForConnectionCallBack(IAsyncResult iar)
        {
            try
            {
                // Get the pipe
                NamedPipeServerStream pipeServer = (NamedPipeServerStream)iar.AsyncState;
                // End waiting for the connection
                pipeServer.EndWaitForConnection(iar);
                Debug.WriteLine("Client connected to named pipe");
                _isConnected = true;
                OnClientConnected(EventArgs.Empty);
            }
            catch
            {
                //
            }
        }

        public void Close()
        {
            if (_pipe == null)
            {
                return;
            }

            try
            {
                _pipe.Close();
                _pipe.Dispose();
                _isConnected = false;
            }
            catch (Exception)
            {
                //
            }
        }

        public void Write(byte[] buf)
        {
            try
            {
                _pipe.Write(buf, 0, buf.Length);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to write to named pipe");
                _isConnected = false;
                OnClientDisconnected(EventArgs.Empty);
            }      
        }

        protected virtual void OnClientConnected(EventArgs e)
        {
            EventHandler handler = ClientConnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnClientDisconnected(EventArgs e)
        {
            EventHandler handler = ClientDisconnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
