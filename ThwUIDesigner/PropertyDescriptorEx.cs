using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using ThW.UI.Controls;
using ThW.UI.Design;
using ThW.UI.Fonts;

namespace ThW.UI.Utils.Designer
{
    class PropertyDescriptorEx : PropertyDescriptor
    {
        public PropertyDescriptorEx(Property property, ThW.UI.Controls.Control control) : base(property.Name, new Attribute[] { new CategoryAttribute(property.Group) })
        {
            this.property = property;
            this.control = control;
        }

        public override object GetValue(object component)
        {
            try
            {
                if (this.property is Property<MousePointers>)
                {
                    return ((Property<MousePointers>)this.property).Value;
                }
                else if (property is PropertyColor)
                {
                    ThW.UI.Utils.Color c = (ThW.UI.Utils.Color)((PropertyColor)this.property).Value;

                    if (null == c)
                    {
                        return System.Drawing.Color.FromArgb(0);
                    }
                    else
                    {
                        return System.Drawing.Color.FromArgb((int)(c.A * 255), (int)(c.R * 255), (int)(c.G * 255), (int)(c.B * 255));
                    }
                }
                else if (this.property is PropertyFont)
                {
                    PropertyFont propertyFont = (PropertyFont)this.property;

                    FontInfo font = this.control.FontInfo;

                    FontStyle style = FontStyle.Regular;

                    if (true == font.Bold)
                    {
                        style |= FontStyle.Bold;
                    }

                    if (true == font.Italic)
                    {
                        style |= FontStyle.Italic;
                    }
                    try
                    {
                        return new Font(font.Name, (float)font.Size, style);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return new Font(font.Name, (float)font.Size);
                    }
                }
                else if (this.property is PropertyAnchor)
                {
                    AnchorStyle s = (AnchorStyle)this.property.Value;

                    AnchorStyles result = AnchorStyles.None;

                    if ((s & AnchorStyle.AnchorTop) > 0)
                    {
                        result |= AnchorStyles.Top;
                    }

                    if ((s & AnchorStyle.AnchorLeft) > 0)
                    {
                        result |= AnchorStyles.Left;
                    }

                    if ((s & AnchorStyle.AnchorRight) > 0)
                    {
                        result |= AnchorStyles.Right;
                    }

                    if ((s & AnchorStyle.AnchorBottom) > 0)
                    {
                        result |= AnchorStyles.Bottom;
                    }

                    return result;
                }
                else if (null != this.property.Value)
                {
                    return this.property.Value;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return "";
            }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(string);
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override Type PropertyType
        {
            get
            {
                if (this.property is PropertyBoolean)
                {
                    return typeof(bool);
                }
                else if (this.property is PropertyInteger)
                {
                    return typeof(int);
                }
                else if (this.property is PropertyFloat)
                {
                    return typeof(float);
                }
                else if (this.property is PropertyColor)
                {
                    return typeof(System.Drawing.Color);
                }
                else if (this.property is Property<MousePointers>)
                {
                    return typeof(MousePointers);
                }
                else if (this.property is PropertyAnchor)
                {
                    return typeof(AnchorStyles);
                }
                else if (this.property is PropertyFont)
                {
                    return typeof(Font);
                }
                else if (this.property.Value != null)
                {
                    return this.property.Value.GetType();
                }
                else
                {
                    return typeof(string); 
                }
            }
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            if (value is System.Drawing.Color)
            {
                System.Drawing.Color c = (System.Drawing.Color)value;

                this.property.FromString((String)"#" + c.R + "," + c.G + "," + c.B + "," + c.A, this.control.Window.Desktop.Theme);
            }
            else if (value is MousePointers)
            {
                this.property.FromString(ThW.UI.Utils.Converter.Convert((MousePointers)value), this.control.Window.Desktop.Theme);
            }
            else if (value is Font)
            {
                Font f = (Font)value;

                this.control.FontInfo.SetFontInfo(f.Name, (int)f.Size, f.Bold, f.Italic);
            }
            else if (value is AnchorStyles)
            {
                AnchorStyles s = (AnchorStyles)value;

                AnchorStyle result = AnchorStyle.None;

                if ((s & AnchorStyles.Top) > 0)
                {
                    result |= AnchorStyle.AnchorTop;
                }

                if ((s & AnchorStyles.Left) > 0)
                {
                    result |= AnchorStyle.AnchorLeft;
                }

                if ((s & AnchorStyles.Right) > 0)
                {
                    result |= AnchorStyle.AnchorRight;
                }

                if ((s & AnchorStyles.Bottom) > 0)
                {
                    result |= AnchorStyle.AnchorBottom;
                }

                this.property.Value = result;
            }
            else
            {
                this.property.Value = value;
            }
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        private ThW.UI.Controls.Control control = null;
        private Property property = null;
    }
}
