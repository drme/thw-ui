using System;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Radio button. Allows to select only one button in the same group.
    /// Each radio boutton has group attribute. If one radio button is checked all others having the
    /// same group are unchecked.
    /// </summary>
	public class RadioButton : Control
	{
        /// <summary>
        /// Creates radio button object.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
		public RadioButton(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.Selected = false;
			this.TextAlignment = ContentAlignment.MiddleLeft;
			this.IconAlignment = ContentAlignment.MiddleLeft;
            this.TextOffset.X = 16;
            this.TextOffset.Y = 0;
			this.BackColor = Colors.None;
        }

        /// <summary>
        /// Releases radio button images.
        /// </summary>
        ~RadioButton()
        {
            this.Engine.DeleteImage(ref this.checkedImage);
            this.Engine.DeleteImage(ref this.unCheckedImage);
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
		protected override void AddProperties()
        {
			base.AddProperties();

            AddProperty(new PropertyBoolean(this.Selected, "selected", "radioButton", "selected", (x) => { this.Selected = x; }, () => { return this.Selected; }));
            AddProperty(new PropertyString(this.Group, "group", "radioButton", "group", (x) => { this.Group = x; }, () => { return this.Group; }));
        }

        /// <summary>
        /// Handles click event.
        /// </summary>
        /// <param name="X">mouse press X position.</param>
        /// <param name="Y">mouse press Y position.</param>
		protected override void OnClick(int x, int y)
        {
			base.OnClick(x, y);

			this.Selected = !this.Selected;
        }

        /// <summary>
        /// The group this radio button control belongs to.
        /// Only one control in the same group can have selected value.
        /// </summary>
		public String Group
        {
            set
            {
                this.group = value;
            }
            get
            {
                return this.group;
            }
        }

        /// <summary>
        /// Radio button can not contain any controls.
        /// </summary>
        /// <returns>always false</returns>
        public override bool CanContainControl(String typeName)
        {
            return false;
        }

        /// <summary>
        /// Is this radio button selected.
        /// Setting to true, unselects all other radio buttons in the same group.
        /// </summary>
        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                this.selected = value;

                String folder = this.Window.Desktop.Theme.ThemeFolder + "/images/radio_button_";

                if (null == this.checkedImage)
                {
                    this.checkedImage = this.Engine.CreateImage(folder + "selected");
                }

                if (null == this.unCheckedImage)
                {
                    this.unCheckedImage = this.Engine.CreateImage(folder + "unselected");
                }

                if (true == this.selected)
                {
                    this.Icon = (folder + "selected");

                    foreach (Control control in this.Window.WindowControls)
                    {
                        if (control is RadioButton)
                        {
                            RadioButton radioButton = (RadioButton)control;

                            if ((radioButton != this) && (radioButton.Group == this.Group))
                            {
                                radioButton.Selected = false;
                            }
                        }
                    }
                }
                else
                {
                    this.Icon = (folder + "unselected");
                }
            }
        }

        /// <summary>
        /// Controls type name.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "radioButton";
            }
        }

        private IImage checkedImage = null;
        private IImage unCheckedImage = null;
        private bool selected = false;
		private String group = "";
	}
}
