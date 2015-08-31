using System;
using System.Globalization;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Design
{
    /// <summary>
    /// Property of a float type.
    /// </summary>
    public class PropertyFloat : Property<float>
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
        public PropertyFloat(float defaultValue, String name, String group, String description, SetValueHandler<float> setter, GetValueHandler<float> getter) : base(defaultValue, name, group, description, setter, getter, TextBox.TypeName)
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
                float floatValue = (float)double.Parse(value.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator));

                this.setter(floatValue);

                RaiseChangeEvent();
            }
        }
    }
}
