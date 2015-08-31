using System;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;
using System.Collections;
using System.Collections.Generic;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Displays a list of items.
    /// </summary>
	public class ListBox : Panel
	{
        /// <summary>
        /// Creates list box control
        /// </summary>
        /// <param name="window">window it belongs to</param>
        /// <param name="creationFlags">creation flags</param>
		public ListBox(Window window, CreationFlag creationFlags) : base(window, creationFlags)
        {
		    this.BackColor = Colors.White;
            this.Border = BorderStyle.BorderLoweredDouble;
            this.Type = TypeName;
        }

        /// <summary>
        /// Adds control design properties list
        /// </summary>
        protected override void AddProperties()
        {
            base.AddProperties();

            AddProperty(new PropertyBoolean(this.AutoSize, "autoSize", "listBox", "autoSize", (x) => { this.AutoSize = x; }, () => { return this.AutoSize; }));
        }

        /// <summary>
        /// Renders listbox control.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X coordiante.</param>
        /// <param name="y1">Y coodiante.</param>
        protected override void Render(Graphics graphics, int x, int y1)
        {
            lock (this.Controls)
            {
                int y = this.borderSize;

                switch (this.listStyle)
                {
                    case ListStyle.LargeIcons:
                        {
                            foreach (Control control in this.Controls)
                            {
                                int height = control.Bounds.Height;

                                if (height <= 0)
                                {
                                    height = this.Bounds.Width - 4;
                                }

                                control.Bounds.UpdateSize(2, y, this.Bounds.Width - 4, height);
                                y += height + 4;
                            }
                        }
                        break;
                    case ListStyle.Details:
                        {
                            foreach (Control control in this.Controls)
                            {
                                int height = 12 + this.FontInfo.Size;

                                control.Bounds.UpdateSize(this.borderSize, y, this.Bounds.Width - this.borderSize * 2, height);
                                
                                y += height + 1;
                            }
                        }
                        break;
                    default:
                        break;
                }

                if (true == this.AutoSize)
                {
                    this.Bounds.Height = y + this.borderSize;
                }

                base.Render(graphics, x, y1);
            }
        }

        /// <summary>
        /// Adds list item.
        /// </summary>
        /// <param name="control">liste item to add</param>
        public override void AddControl(Control control)
        {
            base.AddControl(control);

            if (control is ListItem)
            {
                ((ListItem)control).Clicked += (x, y) => { this.SelectedItem = x; };
            }
        }

        /// <summary>
        /// Selected item on the listbox
        /// </summary>
		public ListItem SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                foreach (Control control in this.Controls)
                {
                    if (value == control)
                    {
                        this.selectedItem = value;

                        if (null != this.SelectedItemChanged)
                        {
                            this.SelectedItemChanged(this, EventArgs.Empty);
                        }

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Selected item index
        /// </summary>
		public int SelectedItemIndex
        {
            set
            {
                this.selectedItem = null;

                int i = 0;

                foreach (ListItem control in this.Controls)
                {
                    if (i == value)
                    {
                        this.selectedItem = control;
                        return;
                    }

                    i++;
                }
            }
            get
            {
                int i = 0;

                foreach (ListItem control in this.Controls)
                {
                    if (control == this.selectedItem)
                    {
                        return i;
                    }

                    i++;
                }

                return -1;
            }
        }

        /// <summary>
        /// List presentation.
        /// </summary>
        public ListStyle ListStyle
        {
            set
            {
                this.listStyle = value;
            }
        }

        /// <summary>
        /// Automatically resizes control as items are added
        /// </summary>
		public bool AutoSize
        {
            set
            {
                this.autoSize = value;
            }
            get
            {
                return this.autoSize;
            }
        }

        /// <summary>
        /// List box items.
        /// </summary>
        public IEnumerable<Control> Items
        {
            get
            {
                return this.Controls;
            }
        }

        internal new static String TypeName
        {
            get
            {
                return "listBox";
            }
        }

        /// <summary>
        /// Selected list item changed event.
        /// </summary>
        public event UIEventHandler<ListBox> SelectedItemChanged = null;

		private ListStyle listStyle = ListStyle.LargeIcons;
		private bool autoSize = false;
		private ListItem selectedItem = null;
	}
}
