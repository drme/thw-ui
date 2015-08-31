using System;
using System.Collections.Generic;
using ThW.UI.Utils.FilesSystem;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Utils.Native
{
    internal interface IPlatform
    {
        String GetMyDocumentsFolder();
        String GetDesktopFolder();
        IEnumerable<RealFolder> GetDrives(IRealFile parent);
        IEnumerable<String> GetDomains();
        IEnumerable<String> GetServers(String domain);
        IEnumerable<String> GetShares(String serverName);
        void FillFiles(String folder, ICollection<String> resultFiles);
        IImage GetMyComputerIcon(bool large, UIEngine engine, Theme theme);
        IImage GetMyDocumentsIcon(bool large, UIEngine engine, Theme theme);
        IImage GetDesktopIcon(bool large, UIEngine engine, Theme theme);
        IImage GetNetworkIcon(bool large, UIEngine engine, Theme theme);
        IImage GetShareIcon(bool large, UIEngine engine, Theme theme);
        IImage GetDomainIcon(bool large, UIEngine engine, Theme theme);
        IImage GetFileIcon(String fileName, bool large, UIEngine engine, Theme theme);
        IImage GetShareIcon(string shareName, bool large, UIEngine engine, Theme theme);
        IImage GetIcon(int id, bool large, UIEngine engine, Theme theme);
        Object AddFontResource(byte[] fontFileData);
        void RemoveFontResource(Object fontHandle);
        List<String> GetAvailableSystemFonts();
        byte[] ReadFile(String fileName, String workingFolder);
    }
}
