using System;
using ThW.UI.Controls;

namespace ThW.UI.Design
{
    /// <summary>
    /// Image property.
    /// </summary>
    public class PropertyImage : PropertyString
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
        public PropertyImage(String currentValue, String name, String group, String description, SetValueHandler<String> setter, GetValueHandler<String> getter) : base(currentValue, name, group, description, setter, getter)
        {
			this.ControlType = FilePicker.TypeName;
        }
    }
}
