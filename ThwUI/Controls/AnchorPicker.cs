using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
	internal class AnchorPicker : Control
	{
		public AnchorPicker(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.selectionButton = CreateControl<Button>(CreationFlag.InternalControl);

			this.Border = BorderStyle.Lowered;
			this.NeedTranslation = false;
			this.selectionButton.NeedTranslation = false;
			this.selectionButton.Text = "..";
            this.selectionButton.Clicked += this.SelectButtonClicked;
			this.BackColor = window.Desktop.Theme.Colors.Window;
			this.TextAlignment = ContentAlignment.MiddleLeft;

			AddControl(this.selectionButton);
        }

        private void SelectButtonClicked(Button sender, EventArgs e)
        {
            if (null != this.Window)
            {
                Desktop desktop = this.Window.Desktop;

                if (null != desktop)
                {
                    Window anchorChooserWindow = desktop.NewRegisteredWindow(AnchorChooser.TypeName);

                    if (null != anchorChooserWindow)
                    {
                        anchorChooserWindow.Closing += this.AnchorChooserWindowClosing;
                        ((AnchorChooser)anchorChooserWindow).SelectedAnchor = this.selectedAnchor;
                    }
                }
            }
        }

        private void AnchorChooserWindowClosing(Window sender, EventArgs args)
        {
            AnchorChooser window = (AnchorChooser)sender;

            if ((null != window) && (DialogResult.DialogResultOK == window.DialogResult))
            {
                this.SelectedAnchor = window.SelectedAnchor;
            }
        }

        protected override void Render(Graphics graphics, int x, int y)
        {
			if (BorderStyle.None == this.Border)
			{
				this.selectionButton.Bounds.UpdateSize(this.Bounds.Width - 16 - 2, 0, 16, this.Bounds.Height - 0);
			}
			else if (BorderStyle.Flat == this.Border)
			{
                this.selectionButton.Bounds.UpdateSize(this.Bounds.Width - 16 - 2, 2, 16, this.Bounds.Height - 4);
			}
			else
			{
                this.selectionButton.Bounds.UpdateSize(this.Bounds.Width - 16 - 2, 2, 16, this.Bounds.Height - 4);
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
                this.SelectedAnchor = ((AnchorStyle)int.Parse(value));
            }
        }

        public AnchorStyle SelectedAnchor
        {
            get
            {
                return this.selectedAnchor;
            }
            set
            {
                this.selectedAnchor = value;
                base.Text = ((int)value).ToString();
                RaiseValueChangedEvent();
            }
        }

        internal static String TypeName
        {
            get
            {
                return "anchorPicker";
            }
        }

        protected Button selectionButton = null;
        protected AnchorStyle selectedAnchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorTop;
	}
}
