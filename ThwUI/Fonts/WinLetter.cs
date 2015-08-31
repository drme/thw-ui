using System;
using System.Runtime.InteropServices;
using ThW.UI.Utils.Native;

namespace ThW.UI.Fonts
{
    /// <summary>
    /// Represents one letter.
    /// </summary>
    internal class WinLetter : WinLetterCached
    {
        /// <summary>
        /// Creates winletter object.
        /// </summary>
        /// <param name="engine">ui engine.</param>
        public WinLetter(UIEngine engine) : base(engine)
        {
        }

        /// <summary>
        /// Assingns fonr ant letter code.
        /// </summary>
        /// <param name="font">font.</param>
        /// <param name="c">letter code.</param>
        internal void SetFont(WinFont font, char c)
        {
            this.character = c;
            this.font = font;
        }

        /// <summary>
        /// Loads letter iamge from win32 font.
        /// </summary>
        /// <param name="cache">should the iamge be cached in tga, xml files.</param>
        /// <returns>loader letter information.</returns>
        protected internal override LetterInfo Load(bool cache)
        {
            LetterInfo result = null;

#if WINDOWS
            IntPtr bitmapHandle = IntPtr.Zero;
            byte[] textureData = null;

            try
            {
                if (null == PlatformWindows.SelectFont(this.font.fontRenderingDisplayContext, this.font.fontHandle))
                {
                    throw new Exception();
                }

                TextMetric tm = new TextMetric();

                if (false == PlatformWindows.GetTextMetrics(this.font.fontRenderingDisplayContext, out tm))
                {
                    throw new Exception();
                }

                if (PlatformWindows.GDI_ERROR == PlatformWindows.SetTextAlign(this.font.fontRenderingDisplayContext, PlatformWindows.TA_LEFT | PlatformWindows.TA_TOP | PlatformWindows.TA_UPDATECP))
                {
                    throw new Exception();
                }

                ABC[] abc = new ABC[1];

                if (false == PlatformWindows.GetCharABCWidths(this.font.fontRenderingDisplayContext, (uint)this.character, (uint)this.character, abc))
                {
                    throw new Exception();
                }

                this.offsetX = abc[0].abcA;
                this.width = (int)(abc[0].abcB + abc[0].abcC);

                BitmapInfo header = new BitmapInfo();
                header.bmiHeader.biSize = Marshal.SizeOf(header);

                GlyphMetrics metrics = new GlyphMetrics();
                MAT2 identity = new MAT2();
                identity.eM12.value = 0;
                identity.eM21.value = 0;
                identity.eM11.value = 1;
                identity.eM22.value = 1;

                if (PlatformWindows.GDI_ERROR == PlatformWindows.GetGlyphOutline(this.font.fontRenderingDisplayContext, this.character, PlatformWindows.GGO_METRICS, out metrics, 0, IntPtr.Zero, ref identity))
                {
                    throw new Exception();
                }

                header.bmiHeader.biWidth = metrics.gmBlackBoxX;
                header.bmiHeader.biHeight = -1 * metrics.gmBlackBoxY;

                header.bmiHeader.biPlanes = 1;
                header.bmiHeader.biBitCount = 32;
                header.bmiHeader.biCompression = 0;// BI_RGB;
                byte[] bitmapData = null;

                IntPtr bitmapDataPointer = IntPtr.Zero;

                bitmapHandle = PlatformWindows.CreateDIBSection(this.font.fontRenderingDisplayContext, ref header, /*DIB_RGB_COLORS*/0, out bitmapDataPointer, IntPtr.Zero, 0);

                if (IntPtr.Zero == bitmapHandle)
                {
                    int err = Marshal.GetLastWin32Error();

                    throw new Exception();
                }

                if (null == PlatformWindows.SelectObject(this.font.fontRenderingDisplayContext, bitmapHandle))
                {
                    throw new Exception();
                }

                if (PlatformWindows.CLR_INVALID == PlatformWindows.SetBkColor(this.font.fontRenderingDisplayContext, new RGB(new byte[] { 0, 0, 0 }).ToInt32()))
                {
                    throw new Exception();
                }

                if (PlatformWindows.CLR_INVALID == PlatformWindows.SetTextColor(this.font.fontRenderingDisplayContext, new RGB(new byte[] { 0xff, 0xff, 0xff }).ToInt32()))
                {
                    throw new Exception();
                }

                if (0 == PlatformWindows.SetBkMode(this.font.fontRenderingDisplayContext, PlatformWindows.OPAQUE))
                {
                    throw new Exception();
                }

                if (false == PlatformWindows.MoveToEx(this.font.fontRenderingDisplayContext, 0 - abc[0].abcA, -1 * (tm.tmAscent - metrics.gmptGlyphOrigin.y), IntPtr.Zero))
                {
                    throw new Exception();
                }

                this.offsetY = tm.tmAscent - metrics.gmptGlyphOrigin.y - tm.tmDescent - 1;

                String str = "" + this.character;
                RECT rect = new RECT();

                if (false == PlatformWindows.ExtTextOut(this.font.fontRenderingDisplayContext, 0, 0, (uint)0, ref rect, str, 1, null))
                {
                    throw new Exception();
                }

                if (0 == PlatformWindows.SetBkMode(this.font.fontRenderingDisplayContext, PlatformWindows.TRANSPARENT))
                {
                    throw new Exception();
                }

                int bitmapWidth = header.bmiHeader.biWidth;
                int bitmapHeight = -header.bmiHeader.biHeight;
                int textureWidth = RoundToPowerOf2(bitmapWidth);
                int textureHeight = RoundToPowerOf2(bitmapHeight);

                bitmapData = new byte[bitmapWidth * bitmapHeight * 4];

                Marshal.Copy(bitmapDataPointer, bitmapData, 0, bitmapWidth * bitmapHeight * 4);

                textureData = new byte[4 * textureWidth * textureHeight];

                for (int j = 0; j < textureHeight; j++)
                {
                    for (int i = 0; i < textureWidth; i++)
                    {
                        textureData[4 * (i + j * textureWidth) + 0] = 0xff;
                        textureData[4 * (i + j * textureWidth) + 1] = 0xff;
                        textureData[4 * (i + j * textureWidth) + 2] = 0xff;
                        textureData[4 * (i + j * textureWidth) + 3] = (byte)((i >= bitmapWidth || j >= bitmapHeight) ? 0 : bitmapData[(i + bitmapWidth * j) * 4 + 2]);
                    }
                }

                if (true == cache)
                {
                    result = new LetterInfo();
                    result.width = bitmapWidth;
                    result.height = bitmapHeight;
                    result.bytes = textureData;
                    result.textureWidth = textureWidth;
                    result.textureHeight = textureHeight;
                }
                else
                {
                    this.image = this.engine.CreateImage("Letter [" + this.character + "]", textureWidth, textureHeight, textureData);
//                    this.internalImage = true;
                }

                this.textureWidth = textureWidth;
                this.textureHeight = textureHeight;
            }
            catch (Exception)
            {
                this.engine.DeleteImage(ref this.image);
            }

            if (null != bitmapHandle)
            {
                PlatformWindows.DeleteObject(bitmapHandle);
            }

            this.loaded = true;
#endif
            return result;
        }

        /// <summary>
        /// The nearest number of powe of 2 matching specified number.
        /// </summary>
        /// <param name="n">number to match</param>
        /// <returns>nerest number of power of 2 matching n</returns>
        private static int RoundToPowerOf2(int n)
        {
            int v = 2;

            while (v < n)
            {
                v *= 2;
            }

            return v;
        }

        /// <summary>
        /// Saves letter to xml code.
        /// </summary>
        /// <param name="id">letter id.</param>
        /// <param name="imgIndex">image index.</param>
        /// <returns>xml code.</returns>
        internal String ToXml(int id, int imgIndex)
        {
            String s = "<letter code=\"" + id + "\" us=\"" + this.uv[0] + "\" vs=\"" + this.uv[1] + "\" ue=\"" + this.uv[2] + "\" ve=\"" + this.uv[3] + "\" index = \"" + imgIndex + "\" x=\"" + this.offsetX + "\" y=\"" + this.offsetY + "\" width=\"" + this.width + "\"  />";

            this.uv = null;

            return s;
        }

        private char character = (char)0;
        private WinFont font = null;
    }
}
