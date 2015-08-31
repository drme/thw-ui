using System;

namespace ThW.UI.Utils.Themes
{
    /// <summary>
    /// Window theme settings.
    /// </summary>
    public class WindowTheme
    {
        /// <summary>
        /// Constructs window theme object.
        /// </summary>
        internal WindowTheme()
        {
        }

        /// <summary>
        /// How many pixels wide is border
        /// </summary>
        public int BorderSize
        {
            get
            {
                return this.windowBorderSize;
            }
            set
            {
                this.windowBorderSize = value;
            }
        }

        /// <summary>
        /// How many pixels window border is covering of a window frame
        /// </summary>
        public int BorderOffset
        {
            get
            {
                return this.windowBorderOffset;
            }
            set
            {
                this.windowBorderOffset = value;
            }
        }

        /// <summary>
        /// The position in pixels where the title split symbol starts in the texture image
        /// </summary>
        public int SplitImagePosition
        {
            get
            {
                return this.windowSplitImagePosition;
            }
            set
            {
                this.windowSplitImagePosition = value;
            }
        }

        /// <summary>
        /// The split image Width
        /// </summary>
        public int SplitWidth
        {
            get
            {
                return this.windowSplitWidth;
            }
            set
            {
                this.windowSplitWidth = value;
            }
        }

        /// <summary>
        /// The window title Height
        /// </summary>
        public int TitleHeight
        {
            get
            {
                return this.windowTitleHeight;
            }
            set
            {
                this.windowTitleHeight = value;
            }
        }

        /// <summary>
        /// How many pixels from the right border the split image is displayed on the window title
        /// </summary>
        public int SplitPosition
        {
            get
            {
                return this.windowSplitPosition;
            }
            set
            {
                this.windowSplitPosition = value;
            }
        }

        /// <summary>
        /// The window shadow image Width
        /// </summary>
        public int ShadowSize
        {
            get
            {
                return this.windowShadowSize;
            }
            set
            {
                this.windowShadowSize = value;
            }
        }

        /// <summary>
        /// Has window title image.
        /// </summary>
        public bool HasTitleImage
        {
            get
            {
                return this.windowHasTitleImage;
            }
        }

        /// <summary>
        /// Loads window theme settings from xml.
        /// </summary>
        /// <param name="element">xml element to load from.</param>
        internal void LoadWindow(IXmlElement element)
        {
            this.windowHasTitleImage = UIUtils.FromString(element.GetAttributeValue("hasTitleImage", "false"), this.windowHasTitleImage);
            this.windowBorderSize = UIUtils.FromString(element.GetAttributeValue("windowBorderSize"), this.windowBorderSize);
            this.windowBorderOffset = UIUtils.FromString(element.GetAttributeValue("windowBorderOffset"), this.windowBorderOffset);
            this.windowSplitImagePosition = UIUtils.FromString(element.GetAttributeValue("windowSplitImagePosition"), this.windowSplitImagePosition);
            this.windowSplitWidth = UIUtils.FromString(element.GetAttributeValue("windowSplitWidth"), this.windowSplitWidth);
            this.windowTitleHeight = UIUtils.FromString(element.GetAttributeValue("windowTitleHeight"), this.windowTitleHeight);
            this.windowSplitPosition = UIUtils.FromString(element.GetAttributeValue("windowSplitPosition"), this.windowSplitPosition);
            this.windowShadowSize = UIUtils.FromString(element.GetAttributeValue("windowShadowSize"), this.windowShadowSize);
        }

        private int windowBorderSize = 4;
        private int windowBorderOffset = 2;
        private int windowSplitImagePosition = 56;
        private int windowSplitWidth = 34;
        private int windowTitleHeight = 22;
        private int windowSplitPosition = 175;
        private int windowShadowSize = 8;
        private bool windowHasTitleImage = false;
    }
}
