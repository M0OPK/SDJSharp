﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDGrabSharp.CLI.Resources {
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
    internal class Strings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SDGrabSharp.CLI.Resources.Strings", typeof(Strings).Assembly);
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
        ///   Looks up a localized string similar to The configuration file {0} could not be loaded. Terminating.
        /// </summary>
        internal static string ConfigNotFound {
            get {
                return ResourceManager.GetString("ConfigNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SDGrabSharp CLI {0}
        ///  Usage:
        ///    --config &lt;config file&gt;            Specify configuration file to use
        ///    --importxmltv &lt;xmlTV filename&gt;    Specify external XMLTV file to import (multiple possible)
        ///    --quiet                           Don&apos;t output anything to console while running.
        /// </summary>
        internal static string HelpText {
            get {
                return ResourceManager.GetString("HelpText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Importing XMLTV file {0}.
        /// </summary>
        internal static string ImportingXMLTV {
            get {
                return ResourceManager.GetString("ImportingXMLTV", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Schedule write process complete.
        /// </summary>
        internal static string ProcessComplete {
            get {
                return ResourceManager.GetString("ProcessComplete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The only argument should be the path to an XML configuration file. No argument supplied will use SDGrabSharp.xml.
        /// </summary>
        internal static string TooManyArgs {
            get {
                return ResourceManager.GetString("TooManyArgs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The XmlTV file {0} could not be loaded. Terminating.
        /// </summary>
        internal static string XmlTVFileNotFound {
            get {
                return ResourceManager.GetString("XmlTVFileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to merge XmlTV file {0}. Terminating.
        /// </summary>
        internal static string XmlTVMergeFailed {
            get {
                return ResourceManager.GetString("XmlTVMergeFailed", resourceCulture);
            }
        }
    }
}
