using System;
using ThW.UI.Controls;
using ThW.UI.Utils;

namespace ThW.UI.Windows
{
    /// <summary>
    /// Window for choosing control anchoring.
    /// </summary>
	internal class AnchorChooser : Window
	{
        /// <summary>
        /// Creates anchor window object.
        /// </summary>
        /// <param name="desktop">desktop it belongs to</param>
		public AnchorChooser(Desktop desktop) : base(desktop, CreationFlag.FlagsNone, "")
        {
            this.top = CreateControl<CheckBox>();
            this.left = CreateControl<CheckBox>();
            this.right = CreateControl<CheckBox>();
            this.bottom = CreateControl<CheckBox>();

			this.Text = "Select anchor";
			this.Modal = true;
			this.Sizeable = false;
			this.MinSize = new Size2d(410, 310);
            this.Bounds = new Rectangle((desktop.Width - this.MinSize.Width) / 2, (desktop.Height - this.MinSize.Height) / 2, this.MinSize.Width, this.MinSize.Height);

			Button cancelButton = CreateControl<Button>();
			cancelButton.Text = "Cancel";
			cancelButton.Bounds = new Rectangle(306, 257, 95, 24);
			cancelButton.DialogResult = DialogResult.DialogResultCancel;

			Button selectButton = CreateControl<Button>();
			selectButton.Text = "Select";
			selectButton.Bounds = new Rectangle(205, 257, 95, 24);
			selectButton.DialogResult = DialogResult.DialogResultOK;

			Label titleLabel = CreateControl<Label>();
			titleLabel.Text = "Stick to";
			titleLabel.Bounds = new Rectangle(10, 10, 387, 24);

			Panel panel = CreateControl<Panel>();
            panel.Bounds = new Rectangle(109, 77, 188, 122);
			panel.Border = BorderStyle.Lowered;

            this.bottom.Bounds = new Rectangle(160, 210, 95, 18);
			this.bottom.Text = "Bottom";
            this.bottom.ValueChanged += this.UpdateStateFromUI;

            this.top.Bounds = new Rectangle(160, 50, 95, 18);
			this.top.Text = "Top";
            this.top.ValueChanged += this.UpdateStateFromUI;

            this.left.Bounds = new Rectangle(10, 123, 95, 18);
			this.left.Text = "Left";
            this.left.ValueChanged += this.UpdateStateFromUI;

            this.right.Bounds = new Rectangle(304, 123, 95, 18);
			this.right.Text = "Right";
            this.right.ValueChanged += this.UpdateStateFromUI;

			AddControl(titleLabel);
			AddControl(panel);
			AddControl(selectButton);
			AddControl(cancelButton);
			AddControl(this.right);
			AddControl(this.left);
			AddControl(this.top);
			AddControl(this.bottom);
        }

        /// <summary>
        /// Selected anchor style.
        /// </summary>
        public AnchorStyle SelectedAnchor
        {
            get
            {
                return this.selectedAnchor;
            }
            set
            {
                this.selectedAnchor = value;
                UpdateStateToUI();
            }
        }

        private void UpdateStateFromUI(Control sender, EventArgs args)
        {
            this.selectedAnchor = 0;

            if (true == this.top.Checked)
            {
                this.selectedAnchor |= AnchorStyle.AnchorTop;
            }

            if (true == this.bottom.Checked)
            {
                this.selectedAnchor |= AnchorStyle.AnchorBottom;
            }

            if (true == this.left.Checked)
            {
                this.selectedAnchor |= AnchorStyle.AnchorLeft;
            }

            if (true == this.right.Checked)
            {
                this.selectedAnchor |= AnchorStyle.AnchorRight;
            }
        }

        private void UpdateStateToUI()
        {
            if ((this.selectedAnchor & AnchorStyle.AnchorTop) > 0)
            {
                this.top.Checked = true;
            }

            if ((this.selectedAnchor & AnchorStyle.AnchorBottom) > 0)
            {
                this.bottom.Checked = true;
            }

            if ((this.selectedAnchor & AnchorStyle.AnchorLeft) > 0)
            {
                this.left.Checked = true;
            }

            if ((this.selectedAnchor & AnchorStyle.AnchorRight) > 0)
            {
                this.right.Checked = true;
            }
        }

        internal static String TypeName
        {
            get
            {
                return "thw.designer.anchorChooser";
            }
        }

		private AnchorStyle selectedAnchor = AnchorStyle.None;
        private CheckBox top = null;
        private CheckBox left = null;
        private CheckBox right = null;
        private CheckBox bottom = null;
	}
}
