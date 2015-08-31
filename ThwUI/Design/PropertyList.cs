using System;
using System.Collections.Generic;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Design
{
    /// <summary>
    /// Generic property for a list of values.
    /// </summary>
    /// <typeparam name="ListType">list of values type</typeparam>
    public class PropertyList<ListType> : Property<ListType>
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
        public PropertyList(ListType defaultValue, String name, String group, String description, SetValueHandler<ListType> setter, GetValueHandler<ListType> getter) : base(defaultValue, name, group, description, setter, getter, ComboBox.TypeName)
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
                ListType v = (ListType)Converter.Convert(value, (ListType)this.getter());

                this.setter(v);

                RaiseChangeEvent();
            }
        }

        /// <summary>
        /// Converts property value to a string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Converter.Convert(this.getter());
        }

        /// <summary>
        /// The list of accepatable property list values.
        /// </summary>
        public override List<String> GetAcceptableValues(Theme theme)
        {
            if (acceptableValues.Count <= 0)
            {
                acceptableValues = Converter.GetValues((ListType)defaultValue);
            }

            return acceptableValues;
        }
    }
}
