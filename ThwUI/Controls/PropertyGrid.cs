using System;
using System.Collections.Generic;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Properties grid control. Displays table of key value pairs with some grouping.
    /// </summary>
	public class PropertyGrid : Control
	{
        /// <summary>
        /// Constructs property grid object.
        /// </summary>
        /// <param name="window">parent window</param>
        /// <param name="creationFlags">creation flags</param>
        public PropertyGrid(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
            this.RowSeparatorColor = window.Desktop.Theme.Colors.Control;
            this.KeyValueSeparatorColor = window.Desktop.Theme.Colors.Control;
            this.Border = BorderStyle.Flat;
            this.BackColor = Colors.White;
			this.ShowSideBar = true;
        }

        /// <summary>
        /// Renders property grid at X, Y position.
        /// </summary>
        /// <param name="graphics">graphics api for rendering.</param>
        /// <param name="X">X coordiante.</param>
        /// <param name="Y">Y coordinate.</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
			RenderBackground(graphics, x, y);
			RenderBorder(graphics, x, y);

			if (true == this.ShowSideBar)
			{
                graphics.SetColor(this.Window.Desktop.Theme.Colors.Control);
				graphics.DrawRectangle(x + this.Bounds.X + 1, y + this.Bounds.Y + 1, 14, this.Bounds.Height - 2);
			}

			RenderControls(graphics, x, y);
        }

        /// <summary>
        /// Finds control by its' name.
        /// </summary>
        /// <param name="controlName">control name.</param>
        /// <returns>discovered control or null if not found.</returns>
        public override Control FindControl(String controlName)
        {
			foreach (Control control in this.Controls)
			{
				if (control.Name == controlName)
				{
					return control;
				}
			}

			return null;
        }

        /// <summary>
        /// Assigns a list of properties to display.
        /// </summary>
        /// <param name="properties"></param>
        public void SetProperties(IEnumerable<Property> properties)
        {
            foreach (Property property in properties)
            {
                AddProperty(property, property.Group);
            }
        }

        /// <summary>
        /// Refreshes properties values
        /// </summary>
        public void UpdateValue()
        {
			foreach (Control propertyGroup in this.Controls)
			{
				((PropertyGroup)propertyGroup).UpdateValue();
			}
        }

        protected override void OnLanguageChanged()
        {
			base.OnLanguageChanged();

			if (false == this.NeedTranslation)
			{
				this.valueText = this.valueTextReference;

				return;
			}

            if (null != this.Engine.Language)
			{
				String group = (null == this.Window) ? this.Name : this.Window.Name;

                this.valueText = this.Engine.Language.Translate("window." + group, this.valueTextReference);
			}
			else
			{
				this.valueText = this.valueTextReference;
			}
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
        protected override void AddProperties()
        {
			base.AddProperties();

            String groupName = "Appearance";

            base.AddProperty(new PropertyString(this.ValueText, "valueText", groupName, "valueText", (x) => { this.ValueText = x; }, () => { return this.ValueText; }));
            base.AddProperty(new PropertyBoolean(this.ShowPlus, "showPlus", groupName, "showPlus", (x) => { this.ShowPlus = x; }, () => { return this.ShowPlus; }));
            base.AddProperty(new PropertyBoolean(this.ShowGroupLine, "showGroupLine", groupName, "showGroupLine", (x) => { this.ShowGroupLine = x; }, () => { return this.ShowGroupLine; }));
            base.AddProperty(new PropertyBoolean(this.ShowRowsSeparator, "showRowsSeparator", groupName, "showRowsSeparator", (x) => { this.ShowRowsSeparator = x; }, () => { return this.ShowRowsSeparator; }));
            base.AddProperty(new PropertyBoolean(this.ShowSideBar, "showSideBar", groupName, "showSideBar", (x) => { this.ShowSideBar = x; }, () => { return this.ShowSideBar; }));
            base.AddProperty(new PropertyInteger(this.RowHeight, "rowHeight", groupName, "row Height", (x) => { this.RowHeight = x; }, () => { return this.RowHeight; }));
            base.AddProperty(new PropertyInteger(this.RowSeparatorHeight, "rowSeparatorHeight", groupName, "row separator Height", (x) => { this.RowSeparatorHeight = x; }, () => { return this.RowSeparatorHeight; }));
            base.AddProperty(new PropertyColor(this.RowSeparatorColor, "rowSeparatorColor", groupName, "row separator color", (x) => { this.RowSeparatorColor = x; }, () => { return this.RowSeparatorColor; }));
            base.AddProperty(new PropertyBoolean(this.ShowKeyValueSeparator, "showKeyValueSeparator", groupName, "show key value separator", (x) => { this.ShowKeyValueSeparator = x; }, () => { return this.ShowKeyValueSeparator; }));
            base.AddProperty(new PropertyInteger(this.KeyValueSeparatorWidth, "keyValueSeparatorWidth", groupName, "key value separator Width", (x) => { this.KeyValueSeparatorWidth = x; }, () => { return this.KeyValueSeparatorWidth; }));
            base.AddProperty(new PropertyColor(this.KeyValueSeparatorColor, "keyValueSeparatorColor", groupName, "key value separator color", (x) => { this.KeyValueSeparatorColor = x; }, () => { return this.KeyValueSeparatorColor; }));
        }

        internal override void UpdateSize()
        {
			UpdateSizeSelf();

			int h = 1;

			foreach (PropertyGroup control in this.Controls)
			{
				control.SetSize(1, h, this.Bounds.Width - 2, 0);
				control.UpdateSize();
				h += control.Bounds.Height;
			}

			if (this.Bounds.Height < h)
			{
				this.Bounds.Height = h;
			}
        }

        public override bool CanContainControl(String typeName)
        {
			return (typeName == PropertyGroup.TypeName);
        }

        protected void AddProperty(Property property, String groupName)
        {
			PropertyGroup propertyGroup = FindGroup(groupName);

			if (null == propertyGroup)
			{
				propertyGroup = CreateControl<PropertyGroup>();
			
				propertyGroup.Name = groupName;
				propertyGroup.Text = groupName;
				propertyGroup.FontInfo.Size = this.FontInfo.Size;
			
				this.AddControl(propertyGroup);
			}

			PropertyRow propertyRow = FindRow(property.Name, propertyGroup);

			if (null == propertyRow)
			{
				propertyRow = CreateControl<PropertyRow>();

				propertyRow.Name = property.Name;
				propertyRow.Text = property.Text;
				propertyRow.FontInfo.Size = this.FontInfo.Size;
				
				propertyGroup.AddControl(propertyRow);
			}

			propertyRow.Property = property;
        }

        protected PropertyGroup FindGroup(String groupName)
        {
			foreach (Control it in this.Controls)
			{
				if (groupName == it.Name)
				{
					return (PropertyGroup)it;
				}
			}

			return null;
        }

        protected PropertyRow FindRow(String propertyName, PropertyGroup propertyGroup)
        {
			foreach (Control it in propertyGroup.Controls)
			{
				if (propertyName == it.Name)
				{
					return (PropertyRow)it;
				}
			}

			return null;
        }

        /// <summary>
        /// Is the expand icon visible.
        /// </summary>
        public bool ShowPlus
        {
            get
            {
                return this.showPlus;
            }
            set
            {
                this.showPlus = value;
            }
        }

        /// <summary>
        /// Is Groups separation label visible.
        /// </summary>
        public bool ShowGroupLine
        {
            get
            {
                return this.showGroupLine;
            }
            set
            {
                this.showGroupLine = value;
            }
        }

        /// <summary>
        /// Is rows separating line visible.
        /// </summary>
        public bool ShowRowsSeparator
        {
            get
            {
                return this.showRowsSeparator;
            }
            set
            {
                this.showRowsSeparator = value;
            }
        }

        /// <summary>
        /// Is vertical side bar visible.
        /// </summary>
		public bool ShowSideBar
		{
			get;
			set;
		}

        /// <summary>
        /// Value column header text.
        /// </summary>
        public String ValueText
        {
            get
            {
                return this.valueText;
            }
            set
            {
                this.valueTextReference = value;

                OnLanguageChanged();
            }
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "propertyGrid";
            }
        }

        /// <summary>
        /// Forces Property row Height.
        /// </summary>
        public int RowHeight
        {
            set
            {
                this.rowHeight = value;
            }
            get
            {
                return this.rowHeight;
            }
        }

        /// <summary>
        /// Row separator Height.
        /// </summary>
        public int RowSeparatorHeight
        {
            get
            {
                return this.rowSeparatorHeight;
            }
            set
            {
                this.rowSeparatorHeight = value;
            }
        }

        /// <summary>
        /// Row separator color.
        /// </summary>
        public Color RowSeparatorColor
        {
            get
            {
                return this.rowSeparatorColor;
            }
            set
            {
                this.rowSeparatorColor = value;
            }
        }

        /// <summary>
        /// Is key value separator visible
        /// </summary>
        public bool ShowKeyValueSeparator
        {
            get
            {
                return this.showKeyValueSeparator;
            }
            set
            {
                this.showKeyValueSeparator = value;
            }
        }

        /// <summary>
        /// Key value separator Width.
        /// </summary>
        public int KeyValueSeparatorWidth
        {
            get
            {
                return this.keyValueSeparatorWidth;
            }
            set
            {
                this.keyValueSeparatorWidth = value;
            }
        }

        /// <summary>
        /// Key value separator color.
        /// </summary>
        public Color KeyValueSeparatorColor
        {
            get
            {
                return this.keyValueSeparatorColor;
            }
            set
            {
                this.keyValueSeparatorColor = value;
            }
        }

		private bool showPlus = true;
		private bool showGroupLine = false;
		private bool showRowsSeparator = true;
		private String valueTextReference = "";
		private String valueText = "";
        private int rowHeight = 0;
        private int rowSeparatorHeight = 1;
        private Color rowSeparatorColor = Colors.White;
        private bool showKeyValueSeparator = true;
        private int keyValueSeparatorWidth = 1;
        private Color keyValueSeparatorColor = Colors.White;
	}
}
