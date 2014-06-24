using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UNIcast_Streamer
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "UNIcast Recorder");

        [STAThread]
        static void Main()
        {
            // Prevent multiple instances
            // http://sanity-free.org/143/csharp_dotnet_single_instance_application.html
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmUNIcastStreamer());
                mutex.ReleaseMutex();
            }
            else
            {
                // send our Win32 message to make the currently running instance
                // jump on top of all the other windows
                NativeMethods.PostMessage(
                    (IntPtr)NativeMethods.HWND_BROADCAST,
                    NativeMethods.WM_SHOWME,
                    IntPtr.Zero,
                    IntPtr.Zero);
            }
        }
    }
}
