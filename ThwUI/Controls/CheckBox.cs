using System;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Control for setting on/off value
    /// </summary>
	public class CheckBox : Control
	{
        /// <summary>
        /// Creates checkbox control.
        /// </summary>
        /// <param name="window">window it belongs to</param>
        /// <param name="creationFlags">creation flags</param>
		public CheckBox(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.Border = BorderStyle.BorderLoweredDouble;
            this.TextAlignment = ContentAlignment.MiddleLeft;
			this.BackColor = Colors.None;
        }

        /// <summary>
        /// Releases tick image
        /// </summary>
		~CheckBox()
        {
            this.Engine.DeleteImage(ref this.tick);
        }

        /// <summary>
        /// Renders contorl ant tick
        /// </summary>
        /// <param name="graphics">graphics context to render to</param>
        /// <param name="X">control offset X</param>
        /// <param name="Y">control offset Y</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
			base.Render(graphics, x, y);

			RenderCheck(graphics, x, y);
        }

        /// <summary>
        /// Handles mouse click on control event.
        /// </summary>
        /// <param name="X">mouse click X position.</param>
        /// <param name="Y">mouse click Y position.</param>
		protected override void OnClick(int x, int y)
        {
			base.OnClick(x, y);
			this.Checked = !this.Checked;
        }

        /// <summary>
        /// Is checkbox ticked.
        /// </summary>
        public bool Checked
        {
            set
            {
                bool changed = (value != this.marked);

                this.marked = value;

                if (true == changed)
                {
                    RaiseValueChangedEvent();
                }
            }
            get
            {
                return this.marked;
            }
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
        protected override void AddProperties()
        {
			base.AddProperties();

            const String groupName = "CheckBox";

            AddProperty(new PropertyBoolean(this.Checked, "checked", groupName, "checked", (x) => { this.Checked = x; }, () => { return this.Checked; }));
        }

        /// <summary>
        /// Renders crontol border - check box
        /// </summary>
        /// <param name="graphics">graphics to render to</param>
        /// <param name="X">rendering offset X</param>
        /// <param name="Y">rendering offset Y</param>
        protected override void RenderBorder(Graphics graphics, int x, int y)
        {
            if (null == this.settings)
            {
                this.settings = this.Window.Desktop.Theme.GetControlSettings(TypeName);
            }

			int off = 2;
            int dx = 0;

            int size = this.settings.ControlSize;

            if (size <= 0)
            {
                size = this.Bounds.Height;
                dx = 0;
            }
            else
            {
                dx = (this.Bounds.Height - size) / 2;
            }

            //if (BorderStyle.BorderNone == this.Border)
            {
                off = 0;
            }

            off += dx;

			graphics.SetColor(this.settings.ColorBack);
			
            graphics.DrawRectangle(off + x + this.Bounds.X, off + y + this.Bounds.Y, size, size);
			
            graphics.SetColor(Colors.White);
			
            RenderBorderXYWH(graphics, off + x + this.Bounds.X, off + y + this.Bounds.Y, size, size, this.Border);

			this.TextOffset.X = off + size + 5;
        }

        /// <summary>
        /// Renders checkbox tick
        /// </summary>
        /// <param name="graphics">graphics t orender to</param>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        protected void RenderCheck(Graphics graphics, int x, int y)
        {
			if (true == this.Checked)
			{
				if (null == this.tick)
				{
                    this.tick = this.Engine.CreateImage(this.Window.Desktop.Theme.ThemeFolder + "/images/checkbox_tick");
				}

				if (null != this.tick)
				{
					int off = (this.Bounds.Height - this.tick.Height) / 2;

					graphics.DrawImage(off + x + this.Bounds.X, off + y + this.Bounds.Y, this.tick.Width, this.tick.Height, this.tick);
				}
			}
        }

        /// <summary>
        /// Control name as serialized in a xml.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "checkBox";
            }
        }

        private ControlSettings settings = null;
        private bool marked = false;
        private IImage tick = null;
	}
}
