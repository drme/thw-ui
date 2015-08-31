using System;
using System.Collections.Generic;
using ThW.UI.Controls;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Design
{
    public class PropertyFont : PropertyString
    {
        /// <summary>
        /// Constructs property object.
        /// </summary>
        /// <param name="engine">ui engine for enumerating available fonts list.</param>
        /// <param name="defaultValue">default value.</param>
        /// <param name="name">property name.</param>
        /// <param name="group">propertry group.</param>
        /// <param name="description">property description.</param>
        /// <param name="setter">property setter function.</param>
        /// <param name="getter">property getter function.</param>
        public PropertyFont(UIEngine engine, String defaultValue, String name, String group, String description, SetValueHandler<String> setter, GetValueHandler<String> getter) : base(defaultValue, name, group, description, setter, getter)
        {
            this.engine = engine;
            this.ControlType = TextBox.TypeName;
        }

        /// <summary>
        /// Get acceptable fonts lists.
        /// </summary>
        public override List<String> GetAcceptableValues(Theme theme)
        {
            if ((acceptableValues.Count <= 0) && (null != engine))
            {
                List<String> fonts = engine.GetFontsFactory(null).GetAvailableFonts(this.engine, theme);

				if (fonts.Count > 0)
				{
					this.ControlType = ComboBox.TypeName;

					foreach (String it in fonts)
					{
						acceptableValues.Add(it);
					}
				}
            }

            return acceptableValues;
        }

        private UIEngine engine = null;
    }
}
