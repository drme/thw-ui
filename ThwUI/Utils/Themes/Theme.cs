using System;
using System.Collections.Generic;

namespace ThW.UI.Utils.Themes
{
    /// <summary>
    /// Specifies all windows, their controls managed by desktop theme settings.
    /// </summary>
	public class Theme
	{
        /// <summary>
        /// Constructs theme object. Loads theme configuration from themeFolder + theme.xml file.
        /// </summary>
        /// <param name="themeFolder">theme folder. This folder contains theme desctiption xml file and all required resource images.</param>
        /// <param name="engine">user interface engine, is used for loading files.</param>
		internal Theme(String themeFolder, UIEngine engine)
        {
			if (themeFolder.Length > 0)
			{
				if ( (themeFolder[themeFolder.Length - 1] != '/') && (themeFolder[themeFolder.Length - 1] != '\\') )
				{
					this.themeFolder = themeFolder + "/";
				}
				else
				{
					this.themeFolder = themeFolder;
				}
			}
			else
			{
				this.themeFolder = themeFolder;
			}
			
			LoadTheme(engine);
        }

        /// <summary>
        /// Windows' settings.
        /// </summary>
        public WindowTheme Window
        {
            get
            {
                if (null == this.windowTheme)
                {
                    this.windowTheme = new WindowTheme();
                }

                return this.windowTheme;
            }
        }

        /// <summary>
        /// Menus' settings.
        /// </summary>
        public MenuTheme Menu
        {
            get
            {
                if (null == this.menuTheme)
                {
                    this.menuTheme = new MenuTheme(this.Colors);
                }

                return this.menuTheme;
            }
        }

        /// <summary>
        /// Default colors for this theme.
        /// </summary>
        public ThemeColors Colors
        {
            get
            {
                if (null == this.colors)
                {
                    this.colors = new ThemeColors();
                }

                return this.colors;
            }
        }

        /// <summary>
        /// Default font dize.
        /// </summary>
		public int DefaultFontSize
        {
            get
            {
                return this.defaultFontSize;
            }
            set
            {
                this.defaultFontSize = value;
            }
        }

        /// <summary>
        /// Theme folder.
        /// </summary>
		public String ThemeFolder
        {
            get
            {
			    return this.themeFolder;
            }
        }

        /// <summary>
        /// Default font name.
        /// </summary>
		public String DefaultFontName
        {
            get
            {
                return this.defaultFontName;
            }
        }

        /// <summary>
        /// Are controls skined or rendered using vector graphics.
        /// </summary>
		public bool Skinned
        {
            get
            {
			    return this.skinned;
            }
            set
            {
			    this.skinned = value;
            }
        }

        /// <summary>
        /// A list of systems colors.
        /// </summary>
        public IEnumerable<Color> SystemColors
        {
            get
            {
                return this.Colors.SystemColors;
            }
        }

        /// <summary>
        /// Gets controls default settings.
        /// </summary>
        /// <param name="controlType">control type.</param>
        /// <returns>control settings object.</returns>
		public ControlSettings GetControlSettings(String controlType)
        {
			ControlSettings cs = null;

            this.controlsSettings.TryGetValue(controlType, out cs);

			if (null == cs)
			{
                cs = new ControlSettings();

				cs.ColorBack = this.Colors.Control;
                cs.ColorBorderLight1 = this.Colors.ControlLight;
                cs.ColorBorderLight2 = this.Colors.Control;
                cs.ColorBorderDark1 = this.Colors.ControlDark;
                cs.ColorBorderDark2 = this.Colors.ControlDarkDark;
                cs.Name = controlType;
                cs.Skinned = this.skinned;
                cs.BordersFilePrefix = "images/control_";
                cs.BorderWidth = 2;
                cs.BackImage = "";

				this.controlsSettings[controlType] = cs;
			}

			return cs;
        }

        /// <summary>
        /// Loads theme configuration from xml file.
        /// </summary>
        /// <param name="engine"></param>
        private void LoadTheme(UIEngine engine)
        {
            using (IXmlReader reader = engine.OpenXmlFile(this.themeFolder + "theme.xml"))
            {
                if (null != reader)
                {
                    Load(reader);
                }
            }
        }

        /// <summary>
        /// Loads theme configuration from xml file.
        /// </summary>
        /// <param name="reader">xml document containing theme configuration.</param>
        private void Load(IXmlReader reader)
        {
			IXmlElement root = reader.RootElement;

			if (null != root)
			{
				this.skinned = UIUtils.FromString(root.GetAttributeValue("skinned", ""), this.skinned);
                this.defaultFontName = root.GetAttributeValue("defaultfont", this.defaultFontName);

				foreach (IXmlElement element in root.Elements)
				{
                    switch (element.Name)
                    {
                        case "colors":
                            this.Colors.LoadColors(element);
                            break;
                        case "menu":
                            this.Menu.LoadMenu(element, this.Colors);
                            break;
                        case "borders":
                        case "controls":
                            LoadBorders(element);
                            break;
                        case "window":
                            this.Window.LoadWindow(element);
                            break;
                        default:
                            throw new NotSupportedException("Unknown tag: " + element.Name);
                    }
				}
			}
        }

        /// <summary>
        /// Loads settings for all controls.
        /// </summary>
        /// <param name="elements">xml file to load settings from.</param>
        private void LoadBorders(IXmlElement elements)
        {
			foreach (IXmlElement element in elements.Elements)
			{
				if ((element.Name == "border") || (element.Name == "control"))
				{
                    LoadControl(element);
				}
			}
        }

        /// <summary>
        /// Loads control settings from xml file.
        /// </summary>
        /// <param name="element">xml file to load from.</param>
        private void LoadControl(IXmlElement element)
        {
            String controlName = element.GetAttributeValue("control", "");

            if (controlName.Length <= 0)
            {
                return;
            }

            ControlSettings controlSettings = new ControlSettings();

            controlSettings.Name = controlName;
            controlSettings.ColorBack = this.Colors.GetColor(element.GetAttributeValue("backColor", ""), Colors.Control);
            controlSettings.ColorBorderLight1 = this.Colors.GetColor(element.GetAttributeValue("borderLight1", ""), Colors.ControlLight);
            controlSettings.ColorBorderLight2 = this.Colors.GetColor(element.GetAttributeValue("borderLight2", ""), Colors.Control);
            controlSettings.ColorBorderDark1 = this.Colors.GetColor(element.GetAttributeValue("borderDark1", ""), Colors.ControlDark);
            controlSettings.ColorBorderDark2 = this.Colors.GetColor(element.GetAttributeValue("borderDark2", ""), Colors.ControlDarkDark);
            controlSettings.BordersFilePrefix = element.GetAttributeValue("imagesPrefix", "images/control_");
            controlSettings.Skinned = UIUtils.FromString(element.GetAttributeValue("skinned", ""), this.skinned);
            controlSettings.BorderWidth = UIUtils.FromString(element.GetAttributeValue("borderWidth", null), 2);
            controlSettings.BackImage = element.GetAttributeValue("backImage", "");
            controlSettings.ControlSize = UIUtils.FromString(element.GetAttributeValue("controlSize", null), controlSettings.ControlSize);

            this.controlsSettings[controlName] = controlSettings;
        }

        private WindowTheme windowTheme = null;
        private MenuTheme menuTheme = null;
        private ThemeColors colors = null;
		private readonly String themeFolder = null;
		private String defaultFontName = "Tahoma";
		private int defaultFontSize = 8;
		private bool skinned = true;
		private IDictionary<String, ControlSettings> controlsSettings = new Dictionary<String, ControlSettings>();
	}
}
