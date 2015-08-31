using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Lists box item.
    /// </summary>
	public class ListItem : Control
	{
        /// <summary>
        /// Creates list box item.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
		public ListItem(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.BackColor = Colors.None;
            this.ListStyle = this.ListStyle;
        }

        /// <summary>
        /// Renders list box item.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
            if (true == this.Visible)
            {
                RenderBackground(graphics, x, y);
                RenderBorder(graphics, x, y);

                if (true == this.isMouseOver)
                {
                    RenderSelection(graphics, x, y);
                }

                RenderIcon(graphics, x, y);
                RenderText(graphics, x, y);
            }
        }

        /// <summary>
        /// List item presentation style.
        /// </summary>
        public virtual ListStyle ListStyle
        {
            set
            {
                this.listItemStyle = value;

                switch (this.listItemStyle)
                {
                    case ListStyle.Details:
                        this.TextAlignment = ContentAlignment.MiddleLeft;
                        this.TextOffset.X = 20;
                        this.TextOffset.Y = 0;
                        this.IconAlignment = ContentAlignment.MiddleLeft;
                        this.IconImageOffset.X = 0;
                        this.IconImageOffset.Y = 1;
                        this.IconSize = IconSize.IconSmall;
                        break;
                    case ListStyle.LargeIcons:
                        this.IconAlignment = ContentAlignment.TopCenter;
                        this.IconImageOffset.X = 0;
                        this.IconImageOffset.Y = 4;
                        this.TextAlignment = ContentAlignment.BottomCenter;
                        this.TextOffset.X = -0;
                        this.TextOffset.Y = -4;
                        break;
                    default:
                        break;
                }
            }
            get
            {
                return this.listItemStyle;
            }
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        public static String TypeName
        {
            get
            {
                return "listItem";
            }
        }

        /// <summary>
        /// Handles control click event.
        /// </summary>
        /// <param name="X">mouse click X position.</param>
        /// <param name="Y">mouse click Y position.</param>
        protected override void OnClick(int x, int y)
        {
            base.OnClick(x, y);

            if (null != this.Clicked)
            {
                this.Clicked(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Event for handling list item click events.
        /// </summary>
        public event UIEventHandler<ListItem> Clicked = null;

		private ListStyle listItemStyle = ListStyle.LargeIcons;
	}
}
