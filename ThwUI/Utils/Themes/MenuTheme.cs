using System;

namespace ThW.UI.Utils.Themes
{
    /// <summary>
    /// Theme settigs for menu.
    /// </summary>
    public class MenuTheme
    {
        /// <summary>
        /// Constructs menu theme object.
        /// </summary>
        internal MenuTheme(ThemeColors colors)
        {
            this.colorMenuPopUpBack = colors.Control;
        }

        /// <summary>
        /// Menu has rendered shadow.
        /// </summary>
        public bool HasShadow
        {
            get
            {
                return this.menuHasShadow;
            }
            set
            {
                this.menuHasShadow = value;
            }
        }

        /// <summary>
        /// Menu has area for displaying menu item icon.
        /// </summary>
        public bool HasIconPlaceHolder
        {
            get
            {
                return this.menuHasIconPlaceHolder;
            }
            set
            {
                this.menuHasIconPlaceHolder = value;
            }
        }

        /// <summary>
        /// Popup menu has background image.
        /// </summary>
        public bool HasPopUpBackImage
        {
            get
            {
                return this.menuHasPopUpBackImage;
            }
            set
            {
                this.menuHasPopUpBackImage = value;
            }
        }

        /// <summary>
        /// Popup menu background color.
        /// </summary>
        public Color PopUpBackColor
        {
            get
            {
                return this.colorMenuPopUpBack;
            }
            set
            {
                this.colorMenuPopUpBack = value;
            }
        }

        /// <summary>
        /// Menu is simple, no fancy borders, icon place holders,
        /// </summary>
        public bool IsSimple
        {
            get
            {
                return this.menuSimple;
            }
            set
            {
                this.menuSimple = value;
            }
        }

        /// <summary>
        /// Loads menu theme settings from xml.
        /// </summary>
        /// <param name="element">xml element to load from.</param>
        internal void LoadMenu(IXmlElement element, ThemeColors colors)
        {
            this.menuHasShadow = UIUtils.FromString(element.GetAttributeValue("hasShadow", ""), this.menuHasShadow);
            this.colorMenuPopUpBack = colors.GetColor(element.GetAttributeValue("popUpBackColor", ""), this.colorMenuPopUpBack);
            this.menuHasPopUpBackImage = UIUtils.FromString(element.GetAttributeValue("hasPopUpBackImage", ""), this.menuHasPopUpBackImage);
            this.menuHasIconPlaceHolder = UIUtils.FromString(element.GetAttributeValue("hasIconPlaceholderImage", ""), this.menuHasIconPlaceHolder);
            this.menuSimple = UIUtils.FromString(element.GetAttributeValue("hasSimpleMenu", ""), this.menuSimple);
        }

        private bool menuHasShadow = true;
        private bool menuHasIconPlaceHolder = true;
        private bool menuHasPopUpBackImage = false;
        private Color colorMenuPopUpBack = Colors.White;
        private bool menuSimple = true;
    }
}
