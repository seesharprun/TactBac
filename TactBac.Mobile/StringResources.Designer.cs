﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TactBac.Mobile {
    using System;
    using System.Reflection;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class StringResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StringResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TactBac.Mobile.StringResources", typeof(StringResources).GetTypeInfo().Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Thank you. Your should receive an e-mail within the next 12-24 hours with your formatted data..
        /// </summary>
        public static string ConfirmationPage_HeaderLabel_Text {
            get {
                return ResourceManager.GetString("ConfirmationPage_HeaderLabel_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must provide an e-mail address..
        /// </summary>
        public static string DestinationPage_Error_Message {
            get {
                return ResourceManager.GetString("DestinationPage_Error_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provide an e-mail where the formatted data will be sent..
        /// </summary>
        public static string DestinationPage_HeaderLabel_Text {
            get {
                return ResourceManager.GetString("DestinationPage_HeaderLabel_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Warning.
        /// </summary>
        public static string Error_Title {
            get {
                return ResourceManager.GetString("Error_Title", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must select a format..
        /// </summary>
        public static string FormatPage_Error_Message {
            get {
                return ResourceManager.GetString("FormatPage_Error_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select a file format for the export operation..
        /// </summary>
        public static string FormatPage_HeaderLabel_Text {
            get {
                return ResourceManager.GetString("FormatPage_HeaderLabel_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Start.
        /// </summary>
        public static string HomePage_StartButton_Text {
            get {
                return ResourceManager.GetString("HomePage_StartButton_Text", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You must have at least one contact selected to export..
        /// </summary>
        public static string ListPage_Error_Message {
            get {
                return ResourceManager.GetString("ListPage_Error_Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your contacts are listed below. You can remove contacts that you do not wish to include in the export operation..
        /// </summary>
        public static string ListPage_HeaderLabel_Text {
            get {
                return ResourceManager.GetString("ListPage_HeaderLabel_Text", resourceCulture);
            }
        }
    }
}