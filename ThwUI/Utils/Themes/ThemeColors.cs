using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThW.UI.Utils.Themes
{
    /// <summary>
    /// Theme colors.
    /// </summary>
    public class ThemeColors
    {
        /// <summary>
        /// Constructs theme colors object.
        /// </summary>
        internal ThemeColors()
        {
            RegisterSystemColor(ControlLight);
            RegisterSystemColor(ControlDark);
            RegisterSystemColor(ControlDarkDark);
            RegisterSystemColor(Control);
            RegisterSystemColor(Highlight);
            RegisterSystemColor(HighlightBorder);
            RegisterSystemColor(TextDark);
            RegisterSystemColor(TextLight);
            RegisterSystemColor(BackLight);
            RegisterSystemColor(BackBorder);
            RegisterSystemColor(BackLightLight);
            RegisterSystemColor(BorderDark);
            RegisterSystemColor(WindowTitleStart);
            RegisterSystemColor(WindowTitleEnd);
            RegisterSystemColor(Window);

            RegisterSystemColor(Colors.Black);
            RegisterSystemColor(Colors.DarkRed);
            RegisterSystemColor(Colors.DarkGreen);
            RegisterSystemColor(Colors.DarkYellow);
            RegisterSystemColor(Colors.DarkBlue);
            RegisterSystemColor(Colors.Violet);
            RegisterSystemColor(Colors.Cyan);
            RegisterSystemColor(Colors.DarkGray);
            RegisterSystemColor(Colors.Gray);
            RegisterSystemColor(Colors.Red);
            RegisterSystemColor(Colors.Green);
            RegisterSystemColor(Colors.Yellow);
            RegisterSystemColor(Colors.Blue);
            RegisterSystemColor(Colors.Magenta);
            RegisterSystemColor(Colors.LightCyan);
            RegisterSystemColor(Colors.White);
            RegisterSystemColor(Colors.None);
        }

        /// <summary>
        /// Loads theme colro values from theme xml file.
        /// </summary>
        /// <param name="element">colors xml element.</param>
        internal void LoadColors(IXmlElement element)
        {
            this.ControlLight.UpdateColor(element.GetAttributeValue("ControlLight", ""), this);
            this.ControlDark.UpdateColor(element.GetAttributeValue("ControlDark", ""), this);
            this.ControlDarkDark.UpdateColor(element.GetAttributeValue("ControlDarkDark", ""), this);
            this.Control.UpdateColor(element.GetAttributeValue("Control", ""), this);
            this.Highlight.UpdateColor(element.GetAttributeValue("Highlight", ""), this);
            this.HighlightBorder.UpdateColor(element.GetAttributeValue("HighlightBorder", ""), this);
            this.TextDark.UpdateColor(element.GetAttributeValue("TextDark", ""), this);
            this.TextLight.UpdateColor(element.GetAttributeValue("TextLight", ""), this);
            this.BackLight.UpdateColor(element.GetAttributeValue("BackLight", ""), this);
            this.BackBorder.UpdateColor(element.GetAttributeValue("BackBorder", ""), this);
            this.BackLightLight.UpdateColor(element.GetAttributeValue("BackLightLight", ""), this);
            this.BorderDark.UpdateColor(element.GetAttributeValue("BorderDark", ""), this);
            this.WindowTitleStart.UpdateColor(element.GetAttributeValue("WindowTitleStart", ""), this);
            this.WindowTitleEnd.UpdateColor(element.GetAttributeValue("WindowTitleEnd", ""), this);
            this.Window.UpdateColor(element.GetAttributeValue("Window", ""), this);
        }

        /// <summary>
        /// Gets color by it's name.
        /// </summary>
        /// <param name="name">color name.</param>
        /// <param name="colorDefault">color to return if requested color is not found.</param>
        /// <returns>actual color object.</returns>
        public Color GetColor(String name, Color colorDefault)
        {
            if ((null == name) || (0 == name.Length))
            {
                return colorDefault;
            }

            if (true == name.StartsWith("#"))
            {
                return new Color(name.Substring(1));
            }

            foreach (Color color in this.systemColors)
            {
                if (true == name.Equals(color.Name))
                {
                    return color;
                }
            }

            return colorDefault;
        }

        /// <summary>
        /// Gets color by it's name.
        /// </summary>
        /// <param name="name">color name.</param>
        /// <returns>actual color object. if not found returs white color object.</returns>
        public Color GetColor(String name)
        {
            return GetColor(name, Colors.White);
        }

        /// <summary>
        /// A list of systems colors.
        /// </summary>
        internal IEnumerable<Color> SystemColors
        {
            get
            {
                return this.systemColors;
            }
        }

        /// <summary>
        /// Registers color in system colors list. The list is used for searhing color by name.
        /// </summary>
        /// <param name="color">color to register to the system colors list.</param>
        private void RegisterSystemColor(Color color)
        {
            this.systemColors.Add(color);
        }

        public readonly Color ControlLight = new Color(1.0f, 1.0f, 1.0f, 1.0f, "ControlLight");
        public readonly Color ControlDark = new Color(0.5f, 0.5f, 0.5f, 1.0f, "ControlDark");
        public readonly Color ControlDarkDark = new Color(0.25f, 0.25f, 0.25f, 1.0f, "ControlDarkDark");
        public readonly Color Control = new Color(212, 208, 200, 255, "Control");
        public readonly Color Highlight = new Color(179, 255, 179, 255, "Highlight");
        public readonly Color HighlightBorder = new Color(0.0f, 1.0f, 0.0f, 1.0f, "HighlightBorder");
        public readonly Color TextDark = new Color(0.0f, 0.0f, 0.0f, 1.0f, "TextDark");
        public readonly Color TextLight = new Color(1.0f, 1.0f, 1.0f, 1.0f, "TextLight");
        public readonly Color BackLight = new Color(0.858f, 0.847f, 0.819f, 1.0f, "BackLight");
        public readonly Color BackBorder = new Color(0.4f, 0.4f, 0.4f, 1.0f, "BackBorder");
        public readonly Color BackLightLight = new Color(0.97f, 0.97f, 0.97f, 1.0f, "BackLightLight");
        public readonly Color BorderDark = new Color(0.65f, 0.65f, 0.65f, 1.0f, "BorderDark");
        public readonly Color WindowTitleStart = new Color(82, 150, 255, 255, "WindowTitleStart");
        public readonly Color WindowTitleEnd = new Color(49, 113, 214, 255, "WindowTitleEnd");
        public readonly Color Window = new Color(1.0f, 1.0f, 1.0f, 1.0f, "Window");
        private List<Color> systemColors = new List<Color>();
    }
}
