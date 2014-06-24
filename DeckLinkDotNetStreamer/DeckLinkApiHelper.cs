using DeckLinkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeckLinkDotNetStreamer
{
    public struct DisplayModeEntry
    {
        public IDeckLinkDisplayMode displayMode;

        public DisplayModeEntry(IDeckLinkDisplayMode displayMode)
        {
            this.displayMode = displayMode;
        }

        public override string ToString()
        {
            string str = String.Empty;

            displayMode.GetName(out str);

            return str;
        }
    }

    public struct AudioSampleTypeEntry
    {
        public _BMDAudioSampleType sampleType;

        public AudioSampleTypeEntry(_BMDAudioSampleType sampleType)
        {
            this.sampleType = sampleType;
        }

        public override string ToString()
        {
            string str = String.Empty;

            switch (sampleType)
            {
                case _BMDAudioSampleType.bmdAudioSampleType16bitInteger:
                    str = "16 bit";
                    break;
                case _BMDAudioSampleType.bmdAudioSampleType32bitInteger:
                    str = "32 bit";
                    break;
                default:
                    break;
            }

            return str;
        }
    }

    public struct AudioSampleRateEntry
    {
        public _BMDAudioSampleRate sampleRate;

        public AudioSampleRateEntry(_BMDAudioSampleRate sampleRate)
        {
            this.sampleRate = sampleRate;
        }

        public override string ToString()
        {
            string str = String.Empty;

            switch (sampleRate)
            {
                case _BMDAudioSampleRate.bmdAudioSampleRate48kHz:
                    str = "48 kHz";
                    break;
                default:
                    break;
            }

            return str;
        }
    }

    public struct H264EntropyCodingEntry
    {
        public _BMDStreamingH264EntropyCoding entropyCoding;

        public H264EntropyCodingEntry(_BMDStreamingH264EntropyCoding entropyCoding)
        {
            this.entropyCoding = entropyCoding;
        }

        public override string ToString()
        {
            string str = String.Empty;

            switch (entropyCoding)
            {
                case _BMDStreamingH264EntropyCoding.bmdStreamingH264EntropyCodingCABAC:
                    str = "CABAC";
                    break;
                case _BMDStreamingH264EntropyCoding.bmdStreamingH264EntropyCodingCAVLC:
                    str = "CAVLC";
                    break;
                default:
                    break;
            }

            return str;
        }
    }

    public struct H264LevelEntry
    {
        public _BMDStreamingH264Level level;

        public H264LevelEntry(_BMDStreamingH264Level level)
        {
            this.level = level;
        }

        public override string ToString()
        {
            string str = String.Empty;

            switch (level)
            {
                case _BMDStreamingH264Level.bmdStreamingH264Level12:
                    str = "1.2";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level13:
                    str = "1.3";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level2:
                    str = "2";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level21:
                    str = "2.1";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level22:
                    str = "2.2";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level3:
                    str = "3";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level31:
                    str = "3.1";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level32:
                    str = "3.2";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level4:
                    str = "4";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level41:
                    str = "4.1";
                    break;
                case _BMDStreamingH264Level.bmdStreamingH264Level42:
                    str = "4.2";
                    break;
                default:
                    break;
            }

            return str;
        }
    }

    public struct H264ProfileEntry
    {
        public _BMDStreamingH264Profile profile;

        public H264ProfileEntry(_BMDStreamingH264Profile profile)
        {
            this.profile = profile;
        }

        public override string ToString()
        {
            string str = String.Empty;

            switch (profile)
            {
                case _BMDStreamingH264Profile.bmdStreamingH264ProfileBaseline:
                    str = "Baseline";
                    break;
                case _BMDStreamingH264Profile.bmdStreamingH264ProfileHigh:
                    str = "High";
                    break;
                case _BMDStreamingH264Profile.bmdStreamingH264ProfileMain:
                    str = "Main";
                    break;
                default:
                    break;
            }

            return str;
        }
    }

    public struct FrameRateEntry
    {
        public _BMDStreamingEncodingFrameRate frameRate;

        public FrameRateEntry(_BMDStreamingEncodingFrameRate frameRate)
        {
            this.frameRate = frameRate;
        }

        public override string ToString()
        {
            string str = String.Empty;

            switch (frameRate)
            {
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate2398p:
                    str = "2398p";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate24p:
                    str = "24p";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate25p:
                    str = "25p";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate2997p:
                    str = "2997p";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate30p:
                    str = "30p";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate50i:
                    str = "50i";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate50p:
                    str = "50p";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate5994i:
                    str = "5994i";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate5994p:
                    str = "5994p";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate60i:
                    str = "60i";
                    break;
                case _BMDStreamingEncodingFrameRate.bmdStreamingEncodedFrameRate60p:
                    str = "60p";
                    break;
                default:
                    break;
            }

            return str;
        }
    }

    public struct EncodingModeEntry
    {
        public IBMDStreamingVideoEncodingMode encodingMode;

        public EncodingModeEntry(IBMDStreamingVideoEncodingMode encodingMode)
        {
            this.encodingMode = encodingMode;
        }

        public override string ToString()
        {
            string str = String.Empty;

            encodingMode.GetName(out str);

            return str;
        }
    }
}
