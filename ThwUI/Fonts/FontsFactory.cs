using System;
using System.Collections.Generic;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Fonts
{
    /// <summary>
    /// Fonts factory. Manages fonts.
    /// By default uses Bitmap fonts rendering.
    /// </summary>
	public class FontsFactory : UIObject
	{
        internal FontsFactory(UIEngine engine, Theme theme) : base("fontsFactory")
        {
            RegisterFontCreator(new BitmapFontCreator(engine));
            RegisterFontCreator(new WinFontCreator());
        }

        /// <summary>
        /// Loads required font. Actually does reference counting and if fount was already
        /// loaded with required parameters the existing object is returned and reference count
        /// is incremented
        /// </summary>
        /// <param name="fontName">font name</param>
        /// <param name="size">font size</param>
        /// <param name="bold">is font bold</param>
        /// <param name="italic">is font italic</param>
        /// <returns></returns>
        internal IFont CreateFont(String fontName, int size, bool bold, bool italic, UIEngine engine, Theme theme)
        {
            foreach (IFont font in this.loadedFonts)
            {
                if ((font.Name == fontName) && (font.Size == size) && (font.Bold == bold) && (font.Italic == italic))
                {
                    font.AddRef();

                    return font;
                }
            }

            for (int i = this.fontCreators.Count - 1; i >= 0; i--)
            {
                IFont font = this.fontCreators[i].GetFont(fontName, size, bold, italic, engine, theme);

                if (null != font)
                {
                    font.AddRef();

                    this.loadedFonts.Add(font);

                    return font;
                }
            }

            CensoredFont censoredFont = new CensoredFont(fontName, size, bold, italic);

            censoredFont.AddRef();

            this.loadedFonts.Add(censoredFont);

            return censoredFont;
        }

        /// <summary>
        /// Releases font object. Cheks reference count to object if its zero deletes fonts object.
        /// </summary>
        /// <param name="font"></param>
        internal void DeleteFont(IFont font)
        {
            if (null != font)
            {
                int count = font.Release();

                if (count <= 0)
                {
                    this.loadedFonts.Remove(font);
                }
            }
        }

        /// <summary>
        /// Registers font creator. User can provide his own fonts creator if built in is not sufficiet.
        /// </summary>
        /// <param name="fontCreator"></param>
        public void RegisterFontCreator(IFontCreator fontCreator)
        {
            if (null != fontCreator)
            {
                this.fontCreators.Add(fontCreator);
                this.fontsNames = null;
            }
        }

        /// <summary>
        /// Unregisters font creator.
        /// </summary>
        /// <param name="fontCreator"></param>
        public void UnRegisterFontCreator(IFontCreator fontCreator)
        {
            this.fontCreators.Remove(fontCreator);

            this.fontsNames = null;
        }

        /// <summary>
        /// Supported fonts list
        /// </summary>
        public List<String> GetAvailableFonts(UIEngine engine, Theme theme)
        {
            if (null == this.fontsNames)
            {
                this.fontsNames = new List<String>();

                foreach (IFontCreator fontCreator in this.fontCreators)
                {
                    fontCreator.GetFonts(this.fontsNames, engine, theme);
                }
            }

            return this.fontsNames;
        }
		
		private	List<IFontCreator> fontCreators = new List<IFontCreator>();
		private	List<IFont> loadedFonts = new List<IFont>();
        private List<String> fontsNames = null;
	}
}
