using System;
using System.Runtime.InteropServices;

namespace LibVLC
{
    /// <summary>
    /// Official libVLC documentation:
    /// http://www.videolan.org/developers/vlc/doc/doxygen/html/group__libvlc.html
    /// Based on:
    /// http://www.helyar.net/2009/libvlc-media-player-in-c-part-2/ by George Helyar
    /// Modified to include more functions and work on .NET 4
    /// </summary>
    public static class LibVlc
    {
        #region core
        [DllImport("libvlc", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_new(int argc, [MarshalAs(UnmanagedType.LPArray,
          ArraySubType = UnmanagedType.LPStr)] string[] argv);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_release(IntPtr instance);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr libvlc_get_version();

        public static string GetLibVlcVersion()
        {
            IntPtr libVlcVersion = LibVlc.libvlc_get_version();
            return Marshal.PtrToStringAnsi(libVlcVersion);
        }

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_event_attach(IntPtr p_event_manager, libvlc_event_e i_event_type, EventCallbackDelegate f_callback, IntPtr user_data);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void EventCallbackDelegate(IntPtr userdata);

        public enum libvlc_event_e
        {
            /* Append new event types at the end of a category.
            * Do not remove, insert or re-order any entry.
            * Keep this in sync with src/control/event.c:libvlc_event_type_name(). */
            libvlc_MediaMetaChanged = 0,
            libvlc_MediaSubItemAdded,
            libvlc_MediaDurationChanged,
            libvlc_MediaParsedChanged,
            libvlc_MediaFreed,
            libvlc_MediaStateChanged,

            libvlc_MediaPlayerMediaChanged = 0x100,
            libvlc_MediaPlayerNothingSpecial,
            libvlc_MediaPlayerOpening,
            libvlc_MediaPlayerBuffering,
            libvlc_MediaPlayerPlaying,
            libvlc_MediaPlayerPaused,
            libvlc_MediaPlayerStopped,
            libvlc_MediaPlayerForward,
            libvlc_MediaPlayerBackward,
            libvlc_MediaPlayerEndReached,
            libvlc_MediaPlayerEncounteredError,
            libvlc_MediaPlayerTimeChanged,
            libvlc_MediaPlayerPositionChanged,
            libvlc_MediaPlayerSeekableChanged,
            libvlc_MediaPlayerPausableChanged,
            libvlc_MediaPlayerTitleChanged,
            libvlc_MediaPlayerSnapshotTaken,
            libvlc_MediaPlayerLengthChanged,

            libvlc_MediaListItemAdded = 0x200,
            libvlc_MediaListWillAddItem,
            libvlc_MediaListItemDeleted,
            libvlc_MediaListWillDeleteItem,

            libvlc_MediaListViewItemAdded = 0x300,
            libvlc_MediaListViewWillAddItem,
            libvlc_MediaListViewItemDeleted,
            libvlc_MediaListViewWillDeleteItem,

            libvlc_MediaListPlayerPlayed = 0x400,
            libvlc_MediaListPlayerNextItemSet,
            libvlc_MediaListPlayerStopped,

            libvlc_MediaDiscovererStarted = 0x500,
            libvlc_MediaDiscovererEnded,

            libvlc_VlmMediaAdded = 0x600,
            libvlc_VlmMediaRemoved,
            libvlc_VlmMediaChanged,
            libvlc_VlmMediaInstanceStarted,
            libvlc_VlmMediaInstanceStopped,
            libvlc_VlmMediaInstanceStatusInit,
            libvlc_VlmMediaInstanceStatusOpening,
            libvlc_VlmMediaInstanceStatusPlaying,
            libvlc_VlmMediaInstanceStatusPause,
            libvlc_VlmMediaInstanceStatusEnd,
            libvlc_VlmMediaInstanceStatusError,
        };
        #endregion

        #region media
        [DllImport("libvlc", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_new_location(IntPtr p_instance,
          [MarshalAs(UnmanagedType.LPStr)] string psz_mrl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_new_path(IntPtr p_instance,
          [MarshalAs(UnmanagedType.LPStr)] string psz_mrl);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_release(IntPtr p_meta_desc);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool libvlc_media_get_stats(IntPtr libvlc_media_t, ref libvlc_media_stats_t p_stats);

        [StructLayout(LayoutKind.Sequential)]
        public struct libvlc_media_stats_t
        {
            public int i_read_bytes;
            public float f_input_bitrate;
            public int i_demux_read_bytes;
            public float f_demux_bitrate;
            public int i_demux_corrupted;
            public int i_demux_discontinuity;
            public int i_decoded_video;
            public int i_decoded_audio;
            public int i_displayed_pictures;
            public int i_lost_pictures;
            public int i_played_abuffers;
            public int i_lost_abuffers;
            public int i_sent_packets;
            public int i_sent_bytes;
            public float f_send_bitrate;
        }
        #endregion

        #region media player
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_new_from_media(IntPtr media);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_release(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_hwnd(IntPtr player, IntPtr drawable);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_get_media(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_set_media(IntPtr player, IntPtr media);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_media_player_play(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_pause(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_media_player_stop(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_toggle_fullscreen(IntPtr player);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_set_fullscreen(IntPtr player, int b_fullscreen);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_media_player_event_manager(IntPtr player);
        #endregion

        #region video
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_crop_geometry(IntPtr player,
            [MarshalAs(UnmanagedType.LPStr)] string psz_geometry);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_logo_int(IntPtr player,
            uint option,
            int value);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_video_set_logo_string(IntPtr player,
            uint option,
            [MarshalAs(UnmanagedType.LPStr)] string psz_value);

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int libvlc_video_get_size(IntPtr player,
            uint num,
            out uint px,
            out uint py);

        public enum libvlc_video_logo_option_t
        {
            libvlc_logo_enable,
            libvlc_logo_file,
            libvlc_logo_x,
            libvlc_logo_y,
            libvlc_logo_delay,
            libvlc_logo_repeat,
            libvlc_logo_opacity,
            libvlc_logo_position
        }
        #endregion

        #region exception
        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern void libvlc_clearerr();

        [DllImport("libvlc", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr libvlc_errmsg();
        #endregion
    }

    class VlcException : Exception
    {
        protected string _err;

        public VlcException()
            : base()
        {
            IntPtr errorPointer = LibVlc.libvlc_errmsg();
            _err = errorPointer == IntPtr.Zero ? "VLC Exception"
                : Marshal.PtrToStringAuto(errorPointer);
        }

        public override string Message { get { return _err; } }
    }

    public class VlcInstance : IDisposable
    {
        internal IntPtr Handle;

        public VlcInstance(string[] args)
        {
            Handle = LibVlc.libvlc_new(args.Length, args);
            if (Handle == IntPtr.Zero) throw new VlcException();
        }

        public void Dispose()
        {
            LibVlc.libvlc_release(Handle);
        }
    }

    public class VlcMedia : IDisposable
    {
        internal IntPtr Handle;
        
        public VlcMedia(VlcInstance instance, string url)
        {
            Handle = LibVlc.libvlc_media_new_path(instance.Handle, url); //LibVlc.libvlc_media_new_location(instance.Handle, url);
            if (Handle == IntPtr.Zero) throw new VlcException();
        }

        internal VlcMedia(IntPtr handle)
        {
            this.Handle = handle;
        }

        internal LibVlc.libvlc_media_stats_t GetStats()
        {
           LibVlc.libvlc_media_stats_t stats = new LibVlc.libvlc_media_stats_t();
           LibVlc.libvlc_media_get_stats(Handle, ref stats);
           
           return stats;
        }

        public void Dispose()
        {
            LibVlc.libvlc_media_release(Handle);
        }
    }

    public class VlcMediaPlayer : IDisposable
    {
        internal IntPtr Handle;
        private IntPtr drawable;
        private IntPtr eventManager;
        private bool playing, paused;

        public VlcMediaPlayer(VlcMedia media)
        {
            Handle = LibVlc.libvlc_media_player_new_from_media(media.Handle);
            if (Handle == IntPtr.Zero) throw new VlcException();
            eventManager = LibVlc.libvlc_media_player_event_manager(Handle);
        }

        public void Dispose()
        {
            LibVlc.libvlc_media_player_release(Handle);
        }

        public IntPtr Drawable
        {
            get
            {
                return drawable;
            }
            set
            {
                LibVlc.libvlc_media_player_set_hwnd(Handle, value);
                drawable = value;
            }
        }

        public VlcMedia Media
        {
            get
            {
                IntPtr media = LibVlc.libvlc_media_player_get_media(Handle);
                if (media == IntPtr.Zero) return null;
                return new VlcMedia(media);
            }
            set
            {
                LibVlc.libvlc_media_player_set_media(Handle, value.Handle);
            }
        }

        public bool IsPlaying { get { return playing && !paused; } }

        public bool IsPaused { get { return playing && paused; } }

        public bool IsStopped { get { return !playing; } }

        public void Play()
        {
            int ret = LibVlc.libvlc_media_player_play(Handle);
            if (ret == -1)
                throw new VlcException();

            playing = true;
            paused = false;
        }

        public void Pause()
        {
            LibVlc.libvlc_media_player_pause(Handle);

            if (playing)
                paused ^= true;
        }

        public void Stop()
        {
            LibVlc.libvlc_media_player_stop(Handle);

            playing = false;
            paused = false;
        }

        public void EventAttach(LibVlc.libvlc_event_e eventType, LibVlc.EventCallbackDelegate callbackDelegate)
        {
            LibVlc.libvlc_event_attach(eventManager, eventType, callbackDelegate, IntPtr.Zero);
        }

        public void UnSetCropGeometry()
        {
            LibVlc.libvlc_video_set_crop_geometry(Handle, null);
        }

        public void SetCropGeometry(int left, int top, int width, int height)
        {
            // psz_geometry is undocumented
            // see: https://forum.videolan.org/viewtopic.php?f=32&t=78702, https://forum.videolan.org/viewtopic.php?f=32&t=112281

            // right x bottom + left + top works :)
            LibVlc.libvlc_video_set_crop_geometry(Handle, String.Format("{0}x{1}+{2}+{3}", left+width, top+height, left, top));
        }

        public void SetLogoString(string value)
        {
            LibVlc.libvlc_video_set_logo_string(Handle, 1, value);
        }

        public void SetLogoInt(uint option, int value)
        {
            LibVlc.libvlc_video_set_logo_int(Handle, option, value);
        }

        public void ToggleFullscreen()
        {
            LibVlc.libvlc_set_fullscreen(Handle,1);
        }

        public System.Drawing.Size GetSize()
        {
            uint w, h;
            if (LibVlc.libvlc_video_get_size(Handle, 0, out w, out h) == 0)
            {
                return new System.Drawing.Size((int)w, (int)h);
            }
            else
            {
                return new System.Drawing.Size(0, 0);
            }
        }
    }
}
