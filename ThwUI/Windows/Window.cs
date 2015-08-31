using System;
using System.Collections.Generic;
using ThW.UI.Controls;
using ThW.UI.Design;
using ThW.UI.Utils;

namespace ThW.UI.Windows
{
    /// <summary>
    /// Window, groups controls and presents in one manageable window.
    /// </summary>
    public class Window : Control
    {
        /// <summary>
        /// Creates window.
        /// </summary>
        /// <param name="desktop">desktop it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
        /// <param name="fileName">xml file name to load window from. if empty default window is created.</param>
        public Window(Desktop desktop, CreationFlag creationFlags, String fileName) : base(desktop, null, creationFlags, "window")
        {
            this.desktop = desktop;

            if (null == this.desktop)
            {
                throw new Exception("Desktop can not be null");
            }

            this.leftBorderColor = this.Desktop.Theme.Colors.WindowTitleStart;
            this.rightBorderColor = this.Desktop.Theme.Colors.WindowTitleEnd;

            ControlSettings settings = this.Desktop.Theme.GetControlSettings(this.Type);
            this.Icon = this.Desktop.Theme.ThemeFolder + "/images/window_icon";
            this.BackImageLayout = ImageLayout.ImageLayoutTile;

            this.IconImageOffset.X = 1;
            this.IconImageOffset.Y = 1;
            this.Moveable = true;
            this.Sizeable = true;
            this.TextColor = Colors.White;
            this.TopOffset = 20;
            this.Border = BorderStyle.BorderRaisedDouble;
            this.Bounds = new Rectangle(10, 10, 300, 300);

            if ((null != fileName) && (0 != fileName.Length))
            {
                using (IXmlReader reader = this.Engine.OpenXmlFile(fileName))
                {
                    if (null != reader)
                    {
                        Load(reader);
                    }
                }
            }
        }

        /// <summary>
        /// Releases window iamges.
        /// </summary>
		~Window()
        {
            this.Engine.DeleteImage(ref this.closeButtonTexture);
            this.Engine.DeleteImage(ref this.closeButtonActiveTexture);
            this.Engine.DeleteImage(ref this.shadowImage);
        }

        /// <summary>
        /// Creates control specified by typeName. For example "button".
		/// Created control has to be added using AddControl to the parent control (ex.: panel).
        /// </summary>
        /// <param name="controlTypeName">control type.</param>
        /// <param name="creationFlags">creation flags.</param>
        /// <returns>created control.</returns>
		public override Control CreateControl(String controlTypeName, CreationFlag creationFlags)
        {
            Control control = this.Engine.CreateControl(this, controlTypeName, creationFlags);

			if (null != control)
			{
				this.createdControls.Add(control);
			}

			return control;
        }

        /// <summary>
        /// Creates control and and to the window.
        /// </summary>
        /// <typeparam name="ControlType">control type</typeparam>
        /// <param name="creationFlags">creation flags</param>
        /// <returns>created control</returns>
        public override ControlType CreateControl<ControlType>(CreationFlag creationFlags)
        {
            ControlType controlType = this.Engine.CreateControl<ControlType>(this, creationFlags);

            Control control = (Control)(Object)controlType;

            if (null != control)
            {
                this.createdControls.Add(control);
            }

            return controlType;
        }

        /// <summary>
        /// Handles mouse move event. Passes to the active control.
        /// </summary>
        /// <param name="X">mouse move X position.</param>
        /// <param name="Y">mouse move Y position.</param>
        protected override void OnMouseMove(int x, int y)
        {
			this.mousePosition.SetCoords(x, y);

            if ((null != this.pressedControl) && (this != this.pressedControl))
            {
                int cx = 0;
                int cy = this.topOffset;

                for (Control control = this.pressedControl.Parent; control != null; control = control.Parent)
                {
                    cx += control.Bounds.X;
                    cy += control.Bounds.Y;
                }

                this.pressedControl.MouseMoveInternal(x - cx, y - cy);
            }
			//else
			{
				base.OnMouseMove(x, y);

				this.mouseOverX = IsOverX(x, y);
			}
        }

        /// <summary>
        /// Handles mouse press envent. Passes to the active control.
        /// </summary>
        /// <param name="X">mouse press X position.</param>
        /// <param name="Y">mouse press Y position.</param>
		protected override void OnMousePressed(int x, int y)
        {
			base.OnMousePressed(x, y);

			/**
			 * Search for pressed control
			 */
			Control topControl = FindControl(x, y);

			if (this == topControl)
			{
				base.OnMousePressed(x, y);
				
				if (true == this.IsOverX(x, y))
				{
					this.pressedOverX = true;
				}
			}
			else if (null != topControl)
			{
				this.pressedControl = topControl;
				this.pressedControl.MousePressedInternal(x, y);
			}
        }

