﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iMposter.Model {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class ModelSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ModelSettings defaultInstance = ((ModelSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ModelSettings())));
        
        public static ModelSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public int cameraIndex {
            get {
                return ((int)(this["cameraIndex"]));
            }
            set {
                this["cameraIndex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool useFakeCamera {
            get {
                return ((bool)(this["useFakeCamera"]));
            }
            set {
                this["useFakeCamera"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Sensor\\TrackerConfig.xml")]
        public string sensorTrackerConfig {
            get {
                return ((string)(this["sensorTrackerConfig"]));
            }
            set {
                this["sensorTrackerConfig"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("640")]
        public int sensorCameraResolutionWidth {
            get {
                return ((int)(this["sensorCameraResolutionWidth"]));
            }
            set {
                this["sensorCameraResolutionWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("480")]
        public int sensorCameraResolutionHeight {
            get {
                return ((int)(this["sensorCameraResolutionHeight"]));
            }
            set {
                this["sensorCameraResolutionHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2048")]
        public int sensorCameraResolutionDepth {
            get {
                return ((int)(this["sensorCameraResolutionDepth"]));
            }
            set {
                this["sensorCameraResolutionDepth"] = value;
            }
        }
    }
}
