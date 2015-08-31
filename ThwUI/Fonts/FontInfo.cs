using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Fonts
{
    /// <summary>
    /// Holds information about controls font.
    /// </summary>
    public class FontInfo
    {
        /// <summary>
        /// Constructs font object.
        /// </summary>
        /// <param name="engine">ui engine.</param>
        /// <param name="window">window control belongs to.</param>
        /// <param name="settings">control default settings.</param>
        /// <param name="name">default font name.</param>
        /// <param name="size">default font size.</param>
        internal FontInfo(UIEngine engine, Window window, ControlSettings settings, String name, int size)
        {
            this.engine = engine;
            this.window = window;
            this.size = size;
            this.name = name;
        }

        /// <summary>
        /// Releases font.
        /// </summary>
        internal void ClearFont()
        {
            if (null != this.font)
            {
                this.engine.GetFontsFactory(this.window.Desktop.Theme).DeleteFont(this.font);
                this.font = null;
            }
        }

        /// <summary>
        /// If requested builds and returns font.
        /// </summary>
        /// <returns></returns>
        private IFont GetFont()
        {
            if ((null != this.font) && (this.font.Name == this.name) && (this.font.Size == this.size) && (this.font.Bold == this.bold) && (this.font.Italic == this.italic))
            {
                return this.font;
            }

            ClearFont();

            this.font = this.engine.GetFontsFactory(this.window.Desktop.Theme).CreateFont(this.name, this.size, this.bold, this.italic, this.engine, this.window.Desktop.Theme);

            return this.font;
        }

        /// <summary>
        /// Is font italic.
        /// </summary>
        public bool Italic
        {
            get
            {
                if (null != this.font)
                {
                    return this.font.Italic;
                }

                return this.italic;
            }
            set
            {
                lock (this)
                {
                    if (value == this.Italic)
                    {
                        return;
                    }

                    this.italic = value;

                    ClearFont();
                }
            }
        }

        /// <summary>
        /// Is font bold.
        /// </summary>
        public bool Bold
        {
            get
            {
                if (null != this.font)
                {
                    return this.font.Bold;
                }

                return this.bold;
            }
            set
            {
                lock (this)
                {
                    if (value == this.Bold)
                    {
                        return;
                    }

                    this.bold = value;

                    ClearFont();
                }
            }
        }

        /// <summary>
        /// Font size.
        /// </summary>
        public int Size
        {
            get
            {
                if (null != this.font)
                {
                    return this.font.Size;
                }

                return this.size;
            }
            set
            {
                lock (this)
                {
                    if (value == this.Size)
                    {
                        return;
                    }

                    this.size = value;

                    ClearFont();
                }
            }
        }

        /// <summary>
        /// Font name.
        /// </summary>
        public String Name
        {
            get
            {
                if (null != this.font)
                {
                    return this.font.Name;
                }

                return this.name;
            }
            set
            {
                lock (this)
                {
                    if (value == this.Name)
                    {
                        return;
                    }

                    this.name = value;

                    ClearFont();
                }
            }
        }

        /// <summary>
        /// Actual font object.
        /// </summary>
        public IFont Font
        {
            get
            {
                return GetFont();
            }
        }

        /// <summary>
        /// Sets all font properties at once.
        /// </summary>
        /// <param name="name">font name.</param>
        /// <param name="size">font size.</param>
        /// <param name="bold">is bold.</param>
        /// <param name="italic">is italic.</param>
        public void SetFontInfo(String name, int size, bool bold, bool italic)
        {
            this.Name = name;
            this.Bold = bold;
            this.Italic = italic;
            this.Size = size;
        }

        private bool italic = false;
        private bool bold = false;
        private int size;
        private String name;
        private IFont font = null;
        private UIEngine engine;
        private Window window;
    }
}
