﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MISA.Fresher.Web12.Core.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResourceVietnam {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourceVietnam() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MISA.Fresher.Web12.Core.Resources.ResourceVietnam", typeof(ResourceVietnam).Assembly);
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
        ///   Looks up a localized string similar to {0} đã tồn tại, vui lòng nhập lại!.
        /// </summary>
        public static string PropNotDuplicated {
            get {
                return ResourceManager.GetString("PropNotDuplicated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} không được phép để trống!.
        /// </summary>
        public static string PropNotEmpty {
            get {
                return ResourceManager.GetString("PropNotEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Đã có lỗi xảy ra, vui lòng liên hệ với MISA!.
        /// </summary>
        public static string UserMsgServerError {
            get {
                return ResourceManager.GetString("UserMsgServerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} không được lớn hơn ngày hiện tại..
        /// </summary>
        public static string WrongDateFormat {
            get {
                return ResourceManager.GetString("WrongDateFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} không đúng định dạng..
        /// </summary>
        public static string WrongFormat {
            get {
                return ResourceManager.GetString("WrongFormat", resourceCulture);
            }
        }
    }
}
