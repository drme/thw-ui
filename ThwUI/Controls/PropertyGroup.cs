using System;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Groups proerties in a property grid.
    /// </summary>
	public class PropertyGroup : Control
	{
        /// <summary>
        /// Constructs poperty group obejct
        /// </summary>
        /// <param name="window">parent window</param>
        /// <param name="creationFlags">creation flags</param>
		public PropertyGroup(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
        }

        protected override void Render(Graphics graphics, int x, int y)
        {
			if (null == this.parentGrid)
			{
				this.parentGrid = (PropertyGrid)this.Parent;
			}

			int hh = 8 + this.parentGrid.FontInfo.Size;

			if (true == this.parentGrid.ShowSideBar)
			{
				// Render TopLine
                graphics.SetColor(this.Window.Desktop.Theme.Colors.Control);
				graphics.DrawRectangle(x + this.Bounds.X, y + this.Bounds.Y, this.Bounds.Width, hh);
			}

			if (true == this.parentGrid.ShowGroupLine)
			{
				graphics.SetColor(this.parentGrid.BorderColor);
				graphics.DrawRectangle(x + this.bounds.X, y + this.bounds.Y + hh - 1, this.bounds.Width, 1);
			}

			if (true == this.parentGrid.ShowPlus)
			{
				// Render Plus
				graphics.SetColor(this.PlusColor);
				graphics.DrawRectangle(this.Bounds.X + x + 3, this.Bounds.Y + y + 4, hh - 7, hh - 7);
                graphics.SetColor(this.Window.Desktop.Theme.Colors.Control);
				graphics.DrawRectangle(this.Bounds.X + x + 4, this.Bounds.Y + y + 5, hh - 9, hh - 9);
                graphics.SetColor(this.PlusColor);
				graphics.DrawRectangle(this.Bounds.X + x + 2 + 3, this.Bounds.Y + y + 4 + 4, 4 + 1, 1);
				
				if (false == this.expanded)
				{
					graphics.DrawRectangle(this.Bounds.X + x + 2 + 2 + 3, this.Bounds.Y + y + 1 + 5, 1, 5);
				}

                if (true == this.HasFocus)
                {
                    graphics.SetColor(this.Window.Desktop.Theme.Colors.HighlightBorder, 0.5f);
                    graphics.DrawBox(x + this.bounds.X, y + this.bounds.Y, this.bounds.Width, hh);
                    graphics.SetColor(this.Window.Desktop.Theme.Colors.Highlight, 0.5f);
                    graphics.DrawRectangle(x + this.bounds.X + 1, y + this.bounds.Y + 1, this.bounds.Width - 2, hh - 2);
                }

				// Render text
				graphics.SetColor(this.parentGrid.TextColor);
                if (null != this.parentGrid.FontInfo.Font)
				{
                    this.parentGrid.FontInfo.Font.DrawText(graphics, this.Bounds.X + x + hh, this.Bounds.Y + y + 4, this.controlText);
				}
			}
			else
			{
                if (true == this.HasFocus)
                {
                    graphics.SetColor(this.Window.Desktop.Theme.Colors.HighlightBorder, 0.5f);
                    graphics.DrawBox(x + this.bounds.X, y + this.Bounds.Y, this.bounds.Width, hh);
                    graphics.SetColor(this.Window.Desktop.Theme.Colors.Highlight, 0.5f);
                    graphics.DrawRectangle(x + this.Bounds.X + 1, y + this.Bounds.Y + 1, this.Bounds.Width - 2, hh - 2);
                }

				// Render text
				graphics.SetColor(this.parentGrid.TextColor);
                if (null != this.parentGrid.FontInfo.Font)
				{
                    this.parentGrid.FontInfo.Font.DrawText(graphics, this.Bounds.X + x + 3, this.Bounds.Y + y + 4, this.controlText);
				}
			}

			// Render text
            if ((0 != this.parentGrid.ValueText.Length) && (null != this.parentGrid.FontInfo.Font))
			{
				graphics.SetColor(this.parentGrid.TextColor);
                this.parentGrid.FontInfo.Font.DrawText(graphics, 4 + x + this.Bounds.X + this.Bounds.Width / 2, this.Bounds.Y + y + 4, this.parentGrid.ValueText);
			}

			RenderControls(graphics, x, y);
        }

        /// <summary>
        /// Handles click on property group event.
        /// </summary>
        /// <param name="X">mouse click X position.</param>
        /// <param name="Y">mouse click Y position.</param>
		protected override void OnClick(int x, int y)
        {
			base.OnClick(x, y);

			if ( (null != this.parentGrid) && (true == this.parentGrid.ShowPlus) )
			{
                x = this.Window.TranslateX(x);// this.mousePosition.X;
                y = this.Window.TranslateY(y);// this.mousePosition.Y;
				int nPlusSize = 16;

				if ( (x >= this.bounds.X) && (x <= this.bounds.X + nPlusSize) && (y >= this.bounds.Y) && (y <= this.bounds.Y + nPlusSize) )
				{
					this.expanded = !this.expanded;
				}
			}
        }

		public override Control FindControl(String controlName)
        {
			foreach (Control it in this.Controls)
			{
				if (controlName == it.Name)
				{
					return it;
				}
			}

			return null;
        }

		public void UpdateValue()
        {
			foreach (Control control in this.Controls)
			{
				((PropertyRow)control).UpdateValue(false);
			}
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
        protected override void AddProperties()
        {
            AddProperty(new PropertyString(this.Name, "name", "PropertyGroup", "name", (x) => { this.Name = x; }, () => { return this.Name; }));
            AddProperty(new PropertyString(this.textReference, "text", "PropertyGroup", "text", (x) => { this.textReference = x; }, () => { return this.textReference; }));
            AddProperty(new PropertyBoolean(this.Expanded, "expanded", "PropertyGroup", "expanded", (x) => { this.Expanded = x; }, () => { return this.Expanded; }));
            AddProperty(new PropertyColor(this.PlusColor, "plusColor", "PropertyGroup", "plus symbol color", (x) => { this.PlusColor = x; }, () => { return this.PlusColor; }));
        }

		internal override void UpdateSize()
        {
			if (null == this.parentGrid)
			{
				this.parentGrid = (PropertyGrid)this.Parent;
			}

			int step = 8 + this.parentGrid.FontInfo.Size;

			int h = step;

            if ((this.parentGrid.RowHeight > 0) && (this.parentGrid.RowHeight > this.parentGrid.FontInfo.Size))
            {
                step = 8 + this.parentGrid.RowHeight;
            }

			if (true == this.expanded)
			{
				foreach (Control controls in this.Controls)
				{
					controls.SetSize(0, h, this.Bounds.Width, step);
					h += step;
				}
			}
			else
			{
				foreach (Control controls in this.Controls)
				{
					controls.SetSize(-10000, -10000, 0, 0);
				}
			}

			this.Bounds.Height = h;
        }

        public override bool CanContainControl(String controlType)
        {
			return (controlType == PropertyRow.TypeName);
        }

        /// <summary>
        /// Is this group expanded.
        /// </summary>
        public bool Expanded
        {
            get
            {
                return this.expanded;
            }
            set
            {
                this.expanded = value;
            }
        }

        /// <summary>
        /// Plus icon color
        /// </summary>
        public Color PlusColor
        {
            get
            {
                return this.plusColor;
            }
            set
            {
                this.plusColor = value;
            }
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "propertyGroup";
            }
        }

		private bool expanded = true;
		private PropertyGrid parentGrid = null;
        private Color plusColor = Colors.Black;
	}
}
