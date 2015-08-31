using System;
using System.Collections.Generic;
using ThW.UI.Utils;
using ThW.UI.Utils.FilesSystem;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Fonts
{
    /// <summary>
    /// Fonts loaded from TTF files using Win32 API.
    /// </summary>
	internal class WinFontCreator : IFontCreator
	{
        /// <summary>
        /// Creates fonts creator.
        /// </summary>
        /// <param name="engine"></param>
        /// <param name="theme"></param>
		public WinFontCreator()
        {
        }

        /// <summary>
        /// Releases registered custom fonts.
        /// </summary>
		~WinFontCreator()
        {
            if (null != this.addedFonts)
            {
                foreach (IntPtr fontHandle in this.addedFonts)
                {
                    FileUtils.Platform.RemoveFontResource(fontHandle);
                }

                this.addedFonts.Clear();
            }
        }

        /// <summary>
        /// Creates font. Does not cache fonts - allwats creates new font object.
        /// </summary>
        /// <param name="fontName">font name.</param>
        /// <param name="size">font size.</param>
        /// <param name="bold">is font bold.</param>
        /// <param name="italic">is font italic.</param>
        /// <param name="engine">ui engine.</param>
        /// <param name="theme">theme.</param>
        /// <returns></returns>
		public IFont GetFont(String fontName, int size, bool bold, bool italic, UIEngine engine, Theme theme)
        {
            Init(engine, theme);

			//engine.Logger.WriteLine(LogLevel.Info, "Requesting font: " + fontName);

            WinFontCached cachedFont = new WinFontCached(engine, fontName, size, bold, italic);

            if (true == cachedFont.Loaded)
            {
                return cachedFont;
            }

			WinFont font = new WinFont(engine, fontName, size, bold, italic);

			if (false == font.Loaded)
			{
				return null;
			}
			else
			{
				return font;
			}
		}

        /// <summary>
        /// Loads a list of available fonts, that could be created by this fonts creator.
        /// </summary>
        /// <param name="fonts">the list to fill with font names.</param>
        /// <param name="engine">ui engine.</param>
        /// <param name="theme">theme.</param>
		public void GetFonts(List<String> fonts, UIEngine engine, Theme theme)
        {
            Init(engine, theme);

            fonts.AddRange(FileUtils.Platform.GetAvailableSystemFonts());
        }

        /// <summary>
        /// Registers some internal fonts places in theme/fonts folder.
        /// </summary>
        /// <param name="engine">ui engine</param>
        /// <param name="theme">theme object</param>
        private void Init(UIEngine engine, Theme theme)
        {
            lock (this)
            {
                if (null != this.addedFonts)
                {
                    engine.Logger.WriteLine(LogLevel.Info, "WinFontCreator init");

                    this.addedFonts = new List<Object>();

                    if (null != theme)
                    {
                        List<String> fontFiles = new List<String>();

                        String fontsFolder = theme.ThemeFolder + "fonts/";

                        engine.GetFiles(fontsFolder, fontFiles);

                        engine.Logger.WriteLine(LogLevel.Info, "Enumerating fonts (in " + fontsFolder + "):");

                        foreach (String fontFile in fontFiles)
                        {
                            String fullPath = fontsFolder + fontFile;

                            byte[] fileBytes = null;
                            uint fileSize = 0;
                            Object file = null;

                            if (true == engine.OpenFile(fullPath, out fileBytes, out fileSize, out file))
                            {
                                Object fontHandle = FileUtils.Platform.AddFontResource(fileBytes);

                                if (null != fontHandle)
                                {
                                    this.addedFonts.Add(fontHandle);

                                    engine.Logger.WriteLine(LogLevel.Info, "Adding font " + fullPath);
                                }

                                engine.CloseFile(ref file);
                            }
                        }
                    }
                }
            }
        }

        private List<Object> addedFonts = null;
	}
}
