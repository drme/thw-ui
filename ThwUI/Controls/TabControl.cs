using System;
using System.Collections.Generic;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Displays panels as tab pages.
    /// </summary>
	public class TabControl : Panel
	{
        /// <summary>
        /// Constructs tab control.
        /// </summary>
        /// <param name="window">window it belongs to</param>
        /// <param name="creationFlags">creation flags</param>
		public TabControl(Window window, CreationFlag creationFlags) : base(window, creationFlags)
        {
            this.Skinned = false;
            this.Border = BorderStyle.BorderRaisedDouble;
            this.Type = TypeName;
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
			CreateControls();
			PlaceTabs();

			if (true == this.simple)
			{
                int textHeight = this.FontInfo.Font.TextHeight(this.controlText);
				int sx = 0;
				int sy = 0;

				base.Render(graphics, x, y);

				for (int i = 0; i < this.tabPages.Count; i++)
				{
					String tabPageHeaderText = this.tabPages[i].Text;
                    int textLength = this.FontInfo.Font.TextLength(tabPageHeaderText);
					RenderBorderXYWH(graphics, x + this.bounds.X + sx, y + this.bounds.Y + sy, textLength + 16, textHeight + 16, this.Border, false);
					sx += textLength + 16 + 1;
				}

                RenderBorderXYWH(graphics, this.bounds.X + x, this.bounds.Y + y + textHeight + 16 - 2, this.bounds.Width, this.bounds.Height - textHeight - 16 + 2, this.Border, true);
			}
			else
			{
				base.Render(graphics, x, y);
			}
        }

        /// <summary>
        /// Adds tabe page control.
        /// </summary>
        /// <param name="tabPage">tab page</param>
        public override void AddControl(Control tabPage)
        {
			this.tabPages.Add((TabPage)(tabPage));
			base.AddControl(tabPage);
        }

        /// <summary>
        /// Can contain only tab pages.
        /// </summary>
        /// <param name="controlType">control type.</param>
        /// <returns>can contain specified control type.</returns>
        public override bool CanContainControl(String controlType)
        {
			if (controlType == TabPage.TypeName)
			{
				return true;
			}
			else
			{
				return false;
			}
        }

        /// <summary>
        /// Removes tab page.
        /// </summary>
        /// <param name="tabPage">tab apge to remove.</param>
        public override void RemoveControl(Control tabPage)
        {
			foreach (TabPage it in this.tabPages)
			{
				if (it == tabPage)
				{
					this.tabPages.Remove(it);
					break;
				}
			}

			base.RemoveControl(tabPage);
        }

        /// <summary>
        /// Removes all controls.
        /// </summary>
        public override void ClearControls()
        {
			this.tabPages.Clear();
			base.ClearControls();
        }

        /// <summary>
        /// Renders control border.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        protected override void RenderBorder(Graphics graphics, int x, int y)
        {
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
        protected override void AddProperties()
        {
			base.AddProperties();

            AddProperty(new PropertyInteger(this.SelectedIndex, "selectedIndex", "tabControl", "selectedIndex", (x) => { this.selectedIndex = x; }, () => { return this.selectedIndex; }));
            AddProperty(new PropertyBoolean(this.Simple, "simple", "tabControl", "simple", (x) => { this.Simple = x; }, () => { return this.Simple; }));
            AddProperty(new PropertyBoolean(this.HasOnlyTop, "hasOnlyTop", "tabControl", "hasOnlyTop", (x) => { this.HasOnlyTop = x; }, () => { return this.HasOnlyTop; }));
        }

        /// <summary>
        /// Updates tab control size.
        /// </summary>
        protected override void UpdateSizeControls()
        {
			CreateControls();
			PlaceTabs();

			if (true == this.simple)
			{
                int textHeight = this.FontInfo.Font.TextHeight(this.Text);
				int sx = 0;
				int sy = 0;

				for (int i = 0; i < this.tabPages.Count; i++)
				{
					String tabPageText = this.tabPages[i].Text;
					Button tabPageButton = this.tabButtons[i];
                    int textLength = this.FontInfo.Font.TextLength(tabPageText);
					tabPageButton.Bounds = new Rectangle(sx, sy, textLength + 16, textHeight + 16);
					tabPageButton.Border = BorderStyle.None;
					tabPageButton.BackColor = Colors.None;
					tabPageButton.BackImage = "";
					sx += textLength + 16 + 1;
				}
			}
			else
			{
				this.tabsList.Bounds = new Rectangle(0, 0, 64, this.bounds.Height);
                this.tabControlHeader.Bounds = new Rectangle(66, 0, this.bounds.Width - 66, 23);
				
				if (this.tabControlHeader.Text.Length <= 0)
				{
					this.tabsList.SelectedItemIndex = this.selectedIndex;

					if (null != this.tabsList.SelectedItem)
					{
						this.tabControlHeader.Text = this.tabsList.SelectedItem.Text;
					}
				}
			}

			base.UpdateSizeControls();
        }

        protected void RenderBorderXYWH(Graphics pRender, int x, int y, int w, int h, BorderStyle eBorderStyle, bool bNoTop)
        {
			if (false == this.Skinned)
			{
				switch (eBorderStyle)
				{
					case BorderStyle.Flat:
						RenderBorderVertexXYWH(pRender, x, y, w, h, false, this.BorderColorDark2, this.BorderColorDark2, this.BorderColorDark2, this.BorderColorDark2, bNoTop);
						break;
					case BorderStyle.BorderFlatDouble:
						RenderBorderVertexXYWH(pRender, x, y, w, h, true, this.BorderColorDark2, this.BorderColorDark2, this.BorderColorDark2, this.BorderColorDark2, bNoTop);
						break;
					case BorderStyle.BorderRaised:
						RenderBorderVertexXYWH(pRender, x, y, w, h, false, this.BorderColorLight1, this.BorderColorLight2, this.BorderColorDark1, this.BorderColorDark2, bNoTop);
						break;
					case BorderStyle.Lowered:
						RenderBorderVertexXYWH(pRender, x, y, w, h, false, this.BorderColorDark2, Colors.None, Colors.None, this.BorderColorLight1, bNoTop);
						break;
					case BorderStyle.BorderRaisedDouble:
						RenderBorderVertexXYWH(pRender, x, y, w, h, true, this.BorderColorLight1, this.BorderColorLight2, this.BorderColorDark1, this.BorderColorDark2, bNoTop);
						break;
					case BorderStyle.BorderLoweredDouble:
						RenderBorderVertexXYWH(pRender, x, y, w, h, true, this.BorderColorDark2, this.BorderColorDark1, this.BorderColorLight2, this.BorderColorLight1, bNoTop);
						break;
					default:
						break;
				}

				return;
			}
			else if (BorderStyle.None != eBorderStyle)
			{
/*				if (null == this.m_pBorders[0])
				{
					ControlSettings settings = UIEngine.GetInstance().GetThemes().GetControlSettings(this.m_type);

					String folder = "";

					switch (eBorderStyle)
					{
						case BorderStyle.BorderFlat:
							folder = UIEngine.GetInstance().GetThemes().GetThemeFolder() + settings.BordersFolder + "/flat/";
							break;
						case BorderStyle.BorderFlatDouble:
							folder = UIEngine.GetInstance().GetThemes().GetThemeFolder() + settings.BordersFolder + "/flat/double/";
							break;
						case BorderStyle.BorderRaised:
							folder = UIEngine.GetInstance().GetThemes().GetThemeFolder() + settings.BordersFolder + "/raised/";
							break;
						case BorderStyle.BorderLowered:
							folder = UIEngine.GetInstance().GetThemes().GetThemeFolder() + settings.BordersFolder + "/lowered/";
							break;
						case BorderStyle.BorderRaisedDouble:
							folder = UIEngine.GetInstance().GetThemes().GetThemeFolder() + settings.BordersFolder + "/raised/double/";
							break;
						case BorderStyle.BorderLoweredDouble:
							folder = UIEngine.GetInstance().GetThemes().GetThemeFolder() + settings.BordersFolder + "/lowered/double/";
							break;
						default:
							break;
					}

					UIEngineImpl pEngine = UIEngineImpl.GetInstance();

					this.m_pBorders[0] = pEngine.CreateImage(folder + "top_left");
					this.m_pBorders[1] = pEngine.CreateImage(folder + "left");
					this.m_pBorders[2] = pEngine.CreateImage(folder + "bottom_left");
					this.m_pBorders[3] = pEngine.CreateImage(folder + "top");
					this.m_pBorders[4] = pEngine.CreateImage(folder + "bottom");
					this.m_pBorders[5] = pEngine.CreateImage(folder + "top_right");
					this.m_pBorders[6] = pEngine.CreateImage(folder + "bottom_right");
					this.m_pBorders[7] = pEngine.CreateImage(folder + "right");
				}

				render.SetColor(this.borderColor);

				render.DrawImage(X - m_pBorders[1].GetWidth() + 2, Y + 2, m_pBorders[1].GetWidth(), Height - 4, m_pBorders[1]);

				if ( (true == noTop) && (this.m_nSelectedIndex >= 0) && (this.m_nSelectedIndex < this.m_lstTabPages.Count) )
				{
					//int textHeight = this.m_pFont.TextHeight(this.controlText);
					int gapStart = 0;
					//int sy = 0;

					int gapWidth = this.m_pFont.TextLength(this.m_lstTabPages[this.m_nSelectedIndex].GetText()) + 16 + 1;

					for (int i = 0; i < this.m_nSelectedIndex; i++)
					{
						String text = this.m_lstTabPages[i].GetText();

						gapStart += this.m_pFont.TextLength(text) + 16 + 1;
					}

					//render after gap
					render.DrawImage(X - 1 - this.borderSize + gapStart + gapWidth, Y - m_pBorders[3].GetHeight() + 2, Width - 0 - gapStart - gapWidth +1, m_pBorders[3].GetHeight(), m_pBorders[3]);

					if (this.m_nSelectedIndex == 0)
					{
						render.DrawImage(X - m_pBorders[1].GetWidth() + 2, Y -4, m_pBorders[1].GetWidth(), 10, m_pBorders[1]);
					}
					else
					{
						// render before gap
						render.DrawImage(X + 2, Y - m_pBorders[3].GetHeight() + 2, gapStart - 3 + this.borderSize, m_pBorders[3].GetHeight(), m_pBorders[3]);
						render.DrawImage(X - m_pBorders[0].GetWidth() + 2, Y - m_pBorders[0].GetHeight() + 2, m_pBorders[0].GetWidth(), m_pBorders[0].GetHeight(), m_pBorders[0]);
					}
				}
				else
				{
					render.DrawImage(X + 2, Y - m_pBorders[3].GetHeight() + 2, Width - 4, m_pBorders[3].GetHeight(), m_pBorders[3]);
					render.DrawImage(X - m_pBorders[0].GetWidth() + 2, Y - m_pBorders[0].GetHeight() + 2, m_pBorders[0].GetWidth(), m_pBorders[0].GetHeight(), m_pBorders[0]);
					render.DrawImage(X - m_pBorders[1].GetWidth() + 2, Y + 2, m_pBorders[1].GetWidth(), Height - 4, m_pBorders[1]);
				}

				if (noTop == true)
				{
					render.DrawImage(X - m_pBorders[2].GetWidth() + 2, Y - 2 + Height, m_pBorders[2].GetWidth(), m_pBorders[2].GetHeight(), m_pBorders[2]);
					render.DrawImage(X + 2, Y - 2 + Height, Width - 4, m_pBorders[4].GetHeight(), m_pBorders[4]);
					render.DrawImage(X + Width - 2, Y - 2 + Height, m_pBorders[6].GetWidth(), m_pBorders[6].GetHeight(), m_pBorders[6]);
				}
				else
				{

				}

				render.DrawImage(X - 2 + Width, Y + 2, m_pBorders[7].GetWidth(), Height - 4, m_pBorders[7]);
				render.DrawImage(X + Width - 2, Y - m_pBorders[5].GetHeight() + 2, m_pBorders[5].GetWidth(), m_pBorders[5].GetHeight(), m_pBorders[5]);
 * 
 * 
 */
			} 
        }

        protected void RenderBorderVertexXYWH(Graphics render, int x, int y, int w, int h, bool bDouble, Color colorLight1, Color colorLight2, Color colorDark1, Color colorDark2, bool noTop)
        {
			if (true == noTop)
			{
 				//int textHeight = this.m_pFont.TextHeight(this.controlText);
				int gapStart = 0;
				//int sy = 0;
				int gapWidth = 0;
				
				if ( (this.selectedIndex >= 0) && (this.selectedIndex < (int)(this.tabPages.Count)) )
				{
                    gapWidth = this.FontInfo.Font.TextLength(this.tabPages[this.selectedIndex].Text) + 16 + 1;
				}

				for (int i = 0; i < this.selectedIndex; i++)
				{
					String strText = this.tabPages[i].Text;
                    gapStart += this.FontInfo.Font.TextLength(strText) + 16 + 1;
				}

				//render after gap
				render.SetColor(colorLight1);
				render.DrawRectangle(x + gapStart + gapWidth-2, y, w - 1 - gapStart - gapWidth+2, 1);
				if (true == bDouble)
				{
					render.SetColor(colorLight2);
					render.DrawRectangle(x + 1 + gapStart + gapWidth-2, y + 1, w - 1 - 2 - gapStart - gapWidth+4, 1);
				}

				if (this.selectedIndex == 0)
				{
				}
				else
				{
					// render before gap
					render.SetColor(colorLight1);
					render.DrawRectangle(x, y, /*Width*/ - 1 + gapStart + 1, 1);
					if (true == bDouble)
					{
						render.SetColor(colorLight2);
						render.DrawRectangle(x + 1, y + 1, /*Width*/ - 1 - 2 + gapStart, 1);
					}
				}

                if (false == this.hasOnlyTop)
                {

                    //bottom
                    render.SetColor(colorDark2);
                    render.DrawRectangle(x, y + h - 1, w, 1);
                    if (true == bDouble)
                    {
                        render.SetColor(colorDark1);
                        render.DrawRectangle(x + 1, y - 1 + h - 1, w - 2, 1);
                    }

                    //right
                    render.SetColor(colorDark2);
                    render.DrawRectangle(x + w - 1, y, 1, h);
                    if (true == bDouble)
                    {
                        //right
                        render.SetColor(colorDark1);
                        render.DrawRectangle(x - 1 + w - 1, y + 1, 1, h - 2);
                    }
                }
			}
			else
			{
				//rendering top
				render.SetColor(colorLight1);
				render.DrawRectangle(x, y, w - 1, 1);
				if (true == bDouble)
				{
					render.SetColor(colorLight2);
					render.DrawRectangle(x + 1, y + 1, w - 1 - 2, 1);
				}

				//right
				render.SetColor(colorDark2);
				render.DrawRectangle(x + w - 1, y, 1, h - 2);
				if (true == bDouble)
				{
					//right
					render.SetColor(colorDark1);
					render.DrawRectangle(x - 1 + w - 1, y + 1, 1, h - 2 - 1);
				}
			}

            if (false == this.hasOnlyTop)
            {
                // left
                render.SetColor(colorLight1);
                render.DrawRectangle(x, y, 1, h - 1);

                if (true == bDouble)
                {
                    //left
                    render.SetColor(colorLight2);
                    render.DrawRectangle(x + 1, y + 1, 1, h - 1 - 2);
                    ////right
                    //render.SetColor(colorDark1);
                    //render.DrawRectangle(X - 1 + Width - 1, Y + 1, 1, Height - 2);
                }
            }
        }

        /// <summary>
        /// Calculates tab pages positions.
        /// </summary>
        protected void PlaceTabs()
        {
            for (int i = 0; i < this.tabPages.Count; i++)
            {
                TabPage tabPage = this.tabPages[i];

                if (true == this.Simple)
                {
                    int textHeight = this.FontInfo.Font.TextHeight(this.Text) + 16;
                    tabPage.Visible = i == this.SelectedIndex;
                    tabPage.Bounds.UpdateSize(this.borderSize, textHeight + this.borderSize, this.Bounds.Width - this.borderSize * 2, this.Bounds.Height - this.borderSize * 2 - textHeight);
                }
                else
                {
                    tabPage.Visible = i == this.SelectedIndex;
                    tabPage.Bounds.UpdateSize(66, 27, this.Bounds.Width - 66, this.Bounds.Height - 27);
                }
            }
        }

        /// <summary>
        /// Creates tab page controls.
        /// </summary>
        protected void CreateControls()
        {
			if (true == this.simple)
			{
				if (this.tabButtons.Count != this.tabPages.Count)
				{
					foreach (TabPage tabPage in this.tabPages)
					{
                        Button tabButton = null;

						foreach (Button button in this.tabButtons)
						{
							if (button.Tag == tabPage)
							{
                                button.Text = tabPage.Text;
                                tabButton = button;
								break;
							}
						}

						if ( (tabButton == null) || (0 == this.tabButtons.Count) )
						{
							Button button = CreateControl<Button>(CreationFlag.InternalControl);
							button.Tag = tabPage;
                            button.Clicked += this.SimpleTabButtonClicked;
                            button.GotFocus += this.TabButtonGotFocus;
                            button.Text = tabPage.Text;
							this.tabButtons.Add(button);
							base.AddControl(button);
						}
					}

                    List<Button> junkButtons = new List<Button>();

                    foreach (Button button in this.tabButtons)
                    {
                        bool found = false;

                        foreach (TabPage tabPage in this.tabPages)
                        {
                            if (button.Tag == tabPage)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (false == found)
                        {
                            junkButtons.Add(button);
                        }
                    }

                    foreach (Button button in junkButtons)
                    {
                        RemoveControl(button);
                        this.tabButtons.Remove(button);
                    }
				}

				// wipe useless data
				if (null != this.tabsList)
				{
					RemoveControl(this.tabsList);
					this.tabsList = null;
				}
				if (null != this.tabControlHeader)
				{
					RemoveControl(this.tabControlHeader);
					this.tabControlHeader = null;
				}
			}
			else
			{
				if (null == this.tabsList)
				{
					this.tabsList = CreateControl<ListBox>(CreationFlag.InternalControl);
                    this.tabsList.Border = this.Border;
					this.tabsList.BackColor = Colors.White;
					this.tabsList.Opacity = this.Opacity;
                    this.tabsList.SelectedItemChanged += this.TabsListSelectedItemChanged;
					base.AddControl(this.tabsList);
				}

				if (null == this.tabControlHeader)
				{
					this.tabControlHeader = CreateControl<Panel>(CreationFlag.InternalControl);
                    this.tabControlHeader.Border = this.Border;
                    this.tabControlHeader.BackColor = this.Window.Desktop.Theme.Colors.HighlightBorder;
					this.tabControlHeader.Opacity = this.Opacity;
					this.tabControlHeader.TextAlignment = ContentAlignment.MiddleLeft;
					this.tabControlHeader.TextOffset.X = 3;
                    this.tabControlHeader.TextOffset.Y = 0;
					
                    base.AddControl(this.tabControlHeader);
				}

                int tabsListCount = 0;

                foreach (Control control in this.tabsList.Controls)
                {
                    tabsListCount++;
                }

                if (this.tabPages.Count != tabsListCount)
				{
					foreach (TabPage tabPage in this.tabPages)
					{
                        Control i = null;

						foreach (Control c1 in this.tabsList.Controls)
						{
							if (c1.Tag == tabPage)
							{
                                i = c1;
								break;
							}
						}

                        if ((i == null) || (0 == tabsListCount))
						{
							ListItem listItem = CreateControl<ListItem>();

/*                            if (null != tabPage.IconPicture)
                            {
                                listItem.IconPicture = tabPage.IconPicture;
                            }
                            else */
                            {
                                listItem.Icon = tabPage.Icon;
                            }

							listItem.Text = tabPage.Text;
							listItem.BackColor = Colors.None;
							listItem.Tag = tabPage;

							this.tabsList.AddControl(listItem);
						}
					}
				}

				// wipe useless data
				if (0 != this.tabButtons.Count)
				{
					while (this.tabButtons.Count > 0)
					{
						foreach (Button it in this.tabButtons)
						{
							RemoveControl(it);
						}

						this.tabButtons.Clear();
					}
				}
			}
        }

        /// <summary>
        /// Handles tab button click event. Switches active tab.
        /// </summary>
        /// <param name="sender">clicked button.</param>
        /// <param name="args">arguments</param>
        private void SimpleTabButtonClicked(Button sender, EventArgs args)
        {
            if (true == this.simple)
            {
                TabPage tabPage = (TabPage)sender.Tag;

                if (tabPage != null)
                {
                    for (int i = 0; i < this.tabPages.Count; i++)
                    {
                        if (tabPage == this.tabPages[i])
                        {
                            this.SelectedIndex = i;
                            return;
                        }
                    }
                }
            }
        }
       
        /// <summary>
        /// Handles got focus event for tab button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void TabButtonGotFocus(Control sender, EventArgs args)
        {
            if (true == this.simple)
            {
                TabPage tabPage = (TabPage)sender.Tag;

                if (tabPage != null)
                {
                    for (int i = 0; i < this.tabPages.Count; i++)
                    {
                        if (tabPage == this.tabPages[i])
                        {
                            this.Window.SetFocus(tabPage);

                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles tab page change event on tabs list panel.
        /// </summary>
        /// <param name="sender">list box with changed selection index.</param>
        /// <param name="args">arguments</param>
        private void TabsListSelectedItemChanged(ListBox sender, EventArgs args)
        {
            this.SelectedIndex = this.tabsList.SelectedItemIndex;

            if (null != this.tabsList.SelectedItem)
            {
                this.tabControlHeader.Text = this.tabsList.SelectedItem.Text;
            }
        }

        /// <summary>
        /// Returns active tab object.
        /// </summary>
        public TabPage ActiveTab
        {
            get
            {
                if ((this.selectedIndex >= 0) && (this.tabPages.Count > this.selectedIndex))
                {
                    return this.tabPages[this.selectedIndex];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Control name.
        /// </summary>
        internal new static String TypeName
        {
            get
            {
                return "tabControl";
            }
        }

        /// <summary>
        /// Should only tab control header be rendered.
        /// </summary>
        public bool HasOnlyTop
        {
            get
            {
                return this.hasOnlyTop;
            }
            set
            {
                this.hasOnlyTop = value;
            }
        }

        /// <summary>
        /// Is control simple, or displayed with listbox.
        /// </summary>
        public bool Simple
        {
            get
            {
                return this.simple;
            }
            set
            {
                this.simple = value;
            }
        }

        /// <summary>
        /// Selected tab id.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                return this.selectedIndex;
            }
            set
            {
                bool raiseEvent = (value == this.selectedIndex);

                this.selectedIndex = value;

                if ((true == raiseEvent) && (null != this.SelectedIndexChanged))
                {
                    this.SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Selected tab has been changed on the tyab control.
        /// </summary>
        public event UIEventHandler<TabControl> SelectedIndexChanged = null;

        private ListBox tabsList = null;
        private Panel tabControlHeader = null;
        private List<TabPage> tabPages = new List<TabPage>();
        private List<Button> tabButtons = new List<Button>();
        private int selectedIndex = 0;
        private bool hasOnlyTop = false;
        private bool simple = true;
	}
}
