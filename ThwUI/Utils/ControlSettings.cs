using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Default settings for a control.
    /// </summary>
	public class ControlSettings
	{
        /// <summary>
        /// Constructs controls settings object.
        /// </summary>
		internal ControlSettings()
		{
		}

        /// <summary>
        /// Control name
        /// </summary>
        public String Name
        {
            get
            {
                return this.name;
            }
            internal set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Folder and file name begging for a border file.
        /// For example, "border_raised_double" would be appended to the returned value
        /// and the resulting image would be loaded.
        /// </summary>
        public String BordersFilePrefix
        {
            get
            {
                return this.borderFilePrefix;
            }
            internal set
            {
                this.borderFilePrefix = value;
            }
        }

        /// <summary>
        /// Specifies amount of pixels used in the image for a border
        /// </summary>
        public int BorderWidth
        {
            get
            {
                return borderWidth;
            }
            internal set
            {
                borderWidth = value;
            }
        }

        /// <summary>
        /// Control uses skins for a borders painting
        /// </summary>
        public bool Skinned
        {
            get
            {
                return this.skinned;
            }
            internal set
            {
                this.skinned = value;
            }
        }

        /// <summary>
        /// Controls default background image
        /// </summary>
        public String BackImage
        {
            get
            {
                return this.backImage;
            }
            internal set
            {
                this.backImage = value;
            }
        }

        /// <summary>
        /// Controls' size. Default Height.
        /// </summary>
        public int ControlSize
        {
            get
            {
                return this.controlSize;
            }
            internal set
            {
                this.controlSize = value;
            }
        }

        /// <summary>
        /// Controls background color.
        /// </summary>
        public Color ColorBack
        {
            get
            {
                return this.colorBack;
            }
            internal set
            {
                this.colorBack = value;
            }
        }

        /// <summary>
        /// Controls first light border color.
        /// </summary>
        public Color ColorBorderLight1
        {
            get
            {
                return this.colorBorderLight1;
            }
            internal set
            {
                this.colorBorderLight1 = value;
            }
        }

        /// <summary>
        /// Controls second light border color.
        /// </summary>
        public Color ColorBorderLight2
        {
            get
            {
                return this.colorBorderLight2;
            }
            internal set
            {
                this.colorBorderLight2 = value;
            }
        }

        /// <summary>
        /// Controls first dark border color.
        /// </summary>
        public Color ColorBorderDark1
        {
            get
            {
                return this.colorBorderDark1;
            }
            internal set
            {
                this.colorBorderDark1 = value;
            }
        }

        /// <summary>
        /// Controls second dark border color.
        /// </summary>
        public Color ColorBorderDark2
        {
            get
            {
                return this.colorBorderDark2;
            }
            internal set
            {
                this.colorBorderDark2 = value;
            }
        }

        private Color colorBack = null;
        private Color colorBorderLight1 = null;
        private Color colorBorderLight2 = null;
        private Color colorBorderDark1 = null;
        private Color colorBorderDark2 = null;
        private bool skinned = false;
        private String name = null;
        private String borderFilePrefix = null;
        private int borderWidth = 0;
        private String backImage = null;
        private int controlSize = -1;
	}
}
