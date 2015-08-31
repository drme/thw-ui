#if WINDOWS

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ThW.UI.Utils.FilesSystem;
using ThW.UI.Utils.Themes;
using System.Drawing.Text;
using System.Drawing;

namespace ThW.UI.Utils.Native
{
    class PlatformWindows : IPlatform
    {
        public byte[] ReadFile(String fileName, String workingFolder)
        {
            Stream fileStream = null;

            try
            {
                fileStream = FileOpenRead(fileName);

                if (null == fileStream)
                {
                    fileStream = FileOpenRead(workingFolder + "/" + fileName);

                    if (null == fileStream)
                    {
                        fileStream = FileOpenRead("../" + workingFolder + "/" + fileName);
                    }
                }

                if (null != fileStream)
                {
                    long fileSize = fileStream.Length;

                    if (fileSize > 0)
                    {
                        byte[] fileBytes = new byte[fileSize];

                        fileStream.Read(fileBytes, 0, (int)fileSize);

                        return fileBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                if (null != fileStream)
                {
                    fileStream.Dispose();
                }
            }

            return null;
        }

        private static Stream FileOpenRead(String fileName)
        {
            try
            {
                if (true == File.Exists(fileName))
                {
                    return new FileStream(fileName, FileMode.Open, FileAccess.Read);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return null;
            }
        }

        public IImage GetMyComputerIcon(bool large, UIEngine engine, Theme theme)
        {
            return GetShell32IconWindows(15, large, engine);
        }

        public IImage GetMyDocumentsIcon(bool large, UIEngine engine, Theme theme)
        {
            return GetShell32IconWindows(126, large, engine);
        }

        public IImage GetDesktopIcon(bool large, UIEngine engine, Theme theme)
        {
            return GetShell32IconWindows(34, large, engine);
        }

        public IImage GetNetworkIcon(bool large, UIEngine engine, Theme theme)
        {
            return GetShell32IconWindows(17, large, engine);
        }

        public IImage GetShareIcon(bool large, UIEngine engine, Theme theme)
        {
            return GetShell32IconWindows(28, large, engine);
        }

        public IImage GetDomainIcon(bool large, UIEngine engine, Theme theme)
        {
            return GetShell32IconWindows(18, large, engine);
        }

        public IImage GetFileIcon(String fileName, bool large, UIEngine engine, Theme theme)
        {
            return GetFileIconWindows(fileName, (uint)FILE_ATTRIBUTE_NORMAL, (uint)(SHGFI_ICON | (large ? SHGFI_LARGEICON : SHGFI_SMALLICON)), engine, large);
        }

        public IImage GetShareIcon(string shareName, bool large, UIEngine engine, Theme theme)
        {
            return GetFileIconWindows(shareName, (uint)FILE_ATTRIBUTE_NORMAL, (uint)(SHGFI_ICON | (large ? SHGFI_LARGEICON : SHGFI_SMALLICON)), engine, large);
        }

        public IImage GetIcon(int id, bool large, UIEngine engine, Theme theme)
        {
            return GetShell32IconWindows(id, large, engine);
        }

        private IImage GetFileIconWindows(String fileName, uint fileFlags, uint flags, UIEngine engine, bool large)
        {
			SHFileInfo shellInfo = new SHFileInfo();

    		IntPtr status = SHGetFileInfo(UIUtils.ConvertToWindows(fileName), fileFlags, ref shellInfo, (uint)Marshal.SizeOf(shellInfo), flags);

			if (IntPtr.Zero != status)
			{
				IntPtr hIcon = shellInfo.hIcon;

				IImage texture = GetIconWindows(hIcon, engine, large);

				DestroyIcon(hIcon);

				return texture;
			}
			else
			{
				return null;
			}
        }

        private IImage GetShell32IconWindows(int id, bool large, UIEngine engine)
        {
            IntPtr[] hIcon = new IntPtr[] { IntPtr.Zero };

			if (true == large)
			{
                if (0 == ExtractIconEx(shell32dll, id, hIcon, null, 1))
                {
                    return null;
                }
			}
			else
			{
                if (0 == ExtractIconEx(shell32dll, id, null, hIcon, 1))
                {
                    return null;
                }
			}					

			IImage image = GetIconWindows(hIcon[0], engine, large);

			DestroyIcon(hIcon[0]);

			return image;
        }

        private IImage GetIconWindows(IntPtr hIcon, UIEngine engine, bool large)
        {
            IconInfo iconInfo = new IconInfo();

            if (true == GetIconInfo(hIcon, out iconInfo))
            {
                BitmapInfo bitmapInfo = new BitmapInfo();

                bitmapInfo.bmiHeader.biSize = Marshal.SizeOf(bitmapInfo);

                IntPtr dc = CreateCompatibleDC(IntPtr.Zero);

                int rez = GetDIBits(dc, iconInfo.hbmColor, 0, 0, null, ref bitmapInfo, Win32.DIB_RGB_COLORS);

                if (rez == 0)
                {
                    DeleteDC(dc);

                    return null;
                }

                byte[] pBuffer = new byte[bitmapInfo.bmiHeader.biWidth * bitmapInfo.bmiHeader.biHeight * 4];
                byte[] buffer = new byte[bitmapInfo.bmiHeader.biWidth * bitmapInfo.bmiHeader.biHeight * 4];

                rez = GetDIBits(dc, iconInfo.hbmColor, 0, (uint)bitmapInfo.bmiHeader.biHeight, buffer, ref bitmapInfo, Win32.DIB_RGB_COLORS);

                bool bAllAlphaTransparent = true;

                for (int y = 0; y < bitmapInfo.bmiHeader.biHeight; y++)
                {
                    for (int x = 0; x < bitmapInfo.bmiHeader.biWidth; x++)
                    {
                        pBuffer[y * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 0] = buffer[(bitmapInfo.bmiHeader.biHeight - y - 1) * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 2];
                        pBuffer[y * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 1] = buffer[(bitmapInfo.bmiHeader.biHeight - y - 1) * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 1];
                        pBuffer[y * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 2] = buffer[(bitmapInfo.bmiHeader.biHeight - y - 1) * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 0];
                        pBuffer[y * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 3] = buffer[(bitmapInfo.bmiHeader.biHeight - y - 1) * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 3];

                        if ((false != bAllAlphaTransparent) && (pBuffer[y * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 3] != 0x0))
                        {
                            bAllAlphaTransparent = false;
                        }
                    }
                }

                rez = GetDIBits(dc, iconInfo.hbmMask, 0, 0, null, ref bitmapInfo, Win32.DIB_RGB_COLORS);

                if ((rez == 0) || (false == bAllAlphaTransparent))
                {
                }
                else
                {
                    byte[] colorBits = new byte[4096 * 4];
                    byte[] maskBits = new byte[4096 * 4];
                    BitmapInfo bmi = new BitmapInfo();

                    bmi.bmiHeader.biSize = Marshal.SizeOf(bmi);
                    bmi.bmiHeader.biWidth = /*32;*/ bitmapInfo.bmiHeader.biWidth;
                    bmi.bmiHeader.biHeight = /*32;*/ bitmapInfo.bmiHeader.biHeight;
                    bmi.bmiHeader.biPlanes = 1;
                    bmi.bmiHeader.biBitCount = 32;
                    bmi.bmiHeader.biCompression = Win32.BI_RGB;

                    // Extract the color bitmap
                    int nBits = bitmapInfo.bmiHeader.biWidth * bitmapInfo.bmiHeader.biHeight * 32 / 8;
                    GetDIBits(dc, iconInfo.hbmColor, 0, 32, colorBits, ref bmi, Win32.DIB_RGB_COLORS);

                    // Extract the mask bitmap
                    GetDIBits(dc, iconInfo.hbmMask, 0, 32, maskBits, ref bmi, Win32.DIB_RGB_COLORS);

                    // Copy the mask alphas into the color bits
                    for (int i = 0; i < nBits; i++)
                    {
                        colorBits[i + 0] = (byte)(colorBits[i + 0] | ((maskBits[i + 0] != 0) ? 0 : 0xff));
                    }

                    memcpy(pBuffer, colorBits, bitmapInfo.bmiHeader.biWidth * bitmapInfo.bmiHeader.biHeight * 4);

                    byte[] buffer1 = colorBits;

                    for (int y = 0; y < bitmapInfo.bmiHeader.biHeight; y++)
                    {
                        for (int x = 0; x < bitmapInfo.bmiHeader.biWidth; x++)
                        {
                            pBuffer[y * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 0] = buffer1[(bitmapInfo.bmiHeader.biHeight - y - 1) * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 2];
                            pBuffer[y * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 1] = buffer1[(bitmapInfo.bmiHeader.biHeight - y - 1) * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 1];
                            pBuffer[y * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 2] = buffer1[(bitmapInfo.bmiHeader.biHeight - y - 1) * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 0];
                            pBuffer[y * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 3] = buffer1[(bitmapInfo.bmiHeader.biHeight - y - 1) * bitmapInfo.bmiHeader.biWidth * 4 + x * 4 + 3];
                        }
                    }
                }

                DeleteDC(dc);
                DeleteObject(iconInfo.hbmColor);
                DeleteObject(iconInfo.hbmMask);

                IImage resultImage = engine.CreateImage("icon_" + hIcon.ToInt64() + "_" + large, bitmapInfo.bmiHeader.biWidth, bitmapInfo.bmiHeader.biHeight, pBuffer);

                return resultImage;
            }

            return null;
        }

        public String GetMyDocumentsFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/";
        }

        public String GetDesktopFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/";
        }

        public IEnumerable<RealFolder> GetDrives(IRealFile parent)
        {
            List<RealFolder> lst = new List<RealFolder>();

            foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
            {
                FileTypes fileType = FileTypes.Disk;

                switch (driveInfo.DriveType)
                {
                    case DriveType.Removable:
                        if (driveInfo.Name.StartsWith("A:") || driveInfo.Name.StartsWith("B:"))
                        {
                            fileType = FileTypes.Floppy;
                        }
                        else
                        {
                            fileType = FileTypes.Disk;
                        }
                        break;
                    case DriveType.CDRom:
                        fileType = FileTypes.DVD;
                        break;
                    default:
                        fileType = FileTypes.Disk;
                        break;
                }

                lst.Add(new RealFolder(driveInfo.Name, parent, driveInfo.Name, fileType));
            }

            return lst;
        }

        public IEnumerable<String> GetDomains()
        {
            List<String> result = new List<String>();

            int entriesRead = 0;
            int totalEntries = 0;
            IntPtr pBuf = IntPtr.Zero;
            IntPtr resumeHandle = IntPtr.Zero;

            int status = NetServerEnum(null, 101, out pBuf, MAX_PREFERRED_LENGTH, ref entriesRead, ref totalEntries, SV101Types.SV_TYPE_DOMAIN_ENUM, null, resumeHandle);

            if ((NERR_Success == status) || (status == ERROR_MORE_DATA))
            {
                if (pBuf != IntPtr.Zero)
                {
                    IntPtr ptr = pBuf;

                    for (int i = 0; i < entriesRead; i++)
                    {
                        ServerInfo101 server = (ServerInfo101)Marshal.PtrToStructure(ptr, typeof(ServerInfo101));

                        ptr = (IntPtr)((ulong)ptr + (ulong)Marshal.SizeOf(server));

                        if (ptr == IntPtr.Zero)
                        {
                            break;
                        }

                        result.Add(server.sv101_name);
                    }
                }
            }

            if (IntPtr.Zero != pBuf)
            {
                NetApiBufferFree(pBuf);
            }

            return result;
        }

        public IEnumerable<String> GetServers(string domain)
        {
            List<String> lst = new List<String>();

            int entriesRead = 0;
            int totalEntries = 0;
            IntPtr buffer = IntPtr.Zero;
            IntPtr resumeHandle = IntPtr.Zero;

            int status = NetServerEnum(null, 101, out buffer, MAX_PREFERRED_LENGTH, ref entriesRead, ref totalEntries, SV101Types.SV_TYPE_ALL, domain, resumeHandle);

            if ((NERR_Success == status) || (status == ERROR_MORE_DATA))
            {
                if (buffer != IntPtr.Zero)
                {
                    IntPtr ptr = buffer;

                    for (int i = 0; i < entriesRead; i++)
                    {
                        ServerInfo101 server = (ServerInfo101)Marshal.PtrToStructure(ptr, typeof(ServerInfo101));

                        ptr = (IntPtr)((ulong)ptr + (ulong)Marshal.SizeOf(server));

                        if (ptr == IntPtr.Zero)
                        {
                            break;
                        }

                        lst.Add(server.sv101_name);
                    }
                }
            }

            if (IntPtr.Zero != buffer)
            {
                NetApiBufferFree(buffer);
            }

            return lst;
        }

        public IEnumerable<String> GetShares(String serverName)
        {
            List<String> shares = new List<String>();

            int status = 0;
            int entriedRead = 0;
            int totalEntries = 0;
            int resumeHandle = 0;
            int structSize = Marshal.SizeOf(typeof(ShareInfo0));
            IntPtr buffer = IntPtr.Zero;

            do
            {
                unchecked
                {
                    status = NetShareEnum(new StringBuilder(serverName), 0, ref buffer, (uint)MAX_PREFERRED_LENGTH, ref entriedRead, ref totalEntries, ref resumeHandle);
                }

                if ((NERR_Success == status) || (ERROR_MORE_DATA == status))
                {
                    IntPtr currentPtr = buffer;

                    if (currentPtr != IntPtr.Zero)
                    {
                        for (int i = 0; i < entriedRead; i++)
                        {
                            if (currentPtr == null)
                            {
                                break;
                            }

                            ShareInfo0 shi0 = (ShareInfo0)Marshal.PtrToStructure(currentPtr, typeof(ShareInfo0));

                            String strName = shi0.shi0_netname;

                            currentPtr = new IntPtr(currentPtr.ToInt32() + structSize);

                            int len = strName.Length;

                            if (len > 0)
                            {
                                shares.Add(strName);
                            }
                        }
                    }

                    NetApiBufferFree(buffer);
                }
            }
            while (status == ERROR_MORE_DATA);

            return shares;
        }

        public void FillFiles(String folder, ICollection<String> resultFiles)
        {
			try
			{
				if (folder.Length <= 0)
				{
					return;
				}

				DirectoryInfo directoryInfo = new DirectoryInfo(folder);

				if (true == directoryInfo.Exists)
				{
					foreach (DirectoryInfo di in directoryInfo.GetDirectories())
					{
						resultFiles.Add(di.Name + "/");
					}

					foreach (FileInfo fi in directoryInfo.GetFiles())
					{
						resultFiles.Add(fi.Name);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
        }

        public Object AddFontResource(byte[] fontFileData)
        {
            if ((null != fontFileData) && (fontFileData.Length > 0))
            {
                uint dwFonts = 0;

                IntPtr fontHandle = AddFontMemResourceEx(fontFileData, fontFileData.Length, IntPtr.Zero, out dwFonts);

                return fontHandle;
            }

            return null;
        }

        public void RemoveFontResource(Object fontHandle)
        {
            if (null != fontHandle)
            {
                RemoveFontMemResourceEx((IntPtr)fontHandle);
            }
        }

        public List<String> GetAvailableSystemFonts()
        {
            List<String> fonts = new List<String>();

            using (InstalledFontCollection fontsCollection = new InstalledFontCollection())
            {
                foreach (FontFamily fontFamily in fontsCollection.Families)
                {
                    String fontName = fontFamily.Name.ToLower();

                    fonts.Add(fontName);
                }
            }

            return fonts;
        }

        private static void memcpy(byte[] dst, byte[] src, int count)
        {
            for (int i = 0; i < count; i++)
            {
                dst[i] = src[i];
            }
        }

        internal static int MulDiv(int a, int b, int c)
        {
            return (int)(((double)a * (double)b) / (double)c);
        }

        internal static IntPtr SelectFont(IntPtr hdc, IntPtr hgdiobj)
        {
            return SelectObject(hdc, hgdiobj);
        }

        [DllImport(shell32dll, CharSet = CharSet.Auto)]
        private static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

        [DllImport(user32dll, EntryPoint = "DestroyIcon", SetLastError = true)]
        private static extern int DestroyIcon(IntPtr hIcon);

        [DllImport(user32dll)]
        private static extern bool GetIconInfo(IntPtr hIcon, out IconInfo piconinfo);

        [DllImport(gdi32dll)]
        private static extern int GetDIBits(IntPtr hdc, IntPtr hbmp, uint uStartScan, uint cScanLines, [Out] byte[] lpvBits, ref BitmapInfo lpbmi, uint uUsage);

        [DllImport(shell32dll, CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFileInfo psfi, uint cbFileInfo, uint uFlags);

        [DllImport(net32dll, EntryPoint = "NetServerEnum")]
        private static extern int NetServerEnum([MarshalAs(UnmanagedType.LPWStr)]string servername, int level, out IntPtr bufptr, int prefmaxlen, ref int entriesread, ref int totalentries, SV101Types servertype, [MarshalAs(UnmanagedType.LPWStr)]string domain, IntPtr resume_handle);

        [DllImport(net32dll, SetLastError = true)]
        private static extern int NetApiBufferFree(IntPtr buffer);

        [DllImport(net32dll, CharSet = CharSet.Unicode)]
        internal static extern int NetShareEnum(StringBuilder ServerName, int level, ref IntPtr bufPtr, uint prefmaxlen, ref int entriesread, ref int totalentries, ref int resume_handle);

        [DllImport(gdi32dll)]
        internal static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BitmapInfo pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport(gdi32dll)]
        private static extern IntPtr AddFontMemResourceEx(byte[] pbFont, int cbFont, IntPtr pdv, out uint pcFonts);

        [DllImport(gdi32dll)]
        private static extern bool RemoveFontMemResourceEx(IntPtr fh);

        [DllImport(gdi32dll)]
        internal static extern IntPtr CreateFont(int nHeight, int nWidth, int nEscapement, int nOrientation, int fnWeight, uint fdwItalic, uint fdwUnderline, uint fdwStrikeOut, uint fdwCharSet, uint fdwOutputPrecision, uint fdwClipPrecision, uint fdwQuality, uint fdwPitchAndFamily, string lpszFace);

        [DllImport(gdi32dll)]
        internal static extern bool DeleteObject(IntPtr hObject);

        [DllImport(gdi32dll)]
        internal static extern bool DeleteDC(IntPtr hdc);

        [DllImport(gdi32dll)]
        internal static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        [DllImport(gdi32dll, SetLastError = true)]
        internal static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport(gdi32dll, CharSet = CharSet.Unicode)]
        internal static extern bool GetTextMetrics(IntPtr hdc, out TextMetric lptm);

        [DllImport(gdi32dll)]
        internal static extern uint SetTextAlign(IntPtr hdc, uint fMode);

        [DllImport(gdi32dll)]
        internal static extern bool GetCharABCWidths(IntPtr hdc, uint uFirstChar, uint uLastChar, [Out] ABC[] lpabc);

        [DllImport(gdi32dll)]
        internal static extern uint GetGlyphOutline(IntPtr hdc, uint uChar, uint uFormat, out GlyphMetrics lpgm, uint cbBuffer, IntPtr lpvBuffer, ref MAT2 lpmat2);

        [DllImport(gdi32dll, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport(gdi32dll)]
        internal static extern uint SetBkColor(IntPtr hdc, int crColor);

        [DllImport(gdi32dll)]
        internal static extern uint SetTextColor(IntPtr hdc, int crColor);

        [DllImport(gdi32dll)]
        internal static extern int SetBkMode(IntPtr hdc, int iBkMode);

        [DllImport(gdi32dll)]
        internal static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);

        [DllImport(gdi32dll)]
        internal static extern bool ExtTextOut(IntPtr hdc, int X, int Y, uint fuOptions, [In] ref RECT lprc, string lpString, uint cbCount, [In] int[] lpDx);

        private const String shell32dll = "shell32.dll";
        private const String user32dll = "user32.dll";
        private const String gdi32dll = "gdi32.dll";
        private const String net32dll = "netapi32.dll";
        private const int FILE_ATTRIBUTE_NORMAL = 0x00000080;
        private const int SHGFI_ICON = 0x000000100;
        private const int SHGFI_LARGEICON = 0x000000000;
        private const int SHGFI_SMALLICON = 0x000000001;
        private const int MAX_PREFERRED_LENGTH = -1;
        private const int NERR_Success = 0;
        private const long ERROR_MORE_DATA = 234L;
        internal const uint ANTIALIASED_QUALITY = 4;
        internal const uint NONANTIALIASED_QUALITY = 3;
        internal const uint DEFAULT_PITCH = 0;
        internal const uint FF_DONTCARE = 0 << 4;
        internal const uint CLIP_DEFAULT_PRECIS = 0;
        internal const uint OUT_DEFAULT_PRECIS = 0;
        internal const uint DEFAULT_CHARSET = 1;
        internal const int FW_NORMAL = 400;
        internal const int FW_BOLD = 700;
        internal const uint TA_UPDATECP = 1;
        internal const uint TA_LEFT = 0;
        internal const uint TA_TOP = 0;
        internal const uint GDI_ERROR = (uint)(0xFFFFFFFFL);
        internal const uint GGO_METRICS = 0;
        internal const uint CLR_INVALID = 0xFFFFFFFF;
        internal const int OPAQUE = 2;
        internal const int TRANSPARENT = 1;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IconInfo
    {
        public bool fIcon;
        public Int32 xHotspot;
        public Int32 yHotspot;
        public IntPtr hbmMask;
        public IntPtr hbmColor;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct BitmapInfo
    {
        public BitmapInfoHeader bmiHeader;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
        public RGBQuad[] bmiColors;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal struct SHFileInfo
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    internal enum SV101Types : uint
    {
        SV_TYPE_WORKSTATION = 0x00000001,
        SV_TYPE_SERVER = 0x00000002,
        SV_TYPE_SQLSERVER = 0x00000004,
        SV_TYPE_DOMAIN_CTRL = 0x00000008,
        SV_TYPE_DOMAIN_BAKCTRL = 0x00000010,
        SV_TYPE_TIME_SOURCE = 0x00000020,
        SV_TYPE_AFP = 0x00000040,
        SV_TYPE_NOVELL = 0x00000080,
        SV_TYPE_DOMAIN_MEMBER = 0x00000100,
        SV_TYPE_PRINTQ_SERVER = 0x00000200,
        SV_TYPE_DIALIN_SERVER = 0x00000400,
        SV_TYPE_XENIX_SERVER = 0x00000800,
        SV_TYPE_SERVER_UNIX = 0x00000800,
        SV_TYPE_NT = 0x00001000,
        SV_TYPE_WFW = 0x00002000,
        SV_TYPE_SERVER_MFPN = 0x00004000,
        SV_TYPE_SERVER_NT = 0x00008000,
        SV_TYPE_POTENTIAL_BROWSER = 0x00010000,
        SV_TYPE_BACKUP_BROWSER = 0x00020000,
        SV_TYPE_MASTER_BROWSER = 0x00040000,
        SV_TYPE_DOMAIN_MASTER = 0x00080000,
        SV_TYPE_SERVER_OSF = 0x00100000,
        SV_TYPE_SERVER_VMS = 0x00200000,
        SV_TYPE_WINDOWS = 0x00400000,
        SV_TYPE_DFS = 0x00800000,
        SV_TYPE_CLUSTER_NT = 0x01000000,
        SV_TYPE_TERMINALSERVER = 0x02000000,
        SV_TYPE_CLUSTER_VS_NT = 0x04000000,
        SV_TYPE_DCE = 0x10000000,
        SV_TYPE_ALTERNATE_XPORT = 0x20000000,
        SV_TYPE_LOCAL_LIST_ONLY = 0x40000000,
        SV_TYPE_DOMAIN_ENUM = 0x80000000,
        SV_TYPE_ALL = 0xFFFFFFFF
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct ShareInfo0
    {
        public string shi0_netname;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ServerInfo101
    {
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
        public UInt32 sv101_platform_id;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        public string sv101_name;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
        public UInt32 sv101_version_major;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
        public UInt32 sv101_version_minor;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
        public UInt32 sv101_type;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        public string sv101_comment;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct TextMetric
    {
        public int tmHeight;
        public int tmAscent;
        public int tmDescent;
        public int tmInternalLeading;
        public int tmExternalLeading;
        public int tmAveCharWidth;
        public int tmMaxCharWidth;
        public int tmWeight;
        public int tmOverhang;
        public int tmDigitizedAspectX;
        public int tmDigitizedAspectY;
        public char tmFirstChar;
        public char tmLastChar;
        public char tmDefaultChar;
        public char tmBreakChar;
        public byte tmItalic;
        public byte tmUnderlined;
        public byte tmStruckOut;
        public byte tmPitchAndFamily;
        public byte tmCharSet;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct ABC
    {
        public int abcA;
        public uint abcB;
        public int abcC;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct GlyphMetrics
    {
        public int gmBlackBoxX;
        public int gmBlackBoxY;
        [MarshalAs(UnmanagedType.Struct)]
        public POINT gmptGlyphOrigin;
        public short gmCellIncX;
        public short gmCellIncY;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MAT2
    {
        [MarshalAs(UnmanagedType.Struct)]
        public FIXED eM11;
        [MarshalAs(UnmanagedType.Struct)]
        public FIXED eM12;
        [MarshalAs(UnmanagedType.Struct)]
        public FIXED eM21;
        [MarshalAs(UnmanagedType.Struct)]
        public FIXED eM22;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RGB
    {
        byte byRed, byGreen, byBlue, RESERVED;

        public RGB(byte[] colorIn)
        {
            byRed = colorIn[0];
            byGreen = colorIn[1];
            byBlue = colorIn[2];
            RESERVED = 0;
        }
        public Int32 ToInt32()
        {
            byte[] RGBCOLORS = new byte[4];
            RGBCOLORS[0] = byRed;
            RGBCOLORS[1] = byGreen;
            RGBCOLORS[2] = byBlue;
            RGBCOLORS[3] = RESERVED;
            return BitConverter.ToInt32(RGBCOLORS, 0);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

	internal class Win32
	{

		internal static uint DIB_RGB_COLORS = 0;
		internal static int BI_RGB = 0;
		internal static int CSIDL_DESKTOP = 0x0000;
		internal static int NOERROR = 0;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct POINT
	{
		public int x;
		public int y;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct BitmapInfoHeader
	{
		public int biSize;
		public int biWidth;
		public int biHeight;
		public short biPlanes;
		public short biBitCount;
		public int biCompression;
		public int biSizeImage;
		public int biXPelsPerMeter;
		public int biYPelsPerMeter;
		public int biClrUsed;
		public int biClrImportant;
	}

	internal struct FIXED
	{
		public short fract;
		public short value;
	}

	[StructLayout(LayoutKind.Sequential)]
	internal struct RGBQuad
	{
		public byte rgbBlue;
		public byte rgbGreen;
		public byte rgbRed;
		public byte rgbReserved;
	}
}

#endif