        /// <summary>
        /// Handles mouse release envent. Passes to the active control.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        protected override void OnMouseReleased(int x, int y)
        {
            base.OnMouseReleased(x, y);

            /**
             * Search for released control
             */
            Control topControl = FindControl(x, y);

            if (null != topControl)
            {
                if (topControl == this.pressedControl)
                {
                    this.currentControl = topControl;

                    topControl.OnClickInternal(x, y);
                }
            }

            if (topControl == this)
            {
                topControl.OnClickInternal(x, y);
            }

            if (null != this.pressedControl)
            {
                this.pressedControl.MouseReleasedInternal(x, y);
                this.currentControl = this.pressedControl;

                foreach (Control control in this.createdControls)
                {
                    control.ClearFocus();
                }

                this.pressedControl.OnFocus();
            }

            //	base.MouseRelease(X, Y);

            this.pressedControl = null;
            this.pressedOverX = false;
        }

        /// <summary>
        /// Sets active control on window.
        /// </summary>
        /// <param name="activeControl">control that has focus.</param>
        public void SetFocus(Control activeControl)
        {
            this.currentControl = activeControl;

            if (null != this.currentControl)
            {
                lock (this.createdControls)
                {
                    foreach (Control control in this.createdControls)
                    {
                        control.ClearFocus();
                    }
                }

                this.currentControl.OnFocus();
            }
        }

        /// <summary>
        /// Handles mouse wheel up envent. Passes to the active control.
        /// </summary>
        /// <returns>was event handled.</returns>
		protected override bool OnMouseWheelUp(int x, int y, int dx, int dy)
        {
			if (null != this.currentControl)
			{
				for (Control i = this.currentControl; (i != null) || (i != this); i = i.Parent)
				{
                    if ((this == i) || (true == i.MouseWheelUpInternal(x, y, dx, dy)))
                    {
                        break;
                    }
				}
			}

			Control control = FindControl(this.mousePosition.X, this.mousePosition.Y);

			if ( (null != control) && (this != control) )
			{
				for (Control i = control; (i != null) || (i != this); i = i.Parent)
				{
					if ( (this == i) || (true == i.MouseWheelUpInternal(x, y, dx, dy)) )
					{
						break;
					}
				}
			}

			return true;
        }

        /// <summary>
        /// Handles mouse wheel down envent. Passes to the active control.
        /// </summary>
        /// <returns>was event handles</returns>
        protected override bool OnMouseWheelDown(int x, int y, int dx, int dy)
        {
			if (null != this.currentControl)
			{
				for (Control i = this.currentControl; (i != null) || (i != this); i = i.Parent)
				{
					if ( (this == i) || (true == i.MouseWheelDownInternal(x, y, dx, dy)) )
					{
						break;
					}
				}
			}

			Control control = FindControl(this.mousePosition.X, this.mousePosition.Y);

			if ( (null != control) && (this != control) )
			{
				for (Control i = control; (i != null) || (i != this); i = i.Parent)
				{
					if ( (this == i) || (true == i.MouseWheelDownInternal(x, y, dx, dy)) )
					{
						break;
					}
				}
			}

			return true;
        }

        /// <summary>
        /// Closes window. Window is removed frmo desktop and deleted.
		/// Also Closing event is beeing raised here.
        /// </summary>
		public virtual void Close()
        {
            if (null != this.Closing)
            {
                this.Closing(this, EventArgs.Empty);
            }

			this.desktop.DeleteWindow(this);
        }

        /// <summary>
        /// Called when mouse is pressed in the window.
        /// </summary>
        /// <param name="X">X mouse position on the desktop.</param>
        /// <param name="Y">Y mouse position on the desktop.</param>
		protected override void OnClick(int x, int y)
        {
			if ( (true == this.pressedOverX) && (true == IsOverX(this.mousePosition.X, this.mousePosition.Y)) )
			{
				this.Close();
			}
        }

        /// <summary>
        /// Renders window.
        /// </summary>
        protected override void Render(Graphics graphics, int x, int y)
        {
			foreach (Control it in this.controlsToDelete)
            {
                if (this.currentControl == it)
                {
					this.currentControl = null;
				}
            }
			
            this.controlsToDelete.Clear();

			UpdateSize();

			if (BorderStyle.None == this.Border)
			{
				this.topOffset = 0;
			}

            if (null != this.menu)
			{
				this.menu.SetSize(this.borderSize + 2, 0 + 0, this.bounds.Width - this.borderSize * 2 - 4, 20);
			}

			RenderShadow(graphics, x, y);

			base.Render(graphics, x, y);
        }

        /// <summary>
        /// Renders window.
        /// </summary>
        /// <param name="graphics">rendering api</param>
        /// <param name="dt">time passed between last frame in seconds</param>
        protected void Render(Graphics graphics, float dt)
        {
            if (this.animations.Count > 0)
            {
                List<PropertyAnimation> ended = new List<PropertyAnimation>();

                foreach (PropertyAnimation animation in this.animations)
                {
                    if (true == animation.Animate(dt))
                    {
                        ended.Add(animation);
                    }
                }

                foreach (PropertyAnimation animation in ended)
                {
                    this.animations.Remove(animation);
                }
            }

            int dx = 0;
            int dy = 0;

            Render(graphics, dx, dy);
        }

