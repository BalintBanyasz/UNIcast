using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ExtensionMethods
{
    // Based on http://stackoverflow.com/a/2367763
    public static class ExtensionMethods
    {
        public static void InvokeIfRequired<T>(this T c, Action<T> action)
            where T : Control
        {
            if (c.InvokeRequired)
            {
                try
                {
                    c.Invoke(new Action(() => action(c)));
                }
                catch (ObjectDisposedException)
                {
                    //
                }
            }
            else
            {
                action(c);
            }
        }
    }
}
