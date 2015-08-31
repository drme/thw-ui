using System;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Design
{
    /// <summary>
    /// Controls archoring to its' parent property.
    /// </summary>
    public class PropertyAnchor : Property<AnchorStyle>
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
        public PropertyAnchor(AnchorStyle defaultValue, String name, String group, String description, SetValueHandler<AnchorStyle> setter, GetValueHandler<AnchorStyle> getter) : base(defaultValue, name, group, description, setter, getter, AnchorPicker.TypeName)
        {
        }

        /// <summary>
        /// Converts property value from a string.
        /// </summary>
        /// <param name="value">value as a string to convert from.</param>
        public override void FromString(String value, Theme theme)
        {
            try
            {
                AnchorStyle style = (AnchorStyle)int.Parse(value);

                this.setter(style);
            }
            catch (Exception)
            {
                AnchorStyle s = AnchorStyle.None;

                value = value.ToLower();

                if (value.Contains("top"))
                {
                    s = s | AnchorStyle.AnchorTop;
                }

                if (value.Contains("left"))
                {
                    s = s | AnchorStyle.AnchorLeft;
                }

                if (value.Contains("bottom"))
                {
                    s = s | AnchorStyle.AnchorBottom;
                }

                if (value.Contains("right"))
                {
                    s = s | AnchorStyle.AnchorRight;
                }

                this.setter(s);
            }

            RaiseChangeEvent();
        }

        /// <summary>
        /// Converts property value to the string.
        /// </summary>
        /// <returns>proeprty value as a string.</returns>
        public override String ToString()
        {
            int value = (int)this.getter();

            return value.ToString();
        }
    }
}
