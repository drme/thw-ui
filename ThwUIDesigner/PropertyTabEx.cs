using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.Design;
using ThW.UI.Controls;
using ThW.UI.Design;

namespace ThW.UI.Utils.Designer
{
    class PropertyTabEx : PropertyTab
    {
        public override PropertyDescriptorCollection GetProperties(object component, Attribute[] attributes)
        {
            if (component is Control)
            {
                PropertyDescriptorCollection properties = null;

                if (attributes != null)
                {
                    properties = TypeDescriptor.GetProperties(component, attributes);
                }
                else
                {
                    properties = TypeDescriptor.GetProperties(component);
                }

                Control control = (Control)component;

                List<PropertyDescriptor> propertiesDescriptors = new List<PropertyDescriptor>();

                foreach (Property property in control.Properties)
                {
                    propertiesDescriptors.Add(new PropertyDescriptorEx(property, control));
                }

                return new PropertyDescriptorCollection(propertiesDescriptors.ToArray());
            }
            else
            {
                return null;
            }
        }

        public override PropertyDescriptorCollection GetProperties(object component)
        {
            return this.GetProperties(component, null);
        }

        public override string TabName
        {
            get
            {
                return "Properties";
            }
        }

        public override Bitmap Bitmap
        {
            get
            {
                return this.icon;
            }
        }

        private Bitmap icon = new Bitmap(16, 16);
    }
}
