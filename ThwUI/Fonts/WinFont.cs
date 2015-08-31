using System;
using System.Text;
using ThW.UI.Utils;
using ThW.UI.Utils.Images;
using ThW.UI.Utils.Native;

namespace ThW.UI.Fonts
{
    class WinFont : WinFontCached
    {
        public WinFont(UIEngine engine, String fontName, int size, bool bold, bool italic) : base(fontName, size, bold, italic)
        {
            this.engine = engine;

#if WINDOWS
            this.fontRenderingDisplayContext = PlatformWindows.CreateCompatibleDC(IntPtr.Zero);

            if (null == this.fontRenderingDisplayContext)
            {
                return;
            }

            int height = -PlatformWindows.MulDiv(size, PlatformWindows.GetDeviceCaps(this.fontRenderingDisplayContext, 90/*LOGPIXELSY*/), 72);

            bool antiAliased = true;

            this.fontHandle = PlatformWindows.CreateFont(height, 0, 0, 0, (bold) ? PlatformWindows.FW_BOLD : PlatformWindows.FW_NORMAL, italic ? (uint)1 : (uint)0, 0, 0, PlatformWindows.DEFAULT_CHARSET, PlatformWindows.OUT_DEFAULT_PRECIS, PlatformWindows.CLIP_DEFAULT_PRECIS, (antiAliased) ? PlatformWindows.ANTIALIASED_QUALITY : PlatformWindows.NONANTIALIASED_QUALITY, PlatformWindows.DEFAULT_PITCH | PlatformWindows.FF_DONTCARE, fontName);

            if (null == this.fontHandle)
            {
                return;
            }

            this.letters = new WinLetter[256 * 256];

            for (uint i = 0; i < 256 * 256; i++)
            {
                this.letters[i] = new WinLetter(this.engine);
                ((WinLetter)this.letters[i]).SetFont(this, (char)(i));
            }

            CacheFirst256Letters((null != engine.GeneratedCachedFontsFolder) && (engine.GeneratedCachedFontsFolder.Length > 0), engine.GeneratedCachedFontsFolder);

            this.loaded = true;
#endif
		}

        /// <summary>
        /// Releases font images.
        /// </summary>
        ~WinFont()
        {
            foreach (IImage image in this.cachedImages)
            {
                IImage img = image;
                this.engine.DeleteImage(ref img);
            }

#if WINDOWS
            if (null == this.fontHandle)
            {
                PlatformWindows.DeleteObject(this.fontHandle);
            }

            if (null == this.fontRenderingDisplayContext)
            {
                PlatformWindows.DeleteDC(this.fontRenderingDisplayContext);
            }
#endif
        }

