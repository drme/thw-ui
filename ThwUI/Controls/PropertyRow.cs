using System;
using System.Collections.Generic;
using ThW.UI.Design;
using ThW.UI.Fonts;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Displays one property row: key, value.
    /// </summary>
	public class PropertyRow : Control
	{
		public PropertyRow(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.BackColor = Colors.White;
            this.IconAlignment = ContentAlignment.MiddleLeft;
        }

        /// <summary>
        /// Renders property row at X, Y position
        /// </summary>
        /// <param name="graphics">graphics api to do rendering</param>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordiante</param>
		protected override void Render(Graphics graphics, int x, int y)
        {
			if (null == this.parentGrid)
			{
				this.parentGrid = (PropertyGrid)this.Parent.Parent;
			}

			if (true == this.parentGrid.ShowRowsSeparator)
			{
				graphics.SetColor(this.parentGrid.RowSeparatorColor);
				graphics.DrawRectangle(x + this.Bounds.X, y + this.Bounds.Y + this.Bounds.Height, this.Bounds.Width, this.parentGrid.RowSeparatorHeight);
			}

            if (true == this.parentGrid.ShowKeyValueSeparator)
            {
                graphics.SetColor(this.parentGrid.KeyValueSeparatorColor);
                graphics.DrawRectangle(x + this.Bounds.X + this.Bounds.Width / 2, y + this.Bounds.Y, this.parentGrid.KeyValueSeparatorWidth, this.Bounds.Height);
            }

			RenderName(graphics, x, y);

			if (null == this.property)
			{
				return;
			}

			//RenderValue(render, X, Y);

			if (null != this.inputControl)
			{
				this.inputControl.Bounds.UpdateSize(this.Bounds.Width / 2 + 1, 1, this.Bounds.Width / 2-1, this.Bounds.Height-1);
			}
			else
			{
				CreateInputControl();
			}

			//if (false == this.inputControl.HasFocus())
			{
				if (this.property.ToString() != this.inputControl.Text)
				{
					this.inputControl.Text = this.property.ToString();
				}
			}

            if (true == this.parentGrid.ShowSideBar)
            {
                this.IconImageOffset.X = 16;
                this.TextOffset.X = 16;
            }
            else
            {
                this.IconImageOffset.X = 0;
                this.TextOffset.X = 0;
            }

			RenderControls(graphics, x, y);
        }

        protected override void RenderControls(Graphics graphics, int x, int y)
        {
			base.RenderControls(graphics, x, y);

			if (true == this.HasFocus)
			{
				RenderSelection(graphics, x, y);
			}
			else if ( (null != this.inputControl) && (true == this.inputControl.HasFocus) )
			{
                graphics.SetColor(this.Window.Desktop.Theme.Colors.Highlight, 0.5f);
				graphics.DrawRectangle(x + this.bounds.X + 1, y + this.bounds.Y + 1, this.bounds.Width / 2 - 1, this.bounds.Height - 2);

                graphics.SetColor(this.Window.Desktop.Theme.Colors.HighlightBorder, 0.5f);
				graphics.DrawBox(x + this.Bounds.X, y + this.Bounds.Y, this.Bounds.Width / 2, this.Bounds.Height);
			}
        }

        /// <summary>
        /// Property object that holds property value and description for this row.
        /// </summary>
		public Property Property
        {
            set
            {
                this.property = value;
                this.property.ValueChanged += this.PropertyValueChanged;

                if (null != this.inputControl)
                {
                    this.inputControl.Text = this.property.ToString();

                    if (this.inputControl.Type == ComboBox.TypeName)
                    {
                        ComboBox comboBox = (ComboBox)(this.inputControl);

                        String propertyValue = this.property.ToString();

                        comboBox.BackColor = Colors.White;

                        IEnumerable<ComboBoxItem> lst = comboBox.Items;

                        foreach (ComboBoxItem it in lst)
                        {
                            if (true == UIUtils.EqualsIgnoringCase(it.Name, propertyValue))
                            {
                                comboBox.SelectedItem = it;
                                break;
                            }
                        }
                    }
                }
            }
            get
            {
                return this.property;
            }
        }

        private void PropertyValueChanged(Property sender, EventArgs args)
        {
            if (null != this.inputControl)
            {
                this.inputControl.Value = this.property.Value;
            }
        }

		public void UpdateValue(bool noCheck)
        {
            if ((null != this.inputControl) && (null != this.property))
            {
                if ((true == noCheck) || (this.inputControl.HasFocus))
                {
                    this.property.FromString(this.inputControl.Text, this.Window.Desktop.Theme);
                    this.property.Value = this.inputControl.Value;
                }
            }
        }

        /// <summary>
        /// Renders property row key text and icon.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        protected void RenderName(Graphics graphics, int x, int y)
        {
            IFont font = this.Parent.Parent.FontInfo.Font;

			if (null != font)
			{
				String name = (null != this.property) ? this.property.Text : this.controlText;

				graphics.SetColor(this.Parent.Parent.TextColor);

				int len = name.Length;

                while (font.TextLength(name, 0, len) > this.Bounds.Width / 2 - 16)
				{
					len--;
				}

                int xOffset = 0;

                if (this.parentGrid.RowHeight > 0)
                {
                    xOffset = this.parentGrid.RowHeight;
                }

                RenderText(graphics, x + xOffset, y, ContentAlignment.MiddleLeft, name);
			}

            RenderIcon(graphics, x, y);
        }

        protected void RenderValue(Graphics graphics, int x, int y)
        {
            IFont font = this.Parent.Parent.FontInfo.Font;

            if (null != font)
            {
                String value = this.property.ToString();

                int len = value.Length;

                while (font.TextLength(value, 0, len) > this.Bounds.Width / 2 - 16)
                {
                    len--;
                }

                font.DrawText(graphics, 4 + x + this.Bounds.X + this.Bounds.Width / 2, y + this.Bounds.Y + 4, value, 0, len);
            }
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
        protected override void AddProperties()
        {
            AddProperty(new PropertyString(this.Name, "name", "Scripting", "name", (x) => { this.Name = x; }, () => { return this.Name; }));
            AddProperty(new PropertyString(this.Text, "text", "Property", "text", (x) => { this.Text = x; }, () => { return this.Text; }));
            AddProperty(new PropertyString(this.PropertyType, "propertyType", "Property", "propertyType", (x) => { this.PropertyType = x; }, () => { return this.PropertyType; }));

            AddIconProperties();

			this.propertiesAreLoaded = true;
        }

		internal override void UpdateSize()
        {
            // controlled by parent
        }

        public override bool CanContainControl(String controlType)
        {
            return false;
        }

        protected override void LoadAttributes(IXmlElement element)
        {
			base.LoadAttributes(element);

			if (this.propertyType.Length > 0)
			{
				Property property = this.Engine.CreateProperty(this.propertyType, this.Name, "", this.textReference);

				if (null != property)
				{
					this.Property = property;
				}
			}
        }

        /// <summary>
        /// Creates actual control for getting a property value.
        /// </summary>
        protected void CreateInputControl()
        {
			if (null == this.inputControl)
			{
				this.inputControl = CreateControl(this.property.ControlType, CreationFlag.FlagsNone);
				this.inputControl.Border = BorderStyle.None;
				this.inputControl.BackColor = Colors.None;
                this.inputControl.Value = this.property.Value;
                this.inputControl.ValueChanged += (x, y) => { UpdateValue(true); };
                this.inputControl.LostFocus += (x, y) => { UpdateValue(true); };
				this.inputControl.Bounds = new Rectangle(this.Bounds.Width / 2 + 1, 1, this.Bounds.Width / 2-1, this.Bounds.Height-1);

				if (this.inputControl.Type == ComboBox.TypeName)
				{
					ComboBox comboBox = (ComboBox)(this.inputControl);

					String value = this.property.ToString();
				
					comboBox.BackColor = Colors.White;

					List<String> acceptableValues = this.property.GetAcceptableValues(this.Window.Desktop.Theme);

					foreach (String acceptableValue in acceptableValues)
					{
						ComboBoxItem item = comboBox.AddItem(acceptableValue, acceptableValue, "", null);

						if (true == UIUtils.EqualsIgnoringCase(acceptableValue, value))
						{
							comboBox.SelectedItem = item;
						}
					}
				}

				this.inputControl.Text = this.property.ToString();

				AddControl(this.inputControl);
			}
        }
        
        /// <summary>
        /// Property type. Specifies which control to use while editing this property value.
        /// </summary>
        public String PropertyType
        {
            get
            {
                return this.propertyType;
            }
            set
            {
                this.propertyType = value;
            }
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "propertyRow";
            }
        }

		internal Control inputControl = null;
        private Property property = null;
        private PropertyGrid parentGrid = null;
		private String propertyType = "";
	}
}
