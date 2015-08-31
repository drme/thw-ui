using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Page in the tab control
    /// </summary>
	public class TabPage : Panel
	{
        /// <summary>
        /// Creates tab page object.
        /// </summary>
        /// <param name="window">window it belong to.</param>
        /// <param name="creationFlags">creation flags.</param>
		public TabPage(Window window, CreationFlag creationFlags) : base(window, creationFlags)
        {
            this.Type = TypeName;
            this.Border = BorderStyle.None;
            this.BackColor = Colors.None;
        }

        /// <summary>
        /// Renders tab page. Ignores control and renders only its' child controls.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X coordiante.</param>
        /// <param name="Y">Y coordinate.</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
            if (true == this.Visible)
            {
                RenderControls(graphics, x, y);
            }
        }

        /// <summary>
        /// Control name.
        /// </summary>
        internal new static String TypeName
        {
            get
            {
                return "tabPage";
            }
        }
	}
}