        /// <summary>
        /// Renders window. Called from desktop.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="dt">time elapsed since laste frame.</param>
        internal void RenderInternal(Graphics graphics, float dt)
        {
            if (true == this.maximized)
            {
                SetSize(0, 0, this.Desktop.Width, this.Desktop.Height);
            }

            Render(graphics, dt);
        }

        /// <summary>
        /// Finds control on this whindow which name is name.
        /// </summary>
        /// <param name="controlName">control name.</param>
        /// <returns>discoered control or null if not found.</returns>
		public override Control FindControl(String controlName)
        {
			foreach (Control it in this.createdControls)
			{
				if (it.Name == controlName)
				{
					return it;
				}
			}

			return null;
        }

        protected void AddClickHandler(String controlName, UIEventHandler<Button> handler)
        {
            Button button = FindControl<Button>(controlName);

            if (null != button)
            {
                button.Clicked += handler;
            }
        }

        /// <summary>
        /// Handles key press event, passes to active control.
        /// </summary>
        protected override void KeyPress(char c, Key key)
        {
            if (key == Key.Tab)
            {
                Control cc = null;
                int minIndex = -1;

                foreach (Control control in this.createdControls)
                {
                    if ((control.TabIndex > this.selectedTabIndex) && (minIndex <= this.selectedTabIndex) && (true == control.TabStop))
                    {
                        minIndex = control.TabIndex;
                        cc = control;
                    }
                }

                if (minIndex == -1)
                {
                    this.selectedTabIndex = -1;

                    foreach (Control control in this.createdControls)
                    {
                        if ((control.TabIndex > this.selectedTabIndex) && (minIndex <= this.selectedTabIndex) && (true == control.TabStop))
                        {
                            minIndex = control.TabIndex;
                            cc = control;
                        }
                    }
                }

                if (null != cc)
                {
                    foreach (Control control in this.createdControls)
                    {
                        control.ClearFocus();
                    }

                    cc.OnFocus();
                    this.selectedTabIndex = minIndex;
                }
            }
            else if (key == Key.Enter)
            {
                Control cc = null;

                foreach (Control control in this.createdControls)
                {
                    if ((control.TabIndex == this.selectedTabIndex) && (true == control.TabStop))
                    {
                        cc = control;
                        break;
                    }
                }

                if (null != cc)
                {
                    cc.OnClickInternal(-1, -1);
                }
            }
            
            if (key != Key.Tab)
            {
                if ((null != this.currentControl) && (this.currentControl != this))
                {
                    this.currentControl.KeyPressInternal(c, key);
                }
            }

            base.KeyPress(c, key);
        }

        /// <summary>
        /// Transform screen coordinates to window coordinates. the distance from top left corner of window.
        /// </summary>
        /// <param name="screenX">X coordinate on screen.</param>
        /// <returns>translated coordiante</returns>
		internal override int TranslateX(int screenX)
        {
			return screenX - this.bounds.X;
        }

        /// <summary>
        /// Transform screen coordinates to window coordinates. the distance from top left corner of window.
        /// </summary>
        /// <param name="screenY">Y coordinate on screen.</param>
        /// <returns>translated coordiante</returns>
		internal override int TranslateY(int screenY)
        {
			return screenY - this.bounds.Y - this.topOffset;;
        }

        /// <summary>
        /// Returns if point is inside window.
        /// </summary>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        /// <returns>is inside</returns>
		internal override bool IsInside(int x, int y)
        {
			int bs = -2;

			if ( (x >= this.bounds.X + bs) && (x <= this.bounds.X + this.bounds.Width - bs) && (y >= this.bounds.Y + bs) && (y <= this.bounds.Y + this.bounds.Height - bs) )
			{
				return true;
			}
			else
			{
				return false;
			}
        }

        /// <summary>
        /// Specifies is window allways on top.
        /// </summary>
        public bool AlwaysOnTop
        {
            set;
            get;
        }

        /// <summary>
        /// Saves window to xml file. Used by designer.
        /// </summary>
        /// <param name="fileName">file name to save window to.</param>
		public void Save(String fileName)
        {
			using (IXmlWriter writer = this.Engine.CreateXmlFile(fileName))
			{
				if (null != writer)
				{
					this.WriteBegin(writer);
					this.WriteAttributes(writer);
					this.WriteControls(writer);
					this.WriteEnd(writer);
				}
			}
        }

        /// <summary>
        /// The window title bar Height.
        /// </summary>
        public int TopOffset
        {
            get
            {
                return this.topOffset;
            }
            set
            {
                this.topOffset = value;
            }
        }

        /// <summary>
        /// Return desktop to which this window belongs.
        /// </summary>
        public Desktop Desktop
        {
            get
            {
                return this.desktop;
            }
        }

        /// <summary>
        /// Returns the list of controls contained on window.
        /// Used by designer.
        /// </summary>
		public List<Control> WindowControls
        {
			get
			{
				return this.createdControls;
			}
        }

