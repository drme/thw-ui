using System;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Pushable button.
    /// </summary>
	public class Button : Control
	{
        /// <summary>
        /// Constructs button
        /// </summary>
        /// <param name="window">window it belongs to</param>
        /// <param name="creationFlags">creation flags</param>
		public Button(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.Bounds.X = 100;
			this.Bounds.Y = 100;
			this.Bounds.Width = 100;
			this.Bounds.Height = 23;
			this.Border = BorderStyle.BorderRaisedDouble;
            this.TabStop = true;
        }

        ~Button()
        {
            this.Engine.RemoveSound(this.clickSound);
            this.Engine.RemoveSound(this.focusSound);
        }

        /// <summary>
        /// Renders button
        /// </summary>
        protected override void Render(Graphics graphics, int x, int y)
        {
            if (false == this.Visible)
            {
                return;
            }

            RenderBackground(graphics, x, y);
            RenderBorder(graphics, x, y);
            RenderIcon(graphics, x, y);

            if (((true == this.isMouseOver) && (this.backColor.A > 0.0f) && (true == this.RenderSelectionOverlay)) || (true == this.HasFocus))
            {
                RenderSelection(graphics, x, y);
            }

            RenderText(graphics, x, y);

            RenderControls(graphics, x, y);
        }

        protected override void KeyPress(char c, Key modifier)
        {
            base.KeyPress(c, modifier);

            if (modifier == Key.Enter)
            {
                OnClick(0, 0);
            }
        }

        protected override void OnMouseMove(int x, int y)
        {
			base.OnMouseMove(x, y);

            if (null != this.MouseMoved)
            {
                this.MouseMoved(this, EventArgs.Empty);
            }
        }

        protected override void OnMousePressed(int x, int y)
        {
			base.OnMousePressed(x, y);

            if (null != this.MousePress)
            {
                if (true == IsInside(x, y))
                {
                    this.MousePress(this, EventArgs.Empty);
                }
            }
        }

        protected override void OnMouseReleased(int x, int y)
        {
			base.OnMouseReleased(x, y);

            if (null != this.MouseRelease)
            {
                this.MouseRelease(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles mouse click event on button.
        /// </summary>
        /// <param name="X">mouse click X position.</param>
        /// <param name="Y">mouse click Y position.</param>
        protected override void OnClick(int x, int y)
        {
            if (null != this.clickSound)
            {
                this.clickSound.Play();
            }

			base.OnClick(x, y);

            if (null != this.Clicked)
            {
                this.Clicked(this, EventArgs.Empty);
            }

			if (null != this.Window)
			{
                if ((DialogResult.DialogResultOK == this.DialogResult) || (DialogResult.DialogResultCancel == this.DialogResult) || (DialogResult.DialogResultYes == this.DialogResult) || (DialogResult.DialogResultNo == this.DialogResult))
				{
					this.Window.DialogResult = this.DialogResult;
					this.Window.Close();
				}
			}
        }

        /// <summary>
        /// The result value assigned to a window after this button is pressed
        /// </summary>
        public DialogResult DialogResult
        {
            get
            {
                return this.dialogResult;
            }
            set
            {
                this.dialogResult = value;
            }
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
        protected override void AddProperties()
        {
            AddProperty(new PropertyList<DialogResult>(this.DialogResult, "dialogResult", "Window", "dialogResult", (x) => { this.DialogResult = x; }, () => { return this.DialogResult; }));
            AddProperty(new PropertyBoolean(this.RenderSelectionOverlay, "renderSelection", "Button", "renderSelection", (x) => { this.RenderSelectionOverlay = x; }, () => { return this.RenderSelectionOverlay; }));
            AddProperty(new PropertyString(this.ClickSound, "clickSound", "button", "click sound", (x) => { this.ClickSound = x; }, () => { return this.ClickSound; }));
            AddProperty(new PropertyString(this.FocusSound, "focusSound", "button", "focus sound", (x) => { this.FocusSound = x; }, () => { return this.FocusSound; }));

            base.AddProperties();
        }

        public override void OnFocus()
        {
            if (null != this.focusSound)
            {
                this.focusSound.Play();
            }

            base.OnFocus();
        }

        /// <summary>
        /// Should the selection overlay be rendered then mouse is over control
        /// </summary>
        public bool RenderSelectionOverlay
        {
            get
            {
                return this.renderSelection;
            }
            set
            {
                this.renderSelection = value;
            }
        }

        /// <summary>
        /// Sound played then the control is clicked.
        /// </summary>
        public String ClickSound
        {
            get
            {
                return null != this.clickSound ? this.clickSound.Name : null;
            }
            set
            {
                this.Engine.RemoveSound(this.clickSound);
                this.clickSound = this.Engine.GetSound(value);
            }
        }

        /// <summary>
        /// Sound played then control gains focus.
        /// </summary>
        public String FocusSound
        {
            get
            {
                return null != this.focusSound ? this.focusSound.Name : null;
            }
            set
            {
                this.Engine.RemoveSound(this.focusSound);
                this.focusSound = this.Engine.GetSound(value);
            }
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "button";
            }
        }

        /// <summary>
        /// Button clicked event.
        /// </summary>
        public event UIEventHandler<Button> Clicked = null;
        /// <summary>
        /// Mouse pressed on this control event.
        /// </summary>
        public event UIEventHandler<Button> MousePress = null;
        /// <summary>
        /// Mouse released on this control event.
        /// </summary>
        public event UIEventHandler<Button> MouseRelease = null;
        /// <summary>
        /// Mouse moved over this control event.
        /// </summary>
        public event UIEventHandler<Button> MouseMoved = null;

		private DialogResult dialogResult = DialogResult.DialogResultNone;
        private bool renderSelection = true;
        private SoundObject clickSound = null;
        private SoundObject focusSound = null;
	}
}
