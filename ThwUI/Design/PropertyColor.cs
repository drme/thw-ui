using System;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Design
{
    public class PropertyColor : Property<Color>
    {
        public PropertyColor(Color defaultValue, String name, String group, String description, SetValueHandler<Color> setter, GetValueHandler<Color> getter) : base(defaultValue, name, group, description, setter, getter, "colorPicker")
        {
        }

        public override String ToString()
        {
            return this.getter().Name;
        }

        public override void FromString(String strValue, Theme theme)
        {
            this.setter(theme.Colors.GetColor(strValue));

            RaiseChangeEvent();
        }
    }
}