        /// <summary>
        /// Sets/gets if window is modal. Ex. if window is modal, focus can not be given to any other background window.
        /// </summary>
        public bool Modal
        {
            set
            {
                this.modal = value;
            }
            get
            {
                return this.modal;
            }
        }

        /// <summary>
        /// Returns is  specified coordinates is on specified border.
        /// Window bounds are:
        ///         v------v      <- offsetCorner (where corner resizing is still active)
		///        +-------------- <- real size + offsetOuterBorder ---------+
		///      > | +------------ <- real window size              -------+ |
		///      | | | +---------- <- real size -offsetInnerBorder  -----+ | |
		///      | | | | title approx 20px                               | | |
		///      > | | +-------------------------------------------------+ | |
		///        | | |                                                 | | |
		///        | | |                                                 | | |
		///        | | |                                                 | | |
		///        | | +-------------------------------------------------+ | |
		///        | +-----------------------------------------------------+ |
		///        +---------------------------------------------------------+        
        /// </summary>
        /// <param name="border">border type</param>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordiante.</param>
        /// <returns></returns>
		public override bool IsOnBorder(ContentAlignment border, int x, int y)
        {
			int offsetCorner = 15;
			int offsetOuterBorder = 2; // -2 pixels from bounds Y, and X and +2*2 pixels to Width and Height makes real size of window
			int offsetInnerBorder = 2; // border is 2 pixels inside 
			int titleHeight = 20;

			if (false == this.Skinned)
			{
				offsetOuterBorder = 0;
			}

			if (this.bounds.Width < 30)
			{
				offsetCorner = 2;
			}

			if (this.bounds.Height - 2 * offsetInnerBorder < titleHeight)
			{
				titleHeight = this.bounds.Height - 2 * offsetInnerBorder;
			}

			switch (border)
			{
                case ContentAlignment.TopCenter:
					return (x >= this.bounds.X + offsetCorner) && (x <= this.bounds.X + this.bounds.Width - offsetCorner) && (y >= this.bounds.Y - offsetOuterBorder) && (y <= this.bounds.Y + offsetInnerBorder);
                case ContentAlignment.BottomCenter:
					return (x >= this.bounds.X + offsetCorner) && (x <= this.bounds.X + this.bounds.Width - offsetCorner) && (y >= this.bounds.Y - offsetOuterBorder + this.bounds.Height) && (y <= this.bounds.Y + offsetInnerBorder + this.bounds.Height);
                case ContentAlignment.TopLeft:
					return (x >= this.bounds.X - offsetOuterBorder) && (x <= this.bounds.X + offsetCorner) && (y >= this.bounds.Y - offsetOuterBorder) && (y <= this.bounds.Y + offsetCorner) &&
						!((x > this.bounds.X + offsetInnerBorder) && (y > this.bounds.Y + offsetInnerBorder));
                case ContentAlignment.TopRight:
					return (x >= this.bounds.X + this.bounds.Width - offsetCorner) && (x <= this.bounds.X + this.bounds.Width + offsetOuterBorder) && (y >= this.bounds.Y - offsetOuterBorder) && (y <= this.bounds.Y + offsetCorner) &&
						!((x < this.bounds.X + this.bounds.Width - offsetInnerBorder) && (y > this.bounds.Y + offsetInnerBorder));
                case ContentAlignment.MiddleLeft:
					return (x >= this.bounds.X - offsetOuterBorder) && (x <= this.bounds.X + offsetInnerBorder) && (y >= this.bounds.Y + offsetCorner) && (y <= this.bounds.Y + this.bounds.Height - offsetCorner);
                case ContentAlignment.MiddleRight:
					return (x >= this.bounds.X - offsetOuterBorder + this.bounds.Width) && (x <= this.bounds.X + offsetInnerBorder + this.bounds.Width) && (y >= this.bounds.Y + offsetCorner) && (y <= this.bounds.Y + this.bounds.Height - offsetCorner);
                case ContentAlignment.BottomLeft:
					return (x >= this.bounds.X - offsetOuterBorder) && (x <= this.bounds.X + offsetCorner) && (y >= this.bounds.Y - offsetCorner + this.bounds.Height) && (y <= this.bounds.Y + this.bounds.Height + offsetOuterBorder) &&
						!((x > this.bounds.X + offsetInnerBorder) && (y < this.bounds.Y + this.bounds.Height - offsetInnerBorder));
                case ContentAlignment.BottomRight:
					return (x >= this.bounds.X + this.bounds.Width - offsetCorner) && (x <= this.bounds.X + this.bounds.Width + offsetOuterBorder) && (y >= this.bounds.Y - offsetCorner + this.bounds.Height) && (y <= this.bounds.Y + this.bounds.Height + offsetOuterBorder) &&
						!((x < this.bounds.X + this.bounds.Width - offsetInnerBorder) && (y < this.bounds.Y + this.bounds.Height - offsetInnerBorder));
                case ContentAlignment.MiddleCenter:
					return (x > this.bounds.X + offsetInnerBorder) && (x < this.bounds.X + this.bounds.Width - offsetInnerBorder) && (y > this.bounds.Y + offsetInnerBorder) && (y < this.bounds.Y + titleHeight);
				default:
					return false;
			}
        }

