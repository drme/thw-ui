using System;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Design
{
    /// <summary>
    /// Property of integer type.
    /// </summary>
    public class PropertyInteger : Property<int>
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
        public PropertyInteger(int defaultValue, String name, String group, String description, SetValueHandler<int> setter, GetValueHandler<int> getter) :  base(defaultValue, name, group, description, setter, getter, TextBox.TypeName)
        {
        }

        /// <summary>
        /// Converts property value from a string.
        /// </summary>
        /// <param name="value">value as a string to convert from.</param>
        public override void FromString(String value, Theme theme)
        {
            if (null != value)
            {
                this.setter(int.Parse(value));

                RaiseChangeEvent();
            }
        }
    }
}
