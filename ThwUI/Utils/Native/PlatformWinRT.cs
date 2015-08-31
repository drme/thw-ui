using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThW.UI;
using ThW.UI.Utils;
using ThW.UI.Utils.FilesSystem;
using ThW.UI.Utils.Native;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Utils.FilesSystem
{
    class PlatformWinRT : IPlatform
    {
        public IEnumerable<RealFolder> GetDrives(IRealFile parent)
        {
            throw new NotImplementedException();
        }

        public string GetMyDocumentsFolder()
        {
            throw new NotImplementedException();
        }

        public string GetDesktopFolder()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetDomains()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetServers(string domain)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetShares(string serverName)
        {
            throw new NotImplementedException();
        }

        public void FillFiles(string folder, ICollection<string> resultFiles)
        {
            //throw new NotImplementedException();
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

        public IImage GetFileIcon(string fileName, bool large, UIEngine engine, Theme theme)
        {
            throw new NotImplementedException();
        }

        public IImage GetShareIcon(string shareName, bool large, UIEngine engine, Theme theme)
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


        public List<string> GetAvailableSystemFonts()
        {
            throw new NotImplementedException();
        }

        public byte[] ReadFile(string fileName, string workingFolder)
        {
            return null;
        }
    }
}
