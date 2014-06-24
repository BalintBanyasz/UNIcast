﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UNIcast_Streamer.res {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("UNIcast_Streamer.res.strings", typeof(strings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error.
        /// </summary>
        internal static string error {
            get {
                return ResourceManager.GetString("error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to read configuration. Your app.config may contain invalid values..
        /// </summary>
        internal static string errorConfig {
            get {
                return ResourceManager.GetString("errorConfig", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This application requires the DeckLink drivers installed.\nPlease install the Blackmagic DeckLink drivers to use the features of this application.
        /// </summary>
        internal static string errorDeckLinkDriver {
            get {
                return ResourceManager.GetString("errorDeckLinkDriver", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This application requires FFmpeg.\nPlease copy a working FFmpeg release into the ffmpeg folder.
        /// </summary>
        internal static string errorFFmpeg {
            get {
                return ResourceManager.GetString("errorFFmpeg", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to N/A.
        /// </summary>
        internal static string notApplicable {
            get {
                return ResourceManager.GetString("notApplicable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Record.
        /// </summary>
        internal static string record {
            get {
                return ResourceManager.GetString("record", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Encoding.
        /// </summary>
        internal static string statusEncoding {
            get {
                return ResourceManager.GetString("statusEncoding", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Idle.
        /// </summary>
        internal static string statusIdle {
            get {
                return ResourceManager.GetString("statusIdle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No device connected.
        /// </summary>
        internal static string statusNoDevice {
            get {
                return ResourceManager.GetString("statusNoDevice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No recording device found. Please connect a device to continue..
        /// </summary>
        internal static string statusScreenNoDevice {
            get {
                return ResourceManager.GetString("statusScreenNoDevice", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No video source found. Please connect a video source to the recording device to continue..
        /// </summary>
        internal static string statusScreenNoSource {
            get {
                return ResourceManager.GetString("statusScreenNoSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ready to stream. Click Next to continue..
        /// </summary>
        internal static string statusScreenReady {
            get {
                return ResourceManager.GetString("statusScreenReady", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stopping.
        /// </summary>
        internal static string statusStopping {
            get {
                return ResourceManager.GetString("statusStopping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stop.
        /// </summary>
        internal static string stop {
            get {
                return ResourceManager.GetString("stop", resourceCulture);
            }
        }
    }
}
