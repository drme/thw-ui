using System;
using ThW.UI.Controls;
using ThW.UI.Utils;

namespace ThW.UI.Windows
{
    /// <summary>
    /// Message box window.
    /// </summary>
	public class MessageBox : Window
	{
        /// <summary>
        /// Creates message box window.
        /// </summary>
        /// <param name="desktop">desktop it belongs to.</param>
        public MessageBox(Desktop desktop) : base(desktop, CreationFlag.FlagsNone, "")
        {
            this.Type = TypeName;

			this.textLabel1 = CreateControl<Label>();
            this.textLabel2 = CreateControl<Label>();
			this.yesButton = CreateControl<Button>();
            this.noButton = CreateControl<Button>();
            this.cancelButton = CreateControl<Button>();

			this.MinSize = new Size2d(590, 130);
			this.Bounds = new Rectangle((desktop.Width - this.MinSize.Width) / 2, (desktop.Height - this.MinSize.Height) / 2, this.MinSize.Width, this.MinSize.Height);
			this.Modal = true;
			this.Sizeable = false;
			this.Skinned = true;

            this.textLabel1.Bounds = new Rectangle(10, 10, 570, 24);
            this.textLabel2.Bounds = new Rectangle(40, 10, 570, 24);
			this.textLabel1.Anchor = AnchorStyle.AnchorTop | AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight;
			this.textLabel2.Anchor = AnchorStyle.AnchorTop | AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight;

			this.yesButton.DialogResult = DialogResult.DialogResultYes;
			this.noButton.DialogResult = DialogResult.DialogResultNo;
			this.cancelButton.DialogResult = DialogResult.DialogResultCancel;
			this.yesButton.Anchor = AnchorStyle.AnchorBottom;
			this.noButton.Anchor = AnchorStyle.AnchorBottom;
			this.cancelButton.Anchor = AnchorStyle.AnchorBottom;

			AddControl(this.yesButton);
			AddControl(this.noButton);
			AddControl(this.cancelButton);
			AddControl(this.textLabel1);
			AddControl(this.textLabel2);

			PlaceButtons();
        }

        /// <summary>
        /// Message line 1
        /// </summary>
        public String MessageLine1
        {
            set
            {
                this.textLabel1.Text = value;
            }
        }

        /// <summary>
        /// Message line 1
        /// </summary>
        public String MessageLine2
        {
            set
            {
                this.textLabel2.Text = value;
            }
        }

        /// <summary>
        /// Yes button text. If empty this button is not visible.
        /// </summary>
        public String YesButtonText
        {
            set
            {
                this.yesButton.Text = value;
                PlaceButtons();
            }
        }

        /// <summary>
        /// No button text. If empty this button is not visible.
        /// </summary>
        public String NoButtonText
        {
            set
            {
                this.noButton.Text = value;
                PlaceButtons();
            }
        }

        /// <summary>
        /// Cancel button text. If empty this button is not visible.
        /// </summary>
        public String CancelButtonText
        {
            set
            {
                this.cancelButton.Text = value;
                PlaceButtons();
            }
        }

        /// <summary>
        /// Calculates mesage box buttons coordinates.
        /// </summary>
        protected virtual void PlaceButtons()
        {
            this.yesButton.X = -10000;
            this.noButton.X = -10000;
            this.cancelButton.X = -10000;

            int w = 75;
            int h = 23;

            int cnt = 0;

            if (this.yesButton.Text.Length > 0)
            {
                cnt++;
            }

            if (this.noButton.Text.Length > 0)
            {
                cnt++;
            }

            if (this.cancelButton.Text.Length > 0)
            {
                cnt++;
            }

            int freeSpace = this.Bounds.Width - (cnt * (w + 10));

            int start = freeSpace / 2;

            if (this.yesButton.Text.Length > 0)
            {
                this.yesButton.Bounds = new Rectangle(start + 5, this.Bounds.Height - h - 10 - this.TopOffset, w, h);
                start += 10 + w;
            }

            if (this.noButton.Text.Length > 0)
            {
                this.noButton.Bounds = new Rectangle(start + 5, this.Bounds.Height - h - 10 - this.TopOffset, w, h);
                start += 10 + w;
            }

            if (this.cancelButton.Text.Length > 0)
            {
                this.cancelButton.Bounds = new Rectangle(start + 5, this.Bounds.Height - h - 10 - this.TopOffset, w, h);
                start += 10 + w;
            }
        }

        /// <summary>
        /// Window type name
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "messageBox";
            }
        }

        protected Label textLabel1 = null;
        protected Label textLabel2 = null;
        protected Button yesButton = null;
        protected Button noButton = null;
        protected Button cancelButton = null;
	}
}
