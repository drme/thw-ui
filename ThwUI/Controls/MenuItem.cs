using System;
using System.Collections.Generic;
using ThW.UI.Fonts;
using ThW.UI.Utils;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Menu item.
    /// </summary>
	public class MenuItem : UIObject
	{
        /// <summary>
        /// Creates menu item.
        /// </summary>
        /// <param name="engine">ui engine.</param>
        /// <param name="menu">menu it belongs to.</param>
        /// <param name="text">menu item text.</param>
        /// <param name="icon">menu item icon.</param>
        /// <param name="name">menu item name.</param>
        /// <param name="parent">parent menu item.</param>
        /// <param name="topItem">is this top menu item.</param>
        internal MenuItem(UIEngine engine, Menu menu, String text, String icon, String name, MenuItem parent, bool topItem) : base("menuItem")
        {
            this.engine = engine;
            this.icon = icon;
            this.text = text;
            this.topItem = topItem;
            this.parent = parent;
            this.menu = menu;
            this.Name = name;

            for (int i = 0; i < 8; i++)
            {
                this.borders[i] = null;
            }
        }

        /// <summary>
        /// Releases images.
        /// </summary>
        ~MenuItem()
        {
            EraseItems();

            for (int i = 0; i < 8; i++)
            {
                this.engine.DeleteImage(ref this.borders[i]);
            }

            this.engine.DeleteImage(ref this.sideTexture);
            this.engine.DeleteImage(ref this.iconImage);
            this.engine.DeleteImage(ref this.arrowImage);
            this.engine.DeleteImage(ref this.backImage);
        }
		
        /// <summary>
        /// Removes al child items.
        /// </summary>
		public void EraseItems()
        {
			this.menuItems.Clear();
        }

        /// <summary>
        /// Renders menu item.
        /// </summary>
        /// <param name="graphics">graphicsto render to.</param>
        /// <param name="font">font for text.</param>
        /// <param name="px">X offset.</param>
        /// <param name="py">Y offset.</param>
        internal void Render(Graphics graphics, IFont font, int px, int py)
        {
			if (true == this.IsHr)
			{
                graphics.SetColor(this.menu.Window.Desktop.Theme.Colors.BorderDark);
				graphics.DrawRectangle((int)(px + 18 + this.bounds.X + Menu.defaultCharWidth), py + 2 + this.bounds.Y, this.bounds.Width - 18 - Menu.defaultCharWidth, 1);
				return;
			}

			if (true == this.selected)
			{
				RenderSelection(graphics, px, py);
			}

			int offXX = 0;

			if (false == this.topItem)
			{
				offXX = 18;
			}

            graphics.SetColor(this.menu.Window.Desktop.Theme.Colors.TextDark);

			if (null != font)
			{
				font.DrawText(graphics, offXX + px + this.bounds.X + Menu.defaultCharWidth, py + this.bounds.Y + 5, this.text);
			}
        }

        /// <summary>
        /// Renders menu item.
        /// </summary>
        /// <param name="graphics">graphicsto render to.</param>
        /// <param name="font">font for text.</param>
        /// <param name="px">X offset.</param>
        /// <param name="py">Y offset.</param>
        internal void RenderInSeparateWindow(Graphics graphics, IFont font, int px, int py)
        {
			if (true == this.IsHr)
			{
                graphics.SetColor(this.menu.Window.Desktop.Theme.Colors.BorderDark);
				graphics.DrawRectangle(px + 18 + this.bounds.X + Menu.defaultCharWidth, py + this.bounds.Y, this.bounds.Width - 18 - Menu.defaultCharWidth, 1);

				return;
			}

            bool simple = this.menu.Window.Desktop.Theme.Menu.IsSimple;

			if (true == this.expanded)
			{
				if (false == this.topItem)
				{
					RenderSelection(graphics, px, py);
				}
				else
				{
					RenderShadow(graphics, px + this.bounds.X, py + this.bounds.Y, this.bounds.Width, this.bounds.Height);

					if (false == simple)
					{
                        graphics.SetColor(this.menu.Window.Desktop.Theme.Colors.BackBorder);
						graphics.DrawRectangle(px + this.bounds.X, py + this.bounds.Y, this.bounds.Width, this.bounds.Height);
					}
					else
					{
						RenderSelection(graphics, px, py);
					}
				}
			}
			else if ( (true == this.selected) )
			{
				RenderSelection(graphics, px, py);
			}

			if ((0 != this.icon.Length) && (false == this.topItem))
			{
				if (null == this.iconImage)
				{
                    this.iconImage = this.engine.CreateImage(this.icon);
				}

				int offX = 2;
				int offY = (this.bounds.Height - 16) / 2;

				graphics.SetColor(Colors.White);
				graphics.DrawImage(px + this.bounds.X + offX, py + this.bounds.Y + offY, 16, 16, this.iconImage);
			}

			if ((0 != this.menuItems.Count) && (false == this.topItem) )
			{
				if (null == this.arrowImage)
				{
                    ControlSettings settings = this.menu.Window.Desktop.Theme.GetControlSettings("menu");
                    this.arrowImage = this.engine.CreateImage(this.menu.Window.Desktop.Theme.ThemeFolder + "/images/menu_arrow");
				}

				int oY = (this.bounds.Height - this.arrowImage.Height) / 2;
				int oX = this.bounds.Width - this.arrowImage.Width - 3;

				graphics.DrawImage(px + this.bounds.X + oX, py + this.bounds.Y + oY, this.arrowImage.Width, this.arrowImage.Height, this.arrowImage);
			}

			if ( (true == this.expanded) )
			{
				RenderFrame(graphics, px + this.controlsBounds.X, py + this.controlsBounds.Y, this.controlsBounds.Width, this.controlsBounds.Height);

				foreach (MenuItem it in this.menuItems)
				{
					it.RenderInSeparateWindow(graphics, font, px, py);
				}
			}

			if (true == this.expanded)
			{
				if (false == this.topItem)
				{
				}
				else
				{
					if (false == simple)
					{
                        graphics.SetColor(this.menu.Window.Desktop.Theme.Colors.BackLight);
						graphics.DrawRectangle(px + this.bounds.X + 1, py + this.bounds.Y + 1, this.bounds.Width - 2, this.bounds.Height + 0);
					}
				}
			}

			int offXX = 0;
			int offYY = 5;

			if (false == this.topItem)
			{
				offXX = 20; //18;
				offYY = 7;
			}

            graphics.SetColor(this.menu.Window.Desktop.Theme.Colors.TextDark);
			
			if (null != font)
			{
				font.DrawText(graphics, offXX + px + this.bounds.X + Menu.defaultCharWidth, py + this.bounds.Y + offYY, this.text);
			}

			if (  (true == this.selected) )
			{
				this.expanded = true;

				if (null != this.parent)
				{
					this.parent.ClearExpanded(this);
				}
			}
        }

        /// <summary>
        /// Sets menu item bounds.
        /// </summary>
        /// <param name="X">X.</param>
        /// <param name="Y">Y.</param>
        /// <param name="Width">Width.</param>
        /// <param name="Height">Height.</param>
		internal void SetSize(int x, int y, int w, int h)
        {
			this.bounds.X = x;
			this.bounds.Y = y;
			this.bounds.Width = w;
			this.bounds.Height = h;
        }

        /// <summary>
        /// Updates control size based on font.
        /// </summary>
        /// <param name="font">font for menu item rendering.</param>
		internal void UpdateSizes(IFont font)
        {
			if (0 == this.menuItems.Count)
			{
				return;
			}

			if (this.controlsBounds.Width < 0)
			{
				int maxWidth = 40;  // minimum Width == 40
				int h = 1;

                // calculate max Width
				foreach (MenuItem menuItem in this.menuItems)
				{
                    if (false == menuItem.IsHr)
					{
                        int len = 24 + ((null != font) ? font.TextLength(menuItem.Text) : menuItem.Text.Length * Menu.defaultCharWidth);

						if (len > maxWidth)
						{
							maxWidth = len;
						}
					}
				}

				maxWidth += 32 + 4 + 4 + Menu.defaultCharWidth * 2;

				int offX = this.bounds.X;
				int offY = this.bounds.Y;
				int nButtonHeight = (null != font) ? 15 + font.TextHeight("") : 23;

				if (true == this.topItem)
				{
					offY += this.bounds.Height;
				}
				else
				{
					offX += this.bounds.Width + 1;
				}

                // Update sizes
                foreach (MenuItem menuItem in this.menuItems)
				{
					if (true == menuItem.IsHr)
					{
						menuItem.SetSize(offX + 2, offY + h + 1, maxWidth - 4, 1);

						h += 2;
					}
					else
					{
						menuItem.SetSize(offX + 2, offY + h + 1, maxWidth - 4, nButtonHeight - 1);

						h += nButtonHeight;

						menuItem.UpdateSizes(font);
					}
				}

				h += 2;

				this.controlsBounds.X = offX;
				this.controlsBounds.Y = offY;
				this.controlsBounds.Width = maxWidth;
				this.controlsBounds.Height = h;
			}
        }

        /// <summary>
        /// Expands or collapses menu item.
        /// </summary>
        /// <param name="expanded">is expanded</param>
		internal void SetExpanded(bool expanded)
        {
			this.expanded = expanded;
        }

        /// <summary>
        /// Handles mouse move event.
        /// </summary>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
		internal void MouseMove(int x, int y)
        {
			this.mousePosition.X = x;
			this.mousePosition.Y = y;

			this.selected = IsInside(x, y);

			foreach (MenuItem menuItem in this.menuItems)
			{
				menuItem.MouseMove(x, y);
			}
        }

        /// <summary>
        /// Sets if menu item is selected.
        /// </summary>
        /// <param name="selected">is selected.</param>
		internal void SetSelected(bool selected)
        {
			this.selected = selected;
        }

        /// <summary>
        /// Menu item text;
        /// </summary>
		public String Text
        {
            get
            {
                return this.text;
            }
        }

        /// <summary>
        /// Is horizontal separator.
        /// </summary>
		public bool IsHr
        {
            get
            {
                return (0 == this.text.Length);
            }
        }

        /// <summary>
        /// is X,Y inside this menu item.
        /// </summary>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        /// <returns>is inside</returns>
		internal bool IsInside(int x, int y)
        {
			return this.bounds.IsInside(x, y);
        }

        /// <summary>
        /// Collapses menu items expect specified one.
        /// </summary>
        /// <param name="exceptThis">do not collapse this menu item.</param>
		internal void ClearExpanded(MenuItem exceptThis)
        {
            foreach (MenuItem menuItem in this.menuItems)
			{
				if (menuItem != exceptThis)
				{
					menuItem.SetExpanded(false);
				}
			}
        }

        /// <summary>
        /// Handles mouse release event.
        /// </summary>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        /// <param name="menu">menu to receive event.</param>
		internal virtual void MouseRelease(int x, int y, Menu menu)
        {
			if (true == IsInside(x, y))
			{
                if (null != this.Clicked)
                {
                    this.Clicked(this, EventArgs.Empty);
                }

				if (null != menu)
				{
					menu.OnMenuItemClicked(this);
				}
				return;
			}

			//if (true == this.m_bSelected)
			{
				foreach (MenuItem menuItem in this.menuItems)
				{
					menuItem.MouseRelease(x, y, menu);
				}
			}
        }

        /// <summary>
        /// Is inside menu, then it's expanded.
        /// </summary>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        /// <returns>is inside.</returns>
		internal bool IsInsideExpandedMenu(int x, int y)
        {
			if (true == IsInside(x, y))
			{
				return true;
			}

			if (true == this.expanded)
			{
				foreach (MenuItem menuItem in this.menuItems)
				{
					if (true == menuItem.IsInsideExpandedMenu(x, y))
					{
						return true;
					}
				}
			}

			return false;
        }
        
        /// <summary>
        /// Creates and adds menu item to the sub menu.
        /// </summary>
        /// <param name="text">menu item text</param>
        /// <param name="icon">menu item icon</param>
        /// <param name="name">menu item name</param>
        /// <returns>created emnu item</returns>
		public MenuItem AddItem(String text, String icon, String name)
        {
			MenuItem item = new MenuItem(this.engine, this.menu, text, icon, name, this, false);

			this.menuItems.Add(item);

			return item;
        }

        /// <summary>
        /// Creates and adds menu item to the sub menu.
        /// </summary>
        /// <param name="text">menu item text</param>
        /// <returns>created menu item</returns>
        public MenuItem AddItem(String text)
        {
            return AddItem(text, "", "");
        }

        /// <summary>
        /// Renders menu item selection.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="px">X position.</param>
        /// <param name="py">Y position.</param>
        internal void RenderSelection(Graphics graphics, int px, int py)
        {
            graphics.SetColor(this.menu.Window.Desktop.Theme.Colors.HighlightBorder, 0.5f);
			graphics.DrawBox(px + this.bounds.X, py + this.bounds.Y, this.bounds.Width, this.bounds.Height);

            graphics.SetColor(this.menu.Window.Desktop.Theme.Colors.Highlight, 0.5f);
			graphics.DrawRectangle(px + this.bounds.X + 1, py + this.bounds.Y + 1, this.bounds.Width - 2, this.bounds.Height - 2);
        }

        /// <summary>
        /// Renders control frame
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        /// <param name="Width">Width.</param>
        /// <param name="Height">Height.</param>
        internal void RenderFrame(Graphics graphics, int x, int y, int w, int h)
        {
			if ( (w <= 0) || (h <= 0) )
			{
				return;
			}

			RenderShadow(graphics, x, y, w, h);
			RenderFrameBack(graphics, x, y, w, h);
			RenderFrameBorder(graphics, x, y, w, h);
			RenderFrameIconPlaceHolder(graphics, x, y, w, h);
        }

        /// <summary>
        /// Renders controls shadow.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        /// <param name="Width">Width.</param>
        /// <param name="Height">Height.</param>
        internal void RenderShadow(Graphics graphics, int x, int y, int w, int h)
        {
            if (true == this.menu.Window.Desktop.Theme.Menu.HasShadow)
			{
				for (int i = 0; i < 4; i++)
				{
					float aVal = 0.1f + 0.03f * (float)(i + 1);

					graphics.SetColor(Colors.Black, aVal);
					graphics.DrawRectangle(4 + x + i, 4 + y + i, w - i - i, h - i - i);
				}
			}
        }

        /// <summary>
        /// Renders menu items frams back.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        /// <param name="Width">Width.</param>
        /// <param name="Height">Height.</param>
        internal void RenderFrameBack(Graphics graphics, int x, int y, int w, int h)
        {
            graphics.SetColor(this.menu.Window.Desktop.Theme.Menu.PopUpBackColor);
            graphics.DrawRectangle(x/*+2*/, y/* + 2*/, w/* - 4*/, h/* - 4*/);

            if (true == this.menu.Window.Desktop.Theme.Menu.HasPopUpBackImage)
            {
                if (null == this.backImage)
                {
                    ControlSettings settings = this.menu.Window.Desktop.Theme.GetControlSettings(Menu.TypeName);
                    this.backImage = this.engine.CreateImage(this.menu.Window.Desktop.Theme.ThemeFolder + "/images/menu_popup_back");
                }

                if ((null != this.backImage) && (0.0f != this.backImage.Width) && (0.0f != this.backImage.Height))
                {
                    //tile
                    float u = (float)(w) / (float)(this.backImage.Width);
                    float v = (float)(h) / (float)(this.backImage.Height);
                    graphics.SetColor(Colors.White);
                    graphics.DrawImage(x, y, w, h, this.backImage, u, v);
                }
            }
        }

        /// <summary>
        /// Renders frame border.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        /// <param name="Width">Width.</param>
        /// <param name="Height">Height.</param>
        internal void RenderFrameBorder(Graphics graphics, int x, int y, int w, int h)
        {
            if (false == this.menu.Skinned)
            {
                if (true == this.menu.Window.Desktop.Theme.Menu.IsSimple)
                {
                    this.menu.RenderBorderVertexXYWH(graphics, x, y, w, h, true, this.menu.BorderColorLight1, this.menu.BorderColorLight2, this.menu.BorderColorDark1, this.menu.BorderColorDark2);
                }
                else
                {
                    graphics.SetColor(this.menu.BorderColorDark2);
                    graphics.DrawBox(x, y, w, h);
                }
            }
            else
            {
                if (null == this.borders[0])
                {
                    ControlSettings settings = this.menu.Window.Desktop.Theme.GetControlSettings(Menu.TypeName);
                    String folder = "";

                    if (false == this.menu.Window.Desktop.Theme.Menu.IsSimple)
                    {
                        folder = this.menu.Window.Desktop.Theme.ThemeFolder + settings.BordersFilePrefix + "_flat";
                    }
                    else
                    {
                        folder = this.menu.Window.Desktop.Theme.ThemeFolder + settings.BordersFilePrefix + "_raised_double";
                    }

                    this.borders[0] = this.engine.CreateImage(folder + "top_left");
                    this.borders[1] = this.engine.CreateImage(folder + "left");
                    this.borders[2] = this.engine.CreateImage(folder + "bottom_left");
                    this.borders[3] = this.engine.CreateImage(folder + "top");
                    this.borders[4] = this.engine.CreateImage(folder + "bottom");
                    this.borders[5] = this.engine.CreateImage(folder + "top_right");
                    this.borders[6] = this.engine.CreateImage(folder + "bottom_right");
                    this.borders[7] = this.engine.CreateImage(folder + "right");
                }

                graphics.SetColor(Colors.White);

                int width = w;
                int height = h;

                graphics.DrawImage(x - borders[1].Width + 2, y + 2, borders[1].Width, height - 4, borders[1]);
                graphics.DrawImage(x + 2, y - borders[3].Height + 2, width - 4, borders[3].Height, borders[3]);
                graphics.DrawImage(x + 2, y - 2 + height, width - 4, borders[4].Height, borders[4]);
                graphics.DrawImage(x - 2 + width, y + 2, borders[7].Width, height - 4, borders[7]);

                graphics.DrawImage(x - borders[0].Width + 2, y - borders[0].Height + 2, borders[0].Width, borders[0].Height, borders[0]);
                graphics.DrawImage(x - borders[2].Width + 2, y - 2 + height, borders[2].Width, borders[2].Height, borders[2]);
                graphics.DrawImage(x + width - 2, y - borders[5].Height + 2, borders[5].Width, borders[5].Height, borders[5]);
                graphics.DrawImage(x + width - 2, y - 2 + height, borders[6].Width, borders[6].Height, borders[6]);
            }
        }

        /// <summary>
        /// Renders icon placeholder.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        /// <param name="Width">Width.</param>
        /// <param name="Height">Height.</param>
        internal void RenderFrameIconPlaceHolder(Graphics graphics, int x, int y, int w, int h)
        {
            if (true == this.menu.Window.Desktop.Theme.Menu.HasIconPlaceHolder)
            {
                if (false == this.menu.Skinned)
                {
                    graphics.SetColor(this.menu.Window.Desktop.Theme.Colors.Control);
                    graphics.DrawRectangle(x + 1, y + 2, 24, h - 4);
                }
                else
                {
                    if (null == this.sideTexture)
                    {
                        ControlSettings settings = this.menu.Window.Desktop.Theme.GetControlSettings(Menu.TypeName);
                        this.sideTexture = this.engine.CreateImage(this.menu.Window.Desktop.Theme.ThemeFolder + "/images/menu_icon_placeholder");
                    }

                    graphics.SetColor(Colors.White);
                    graphics.DrawImage(x + 1, y + 2, 24, h - 4, this.sideTexture);
                }
            }
        }

        /// <summary>
        /// Load menu item from xml.
        /// </summary>
        /// <param name="root">root xml element.</param>
        internal void LoadControls(IXmlElement root)
        {
			if (null == root)
			{
				return;
			}

            foreach (IXmlElement element in root.Elements)
			{
				if (element.Name == "item")
				{
					String name = element.GetAttributeValue("name", "");
					String icon = element.GetAttributeValue("icon", "");
					String text = element.GetAttributeValue("text", "");

					MenuItem menuItem = AddItem(text, icon, name);

					menuItem.LoadControls(element);
				}
			}
        }

        /// <summary>
        /// Finds menu item by name.
        /// </summary>
        /// <param name="name">menu item name.</param>
        /// <returns>discovered item or null if not found.</returns>
        internal MenuItem FindItem(String name)
        {
            foreach (MenuItem item in this.menuItems)
            {
                if (item.Name == name)
                {
                    return item;
                }

                MenuItem res = item.FindItem(name);

                if (null != res)
                {
                    return res;
                }
            }

            return null;
        }

        /// <summary>
        /// Menu item click event.
        /// </summary>
        public event UIEventHandler<MenuItem> Clicked = null;

        private UIEngine engine = null;
		private	String icon = "";
		private	String text = "";
		private	List<MenuItem> menuItems = new List<MenuItem>();
		private	bool topItem = false;
		private	bool selected = false;
		private	bool expanded = false;
		private	Rectangle bounds = new Rectangle(-1, -1, -1, -1);
        private Rectangle controlsBounds = new Rectangle(-1, -1, -1, -1);
		private	IImage[] borders = new IImage[8];
		private	IImage sideTexture = null;
		private	IImage iconImage = null;
		private	IImage arrowImage = null;
		private	IImage backImage = null;
		private	Point2D mousePosition = new Point2D();
		private	MenuItem parent = null;
		private	Menu menu = null;
    }
}

