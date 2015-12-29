using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;
using System.Configuration;

namespace JFPGeneric
{
    public static class AppSettingsExtensions
    {
        /// <summary>
        /// Returns an application setting based on the passed in string
        /// used primarily to cut down on typing
        /// e.g.: Foo.Text = "SettingName".AppSetting(); 
        /// </summary>
        /// <param name="Key">The name of the key</param>
        /// <returns>
        /// The value of the app setting in the web.Config
        /// or String.Empty if no setting found
        /// </returns>
        public static string AppSetting(this string Key)
        {
            string ret = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings[Key] != null)
                ret = ConfigurationManager.AppSettings[Key];
            return ret;
        }
    }
}
