using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UNIcast_Streamer
{
    public class InputMonitor
    {
        volatile bool isRunning;

        public delegate void InputPositionReceivedEventHandler(object sender, InputEventArgs e);
        public event InputPositionReceivedEventHandler InputPositionReceived;

        public class InputEventArgs : System.EventArgs
        {
            public Point cursorPosition;
            public Rectangle caretPosition;
        }

        public InputMonitor()
        {
            isRunning = true;
        }

        public void Start()
        {
            isRunning = true;
            System.Threading.Thread t = new System.Threading.Thread(Update);
            t.Start();
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void Update()
        {
            while (isRunning)
            {
                InputEventArgs e = new InputEventArgs();
                e.cursorPosition = GetCursorPosition();
                e.caretPosition = GetCaretPosition();
                InputPositionReceived(this, e);
                Thread.Sleep(100);
            }
        }

        /// <summary>
        /// Retrieves the caret position
        /// </summary>
        public Rectangle GetCaretPosition()
        {
            guiInfo = new GUITHREADINFO();
            guiInfo.cbSize = (uint)Marshal.SizeOf(guiInfo);

            GetGUIThreadInfo(0, out guiInfo);
            ClientToScreen(guiInfo.hwndCaret, out guiInfo.rcCaret);

            return guiInfo.rcCaret;
        }

        /// <summary>
        /// Retrieves the cursor position
        /// </summary>
        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);

            return lpPoint;
        }

        #region Native structures

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public static implicit operator Rectangle(RECT rect)
            {
                return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
            }
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct GUITHREADINFO
        {
            public uint cbSize;
            public uint flags;
            public IntPtr hwndActive;
            public IntPtr hwndFocus;
            public IntPtr hwndCapture;
            public IntPtr hwndMenuOwner;
            public IntPtr hwndMoveSize;
            public IntPtr hwndCaret;
            public RECT rcCaret;
        };

        GUITHREADINFO guiInfo;                     // To store GUI Thread Information

        #endregion

        #region DllImports

        [DllImport("user32.dll", EntryPoint = "GetGUIThreadInfo")]
        public static extern bool GetGUIThreadInfo(uint tId, out GUITHREADINFO threadInfo);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, out RECT lpRect);

        #endregion      
    }
}
