using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using System.Collections.Specialized;

namespace UNIcast_Streamer
{
    public class Sound
    {
        // Windows 7/8 Sound Recording tab 
        // control /name Microsoft.Sound
        const string FILE_NAME = "control";
        const string ARGUMENTS = "mmsys.cpl,,{0}";

        // Control IDs
        const int CID_DEVICE_LISTVIEW = 0x000003E8;
        const int CID_SET_DEFAULT_BTN = 0x000003EA;
        const int CID_PROPERTIES_BTN = 0x000003EB;
        const int CID_LISTEN_TO_THIS_DEVICE_CHB = 0x00000641;
        const int CID_APPLY_BTN = 0X00003021;

        // Event handlers
        public delegate void DeviceListReturnedEventHandler(object sender, DeviceListEventArgs e);
        public event DeviceListReturnedEventHandler OnDeviceListReturned;

        public delegate void SoundDialogTitleFound(object sender, SoundDialogTitleEventArgs e);
        public event SoundDialogTitleFound OnSoundDialogTitleFound;

        public class DeviceListEventArgs : System.EventArgs
        {
            public List<SoundDevice> playbackDevices;
            public List<SoundDevice> recordingDevices;
        }

        public class SoundDialogTitleEventArgs : System.EventArgs
        {
            public string soundDialogTitle;

            public SoundDialogTitleEventArgs(string soundDialogTitle)
            {
                this.soundDialogTitle = soundDialogTitle;
            }
        }

        // Variables
        string soundDialogTitle;

        List<SoundDevice> playbackDevices;
        List<SoundDevice> recordingDevices;

        public struct SoundDevice
        {
            public string deviceName;
            public AutomationElement automationElement;
            public override string ToString()
            {
                return deviceName;
            }
        }

        /// <summary>
        /// A simple class that manages the Win7+ Sound settings using UIAutomation
        /// </summary>
        /// <param name="soundDialogTitle">The localized title of the Sound dialog e.g. 'Hang' in Hungarian. 
        /// If not given, the class will try to determine it from the registry and fire the OnSoundDialogTitleFound event with the title passed as EventArg.
        /// You should save it to be able to create the class with this parameter next time to speed up the process.
        /// </param>
        public Sound(string soundDialogTitle)
        {
            this.soundDialogTitle = soundDialogTitle;
        }

        public Sound()
        {
            //
        }

