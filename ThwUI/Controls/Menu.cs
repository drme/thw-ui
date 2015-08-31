using System;
using System.Collections.Generic;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Menu bar control.
    /// </summary>
	public class Menu : Control
	{
        /// <summary>
        /// Creates menu object.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
        public Menu(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
            this.BackImageLayout = ImageLayout.ImageLayoutTile;
        }

        /// <summary>
        /// Renders menu.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
			base.Render(graphics, x, y);

			UpdateSizes();

			if (false == this.inMenu)
			{
				foreach (MenuItem menuItem in this.menuItems)
				{
					menuItem.Render(graphics, this.FontInfo.Font, x + this.Bounds.X, y + this.Bounds.Y);
				}
			}
        }

        /// <summary>
        /// Renders menu in other control.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        internal virtual void RenderInSeparateWindow(Graphics graphics, int x, int y)
        {
			base.Render(graphics, x, y);

			UpdateSizes();

			for (int i = 0; i < this.menuItems.Count; i++)
			{
				MenuItem menuItem = this.menuItems[i];

				menuItem.SetExpanded(i == this.selected);

				menuItem.RenderInSeparateWindow(graphics, this.FontInfo.Font, x + this.Bounds.X, y + this.Bounds.Y);
			}
        }

        /// <summary>
        /// Handles mouse move event.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        protected override void OnMouseMove(int x, int y)
        {
			this.mousePosition.SetCoords(x, y);

			int i = 0;

			foreach (MenuItem menuItem in this.menuItems)
			{
				menuItem.MouseMove(x - this.Bounds.X, y - this.Bounds.Y);

				if (true == this.inMenu)
				{
					if (true == menuItem.IsInside(x - this.Bounds.X, y - this.Bounds.Y))
					{
						this.selected = i;
					}
				}
				else
				{
					if (true == menuItem.IsInside(x - this.Bounds.X, y - this.Bounds.Y))
					{
						menuItem.SetSelected(true);
					} else
					{
						menuItem.SetSelected(false);
					}
				}

				i++;
			}
        }

        /// <summary>
        /// Handles mouse relese event.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        protected override void OnMouseReleased(int x, int y)
        {
			int i = 0;

			foreach (MenuItem menuItem in this.menuItems)
			{
				if (true == menuItem.IsInside(x - this.Bounds.X, y - this.Bounds.Y))
				{
					this.selected = i;
					return;
				}

                i++;
			}

			if ( (this.selected >= 0) && (this.selected < (int)(this.menuItems.Count)) )
			{
				this.menuItems[this.selected].MouseRelease(x - this.Bounds.X, y - this.Bounds.Y, this);
			}
        }

        /// <summary>
        /// Handles click on menu.
        /// </summary>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        protected override void OnClick(int x, int y)
        {
			if (false == this.inMenu)
			{
				int i = 0;

				foreach (MenuItem menuItem in this.menuItems)
				{
					if (true == menuItem.IsInside(this.mousePosition.X - this.Bounds.X, this.mousePosition.Y - this.Bounds.Y))
					{
						OpenMenu();
						this.selected = i;
						return;
					}
				}
			}
        }

        /// <summary>
        /// Are coordinates inside menu.
        /// </summary>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        /// <returns></returns>
        internal bool IsInsideExpandedMenu(int x, int y)
        {
			foreach (MenuItem it in this.menuItems)
			{
				if (true == it.IsInsideExpandedMenu(x, y))
				{
					return true;
				}
			}

			return false;
        }

        /// <summary>
        /// Creates and adds menu item to the menu.
        /// </summary>
        /// <param name="text">menu item text.</param>
        /// <param name="icon">menu item icon.</param>
        /// <param name="name">menu item name.</param>
        /// <returns>created menu item.</returns>
        public MenuItem AddItem(String text, String icon, String name)
        {
			MenuItem item = new MenuItem(this.Engine, this, text, icon, name, null, true);
			
			this.menuItems.Add(item);

			return item;
        }

        /// <summary>
        /// Creates and adds menu item to the menu.
        /// </summary>
        /// <param name="text">menu item text.</param>
        /// <returns>created menu item.</returns>
        public MenuItem AddItem(String text)
        {
            return AddItem(text, "", "");
        }

        /// <summary>
        /// Loads menu from xml file.
        /// </summary>
        /// <param name="root">menu root element</param>
        protected override void LoadControls(IXmlElement root)
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
        /// Removes menu items.
        /// </summary>
        private void EraseItems()
        {
			this.menuItems.Clear();
        }

        /// <summary>
        /// Calculates menu size.
        /// </summary>
        private void UpdateSizes()
        {
			int x = 3;
			int y = 3;

            foreach (MenuItem menuItem in this.menuItems)
			{
                int textLength = (null != this.FontInfo.Font) ? this.FontInfo.Font.TextLength(menuItem.Text) : menuItem.Text.Length * defaultCharWidth;
				int length = defaultCharWidth * 2 + textLength;

                menuItem.SetSize(x, y - 2, length, 10 + this.FontInfo.Font.TextHeight(menuItem.Name));

				x += length + 2;

                menuItem.UpdateSizes(this.FontInfo.Font);
			}
        }

        /// <summary>
        /// Opens menu. Shows menu window.
        /// </summary>
        private void OpenMenu()
        {
			int cx = 0;
			int cy = this.Window.TopOffset;

			for (Control control = this; control != null; control = control.Parent)
			{
				cx += control.Bounds.X;
				cy += control.Bounds.Y;
			}

			Desktop desktop = this.Window.Desktop;
			Window menuWindow = desktop.NewTempWindow(new MenuWindow(desktop, this));

			menuWindow.Border = BorderStyle.None;
			menuWindow.Bounds = new Rectangle(cx - 2, cy, 400, 100);
            menuWindow.Closing += this.OnMenuWindowClosing;

			this.inMenu = true;
        }

        /// <summary>
        /// Menu window closig event handler.
        /// </summary>
        /// <param name="sender">window</param>
        private void OnMenuWindowClosing(Window sender, EventArgs args)
        {
            this.inMenu = false;
            this.selected = -1;
        }

        /// <summary>
        /// Handles menu item click event from any menu item.
        /// Hides menu window.
        /// </summary>
        /// <param name="menuItem">clicked menu item.</param>
        internal void OnMenuItemClicked(MenuItem menuItem)
        {
			this.inMenu = false;

            if (null != this.MenuItemClicked)
            {
                this.MenuItemClicked(menuItem, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Finds menu item by its name
        /// </summary>
        /// <param name="name">item name</param>
        /// <returns>item or null if not found</returns>
        public MenuItem FindItem(String name)
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
        /// Control type name
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "menu";
            }
        }

        /// <summary>
        /// Global notification about clicked any menu item.
        /// </summary>
        public event UIEventHandler<MenuItem> MenuItemClicked = null;

        private List<MenuItem> menuItems = new List<MenuItem>();
        private bool inMenu = false;
        private int selected = -1;
        internal const int defaultCharWidth = 8;
        internal const int defaultCharHeight = 16;
    }
}
