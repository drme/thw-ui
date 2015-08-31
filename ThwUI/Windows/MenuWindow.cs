using ThW.UI.Controls;
using ThW.UI.Utils;

namespace ThW.UI.Windows
{
    /// <summary>
    /// Window for displaying menu.
    /// </summary>
    internal class MenuWindow : Window
    {
        /// <summary>
        /// Constructs window object.
        /// </summary>
        /// <param name="desktop">desktop it belongs to.</param>
        /// <param name="menu">menu to display in this window.</param>
		internal MenuWindow(Desktop desktop, Menu menu) : base(desktop, CreationFlag.FlagsNone, "")
		{
            this.menu = menu;
			this.BackColor = Colors.None;
		}

        /// <summary>
        /// Renders menu window.
        /// </summary>
        /// <param name="graphics">graphics to draw to</param>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        protected override void Render(Graphics graphics, int x, int y)
		{
			base.Render(graphics, x, y);

			this.menu.RenderInSeparateWindow(graphics, 2 + this.Bounds.X - this.menu.Bounds.X, 0 + this.Bounds.Y - this.menu.Bounds.Y);
		}

        /// <summary>
        /// Handles mouse move event.
        /// </summary>
        /// <param name="X">mouse X position</param>
        /// <param name="Y">mouse Y position</param>
		protected override void OnMouseMove(int x, int y)
		{
			this.menu.MouseMoveInternal(x - this.Bounds.X, y - this.Bounds.Y);
		}

        /// <summary>
        /// Handles mouse release event.
        /// </summary>
        /// <param name="X">mouse X position</param>
        /// <param name="Y">mouse Y position</param>
        protected override void OnMouseReleased(int x, int y)
		{
			this.menu.MouseReleasedInternal(x - this.Bounds.X, y - this.Bounds.Y);

			Close();
		}

        /// <summary>
        /// Are coordinates inside this window.
        /// </summary>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        /// <returns></returns>
		internal override bool IsInside(int x, int y)
		{
			bool rez = this.menu.IsInsideExpandedMenu(x - this.Bounds.X, y - this.Bounds.Y);

			return rez;
		}
    }
}
