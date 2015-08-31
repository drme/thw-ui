using System;
using ThW.UI.Utils.Native;

namespace ThW.UI.Utils.FilesSystem
{
    /// <summary>
    /// Internal utilities for files management.
    /// </summary>
    internal class FileUtils
    {
        static FileUtils()
        {
#if XBOX360
            platform = new PlatformXbox360();
#elif WINDOWS
            platform = new PlatformWindows();
#elif WINDOWS_PHONE
            platform = new PlatformWP7();
#elif NETFX_CORE
            platform = new PlatformWinRT();
#else
			platform = new PlatformWinRT();
//			platform = new PlatformUnix();
#endif
        }

        /// <summary>
        /// Gets ssytenm folder path.
        /// </summary>
        /// <param name="fileType">system folder type.</param>
        /// <returns>system folder path or empty string if not found.</returns>
        internal static String GetSystemFolder(FileTypes fileType)
        {
            switch (fileType)
            {
                case FileTypes.MyDocuments:
                    return platform.GetMyDocumentsFolder();
                case FileTypes.Desktop:
                    return platform.GetDesktopFolder();
                default:
                    return "";
            }
        }

        /// <summary>
        /// Platform specific API.
        /// </summary>
        internal static IPlatform Platform
        {
            get
            {
                return platform;
            }
        }

        private static IPlatform platform = null;
    }
}
