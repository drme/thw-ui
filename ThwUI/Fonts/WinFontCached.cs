using System;
using System.Collections.Generic;
using ThW.UI.Utils;

namespace ThW.UI.Fonts
{
    /// <summary>
    /// Cached window font, loaded from xml and tga files.
    /// </summary>
    class WinFontCached : IFont
    {
        /// <summary>
        /// Constructs cached font.
        /// </summary>
        /// <param name="engine">ui engine.</param>
        /// <param name="fontName">font name.</param>
        /// <param name="size">font size.</param>
        /// <param name="bold">is font bold.</param>
        /// <param name="italic">is font italic.</param>
        public WinFontCached(UIEngine engine, String fontName, int size, bool bold, bool italic) : this(fontName, size, bold, italic)
        {
            IXmlReader reader = engine.OpenXmlFile(cacheFolder + ToString() + ".xml");

            if ((null != reader) && (null != reader.RootElement))
            {
                this.letters = new WinLetterCached[256 * 256];

                foreach (IXmlElement element in reader.RootElement.Elements)
                {
                    int code = int.Parse(element.GetAttributeValue("code", "0"));
                    int index = int.Parse(element.GetAttributeValue("index", "0"));
                    int us = int.Parse(element.GetAttributeValue("us", "0"));
                    int vs = int.Parse(element.GetAttributeValue("vs", "0"));
                    int ue = int.Parse(element.GetAttributeValue("ue", "0"));
                    int ve = int.Parse(element.GetAttributeValue("ve", "0"));
                    int w = int.Parse(element.GetAttributeValue("width", "0"));
                    int offX = int.Parse(element.GetAttributeValue("x", "0"));
                    int offY = int.Parse(element.GetAttributeValue("y", "0"));

                    if ((this.cachedImages.Count <= index) || (this.cachedImages[index] != null))
                    {
                        IImage img = engine.CreateImage(cacheFolder + ToString() + "_" + index);

                        if (this.cachedImages.Count == index)
                        {
                            this.cachedImages.Add(img);
                        }
                        else
                        {
                            this.cachedImages[index] = img;
                        }
                    }

                    if (code >= 0 && code < cacheLetters)
                    {
                        this.letters[code] = new WinLetterCached(engine);
                        this.letters[code].SetCachedData(this.cachedImages[index], us, vs, ue, ve, w, offX, offY);
                    }
                }

                this.loaded = true;
            }
        }

        /// <summary>
        /// Creates cached font.
        /// </summary>
        /// <param name="fontName">font name.</param>
        /// <param name="size">font size.</param>
        /// <param name="bold">is font bold.</param>
        /// <param name="italic">is font italic.</param>
        protected WinFontCached(string fontName, int size, bool bold, bool italic) : base(fontName, size, bold, italic)
        {
        }

        /// <summary>
        /// Renders text string starting from start and ending with end indexes.
        /// </summary>
        /// <param name="render">graphics to render to.</param>
        /// <param name="X">X position to render to.</param>
        /// <param name="Y">Y position to render to.</param>
        /// <param name="text">text to render.</param>
        /// <param name="start">text starting index.</param>
        /// <param name="stop">text ending index.</param>
        public override void DrawText(Graphics render, int x, int y, String text, int start, int stop)
        {
            if (false == this.loaded)
            {
                return;
            }

            if ((null == text) || (text.Length == 0))
            {
                return;
            }

            int renderX = x;

            if (stop < 0)
            {
                stop = (int)(text.Length);
            }

            if (stop > text.Length)
            {
                stop = text.Length;
            }

            if (start < 0)
            {
                start = 0;
            }

            for (int i = start; i < stop; i++)
            {
                WinLetterCached letter = this.letters[text[i]];

                if (null != letter)
                {
                    renderX += letter.Render(render, renderX, y);
                }
            }
        }

        /// <summary>
        /// Measures text length.
        /// </summary>
        /// <param name="text">text to measure.</param>
        /// <param name="start">text starting index.</param>
        /// <param name="stop">text ending index.</param>
        /// <returns>rendered text length.</returns>
        public override int TextLength(String text, int start, int stop)
        {
            if ((null == text) || (text.Length == 0))
            {
                return 0;
            }

            if (false == this.loaded)
            {
                return 0;
            }

            int length = 0;

            if ((stop < 0) || (stop > text.Length))
            {
                stop = (int)(text.Length);
            }

            if (start < 0)
            {
                start = 0;
            }

            for (int i = start; i < stop; i++)
            {
                WinLetterCached letter = this.letters[text[i]];

                if (null != letter)
                {
                    length += letter.Width;
                }
            }

            return length;
        }

        /// <summary>
        /// Was font loaded.
        /// </summary>
        internal bool Loaded
        {
            get
            {
                return this.loaded;
            }
        }

        /// <summary>
        /// Converts font info to string.
        /// </summary>
        /// <returns>font information.</returns>
        public override String ToString()
        {
            return (this.Name + "_" + this.Size + "_b" + (this.Bold ? "t" : "f") + "_i" + (this.Italic ? "t" : "f")).Replace(' ', '_').ToLower();
        }

        protected WinLetterCached[] letters = null;
        protected bool loaded = false;
        protected List<IImage> cachedImages = new List<IImage>();
        protected const String cacheFolder = "fonts/cache/";
        protected const int cacheLetters = 256;
        protected internal const int cacheTextureSize = 256;
    }
}
