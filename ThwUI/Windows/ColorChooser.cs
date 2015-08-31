using System;
using ThW.UI.Controls;
using ThW.UI.Utils;

namespace ThW.UI.Windows
{
    /// <summary>
    /// Color chooser window.
    /// </summary>
	internal class ColorChooser : Window
	{
        /// <summary>
        /// Creates color chooser window.
        /// </summary>
        /// <param name="desktop">desktop it belongs to.</param>
		public ColorChooser(Desktop desktop) : base(desktop, CreationFlag.FlagsNone, "")
        {
			this.colorPanel = CreateControl<Panel>();
            this.redSelector = CreateControl<TrackBar>();
            this.greenSelector = CreateControl<TrackBar>();
            this.blueSelector = CreateControl<TrackBar>();
            this.alphaSelector = CreateControl<TrackBar>();
			this.colors = CreateControl<ComboBox>();

			this.Text = "Select color";
			this.Modal = true;
			this.MinSize = new Size2d(450, 330);
			this.Bounds = new Rectangle((desktop.Width - this.MinSize.Width) / 2, (desktop.Height - this.MinSize.Height) / 2, this.MinSize.Width, this.MinSize.Height);
			
			this.colorPanel.Bounds = new Rectangle(121, 210, 317, 50);
			this.colorPanel.Border = BorderStyle.Lowered;

			Button cancelButton = CreateControl<Button>();
			cancelButton.Text = "Cancel";
            cancelButton.Bounds = new Rectangle(343, 277, 95, 24);
			cancelButton.DialogResult = DialogResult.DialogResultCancel;
            cancelButton.Anchor = AnchorStyle.AnchorBottom | AnchorStyle.AnchorRight;

            Button selectButton = CreateControl<Button>();
			selectButton.Text = "Select";
            selectButton.Bounds = new Rectangle(245, 277, 95, 24);
            selectButton.DialogResult = DialogResult.DialogResultOK;
            selectButton.Anchor = AnchorStyle.AnchorBottom | AnchorStyle.AnchorRight;

            Control systemColorsLabel = CreateControl<Label>();
			systemColorsLabel.Text = "System color";
            systemColorsLabel.Bounds = new Rectangle(10, 43 - 35, 120, 24);

            Control redLabel = CreateControl<Label>();
			redLabel.Text = "Red";
            redLabel.Bounds = new Rectangle(10, 43, 120, 35);

            Control greenLabel = CreateControl<Label>();
			greenLabel.Text = "Green";
            greenLabel.Bounds = new Rectangle(10, 43 + 35, 120, 35);

            Control blueLabel = CreateControl<Label>();
			blueLabel.Text = "Blue";
            blueLabel.Bounds = new Rectangle(10, 43 + 35 + 35, 120, 35);

            Control alphaLabel = CreateControl<Label>();
			alphaLabel.Text = "Alpha";
            alphaLabel.Bounds = new Rectangle(10, 43 + 35 + 35 + 35, 120, 35);

            this.redSelector.Bounds = new Rectangle(121, 43, 317, 35);
            this.redSelector.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
            this.redSelector.ValueChanged += this.ColorSelectorValueChanged;

            this.greenSelector.Bounds = new Rectangle(121, 43 + 35, 317, 35);
            this.greenSelector.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
			this.greenSelector.ValueChanged += this.ColorSelectorValueChanged;

            this.blueSelector.Bounds = new Rectangle(121, 43 + 35 + 35, 317, 35);
            this.blueSelector.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
            this.blueSelector.ValueChanged += this.ColorSelectorValueChanged;

            this.alphaSelector.Bounds = new Rectangle(121, 43 + 35 + 35 + 35, 317, 35);
            this.alphaSelector.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
            this.alphaSelector.ValueChanged += this.ColorSelectorValueChanged;

            this.colors.Bounds = new Rectangle(121, 43 - 35, 317, 24);
            this.colors.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
            this.colors.SelectedItemChanged += new UIEventHandler<ComboBox>(ColorItemChanged);

			foreach (Color color in this.Desktop.Theme.Colors.SystemColors)
			{
				this.colors.AddItem(color.Name, color.Name, "", color);
			}
			
			AddControl(systemColorsLabel);
			AddControl(redLabel);
			AddControl(greenLabel);
			AddControl(blueLabel);
			AddControl(alphaLabel);
			AddControl(selectButton);
			AddControl(cancelButton);
			AddControl(this.colorPanel);
			AddControl(this.redSelector);
			AddControl(this.greenSelector);
			AddControl(this.blueSelector);
			AddControl(this.alphaSelector);
			AddControl(this.colors);
        }

        private void ColorItemChanged(ComboBox sender, EventArgs args)
        {
            Color color = (Color)this.colors.SelectedItem.Tag;

            this.colorPanel.BackColor = color;
            this.redSelector.Position = color.R;
            this.greenSelector.Position = color.G;
            this.blueSelector.Position = color.B;
            this.alphaSelector.Position = color.A;
        }

        private void ColorSelectorValueChanged(Control sender, EventArgs args)
        {
            Color color = this.SelectedColor;

            if (sender == this.redSelector)
            {
                color.R = this.redSelector.Position;
            }
            if (sender == this.greenSelector)
            {
                color.G = this.greenSelector.Position;
            }
            if (sender == this.blueSelector)
            {
                color.B = this.blueSelector.Position;
            }
            if (sender == this.alphaSelector)
            {
                color.A = this.alphaSelector.Position;
            }

            this.colorPanel.BackColor = color;
        }

        /// <summary>
        /// Selected color
        /// </summary>
        public Color SelectedColor
        {
            set
            {
                Color selectedColor = value;

                foreach (Color color in this.Desktop.Theme.SystemColors)
                {
                    if (value == color)
                    {
                        selectedColor = color;
                        break;
                    }
                }

                this.colorPanel.BackColor = selectedColor;

                this.redSelector.Position = value.R;
                this.greenSelector.Position = value.G;
                this.blueSelector.Position = value.B;
                this.alphaSelector.Position = value.A;
            }
            get
            {
                return this.colorPanel.BackColor;
            }
        }

        internal static String TypeName
        {
            get
            {
                return "thw.designer.colorChooser";
            }
        }

        private Panel colorPanel = null;
        private TrackBar redSelector = null;
        private TrackBar greenSelector = null;
        private TrackBar blueSelector = null;
        private TrackBar alphaSelector = null;
        private ComboBox colors = null;
	}
}
