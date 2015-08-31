using System;
using System.Collections.Generic;
using ThW.UI.Utils.FilesSystem;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Utils.Native
{
    class PlatformWP7 : IPlatform
    {
        public String GetMyDocumentsFolder()
        {
            throw new NotImplementedException();
        }

        public String GetDesktopFolder()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RealFolder> GetDrives(IRealFile parent)
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
            return null;
        }

        public Object AddFontResource(byte[] fontFileData)
        {
            throw new NotImplementedException();
        }

        public void RemoveFontResource(Object fontHandle)
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
