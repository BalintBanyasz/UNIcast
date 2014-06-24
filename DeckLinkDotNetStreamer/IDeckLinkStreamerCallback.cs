using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeckLinkDotNetStreamer
{
    public interface IDeckLinkStreamerCallback
    {
        /// <summary>
        /// The DeviceArrived method is called when a new streaming device becomes available.
        /// </summary>
        void DeviceArrived(string deviceName, DisplayModeEntry inputMode, List<DisplayModeEntry> displayModes, List<EncodingModeEntry> encodingPresets);

        /// <summary>
        /// The DeviceRemoved method is called when a streaming device is removed.
        /// </summary>
        void DeviceRemoved();

        /// <summary>
        /// The DeviceModeChanged method is called when a streaming device’s mode has changed.
        /// </summary>
        void DeviceModeChanged(DeckLinkAPI._BMDStreamingDeviceMode mode);

        /// <summary>
        /// The InputModeChanged method is called when the streaming device detects a change to the video input display mode.
        /// </summary>
        void InputModeChanged(DisplayModeEntry inputMode, List<EncodingModeEntry> encodingPresets);

        /// <summary>
        /// The VideoInputConnectorChanged method is called when the streaming device detects a change to the input connector.
        /// </summary>
        void VideoInputConnectorChanged(DeckLinkAPI._BMDVideoConnection videoConnection);

        /// <summary>
        /// The VideoInputConnectorScanningChanged method is called when the input connect scanning mode has changed.
        /// </summary>
        void VideoInputConnectorScanningChanged(bool isScanning);

        /// <summary>
        /// The Mpeg2TsReceived method is called when the MPEG2 Transport Stream becomes available from the streaming device while a capture is in progress.
        /// </summary>
        void Mpeg2TsReceived(System.Net.Sockets.NetworkStream mpeg2ts);
    }
}
