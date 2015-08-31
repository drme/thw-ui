using System;
using System.Text;
using ThW.UI.Controls;
using ThW.UI.Utils;

namespace ThW.UI.Windows
{
    /// <summary>
    ///	Simple window for displaying application name, version, etc..
	/// used by UI designer.
    /// </summary>
	internal class AboutWindow : Window
	{
        /// <summary>
        /// Constructs about window object.
        /// </summary>
        /// <param name="desktop">desktop it belongs to</param>
        public AboutWindow(Desktop desktop) : base(desktop, CreationFlag.FlagsNone, "")
        {
			this.Text = "About...";
			this.Modal = true;
            this.CenterDesktop = true;
			this.Sizeable = false;
            this.HasShadow = true;
			this.MinSize = new Size2d(490, 390);
			this.Bounds = new Rectangle((desktop.Width - this.MinSize.Width) / 2, (desktop.Height - this.MinSize.Height) / 2, this.MinSize.Width, this.MinSize.Height);

			Button closeButton = CreateControl<Button>();
			closeButton.Text = "Ok";
			closeButton.Bounds = new Rectangle(410, 342, 75, 23);
			closeButton.DialogResult = DialogResult.DialogResultOK;

			Picture picture = CreateControl<Picture>();
			picture.Bounds = new Rectangle(3, 1, 155, 334);
			picture.BackImage = "ui/design/about";
			picture.Border = BorderStyle.Flat;

			Label titleLabel = CreateControl<Label>();
			titleLabel.Text = "ThW::UI";
			titleLabel.Bounds = new Rectangle(165, 5, 321, 16);

			Label VersionLabel = CreateControl<Label>();
            VersionLabel.Text = "Version " + VersionInfo.Version;
			VersionLabel.Bounds = new Rectangle(165, 25, 321, 16);

			Label buildLabel = CreateControl<Label>();
			buildLabel.Text = "Build " + VersionInfo.Build;
			buildLabel.Bounds = new Rectangle(165, 45, 321, 16);

			Label urlLabel = CreateControl<Label>();
			urlLabel.Text = VersionInfo.Url;
			urlLabel.Bounds = new Rectangle(165, 65, 321, 16);
			urlLabel.TextColor = Colors.Blue;

			ScrollPanel scrollPanel = CreateControl<ScrollPanel>();
			scrollPanel.Bounds = new Rectangle(165, 88, 320, 247);
			scrollPanel.Border = BorderStyle.Lowered;

			TextBox licenseTextBox = CreateControl<TextBox>();
			licenseTextBox.Bounds = new Rectangle(3, 3, 10, 10);
			licenseTextBox.Border = BorderStyle.None;
			licenseTextBox.BackColor = Colors.None;
			licenseTextBox.AutoSize = true;
			licenseTextBox.MultiLine = true;
			scrollPanel.AddControl(licenseTextBox);

			byte[] fileBytes = null;
			uint fileSize = 0;
			Object fileHandle = null;

            this.Engine.OpenFile("ui/design/eula-utf8.txt", out fileBytes, out fileSize, out fileHandle);

			if ((null != fileHandle) && (null != fileBytes) && (fileBytes.Length > 0))
			{
				licenseTextBox.Text = UTF8Encoding.UTF8.GetString(fileBytes, 0, fileBytes.Length);

				this.Engine.CloseFile(ref fileHandle);
			}

			AddControl(titleLabel);
			AddControl(VersionLabel);
			AddControl(buildLabel);
			AddControl(urlLabel);
			AddControl(scrollPanel);
			AddControl(closeButton);
			AddControl(picture);
        }

        /// <summary>
        /// Design window type name
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "thw.aboutWindow";
            }
        }
	}
}
