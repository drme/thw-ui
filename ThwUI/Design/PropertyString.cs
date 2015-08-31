using System;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Design
{
    /// <summary>
    /// Control property of a string type.
    /// </summary>
    public class PropertyString : Property<String>
    {
        /// <summary>
        /// Constructs property object.
        /// </summary>
        /// <param name="defaultValue">default value.</param>
        /// <param name="name">property name.</param>
        /// <param name="group">propertry group.</param>
        /// <param name="description">property description.</param>
        /// <param name="setter">property setter function.</param>
        /// <param name="getter">property getter function.</param>
        public PropertyString(String defaultValue, String name, String group, String description, SetValueHandler<String> setter, GetValueHandler<String> getter) : base(defaultValue, name, group, description, setter, getter, TextBox.TypeName)
        {
        }

        public override void FromString(String value, Theme theme)
        {
            this.setter(value);

            RaiseChangeEvent();
        }
    }
}
