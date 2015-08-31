using System;
using System.Collections.Generic;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Combo Box showing a list of possible choices.
    /// </summary>
	public class ComboBox : Control
	{
        /// <summary>
        /// Creates combo box control
        /// </summary>
        /// <param name="window">window it belongs to</param>
        /// <param name="creationFlags">creation flags</param>
		public ComboBox(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
            this.button = CreateControl<Button>(CreationFlag.InternalControl);

			this.TextAlignment = ContentAlignment.MiddleLeft;
			this.IconAlignment = ContentAlignment.MiddleLeft;
			this.IconSize = IconSize.IconSmall;
			this.IconImageOffset.X = 2;
            this.IconImageOffset.Y = 3;
            this.Border = BorderStyle.BorderLoweredDouble;
			
            this.button.IconAlignment = ContentAlignment.MiddleCenter;
            this.button.Icon = this.Window.Desktop.Theme.ThemeFolder + "/images/combobox_icon_down";

            this.button.Clicked += this.ButtonClicked;

            this.AddControl(this.button);
        }

        protected override void OnClick(int x, int y)
        {
            if (null != this.listWindow)
            {
                this.listWindow.Close();
                this.listWindow = null;
            }
            else
            {
                OpenList();
            }
        }

        private void ButtonClicked(Button sender, EventArgs e)
        {
            if (null == this.listWindow)
            {
                OpenList();
            }
        }

        protected override void Render(Graphics graphics, int x, int y)
        {
			if (BorderStyle.None == this.Border)
			{
				this.button.Bounds.UpdateSize(this.Bounds.Width - 16 - 2, 0, 16, this.Bounds.Height - 0);
			}
			else if (BorderStyle.Flat == this.Border)
			{
                this.button.Bounds.UpdateSize(this.Bounds.Width - 16 - 2, 2, 16, this.Bounds.Height - 4);
			}
			else
			{
                this.button.Bounds.UpdateSize(this.Bounds.Width - 16 - 2, 2, 16, this.Bounds.Height - 4);
			}

			if (null != this.selectedItem)
			{
				this.Text = this.selectedItem.Text;
				this.Icon = this.selectedItem.Icon;
			}

			base.Render(graphics, x, y);
        }

        /// <summary>
        /// Creates and adds combo box item.
        /// </summary>
        /// <param name="text">item display label</param>
        /// <param name="name">item name</param>
        /// <param name="icon">item icon</param>
        /// <param name="tag">itam tag object</param>
        /// <returns>created combo box item</returns>
		public ComboBoxItem AddItem(String text, String name, String icon, Object tag)
        {
            ComboBoxItem item = new ComboBoxItem(this.Engine);

            item.Name = name;
            item.Text = text;
            item.Icon = icon;
            item.Tag = tag;

            this.items.Add(item);

            if (this.items.Count == 1)
            {
                this.selectedItem = item;
                this.Icon = item.Icon;

                if ((null != this.Icon) && (this.Icon.Length > 0))
                {
                    this.TextOffset.X = 20;
                    this.TextOffset.Y = 0;
                }
                else
                {
                    this.TextOffset.X = 4;
                    this.TextOffset.Y = 0;
                }
            }

            return item;
        }

        /// <summary>
        /// Selected item
        /// </summary>
		public ComboBoxItem SelectedItem
        {
            set
            {
                if (null != value)
                {
                    this.selectedItem = value;
                    this.Icon = value.Icon;
                    this.Text = this.selectedItem.Text;
                    this.Icon = this.selectedItem.Icon;

                    if ((null != this.Icon) && (this.Icon.Length > 0))
                    {
                        this.TextOffset.X = 20;
                        this.TextOffset.Y = 0;
                    }
                    else
                    {
                        this.TextOffset.X = 4;
                        this.TextOffset.Y = 0;
                    }
                }

                if (null != this.SelectedItemChanged)
                {
                    this.SelectedItemChanged(this, EventArgs.Empty);
                }
            }
            get
            {
                return this.selectedItem;
            }
        }

        /// <summary>
        /// Selected item index
        /// </summary>
        public int SelectedItemIndex
        {
            set
            {
                if ((value >= 0) && (value < (int)(this.items.Count)))
                {
                    this.SelectedItem = this.items[value];
                }
            }
            get
            {
                if (null == this.selectedItem)
                {
                    return -1;
                }
                else
                {
                    for (int i = 0; i < this.items.Count; i++)
                    {
                        if (this.items[i] == this.selectedItem)
                        {
                            return i;
                        }
                    }

                    return -1;
                }
            }
        }

        /// <summary>
        /// Selects item by its display name
        /// </summary>
		public String SelectedValue
        {
            set
            {
                foreach (ComboBoxItem it in this.items)
                {
                    if (it.Name == value)
                    {
                        this.SelectedItem = it;

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Removes all items from combo box.
        /// </summary>
		public void ClearItems()
        {
			this.items.Clear();
        }

        /// <summary>
        /// ComboBox items
        /// </summary>
        public IEnumerable<ComboBoxItem> Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// Removes item from combo box items list.
        /// </summary>
        /// <param name="item">item to remove</param>
        public void RemoveItem(ComboBoxItem item)
        {
            this.items.Remove(item);
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
        protected override void AddProperties()
        {
			base.AddProperties();

            AddProperty(new PropertyInteger(-1, "selectedIndex", "ComboBox", "selectedIndex", (x) => { this.SelectedItemIndex = x; }, () => { return this.SelectedItemIndex; }));
        }

        protected override void LoadControls(IXmlElement element)
        {
			foreach (IXmlElement p in element.Elements)
			{
				if (p.Name == "item")
				{
					ComboBoxItem item = new ComboBoxItem(this.Engine);
					item.LoadAttributes(p);
					this.items.Add(item);
				}
			}
        }

        protected override void WriteControls(IXmlWriter serializer)
        {
			foreach (ComboBoxItem item in this.items)
			{
				serializer.OpenTag("item");
				item.WriteAttributes(serializer);
				serializer.CloseTag();
			}
        }

        private void OpenList()
        {
			if (0 == this.items.Count)
			{
				return;
			}

			Window window = this.Window.Desktop.NewTempWindow("");

			int cx = 0;
			int cy = window.TopOffset;

			for (Control control = this; control != null; control = control.Parent)
			{
				cx += control.Bounds.X;
				cy += control.Bounds.Y;
			}

			ScrollPanel scrollPanel = window.CreateControl<ScrollPanel>();
			scrollPanel.Border = BorderStyle.Flat;
            scrollPanel.BackColor = Colors.White;
			scrollPanel.FillParent = true;
			window.AddControl(scrollPanel);

			ListBox listBox = window.CreateControl<ListBox>();
			listBox.ListStyle = ListStyle.Details;
            listBox.Border = BorderStyle.None;
			listBox.Name = "listBoxItems";
			scrollPanel.AddControl(listBox);

			bool hasIcon = false;

			foreach (ComboBoxItem item in this.items)
			{
				if (((null != item.Icon) && (item.Icon.Length > 0)) || (null != item.image))
				{
					hasIcon = true;
					break;
				}
			}

			foreach (ComboBoxItem item in this.items)
			{
				ListItem listItem = window.CreateControl<ListItem>();

				listItem.ListStyle = ListStyle.Details;
				listItem.Text = item.Text;
				listItem.Tag = item;

/*				if (null != item.image)
				{
                    listItem.IconPicture = item.image;
				}
				else */
				{
					listItem.Icon = item.Icon;
				}

                listItem.Clicked += this.ItemClicked;
				listItem.BackColor = Colors.None;
//				listItem.SetFont(this.m_strFontName, this.m_nFontSize, this.m_bFontBold, this.m_bFontItalic);
				listItem.NeedTranslation = this.NeedTranslation;

				if (false == hasIcon)
				{
					listItem.TextOffset.X = 0;
                    listItem.TextOffset.Y = 0;
                }

				listBox.AddControl(listItem);
			}

			int maxHeight = this.items.Count * (12 + this.FontInfo.Size + 1);

			if (maxHeight > 300)
			{
				maxHeight = 300;
                listBox.Bounds = new Rectangle(1, 1, this.Bounds.Width - 4 - scrollPanel.horizontalScrollBar.ButtonSize, 0);
			}
			else
			{
                listBox.Bounds = new Rectangle(0, 0, this.Bounds.Width - 4, 0);
			}

			listBox.BackColor = Colors.None;
			listBox.AutoSize = true;

			window.Bounds = new Rectangle(cx, cy + this.Bounds.Height, this.Bounds.Width, maxHeight+3);
			window.Border = BorderStyle.None;
			window.BackColor = Colors.None;
			window.Moveable = false;
			window.Sizeable = true;
            window.Closing += this.WindowClosing;

			this.listWindow = window;
        }

        private void WindowClosing(Window sender, EventArgs args)
        {
            WindowClosed();

            this.listWindow = null;
        }

        private void ItemClicked(Control sender, EventArgs args)
        {
            foreach (ComboBoxItem item in this.items)
            {
                if (sender.Tag == item)
                {
                    this.SelectedItem = item;
                    break;
                }
            }

            this.listWindow.Close();
            this.listWindow = null;
        }

        private void WindowClosed()
        {
			if (null != this.listWindow)
			{
				ListBox listBox = this.listWindow.FindControl<ListBox>("listBoxItems");

				if (null != listBox)
				{
					foreach (Control control in listBox.Controls)
					{
/*                        IImage image = control.IconPicture;

						foreach (ComboBoxItem item in this.items)
						{
							if (image == item.image)
							{
                                control.IconPicture = null;
								break;
							}
						} */
					}
				}
			}
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "comboBox";
            }
        }

        /// <summary>
        /// Selected item changed.
        /// </summary>
        public event UIEventHandler<ComboBox> SelectedItemChanged = null;

		private Button button = null;
		private List<ComboBoxItem> items = new List<ComboBoxItem>();
		private ComboBoxItem selectedItem = null;
		private Window listWindow = null;
	}
}
