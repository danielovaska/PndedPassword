using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrue.OwnedPassword
{
    public static class AppSettings
    {
        public static int RequiredLength { get; set; } 
        public static bool RequireNonLetterOrDigit { get; set; } 
        public static bool RequireDigit { get; set; } 
        public static bool RequireLowercase { get; set; }
        public static bool RequireUppercase { get; set; } 
        public static bool RequireOwnedPasswordsCheck { get; set; } 
        public static int MaxAllowedOwnedPasswords { get; set; }
        static AppSettings()
        {
            RequiredLength = Get<int>("OwnedPassword:RequiredLength", "0");
            RequireNonLetterOrDigit = Get<bool>("OwnedPassword:RequireNonLetterOrDigit", "false");
            RequireDigit = Get<bool>("OwnedPassword:RequireDigit", "false");
            RequireLowercase = Get<bool>("OwnedPassword:RequireLowercase", "false");
            RequireUppercase = Get<bool>("OwnedPassword:RequireUppercase", "false");
            RequireOwnedPasswordsCheck = Get<bool>("OwnedPassword:RequireOwnedPasswordsCheck", "true");
            MaxAllowedOwnedPasswords = Get<int>("OwnedPassword:MaxAllowedOwnedPasswords", "0");
        }
        public static T Get<T>(string key,string defaultValue)
        {
            var appSetting = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(appSetting))
            {
                appSetting = defaultValue;
            }
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)(converter.ConvertFromInvariantString(appSetting));
        }
    }

   
}
