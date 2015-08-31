using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
	internal class FilePicker : Control
	{
		public FilePicker(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
            this.selectButton = CreateControl<Button>(CreationFlag.InternalControl);

			this.Border = BorderStyle.Lowered;
			this.NeedTranslation = false;
			this.selectButton.NeedTranslation = false;
			this.selectButton.Text = "..";
            this.selectButton.Clicked += this.ButtonClicked;
			this.BackColor = window.Desktop.Theme.Colors.Window;
			this.TextAlignment = ContentAlignment.MiddleLeft;

			AddControl(this.selectButton);
        }

        private void ButtonClicked(Button sender, EventArgs e)
        {
            if (null != this.Window)
            {
                Desktop desktop = this.Window.Desktop;

                if (null != desktop)
                {
                    Window window = desktop.NewRegisteredWindow(FileChooser.TypeName);

                    if (null != window)
                    {
                        window.Closing += this.FileChooserClosing;
                    }
                }
            }
        }

        private void FileChooserClosing(Window sender, EventArgs args)
        {
            FileChooser fileChooser = (FileChooser)sender;

            if ((null != fileChooser) && (DialogResult.DialogResultOK == fileChooser.DialogResult))
            {
                this.SelectedFile = fileChooser.SelectedFilePath;
            }
        }

        protected override void Render(Graphics graphics, int x, int y)
        {
			if (BorderStyle.None == this.Border)
			{
                this.selectButton.Bounds.UpdateSize(this.Bounds.Width - 16 - 2, 0, 16, this.Bounds.Height - 0);
			}
			else if (BorderStyle.Flat == this.Border)
			{
                this.selectButton.Bounds.UpdateSize(this.Bounds.Width - 16 - 2, 2, 16, this.Bounds.Height - 4);
			}
			else
			{
                this.selectButton.Bounds.UpdateSize(this.Bounds.Width - 16 - 2, 2, 16, this.Bounds.Height - 4);
			}

			base.Render(graphics, x, y);
        }

        public String SelectedFile
        {
            get
            {
                return this.textReference;
            }
            set
            {
                this.Text = value;
                RaiseValueChangedEvent();
            }
        }

        internal static String TypeName
        {
            get
            {
                return "filePicker";
            }
        }

		protected Button selectButton = null;
	}
}