        /// <summary>
        /// Renders window border.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        protected override void RenderBorder(Graphics graphics, int x, int y)
        {
			if (BorderStyle.Flat == this.Border)
			{
				this.topOffset = 0;
				base.RenderBorder(graphics, x, y);
				
                return;
			}

			if (false == this.Skinned)
			{
				RenderTitle(graphics, x, y);

				base.RenderBorder(graphics, x, y);
			}
            else if (BorderStyle.None != this.Border)
			{
				if (null == this.borderImage)
				{
                    this.borderImage = this.Engine.CreateImage(this.Desktop.Theme.ThemeFolder + "/images/window_border");
				}

                if (null != this.borderImage)
                {
                    int px = x + this.Bounds.X;
                    int py = y + this.Bounds.Y;
                    int ss = this.Desktop.Theme.Window.BorderSize;
                    int of = this.Desktop.Theme.Window.BorderOffset;
                    int splitPos = this.Desktop.Theme.Window.SplitImagePosition;
                    int splitWidth = this.Desktop.Theme.Window.SplitWidth;
                    int splitHeight = this.Desktop.Theme.Window.TitleHeight;
                    int w = this.borderImage.Width;
                    int h = this.borderImage.Height;
                    float ss_h = (float)ss / (float)h;
                    float ss_w = (float)ss / (float)w;
                    int splitterX = this.Bounds.Width - this.Desktop.Theme.Window.SplitPosition;

                    RenderTitle(graphics, x, y);

                    if (null != this.borderColor)
                    {
                        graphics.SetColor(this.borderColor, this.Opacity);
                        // bottom left corner
                        graphics.DrawImage(px - ss + of, py - of + this.bounds.Height, ss, ss, this.borderImage, 0.0f, 1.0f - ss_h, ss_w, 1.0f, false);
                        // bottom right corner
                        graphics.DrawImage(px + this.bounds.Width - of, py - of + this.bounds.Height, ss, ss, this.borderImage, 1.0f - ss_w, 1.0f - ss_h, 1.0f, 1.0f, false);
                        // bottom
                        graphics.DrawImage(px + of, py - of + this.bounds.Height, this.bounds.Width - of * 2, ss, this.borderImage, ss_w, 1.0f - ss_h, 1.0f - ss_w, 1.0f, false);
                        // left
                        graphics.DrawImage(px - ss + of, py - ss + splitHeight + of, ss, this.bounds.Height - 2 * of + ss - splitHeight, this.borderImage, 0.0f, (float)(splitHeight + 1) / (float)h, ss_w, 1.0f - ss_h, false);
                        // right
                        graphics.DrawImage(px - of + this.bounds.Width, py - ss + splitHeight + of, ss, this.bounds.Height - 2 * of + ss - splitHeight, this.borderImage, 1.0f - ss_w, (float)splitHeight / (float)h, 1.0f, 1.0f - ss_h, false);
                        // top left corner
                        graphics.DrawImage(px - ss + of, py - ss + of, ss, splitHeight, this.borderImage, 0.0f, 0.0f, ss_w, (float)splitHeight / (float)h, false);
                        // top after split
                        graphics.DrawImage(px + splitterX + splitWidth, py - ss + of, this.bounds.Width - splitterX - splitWidth - of, splitHeight, this.borderImage, (float)(splitPos + splitWidth) / (float)w, 0.0f, 1.0f - ss_w, (float)splitHeight / (float)h, false);
                        // top before split
                        graphics.DrawImage(px + of, py - ss + of, splitterX - of, splitHeight, this.borderImage, ss_w, 0.0f, (splitPos) / (float)w, (float)(splitHeight) / (float)h, false);
                        // top split
                        graphics.DrawImage(px + splitterX, py - ss + of, splitWidth, splitHeight, this.borderImage, (float)splitPos / (float)w, 0.0f, (float)(splitPos + splitWidth) / (float)w, (float)splitHeight / (float)h, false);
                        // top right corner
                        graphics.DrawImage(px + this.bounds.Width - of, py - ss + of, ss, splitHeight, this.borderImage, 1.0f - ss_w, 0.0f, 1.0f, (float)splitHeight / (float)h, false);
                    }
                }

                if (BorderStyle.None != this.Border)
			    {
				    if (null == this.closeButtonActiveTexture)
				    {
                        String strFolder = this.Desktop.Theme.ThemeFolder + "/images";
                        this.closeButtonTexture = this.Engine.CreateImage(strFolder + "/window_close_button");
                        this.closeButtonActiveTexture = this.Engine.CreateImage(strFolder + "/window_close_over_button");
				    }

				    IImage p = (true == this.mouseOverX) ? p = this.closeButtonActiveTexture : this.closeButtonTexture;

				    if ((null != p) && (null != this.borderColor))
				    {
					    graphics.SetColor(this.borderColor, this.Opacity);
					    graphics.DrawImage(x + this.bounds.X + this.bounds.Width - 17 - 2, y + this.bounds.Y + 1, p.Width, p.Height, p);
				    }
			    }
			}	
			
			if (true == this.mouseOverX)
			{
				UpdateCursor(MousePointers.PointerStandard);
			}
        }

