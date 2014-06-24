using System;
using DeckLinkAPI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;

namespace DeckLinkDotNetStreamer
{
    /// <summary>
    /// Module created to provide a working DeckLink API for .NET with additional funtionality for the framework.
    /// Contains a workaround for the non-working streaming part of the original API.
    /// Note: requires multi threaded apartment.
    /// </summary>
    public sealed class DeckLinkStreamer : IBMDStreamingDeviceNotificationCallback, IBMDStreamingH264InputCallback
    {
        // DeckLink
        private IDeckLinkIterator deckLinkIterator;
        private IBMDStreamingDiscovery streamingDiscovery;
        private IDeckLink streamingDevice;
        private IBMDStreamingDeviceInput streamingDeviceInput;
        private _BMDStreamingDeviceMode deviceMode;
        private _BMDDisplayMode inputMode;
        private DisplayModeEntry currentInputMode;
        private List<DisplayModeEntry> displayModes;

        private string _deckLinkAPIVersion;
        private bool _isStreaming;

        private const int MinBitrate = 1000;
        private const int MaxBitrate = 20000;

        public const int TsPacketSize = 188;

        // Instances
        private TcpStreamHandler tcpStreamHandler;
        private IDeckLinkStreamerCallback callback;

        // Constructor
        public DeckLinkStreamer()
        {
            _isStreaming = false;
            tcpStreamHandler = new TcpStreamHandler();
            tcpStreamHandler.Mpeg2TsReceived += tcpStreamHandler_Mpeg2TsReceived;
        }

        // Properties
        public string DeckLinkAPIVersion
        {
            get { return _deckLinkAPIVersion; }
        }

        public bool IsStreaming
        {
            get { return _isStreaming; }
        }

        public IBMDStreamingMutableVideoEncodingMode EncodingMode { get; set; }

        /// <summary>
        /// Tries to initialise the DeckLink Streaming API
        /// </summary>
        /// <returns>Returns true on success, false on failure.</returns>
        public bool TryInitializeAPI()
        {
            // Initialise Blackmagic API
            try
            {
                deckLinkIterator = new CDeckLinkIterator();
                if (deckLinkIterator == null)
                {
                    throw new COMException();
                }
            }
            catch (COMException)
            {
                return false;
            }

            _deckLinkAPIVersion = GetDeckLinkAPIVersion(deckLinkIterator);
            Debug.WriteLine("DeckLink API v" + DeckLinkAPIVersion);

            // Initialise Blackmagic Streaming API
            try
            {
                streamingDiscovery = new CBMDStreamingDiscovery();
                if (streamingDiscovery == null)
                {
                    throw new COMException();
                }
            }
            catch (COMException)
            {
                return false;
            }

            // Note: at this point you may get device notification messages!
            streamingDiscovery.InstallDeviceNotifications(this);
            Debug.WriteLine("Device notifications installed");

            return true;
        }

        /// <summary>
        /// Sets the notification callback.
        /// </summary>
        /// <param name="callback">Callback implementing the IDeckLinkStreamerCallback interface. Use <i>null</i> parameter to remove the callback.</param>
        public void SetCallback(IDeckLinkStreamerCallback callback)
        {
            this.callback = callback;
            Debug.WriteLine("Callback installed");
        }

        /// <summary>
        /// Starts the encoding process.
        /// </summary>
        /// <param name="encodingMode">The encoding profile to use.</param>
        /// <param name="bitrate">Bitrate in kbps. Ranges between 1000 and 20000.</param>
        public void StartStreaming(EncodingModeEntry encodingMode, int bitrate)
        {
            if (_isStreaming || tcpStreamHandler == null)
            {
                return;
            }

            if (bitrate < MinBitrate || bitrate > MaxBitrate)
            {
                return;
            }

            IBMDStreamingMutableVideoEncodingMode mutableVideoEncodingMode;
            encodingMode.encodingMode.CreateMutableVideoEncodingMode(out mutableVideoEncodingMode);
            mutableVideoEncodingMode.SetInt(_BMDStreamingEncodingModePropertyID.bmdStreamingEncodingPropertyVideoBitRateKbps, bitrate);
            streamingDeviceInput.SetVideoEncodingMode(mutableVideoEncodingMode);

            // Use tcp command to start (streamingDeviceInput.StartCapture() causes memory leak)
            tcpStreamHandler.StartReceiving();

            _isStreaming = true;
        }

