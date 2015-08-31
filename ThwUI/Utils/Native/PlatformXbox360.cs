using System;
using System.Collections.Generic;
using ThW.UI.Utils.FilesSystem;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Utils.Native
{
    class PlatformXbox360 : IPlatform
    {
        public IEnumerable<RealFolder> GetDrives(IRealFile parent)
        {
            List<RealFolder> lst = new List<RealFolder>();

            lst.Add(new RealFolder("T:\\", parent, "T:\\", FileTypes.Disk));
            lst.Add(new RealFolder("GAME:\\", parent, "GAME:\\", FileTypes.Disk));

            return lst;
        }

        public string GetMyDocumentsFolder()
        {
            throw new NotImplementedException();
        }

        public string GetDesktopFolder()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<String> GetDomains()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<String> GetServers(String domain)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<String> GetShares(String serverName)
        {
            throw new NotImplementedException();
        }

        public void FillFiles(String folder, ICollection<String> resultFiles)
        {
            throw new NotImplementedException();
        }

        public IImage GetMyComputerIcon(bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }

        public IImage GetMyDocumentsIcon(bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }

        public IImage GetDesktopIcon(bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }

        public IImage GetNetworkIcon(bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }

        public IImage GetShareIcon(bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }

        public IImage GetDomainIcon(bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }

        public IImage GetFileIcon(String fileName, bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }

        public IImage GetShareIcon(String shareName, bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }

        public IImage GetIcon(int id, bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }


        public object AddFontResource(byte[] fontFileData)
        {
            throw new NotImplementedException();
        }

        public void RemoveFontResource(object fontHandle)
        {
            throw new NotImplementedException();
        }

        public List<String> GetAvailableSystemFonts()
        {
            throw new NotImplementedException();
        }

        public byte[] ReadFile(String fileName, String workingFolder)
        {
            throw new NotImplementedException();
        }
    }
}