        /// <summary>
        /// Renders text.
        /// </summary>
        /// <param name="render">graphics to render to.</param>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        protected override void RenderText(Graphics render, int x, int y)
        {
			if ((null != this.FontInfo.Font) && (null != this.textColor))
			{
				render.SetColor(this.textColor, this.Opacity);

                int ofY = (20 - this.FontInfo.Font.TextHeight(this.controlText)) / 2;

				if (true == this.Skinned)
				{
                    this.FontInfo.Font.DrawText(render, this.TextOffset.X + 20 + x + this.Bounds.X, ofY + y + this.Bounds.Y + this.TextOffset.Y, this.controlText);
				}
				else
				{
                    this.FontInfo.Font.DrawText(render, this.topOffset + x + this.Bounds.X + 6, ofY + y + this.Bounds.Y + 3, this.controlText);
				}
			}
        }

        /// <summary>
        /// Renders window icon.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        protected override void RenderIcon(Graphics graphics, int x, int y)
        {
            if ((BorderStyle.None == this.Border) || (BorderStyle.Flat == this.Border) || (null == this.icon))
			{
				return;
			}

            if (true == this.Skinned)
            {
                this.icon.Render(graphics, this.iconImageOffset.X + this.bounds.X + x + this.borderSize, this.iconImageOffset.Y + this.bounds.Y + y + this.borderSize, 16, 16, this.IconColor, this.Opacity, ImageLayout.ImageLayoutZoom);
            }
            else
            {
                int w = this.topOffset - 4;
                this.icon.Render(graphics, this.iconImageOffset.X + this.bounds.X + x + 4, this.iconImageOffset.Y + this.bounds.Y + y + 4, w, w, this.IconColor, this.Opacity, ImageLayout.ImageLayoutZoom);
            }
        }

        /// <summary>
        /// Renders window shadown.
        /// </summary>
        /// <param name="render">graphics to render to.</param>
        /// <param name="x">X coordiante.</param>
        /// <param name="y">Y coordinate.</param>
        protected virtual void RenderShadow(Graphics render, int x, int y)
        {
			if (false == this.hasShadow)
			{
				return;
			}

			if (true == this.Skinned)
			{
				if (null == this.shadowImage)
				{
                    this.shadowImage = this.Engine.CreateImage(this.Desktop.Theme.ThemeFolder + "/images/window_shadow");
				}

                if (null != this.shadowImage)
                {
                    int ss = this.Desktop.Theme.Window.ShadowSize;
                    int w = this.shadowImage.Width;
                    int h = this.shadowImage.Height;

                    render.DrawImage(this.bounds.X + this.bounds.Width, this.bounds.Y, ss, ss, this.shadowImage, 1.0f - (float)ss / (float)w, 0.0f, 1.0f, (float)ss / (float)h, false);
                    render.DrawImage(this.bounds.X + this.bounds.Width, this.bounds.Y + this.bounds.Height, ss, ss, this.shadowImage, 1.0f - (float)ss / (float)w, 1.0f - (float)ss / (float)h, 1.0f, 1.0f, false);
                    render.DrawImage(this.bounds.X, this.bounds.Y + this.bounds.Height, ss, ss, this.shadowImage, 0.0f, 1.0f - (float)ss / (float)h, (float)ss / (float)w, 1.0f, false);
                    render.DrawImage(this.bounds.X + this.bounds.Width, this.bounds.Y + ss, ss, this.bounds.Height - ss, this.shadowImage, 1.0f - (float)ss / (float)w, (float)ss / (float)h, 1.0f, 1.0f - (float)ss / (float)h, false);
                    render.DrawImage(this.bounds.X + ss, this.bounds.Y + this.bounds.Height, this.bounds.Width - ss, ss, this.shadowImage, (float)ss / (float)w, 1.0f - (float)ss / (float)h, 1.0f - (float)ss / (float)w, 1.0f, false);
                }
			}
			else
			{
			    render.SetColor(Colors.Black, 0.13f);
				render.DrawRectangle(this.bounds.X+4, this.bounds.Y+4, this.bounds.Width, this.bounds.Height);

				render.SetColor(Colors.Black, 0.16f);
				render.DrawRectangle(this.bounds.X+5, this.bounds.Y+5, this.bounds.Width-2, this.bounds.Height-2);

				render.SetColor(Colors.Black, 0.19f);
				render.DrawRectangle(this.bounds.X+6, this.bounds.Y+6, this.bounds.Width-4, this.bounds.Height-4);

				render.SetColor(Colors.Black, 0.22f);
				render.DrawRectangle(this.bounds.X+7, this.bounds.Y+7, this.bounds.Width-6, this.bounds.Height-6);
            }
        }