        /// <summary>
        /// Stops the encoding process.
        /// </summary>
        public void StopStreaming()
        {
            _isStreaming = false;
            tcpStreamHandler.StopReceiving();

            if (streamingDeviceInput != null)
            {
                streamingDeviceInput.StopCapture();
            }
        }

        // Private methods

        /// <summary>
        /// Gets the detected DeckLink API version
        /// </summary>
        /// <returns>Returns the version as a string in the following format: major.minor.point</returns>
        private static string GetDeckLinkAPIVersion(IDeckLinkIterator deckLinkIterator)
        {
            IDeckLinkAPIInformation deckLinkAPIInformation = (IDeckLinkAPIInformation)deckLinkIterator;
            long deckLinkVersion;

            deckLinkAPIInformation.GetInt(_BMDDeckLinkAPIInformationID.BMDDeckLinkAPIVersion, out deckLinkVersion);

            int dlVerMajor = (int)((deckLinkVersion & 0xFF000000) >> 24);
            int dlVerMinor = (int)((deckLinkVersion & 0x00FF0000) >> 16);
            int dlVerPoint = (int)((deckLinkVersion & 0x0000FF00) >> 8);
            return String.Format("{0}.{1}.{2}", dlVerMajor, dlVerMinor, dlVerPoint);
        }

        /// <summary>
        /// Gets the list of available encoding presets for the given input mode.
        /// </summary>
        /// <param name="inputMode">The input mode</param>
        /// <returns>Returns the list of the supported encoding presets.</returns>
        private List<EncodingModeEntry> GetEncodingPresetsForInputMode(_BMDDisplayMode inputMode)
        {
            List<EncodingModeEntry> encodingPresets = new List<EncodingModeEntry>();

            IBMDStreamingVideoEncodingModePresetIterator presetIterator;
            streamingDeviceInput.GetVideoEncodingModePresetIterator(inputMode, out presetIterator);

            while (true)
            {
                IBMDStreamingVideoEncodingMode encodingMode;

                presetIterator.Next(out encodingMode);
                if (encodingMode == null)
                {
                    break;
                }

                encodingPresets.Add(new EncodingModeEntry(encodingMode));
            }

            return encodingPresets;
        }

        public void Dispose()
        {
            if (callback != null)
            {
                callback.DeviceRemoved();
            }
            Debug.WriteLine("Disposed");
        }

        private void tcpStreamHandler_Mpeg2TsReceived(object sender, TcpStreamHandler.Mpeg2TsReceivedEventArgs e)
        {
            if (callback != null)
            {
                callback.Mpeg2TsReceived(e.Mpeg2Ts);
            }
        }

        #region IBMDStreamingDeviceNotificationCallback interface implementation
        void IBMDStreamingDeviceNotificationCallback.StreamingDeviceArrived(IDeckLink device)
        {
            // Check we don't already have a device.
            if (streamingDevice != null)
            {
                return;
            }

            // See if it can do input:
            streamingDeviceInput = device as IBMDStreamingDeviceInput;
            if (streamingDeviceInput == null)
            {
                // This device doesn't support input. We can ignore this device.
                return;
            }

            // Ok, we're happy with this device, hold a reference to the device.
            streamingDevice = device;
            string deviceName;
            streamingDevice.GetDisplayName(out deviceName);

            Debug.WriteLine("Device arrived: " + deviceName);

            // Now install our callbacks.
            streamingDeviceInput.SetCallback(this);
            Debug.WriteLine("Device callback installed");

            // Add video input modes:
            IDeckLinkDisplayModeIterator inputModeIterator;
            streamingDeviceInput.GetVideoInputModeIterator(out inputModeIterator);

            _BMDDisplayMode currentInputModeValue;
            streamingDeviceInput.GetCurrentDetectedVideoInputMode(out currentInputModeValue);

            displayModes = new List<DisplayModeEntry>();
            IDeckLinkDisplayMode inputMode;
            while (true)
            {
                inputModeIterator.Next(out inputMode);
                if (inputMode == null)
                {
                    break;
                }

                DisplayModeEntry displayModeEntry = new DisplayModeEntry(inputMode);
                displayModes.Add(displayModeEntry);

                if (inputMode.GetDisplayMode() == currentInputModeValue)
                {
                    currentInputMode = displayModeEntry;
                }
            }

            tcpStreamHandler.GetDeviceId();

            if (callback != null)
            {
                callback.DeviceArrived(deviceName, currentInputMode, displayModes, GetEncodingPresetsForInputMode(currentInputModeValue));
            }
        }

