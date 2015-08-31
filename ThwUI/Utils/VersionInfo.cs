using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Information about library
    /// </summary>
    public class VersionInfo
    {
        /// <summary>
        /// Returns library version.
        /// </summary>
        public static String Version
        {
            get
            {
                return "1.0.0.0";
            }
        }

        /// <summary>
        /// Returns library build number.
        /// </summary>
        public static String Build
        {
            get
            {
                return "NOW";
            }
        }

        /// <summary>
        /// Return library homepage.
        /// </summary>
        public static String Url
        {
            get
            {
                return "http://twn.sourceforge.net/ui/";
            }
        }
    }
}