        /// <summary>
        /// Renders window background.
        /// </summary>
        /// <param name="render">graphics t orender to.</param>
        /// <param name="x">X postion.</param>
        /// <param name="y">Y position.</param>
        protected override void RenderBackground(Graphics render, int x, int y)
        {
            if ((BorderStyle.None == this.Border) || (BorderStyle.Flat == this.Border) || (BorderStyle.BorderFlatDouble == this.Border))
			{
				base.RenderBackground(render, x, y);
			}
			else
			{
				RenderBackgroundXYWH(render, x + this.bounds.X + this.borderSize, y + this.bounds.Y + 20, this.bounds.Width - this.borderSize * 2, this.bounds.Height - 20 - this.borderSize);
			}
        }

        /// <summary>
        /// Adds controls properties.
        /// </summary>
		protected override void AddProperties()
        {
			base.AddProperties();

            const string group = "Window";

            AddProperty(new PropertyBoolean(this.AlwaysOnTop, "alwaysOnTop", "Window", "alwaysOnTop", (x) => { this.AlwaysOnTop = x; }, () => { return this.AlwaysOnTop; }));
            AddProperty(new PropertyBoolean(this.HasShadow, "hasShadow", "Window", "hasShadow", (x) => { this.HasShadow = x; }, () => { return this.HasShadow; }));
            AddProperty(new PropertyBoolean(this.modal, "modal", group, "modal", (x) => { this.Modal = x; }, () => { return this.Modal; }));
            AddProperty(new PropertyColor(this.LeftTitleBorderColor, "title.startColor", "Background", "title.startColor", (x) => { this.LeftTitleBorderColor = x; }, () => { return this.LeftTitleBorderColor; }));
            AddProperty(new PropertyColor(this.RightTitleBorderColor, "title.endColor", "Background", "title.endColor", (x) => { this.RightTitleBorderColor = x; }, () => { return this.RightTitleBorderColor; }));
            AddProperty(new PropertyBoolean(this.Moveable, "moveable", "Size", "moveable", (x) => { this.Moveable = x; }, () => { return this.Moveable; }));
            AddProperty(new PropertyBoolean(this.Sizeable, "sizeable", "Size", "sizeable", (x) => { this.Sizeable = x; }, () => { return this.Sizeable; }));
            AddProperty(new PropertyInteger(this.TopOffset, "titleHeight", "Size", "titleHeight", (x) => { this.TopOffset = x; }, () => { return this.TopOffset; }));
            AddProperty(new PropertyBoolean(this.CenterDesktop, "centerDesktop", "Size", "centerDesktop", (x) => { this.CenterDesktop = x; }, () => { return this.CenterDesktop; }));
            AddProperty(new PropertyBoolean(this.Maximized, "maximized", "Size", "maximized", (x) => { this.Maximized = x; }, () => { return this.Maximized; }));
        }

        /// <summary>
        /// Loads window from xml file.
        /// </summary>
        /// <param name="reader">xml file to load from.</param>
		internal virtual void Load(IXmlReader reader)
        {
			IXmlElement root = reader.RootElement;

			if (null != root)
			{
				LoadBegin(root);
				LoadAttributes(root);
				LoadControls(root);
				LoadEnd(root);
			}

			PositionWindow();
        }

        /// <summary>
        /// Cleanups control.
        /// </summary>
        /// <param name="control">control ro release.</param>
		internal virtual void DestroyControl(Control control)
        {
			foreach (Control it in this.createdControls)
			{
				if (it == control)
				{
					this.createdControls.Remove(it);
					this.controlsToDelete.Add(control);
					
                    return;
				}
			}
        }

        /// <summary>
        /// Finds control at X, Y cooridantes.
        /// </summary>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        /// <returns></returns>
		public virtual Control FindControl(int x, int y)
        {
			Control result = this;

            foreach (Control control in this.createdControls)
			{
                if (true == IsVisible(control))
                {
                    int cx = 0;
                    int cy = this.TopOffset;

                    for (Control i = control.Parent; i != null; i = i.Parent)
                    {
                        cx += i.Bounds.X;
                        cy += i.Bounds.Y;
                    }

                    if (true == control.IsInside(x - cx, y - cy))
                    {
                        if (control.ZOrder >= result.ZOrder)
                        {
                            result = control;
                        }
                    }
                }
			}

			return result;
        }

        /// <summary>
        /// Determines if control is visible, by checking it's and is parents Visble attribute.
        /// </summary>
        /// <param name="control">control</param>
        /// <returns>if control is visible.</returns>
        private bool IsVisible(Control control)
        {
            if (false == control.Visible)
            {
                return false;
            }

            if (null != control.Parent)
            {
                return IsVisible(control.Parent);
            }

            return true;
        }

