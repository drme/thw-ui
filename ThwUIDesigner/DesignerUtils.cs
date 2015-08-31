using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ThW.UI.Utils.Designer
{
    class DesignerUtils
    {
        public static Icon GetSystemIcon(int indx)
        {
            try
            {
                IntPtr[] iconPtrs = new IntPtr[1];

                uint cnt = ExtractIconEx(shell32, indx, null, iconPtrs, 1);

                if (cnt > 0)
                {
                    Icon icon = (Icon)Icon.FromHandle(iconPtrs[0]).Clone();

                    DestroyIcon(iconPtrs[0]);

                    return icon;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        [DllImport(shell32, CharSet = CharSet.Auto)]
        private static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);

        [DllImport(user32, SetLastError = true)]
        private static extern bool DestroyIcon(IntPtr hIcon);

        private const String shell32 = "shell32.dll";
        private const String user32 = "user32.dll";
    }
}