        /// <summary>
        /// Caches the first 256 letters.
        /// </summary>
        /// <param name="save">are the cached letters supposed to be saved to tga files.</param>
        private void CacheFirst256Letters(bool save, String folder)
        {
            int imageIndex = 0;
            byte[] imageBuffer = new byte[cacheTextureSize * cacheTextureSize * 4];
            int rowStart = 0;
            int colStart = 0;
            int rowMaxHeight = 0;

            int[] imageIndexes = new int[cacheLetters];
            int[][] imageUvs = new int[cacheLetters][];

            for (int i = 0; i < cacheLetters; i++)
            {
                WinLetter letter = (WinLetter)this.letters[i];

                LetterInfo letterInfo = letter.Load(true);

                if (null == letterInfo)
                {
                    letterInfo = new LetterInfo();
                    letterInfo.width = 2;
                    letterInfo.height = 2;
                    letterInfo.textureHeight = 2;
                    letterInfo.textureWidth = 2;
                    letterInfo.bytes = new byte[2*2*4];
                }

                if (colStart + letterInfo.width + 1 > cacheTextureSize)
                {
                    colStart = 0;
                    rowStart += rowMaxHeight + 1;
                }

                if (rowStart + 1 + letterInfo.height >= cacheTextureSize)
                {
                    this.cachedImages.Add(this.engine.CreateImage(cacheFolder + ToString() + "_" + (imageIndex+1), cacheTextureSize, cacheTextureSize, imageBuffer));

                    if (true == save)
                    {
                        byte[] tga = Image.GetTGA(cacheTextureSize, cacheTextureSize, imageBuffer, 32);

                        this.engine.CreateFile(folder + cacheFolder + ToString() + "_" + (imageIndex+1) + ".tga", tga, (uint)tga.Length);
                    }

                    rowStart = 0;
                    colStart = 0;
                    rowMaxHeight = 0;

                    Array.Clear(imageBuffer, 0, imageBuffer.Length);

                    imageIndex++;
                }

                for (int y = 0; y < letterInfo.height; y++)
                {
                    for (int x = 0; x < letterInfo.width; x++)
                    {
                        imageBuffer[(rowStart + y) * 4 * cacheTextureSize + (colStart + x) * 4 + 0] = letterInfo.bytes[y * letterInfo.textureWidth * 4 + x * 4 + 0];
                        imageBuffer[(rowStart + y) * 4 * cacheTextureSize + (colStart + x) * 4 + 1] = letterInfo.bytes[y * letterInfo.textureWidth * 4 + x * 4 + 1];
                        imageBuffer[(rowStart + y) * 4 * cacheTextureSize + (colStart + x) * 4 + 2] = letterInfo.bytes[y * letterInfo.textureWidth * 4 + x * 4 + 2];
                        imageBuffer[(rowStart + y) * 4 * cacheTextureSize + (colStart + x) * 4 + 3] = letterInfo.bytes[y * letterInfo.textureWidth * 4 + x * 4 + 3];
                    }
                }

                imageUvs[i] = new int[4];
                imageUvs[i][0] = colStart;
                imageUvs[i][1] = rowStart;
                imageUvs[i][2] = colStart + letterInfo.width + 1;
                imageUvs[i][3] = rowStart + letterInfo.height + 1;
                imageIndexes[i] = imageIndex;

                colStart += letterInfo.width + 1;

                if (letterInfo.height > rowMaxHeight)
                {
                    rowMaxHeight = letterInfo.height;
                }
            }

            this.cachedImages.Add(this.engine.CreateImage(folder + cacheFolder + ToString() + "_" + (imageIndex + Int16.MaxValue), cacheTextureSize, cacheTextureSize, imageBuffer));

            for (int i = 0; i < cacheLetters; i++)
            {
                WinLetter letter = (WinLetter)this.letters[i];

                letter.SetCachedData(this.cachedImages[imageIndexes[i]], imageUvs[i][0], imageUvs[i][1], imageUvs[i][2], imageUvs[i][3]);
            }

            if (true == save)
            {
                byte[] tga = Image.GetTGA(cacheTextureSize, cacheTextureSize, imageBuffer, 32);

                this.engine.CreateFile(folder + cacheFolder + ToString() + "_" + (imageIndex++) + ".tga", tga, (uint)tga.Length);

                String xml = "<font>\n";

                for (int i = 0; i < cacheLetters; i++)
                {
                    xml += "\t" + ((WinLetter)this.letters[i]).ToXml(i, imageIndexes[i]) + "\n";
                }

                xml += "</font>";

                byte[] bytes = Encoding.UTF8.GetBytes(xml);

                String cacheFileName = folder + cacheFolder + ToString() + ".xml";

                this.engine.Logger.WriteLine(LogLevel.Info, "Caching font to: " + cacheFileName);

                this.engine.CreateFile(cacheFileName, bytes, (uint)bytes.Length);
            }
        }

#if WINDOWS
        internal IntPtr fontRenderingDisplayContext = IntPtr.Zero;
        internal IntPtr fontHandle = IntPtr.Zero;
#endif

        private UIEngine engine = null;
    }
}