        /// <summary>
        /// Renders windows title text.
        /// </summary>
        /// <param name="render">graphics t orender to.</param>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        protected void RenderTitle(Graphics render, int x, int y)
        {
			if (false == this.Skinned)
			{
				if (BorderStyle.None != this.Border)
				{
                    int off = 2;

					render.SetColor(this.leftBorderColor, this.Opacity);
					render.DrawRectangle(x + this.bounds.X + off, y + this.bounds.Y + off, this.bounds.Width - off * 2, this.topOffset);

					render.SetColor(this.rightBorderColor, this.Opacity);

					int sx = x + this.bounds.X + this.bounds.Width - this.topOffset;
					int sy = y + this.bounds.Y + 5;
					int sw = this.topOffset - 4;
					int sh = this.topOffset - 4;

					render.DrawRectangle(sx, sy, sw, sh);
					RenderBorderXYWH(render, sx, sy, sw, sh, BorderStyle.BorderRaised);

                    render.SetColor(this.Desktop.Theme.Colors.ControlDarkDark);
					render.DrawLine(sx + 3, sy + 3, sx + sw - 5, sy + sh - 5);
					render.DrawLine(sx + 4, sy + 3, sx + sw - 4, sy + sh - 5);
					render.DrawLine(sx + sw - 5, sy + 3, sx + 3, sy + sh - 5);
					render.DrawLine(sx + sw - 4, sy + 3, sx + 4, sy + sh - 5);
				}
			}
        }

        /// <summary>
        /// Centers window if CenterDesktop flag is set to true.
        /// </summary>
		protected void PositionWindow()
        {
			if ((true == this.CenterDesktop) && (null != this.Desktop))
			{
				this.Bounds.X = (this.Desktop.Width - this.Bounds.Width) / 2;
				this.Bounds.Y = (this.Desktop.Height - this.Bounds.Height) / 2;
			}
        }

        /// <summary>
        /// Assigns window desktop.
        /// </summary>
        /// <param name="desktop">desktop this window belongs to.</param>
		internal void SetDesktop(Desktop desktop)
        {
			this.desktop = desktop;
        }

        /// <summary>
        /// Ckecks if X, Y coordinates are inside window close button bounds.
        /// </summary>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        /// <returns>true if coordiantes are over close button.</returns>
	    private	bool IsOverX(int x, int y)
        {
			if (BorderStyle.None == this.Border)
			{
				return false;
			}

			if (true == this.Skinned)
			{
				return ( (x >= this.bounds.X + this.bounds.Width - 19) && (x <= this.bounds.X + this.bounds.Width - 2) && (y >= this.bounds.Y + 1) && (y <= this.bounds.Y + 17) );
			}
			else
			{
				return ( (x >= this.bounds.X + this.bounds.Width - this.topOffset - 1) && (x <= this.bounds.X + this.bounds.Width - this.topOffset - 1+this.topOffset - 4) && (y >= this.bounds.Y + 5) && (y <= this.bounds.Y + 5 + this.topOffset - 4) );
			}
        }

        /// <summary>
        /// Has window shadow.
        /// </summary>
        public bool HasShadow
        {
            get
            {
                return this.hasShadow;
            }
            set
            {
                this.hasShadow = value;
            }
        }

        /// <summary>
        /// Gets window dialog result. Ex.: was window closed by cancel button.
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
        /// Is window maximized.
        /// </summary>
        public bool Maximized
        {
            set
            {
                this.maximized = value;
            }
            get
            {
                return this.maximized;
            }
        }

        /// <summary>
        /// Is window centered on the desktop.
        /// </summary>
        public bool CenterDesktop
        {
            set;
            get;
        }

        /// <summary>
        /// Title bar is splitted into two parts. This property specifies left part border color.
        /// </summary>
        public Color LeftTitleBorderColor
        {
            get
            {
                return this.leftBorderColor;
            }
            set
            {
                this.leftBorderColor = value;
            }
        }

        /// <summary>
        /// Title bar is splitted into two parts. This property specifies right part border color.
        /// </summary>
        public Color RightTitleBorderColor
        {
            get
            {
                return this.rightBorderColor;
            }
            set
            {
                this.rightBorderColor = value;
            }
        }

        /// <summary>
        /// Window animations.
        /// The added animations are plated and removed from the list after theri compleation.
        /// </summary>
        public IList<PropertyAnimation> Animations
        {
            get
            {
                return this.animations;
            }
        }

        /// <summary>
        /// Window is closing event.
        /// </summary>
        public event UIEventHandler<Window> Closing = null;

        private IImage closeButtonTexture = null;
        private IImage closeButtonActiveTexture = null;
        private List<Control> createdControls = new List<Control>();
        private Desktop desktop = null;
        internal Control pressedControl = null;
        private bool mouseOverX = false;
        private bool hasShadow = false;
        private bool modal = false;
        private bool pressedOverX = false;
        internal bool releaseOnClose = true;
        protected Menu menu = null;
        private DialogResult dialogResult = DialogResult.DialogResultNone;
        private Control currentControl = null;
        private List<Control> controlsToDelete = new List<Control>();
        private IImage shadowImage = null;
        private int selectedTabIndex = -1;
        private bool maximized = false;
        private Color leftBorderColor = Colors.White;
        private Color rightBorderColor = Colors.White;
        private List<PropertyAnimation> animations = new List<PropertyAnimation>();
    }
}
