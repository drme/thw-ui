using System;
using System.Collections.Generic;
using ThW.UI.Utils.FilesSystem;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Utils.Native
{
    class PlatformUnix : IPlatform
    {
        public String GetHomeFolder()
        {
            return "~/";
        }

        public String GetMyDocumentsFolder()
        {
            return GetHomeFolder() + "Documents/";
        }

        public String GetDesktopFolder()
        {
            return GetHomeFolder() + "/Desktop/";
        }

        private IImage GetIcon(bool large, String name, UIEngine engine, Theme theme)
        {
            String iconsPath = theme.ThemeFolder + "/icons/fsys/";

            if (true == large)
            {
                return engine.CreateImage(iconsPath + name + "_32x32");
            }
            else
            {
                return engine.CreateImage(iconsPath + name + "_16x16");
            }
        }

        public IEnumerable<RealFolder> GetDrives(IRealFile parent)
        {
            return new List<RealFolder>();
        }

        public IEnumerable<String> GetDomains()
        {
            return new List<String>();
        }

        public IEnumerable<String> GetServers(String domain)
        {
            return new List<String>();
        }

        public IEnumerable<string> GetShares(String serverName)
        {
            return new List<String>();
        }

        public void FillFiles(String folder, ICollection<String> resultFiles)
        {
        }

        public IImage GetMyComputerIcon(bool large, UIEngine engine, Theme theme)
        {
            return GetIcon(large, "mypc", engine, theme);
        }

        public IImage GetMyDocumentsIcon(bool large, UIEngine engine, Theme theme)
        {
            return GetIcon(large, "mydoc", engine, theme);
        }

        public IImage GetDesktopIcon(bool large, UIEngine engine, Theme theme)
        {
            return GetIcon(large, "desktop", engine, theme);
        }

        public IImage GetNetworkIcon(bool large, UIEngine engine, Theme theme)
        {
			return GetIcon(large, "network", engine, theme);
		}

        public IImage GetShareIcon(bool large, UIEngine engine, Theme theme)
        {
			return GetIcon(large, "share", engine, theme);
		}

        public IImage GetDomainIcon(bool large, UIEngine engine, Theme theme)
        {
			return GetIcon(large, "domain", engine, theme);
		}

        public IImage GetFileIcon(String fileName, bool large, UIEngine engine, Theme theme)
        {
            return GetIcon(large, fileName.EndsWith("/") ? "folder" : "file", engine, theme);
        }

        public IImage GetShareIcon(String shareName, bool large, UIEngine engine, Theme theme)
        {
			return GetIcon(large, "share", engine, theme);
		}

        public IImage GetIcon(int id, bool large, UIEngine engine, Theme theme)
        {
            if (4 == id)
            {
                return GetIcon(large, "vfs", engine, theme);
            }

			return GetIcon(large, "generic", engine, theme);
		}

        public Object AddFontResource(byte[] fontFileData)
        {
			return null;
        }

        public void RemoveFontResource(Object fontHandle)
        {
        }

        public List<String> GetAvailableSystemFonts()
        {
			return new List<String>();
        }

        public byte[] ReadFile(String fileName, String workingFolder)
        {
			return null;
        }
    }
}
