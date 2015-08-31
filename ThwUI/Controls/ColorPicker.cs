using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
	internal class ColorPicker : Control
	{
		public ColorPicker(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
            this.button = CreateControl<Button>(CreationFlag.InternalControl);

			this.Border = BorderStyle.Lowered;
			this.NeedTranslation = false;
			this.button.NeedTranslation = false;
			this.button.Text = "..";
            this.button.Clicked += this.ButtonClicked;
            this.BackColor = window.Desktop.Theme.Colors.Window;
			this.TextAlignment = ContentAlignment.MiddleLeft;

			AddControl(this.button);
        }

        private void ButtonClicked(Button sender, System.EventArgs e)
        {
            if (null != this.Window)
            {
                Desktop desktop = this.Window.Desktop;

                if (null != desktop)
                {
                    Window window = desktop.NewRegisteredWindow(ColorChooser.TypeName);

                    if (null != window)
                    {
                        window.Closing += this.ColorChooserClosing;
                        ((ColorChooser)window).SelectedColor = this.BackColor;
                    }
                }
            }
        }

        private void ColorChooserClosing(Window sender, EventArgs args)
        {
            ColorChooser window = (ColorChooser)sender;

            if ((null != window) && (DialogResult.DialogResultOK == window.DialogResult))
            {
                this.SelectedColor = window.SelectedColor;
            }
        }

        protected override void Render(Graphics graphics, int x, int y)
        {
			if (BorderStyle.None == this.Border)
			{
				this.button.Bounds.UpdateSize(this.bounds.Width - 16 - 2, 0, 16, this.bounds.Height - 0);
			}
            else if (BorderStyle.Flat == this.Border)
			{
                this.button.Bounds.UpdateSize(this.bounds.Width - 16 - 2, 2, 16, this.bounds.Height - 4);
			}
			else
			{
                this.button.Bounds.UpdateSize(this.bounds.Width - 16 - 2, 2, 16, this.bounds.Height - 4);
			}

			base.Render(graphics, x, y);
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                this.SelectedColor = this.Window.Desktop.Theme.Colors.GetColor(value);
            }
        }

		public Color SelectedColor
        {
            get
            {
                return this.BackColor;
            }
            set
            {
                foreach (Color color in this.Window.Desktop.Theme.SystemColors)
                {
                    if (value == (color))
                    {
                        base.BackColor = color;
                        base.Text = color.Name;

                        RaiseValueChangedEvent();

                        return;
                    }
                }

                base.BackColor = value;
                base.Text = value.Name;

                RaiseValueChangedEvent();
            }
        }

        internal static String TypeName
        {
            get
            {
                return "colorPicker";
            }
        }

        protected Button button = null;
	}
}
