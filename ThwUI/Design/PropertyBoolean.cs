using System;
using System.Collections.Generic;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Design
{
    /// <summary>
    /// Property of a boolean type.
    /// </summary>
    public class PropertyBoolean : Property<bool>
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
        public PropertyBoolean(bool defaultValue, String name, String group, String description, SetValueHandler<bool> setter, GetValueHandler<bool> getter) : base(defaultValue, name, group, description, setter, getter, ComboBox.TypeName)
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
                if ((value.Length > 0) && (('t' == value[0]) || ('T' == value[0]) || ('1' == value[0])))
                {
                    this.setter(true);
                }
                else
                {
                    this.setter(false);
                }

                RaiseChangeEvent();
            }
        }

        /// <summary>
        /// Acceptable values.
        /// </summary>
        public override List<String> GetAcceptableValues(Theme theme)
        {
            return new List<String>(new String[] { "true", "false" });
        }
    }
}