        void IBMDStreamingDeviceNotificationCallback.StreamingDeviceModeChanged(IDeckLink device, _BMDStreamingDeviceMode mode)
        {
            if (mode == deviceMode)
            {
                return;
            }

            deviceMode = mode;

            if (callback != null)
            {
                callback.DeviceModeChanged(mode);
            }
        }

        void IBMDStreamingDeviceNotificationCallback.StreamingDeviceRemoved(IDeckLink device)
        {
            // We only care about removal of the device we are using
            if (device != streamingDevice)
            {
                return;
            }

            streamingDeviceInput.SetCallback(null);

            streamingDeviceInput = null;
            streamingDevice = null;

            StopStreaming();

            if (callback != null)
            {
                callback.DeviceRemoved();
            }
        }
        #endregion

        #region IBMDStreamingH264InputCallback interface implementation
        void IBMDStreamingH264InputCallback.H264AudioPacketArrived(IBMDStreamingAudioPacket audioPacket)
        {
            // not working
        }

        void IBMDStreamingH264InputCallback.H264NALPacketArrived(IBMDStreamingH264NALPacket nalPacket)
        {
            // not working
        }

        void IBMDStreamingH264InputCallback.H264VideoInputConnectorChanged()
        {
            IDeckLinkConfiguration deckLinkConfiguration = streamingDevice as IDeckLinkConfiguration;
            if (deckLinkConfiguration == null)
            {
                return;
            }

            long value;
            deckLinkConfiguration.GetInt(_BMDDeckLinkConfigurationID.bmdDeckLinkConfigVideoInputConnection, out value);
            _BMDVideoConnection videoConnection = (_BMDVideoConnection)value;

            if (callback != null)
            {
                callback.VideoInputConnectorChanged(videoConnection);
            }
        }

        void IBMDStreamingH264InputCallback.H264VideoInputConnectorScanningChanged()
        {
            IDeckLinkConfiguration deckLinkConfiguration = streamingDevice as IDeckLinkConfiguration;
            if (deckLinkConfiguration == null)
            {
                return;
            }

            int scanning;
            deckLinkConfiguration.GetFlag(_BMDDeckLinkConfigurationID.bmdDeckLinkConfigVideoInputScanning, out scanning);

            if (callback != null)
            {
                callback.VideoInputConnectorScanningChanged(scanning == 1);
            }
        }

        void IBMDStreamingH264InputCallback.H264VideoInputModeChanged()
        {
            streamingDeviceInput.GetCurrentDetectedVideoInputMode(out inputMode);

            // reset current input mode to empty (unknown display mode is not in list)
            currentInputMode = new DisplayModeEntry();

            foreach (DisplayModeEntry displayMode in displayModes)
            {
                if (((IDeckLinkDisplayMode)displayMode.displayMode).GetDisplayMode() == inputMode)
                {
                    currentInputMode = displayMode;
                    break;
                }
            }

            if (callback != null)
            {
                callback.InputModeChanged(currentInputMode, GetEncodingPresetsForInputMode(inputMode));
            }
        }

        void IBMDStreamingH264InputCallback.MPEG2TSPacketArrived(IBMDStreamingMPEG2TSPacket tsPacket)
        {
            // not working
        }
        #endregion
    }
}