        public void GetDeviceLists()
        {
            System.Threading.Thread t = new System.Threading.Thread(() =>
            {
                // Get the handle of the window
                IntPtr soundDialogHandle = OpenSoundDialog(0);
                if (soundDialogHandle == IntPtr.Zero)
                    return;

                // Hide the window
                if (soundDialogHandle != IntPtr.Zero)
                    NativeMethods.SetWindowPos(soundDialogHandle, IntPtr.Zero, -100, -100, 100, 100, 0);

                // Create the AutomationElement from the window handle
                AutomationElement aeSoundDialog = AutomationElement.FromHandle(soundDialogHandle);

                // Get the TabControl
                AutomationElement aeTab = aeSoundDialog.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tab));

                // Switch to Playback tab in case the user opened the window with an other tab selected
                SwitchTab(aeTab, 0);

                // Get the list of available playback devices
                playbackDevices = new List<SoundDevice>();
                playbackDevices = GetDeviceList(aeSoundDialog);


                // Switch to Recording tab
                SwitchTab(aeTab, 1);

                // Get the list of available recording devices
                recordingDevices = new List<SoundDevice>();
                recordingDevices = GetDeviceList(aeSoundDialog);

                CloseDialog(aeSoundDialog);

                DeviceListEventArgs e = new DeviceListEventArgs();
                e.playbackDevices = playbackDevices;
                e.recordingDevices = recordingDevices;

                if (OnDeviceListReturned != null)
                    OnDeviceListReturned(this, e);
            });
            t.Start();
        }

        private string GetLocalizedSoundDialogTitle()
        {
            string soundDialogTitle = String.Empty;
            string regWin7 = @"Local Settings\MuiCache";
            string regWin8 = @"Local Settings\ImmutableMuiCache\Strings";

            Version osVersion = Environment.OSVersion.Version;
            if (osVersion.Major < 6)
                return soundDialogTitle;

            string regKey = osVersion.Minor == 1 ? regWin7 : regWin8;
            int level = osVersion.Minor == 1 ? 2 : 1;

            // Get the whole path e.g. Local Settings\MuiCache\331\7F71D2D2
            for (int i = 0; i < level; i++)
            {
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(regKey);
                if (key != null && key.SubKeyCount != 0)
                    regKey += @"\" + key.GetSubKeyNames().First();
            }
            Debug.WriteLine(regKey);
            Microsoft.Win32.RegistryKey path = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(regKey);

            foreach (String valueName in path.GetValueNames())
            {
                if (valueName.StartsWith(@"@C:\Windows\System32\mmsys.cpl,-300"))
                {
                    soundDialogTitle = path.GetValue(valueName).ToString();
                    break;
                }
            }

            return soundDialogTitle;
        }

        /// <summary>
        /// Opens the Sound settings dialog from Control Panel with the given tab focused
        /// </summary>
        /// <returns>
        /// The handle to the Sound window
        /// </returns>
        private IntPtr OpenSoundDialog(int tabIndex)
        {
            // Get localized dialog title
            if (soundDialogTitle == null)
                soundDialogTitle = GetLocalizedSoundDialogTitle();
            if (soundDialogTitle == String.Empty)
            {
                // Try another trick:
                // Open Control Panel to create the registry entry
                Process.Start(FILE_NAME);
                Thread.Sleep(1000);
                // Try again
                soundDialogTitle = GetLocalizedSoundDialogTitle();
                // Still no luck :(
                if (soundDialogTitle == String.Empty)
                    return IntPtr.Zero;
                else
                    if (OnSoundDialogTitleFound != null)
                        OnSoundDialogTitleFound(this, new SoundDialogTitleEventArgs(soundDialogTitle));
            }

            // Start the process
            Process p = Process.Start(FILE_NAME, String.Format(ARGUMENTS, tabIndex));

            // Wait for the window to be created
            TimeSpan timeout = TimeSpan.FromSeconds(5);
            Stopwatch sw = Stopwatch.StartNew();
            IntPtr soundDialogHandle = IntPtr.Zero;
            while (sw.Elapsed < timeout && soundDialogHandle == IntPtr.Zero)
            {
                soundDialogHandle = NativeMethods.FindWindow(null, soundDialogTitle);
            }
            sw.Stop();

            return soundDialogHandle;
        }

        private void CloseDialog(AutomationElement dialog)
        {
            WindowPattern windowPattern = dialog.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern;
            if (windowPattern != null)
                windowPattern.Close();
        }

        private bool SwitchTab(AutomationElement tabControl, int tabIndex)
        {
            // Get a collection of tab pages
            AutomationElementCollection aeTabItems = tabControl.FindAll(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem));

            if (!(tabIndex < aeTabItems.Count))
                return false;

            // Set focus to the Listen tab
            AutomationElement aeListenTab = aeTabItems[tabIndex];
            aeListenTab.SetFocus();

            return true;
        }

        private List<SoundDevice> GetDeviceList(AutomationElement aeSoundDialog)
        {
            List<SoundDevice> devices = new List<SoundDevice>();

            // Get the ListView
            AutomationElement aeListView = aeSoundDialog.FindFirst
            (TreeScope.Descendants, new PropertyCondition
            (AutomationElement.AutomationIdProperty, CID_DEVICE_LISTVIEW.ToString()));

            TreeWalker walker = TreeWalker.ContentViewWalker;
            for (AutomationElement child = walker.GetFirstChild(aeListView);
                child != null; child = walker.GetNextSibling(child))
            {
                Debug.WriteLine(child.Current.Name);
                SoundDevice dev = new SoundDevice();
                dev.deviceName = child.Current.Name;
                dev.automationElement = child;
                devices.Add(dev);
            }

            return devices;
        }

        public bool SetDefaultDevice(AutomationElement aeSoundDialog, AutomationElement aeDevice)
        {
            //if (!(deviceIndex < recordingDevices.Count))
            //    return false;
            SelectionItemPattern select = aeDevice.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            select.Select();

            AutomationElement aeSetDefaultBtn = aeSoundDialog.FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, CID_SET_DEFAULT_BTN.ToString()));

            if (!aeSetDefaultBtn.Current.IsEnabled)
                return false;

            InvokePattern invokePattern = aeSetDefaultBtn.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            invokePattern.Invoke();

            return true;
        }

        public bool ToggleListenToThisDevice(AutomationElement aeSoundDialog, AutomationElement aeDevice, bool toggleState)
        {
            //if (!(deviceIndex < recordingDevices.Count))
            //    return false;

            SelectionItemPattern select = aeDevice.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern;
            select.Select();

            // Get the Properties button
            AutomationElement aePropertiesBtn = aeSoundDialog.FindFirst(
                TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, CID_PROPERTIES_BTN.ToString()));

            var invokePattern = aePropertiesBtn.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            invokePattern.Invoke();

            // Wait until the window is shown
            TimeSpan timeout = TimeSpan.FromSeconds(2);
            Stopwatch sw = Stopwatch.StartNew();
            AutomationElement aePropertiesDialog = aeSoundDialog.FindFirst(
                TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            while (sw.Elapsed < timeout && aePropertiesDialog == null)
            {
                aePropertiesDialog = aeSoundDialog.FindFirst(
                    TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            }
            sw.Stop();

            if (aePropertiesDialog == null)
                return false;

            // Get the tabs control
            AutomationElement aeTab = aePropertiesDialog.FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Tab));

            // Set focus to the Listen tab
            if (!SwitchTab(aeTab, 1))
                return false;

            // Get the checkbox
            AutomationElement aeListenToThisDeviceChb = aePropertiesDialog.FindFirst
                (TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, CID_LISTEN_TO_THIS_DEVICE_CHB.ToString()));

            if (aeListenToThisDeviceChb == null)
                return false;

            // Toggle if needed
            var togglePattern = aeListenToThisDeviceChb.GetCurrentPattern(TogglePattern.Pattern) as TogglePattern;
            if (togglePattern.Current.ToggleState != (toggleState ? ToggleState.On : ToggleState.Off))
                togglePattern.Toggle();

            // Apply
            AutomationElement aeApplyBtn = aeSoundDialog.FindFirst(
                TreeScope.Descendants, new PropertyCondition(AutomationElement.AutomationIdProperty, CID_APPLY_BTN.ToString()));

            if (!aeApplyBtn.Current.IsEnabled)
                return false;

            invokePattern = aeApplyBtn.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
            invokePattern.Invoke();

            return true;
        }
    }
}
