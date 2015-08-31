using System;
using System.Collections.Generic;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
	public class ScrollPanel : Control
	{
		public ScrollPanel(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.verticalScrollBar = CreateControl<ScrollBar>(CreationFlag.InternalControl);
            this.horizontalScrollBar = CreateControl<ScrollBar>(CreationFlag.InternalControl);
            this.internalPanel = CreateControl<ScrollPanelInternal>(CreationFlag.InternalControl);

			this.horizontalScrollBar.Vertical = false;
			this.internalPanel.BackColor = Colors.None;
			
            base.AddControl(this.verticalScrollBar);
			base.AddControl(this.horizontalScrollBar);
			base.AddControl(this.internalPanel);
        }

        protected override void Render(Graphics graphics, int x, int y)
        {
			CalculateSizes();

			if ( (true == this.horizontalScrollBar.Visible) && (true == this.verticalScrollBar.Visible) )
			{
				int w = this.horizontalScrollBar.ButtonSize;

                graphics.SetColor(this.Window.Desktop.Theme.Colors.Control, this.Opacity);
				
				graphics.DrawRectangle(x + this.bounds.X + this.bounds.Width - w - this.borderSize, y + this.bounds.Y + this.bounds.Height - w - this.borderSize, w, w);
			}

			base.Render(graphics, x, y);
        }

		public override void AddControl(Control pControl)
        {
			this.internalPanel.AddControl(pControl);
        }

        /// <summary>
        /// Child controls.
        /// </summary>
		public override IList<Control> Controls
        {
            get
            {
                return this.internalPanel.Controls;
            }
        }

        protected override bool OnMouseWheelUp(int x, int y, int dx, int dy)
        {
			if (true == this.verticalScrollBar.Visible)
			{
				this.verticalScrollBar.MouseWheelUpInternal(x, y, dx, dy);
			}

			return true;
        }

        protected override bool OnMouseWheelDown(int x, int y, int dx, int dy)
        {
			if (true == this.verticalScrollBar.Visible)
			{
				this.verticalScrollBar.MouseWheelDownInternal(x, y, dx, dy);
			}

			return true;
        }

		public void ScrollToVEnd()
        {
			CalculateSizes();
			this.verticalScrollBar.Position = (float)this.verticalScrollBar.MaxSize;
        }

		public void ScrollToHEnd()
        {
			CalculateSizes();
			this.horizontalScrollBar.Position = (float)this.horizontalScrollBar.MaxSize;
        }

        protected override void RenderControls(Graphics pRender, int x/* = 0*/, int y/* = 0*/)
        {
            this.verticalScrollBar.Opacity = this.Opacity;
            this.horizontalScrollBar.Opacity = this.Opacity;

            this.verticalScrollBar.RenderInternal(pRender, x + this.bounds.X, y + this.bounds.Y + this.topOffset);
            this.horizontalScrollBar.RenderInternal(pRender, x + this.bounds.X, y + this.bounds.Y + this.topOffset);

            int mX = this.verticalScrollBar.Visible ? this.verticalScrollBar.ButtonSize : 0;
            int mY = this.horizontalScrollBar.Visible ? this.horizontalScrollBar.ButtonSize : 0;
            int off = this.borderSize;

            pRender.SetRegion(off + x + this.bounds.X, off + y + this.bounds.Y, this.bounds.Width - 2 * off - mX, this.bounds.Height - 2 * off - mY);
            this.internalPanel.RenderInternal(pRender, x + this.bounds.X, y + this.bounds.Y + this.topOffset);
            pRender.ClearRegion();
        }

        /// <summary>
        /// Saves control to file stream.
        /// </summary>
        /// <param name="writer">stream to write to.</param>
        protected override void WriteControls(IXmlWriter writer)
        {
			base.WriteControls(writer, this.internalPanel.Controls);
        }

        protected void CalculateSizes()
        {
			int mx = 0;
			int my = 0;

			foreach (Control it in this.internalPanel.Controls)
			{
				Rectangle r1 = it.Bounds;

				int xx = r1.X + r1.Width;
				int yy = r1.Y + r1.Height;

				if (xx > mx)
				{
					mx = xx;
				}

				if (yy > my)
				{
					my = yy;
				}
			}

			this.internalPanel.Bounds = new Rectangle(0, 0, mx, my);

			int visibleWidth = this.bounds.Width - this.borderSize * 2;
			int visibleHeight = this.bounds.Height - this.borderSize * 2;

			this.horizontalScrollBar.Visible = (mx >= visibleWidth);
			this.verticalScrollBar.Visible = (my >= visibleHeight);


			if (true == this.horizontalScrollBar.Visible)
			{
				visibleHeight -= this.horizontalScrollBar.ButtonSize;
			}

			if (true == this.verticalScrollBar.Visible)
			{
				visibleWidth -= this.verticalScrollBar.ButtonSize;
			}

			if (true == this.verticalScrollBar.Visible)
			{
				int h = this.bounds.Height - 2 * this.borderSize;

				if (true == this.horizontalScrollBar.Visible)
				{
					h -= this.horizontalScrollBar.ButtonSize;
				}

				this.verticalScrollBar.Bounds = new Rectangle(this.bounds.Width - this.verticalScrollBar.ButtonSize - this.borderSize, this.borderSize, this.horizontalScrollBar.ButtonSize, h);
			}

			if (true == this.horizontalScrollBar.Visible)
			{
				int w = this.bounds.Width - 2 * this.borderSize;

				if (true == this.verticalScrollBar.Visible)
				{
					w -= this.horizontalScrollBar.ButtonSize;
				}

				this.horizontalScrollBar.Bounds = new Rectangle(this.borderSize, this.bounds.Height - this.horizontalScrollBar.ButtonSize - this.borderSize, w, this.horizontalScrollBar.ButtonSize);
			}

			this.verticalScrollBar.MaxSize = (my - visibleHeight - 0);
			this.horizontalScrollBar.MaxSize = (mx - visibleWidth - 0);
			
			this.internalPanel.Y = (int)(-1.0f * this.verticalScrollBar.Position + this.borderSize);
			this.internalPanel.X = (int)(-1.0f * this.horizontalScrollBar.Position + this.borderSize);
        }

        public static String TypeName
        {
            get
            {
                return "scrollPanel";
            }
        }

		internal ScrollBar verticalScrollBar = null;
		internal ScrollBar horizontalScrollBar = null;
		protected Panel internalPanel = null;
	}
}
