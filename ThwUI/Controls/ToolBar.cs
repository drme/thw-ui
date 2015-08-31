using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Toolbar control. Contains a row of buttons. Each button has an icon.
    /// </summary>
	public class ToolBar : Control
	{
        /// <summary>
        /// Creates toolbar control.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
		public ToolBar(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
        }

        /// <summary>
        /// Renders toolbar.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
		protected override void Render(Graphics graphics, int x, int y)
        {
			base.Render(graphics, x, y);

            graphics.SetColor(this.Window.Desktop.Theme.Colors.ControlDark, this.Opacity);

			for (int i = 2; i < this.bounds.Height - 3; i += 2)
			{
				graphics.DrawRectangle(x + this.bounds.X + 4, y + this.bounds.Y + i, 3, 1);
			}
        }

        /// <summary>
        /// Updates control size.
        /// </summary>
		protected override void UpdateSizeControls()
        {
			int x = 8;

            foreach (Control control in this.Controls)
			{
				Rectangle r = control.Bounds;

				if (r.Width > this.bounds.Height)
				{
					control.Bounds.UpdateSize(x, 1, r.Width - 2, this.bounds.Height - 2);
				}
				else
				{
                    control.Bounds.UpdateSize(x, 1, this.bounds.Height - 2, this.bounds.Height - 2);
				}

				x += r.Width + 1;
			}

			base.UpdateSizeControls();
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "toolBar";
            }
        }
	}
}
