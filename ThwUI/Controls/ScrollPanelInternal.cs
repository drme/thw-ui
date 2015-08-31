using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Internal scroll panels' panel, used by scrool panel.
    /// </summary>
	internal class ScrollPanelInternal : Panel
	{
        /// <summary>
        /// Creates iternal scoll panel object.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
		public ScrollPanelInternal(Window window, CreationFlag creationFlags) : base(window, creationFlags)
        {
			this.Type = TypeName;
        }

        /// <summary>
        /// Determines if coordinated are inside this control.
        /// </summary>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        /// <returns></returns>
		internal override bool IsInside(int x, int y)
        {
			ScrollPanel scrollPanel = (ScrollPanel)this.Parent;

			Rectangle r = scrollPanel.Bounds;

			int w = r.Width;
			int h = r.Height;

			if ( (null != scrollPanel.verticalScrollBar) && (scrollPanel.verticalScrollBar.Visible) )
			{
				w -= scrollPanel.verticalScrollBar.ButtonSize;
			}

			if ( (null != scrollPanel.horizontalScrollBar) && (scrollPanel.horizontalScrollBar.Visible) )
			{
				h -= scrollPanel.horizontalScrollBar.ButtonSize;
			}

			if ( (x >= 0) && (y >= 0) && (x <= w) && (y <= h) )
			{
				return base.IsInside(x, y);
			}
			else
			{
				return false;
			}
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal new static String TypeName
        {
            get
            {
                return "scrollPanelInternal";
            }
        }
	}
}
