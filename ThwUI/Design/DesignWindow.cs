using System;
using System.Collections.Generic;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Design
{
    /// <summary>
    /// User interface designer window.
    /// </summary>
	internal class DesignWindow : Window
	{
        /// <summary>
        /// Constructs designer window object.
        /// </summary>
        /// <param name="engine">engine to get avaialble controls</param>
        /// <param name="desktop">desktop this window belongs to</param>
		public DesignWindow(UIEngine engine, Desktop desktop) : base(desktop, CreationFlag.FlagsNone, "")
        {
			this.controlsList = CreateControl<ComboBox>();
			this.controlProperties = CreateControl<PropertyGrid>();
			this.designPanel = CreateControl<Panel>();
			this.toolBox = CreateControl<ScrollPanel>();

            this.BackColor = this.Desktop.Theme.Colors.ControlDark;
			this.Bounds = new Rectangle(50, 50, 640, 480);
			this.MinSize = new Size2d(640, 480);

			this.menu = CreateControl<Menu>();
			this.AddControl(this.menu);

			MenuItem fileMenu = this.menu.AddItem("File");
			fileMenu.AddItem("New").Clicked += (x, y) => { NewWindow(); };
			fileMenu.AddItem("");
            fileMenu.AddItem("Open...").Clicked += new UIEventHandler<MenuItem>(OpenClicked);
            fileMenu.AddItem("Save").Clicked += (x, y) => { SaveWindow(); };
            fileMenu.AddItem("Save as...").Clicked += (x, y) => { SaveAsWindow(); };
			fileMenu.AddItem("");
            fileMenu.AddItem("Exit").Clicked += new UIEventHandler<MenuItem>(DesignWindowCloseClicked);

			MenuItem helpMenu = this.menu.AddItem("Help");
            helpMenu.AddItem("About").Clicked += (x, y) => { this.Desktop.NewRegisteredWindow(AboutWindow.TypeName); };

			ToolBar toolBar = CreateControl<ToolBar>();
			toolBar.Bounds = new Rectangle(2, 20, this.Bounds.Width - 4, 24);
			toolBar.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorTop | AnchorStyle.AnchorRight;
			toolBar.BackColor = new Color(217, 214, 207);
			this.AddControl(toolBar);

			Button newButton = CreateControl<Button>();
			newButton.Icon = "ui/design/new";
			newButton.Border = BorderStyle.None;
			newButton.BackColor = new Color(217, 214, 207);
			newButton.BackImage = "";
			newButton.Bounds = new Rectangle(8, 2, 20, 20);
            newButton.Clicked += (x, y) => { NewWindow(); };
			toolBar.AddControl(newButton);

			Button deleteButton = CreateControl<Button>();
			deleteButton.Icon = ("ui/design/delete");
			deleteButton.Border = BorderStyle.None;
			deleteButton.BackColor = new Color(217, 214, 207);
			deleteButton.BackImage = "";
			deleteButton.Bounds = new Rectangle(34, 2, 20, 20);
            deleteButton.Clicked += this.DeleteButtonClicked;
			toolBar.AddControl(deleteButton);

			Label controls = CreateControl<Label>();
            controls.BackColor = this.Desktop.Theme.Colors.HighlightBorder;
			controls.Text = "Controls";
			controls.TextAlignment = ContentAlignment.MiddleLeft;
			controls.TextColor = Colors.Black;
			controls.Bounds = new Rectangle(2, 44, 150, 17);
			controls.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorTop;
			this.AddControl(controls);

			this.toolBox.Bounds = new Rectangle(2, 61, 150, 374);
			this.toolBox.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorTop | AnchorStyle.AnchorBottom;
			this.AddControl(this.toolBox);

			int h = 5;

            foreach (KeyValuePair<String, KeyValuePair<Type, IControlsCreator>> controlType in this.Engine.ControlTypes)
			{
                KeyValuePair<Type, IControlsCreator> creator = controlType.Value;

				if (true == creator.Value.ShowInDesigner)
				{
					Button button = this.toolBox.CreateControl<Button>();
					button.Bounds = new Rectangle(8, h, this.toolBox.Bounds.Width - 30, 20);
					button.Border = BorderStyle.None;
					button.BackImage = "";
                    button.Name = controlType.Key;
					button.TextAlignment = ContentAlignment.MiddleLeft;
                    button.TextOffset.X = 4;
                    button.TextOffset.Y = 0;
                    button.Text = controlType.Key;
                    button.Clicked += (x, y) => { SelectControlType(x); };
					this.toolBox.AddControl(button);

					h += 24;
				}
			}

			int propertiesWidth = 210;

			Label propertiesLabel = CreateControl<Label>();
            propertiesLabel.BackColor = this.Desktop.Theme.Colors.HighlightBorder;
			propertiesLabel.Text = "Properties";
			propertiesLabel.TextAlignment = ContentAlignment.MiddleLeft;
			propertiesLabel.Bounds = new Rectangle(this.Bounds.Width - propertiesWidth - 2, 44, propertiesWidth - 2, 17);
			propertiesLabel.Anchor = AnchorStyle.AnchorTop | AnchorStyle.AnchorRight;
			this.AddControl(propertiesLabel);

			this.controlsList.Bounds = new Rectangle(this.Bounds.Width - propertiesWidth - 2, 61, propertiesWidth - 2, 23);
			this.controlsList.Anchor = AnchorStyle.AnchorTop | AnchorStyle.AnchorRight;
			this.controlsList.Border = BorderStyle.Lowered;
			this.controlsList.BackColor = Colors.White;
            this.controlsList.SelectedItemChanged += this.ControlsListSelectedItemChanged;
			this.AddControl(this.controlsList);

			this.controlProperties.Bounds = new Rectangle(this.Bounds.Width - propertiesWidth - 2, 84, propertiesWidth - 2, 351);
			this.controlProperties.Anchor = AnchorStyle.AnchorTop | AnchorStyle.AnchorRight | AnchorStyle.AnchorBottom;
			this.AddControl(this.controlProperties);

            StatusBar statusBar = CreateControl<StatusBar>();
			statusBar.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorBottom;
			statusBar.Bounds = new Rectangle(2, this.Bounds.Height - 23 - 4 - 18, this.Bounds.Width - 4, 23);
			statusBar.Text = "Ready";
			statusBar.TextAlignment = ContentAlignment.MiddleLeft;
			this.AddControl(statusBar);

			this.designPanel.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorBottom | AnchorStyle.AnchorTop;
			this.designPanel.Bounds = new Rectangle(152, 44, 276, 391);
            this.designPanel.BackColor = this.Desktop.Theme.Colors.ControlDark;
			this.designPanel.Border = BorderStyle.Lowered;
			this.AddControl(this.designPanel);

            this.HasShadow = true;
            this.CenterDesktop = true;
        }

        /// <summary>
        /// Active control selected from combo box.
        /// </summary>
        /// <param name="sender">combobox that has changed value</param>
        /// <param name="args">arguments</param>
        private void ControlsListSelectedItemChanged(ComboBox sender, EventArgs args)
        {
            ComboBoxItem selectedItem = this.controlsList.SelectedItem;

            if (null != selectedItem)
            {
                SetControl((Control)selectedItem.Tag, false);
            }
        }

        /// <summary>
        /// Open menu item clicked.
        /// </summary>
        /// <param name="sender">clicked button</param>
        /// <param name="args">arguments</param>
        private void OpenClicked(MenuItem sender, EventArgs args)
        {
            List<String> filter = new List<String>();
            filter.Add(".window.xml");

            FileChooser fileChooserWindow = (FileChooser)(this.Desktop.NewRegisteredWindow(FileChooser.TypeName));
            fileChooserWindow.Name = "fileChooserOpenFile";
            fileChooserWindow.ActiveFilter = fileChooserWindow.AddFilter("Windows (*.window.xml)", filter);
            fileChooserWindow.FileMustExist = true;
            fileChooserWindow.Text = "Open Window...";
            fileChooserWindow.SelectButtonText = "Open";
            fileChooserWindow.Closing += this.OpenFileWindowClosing;
        }

        /// <summary>
        /// Open file window closed event handler.
        /// </summary>
        /// <param name="sender">closed window.</param>
        private void OpenFileWindowClosing(Window sender, EventArgs args)
        {
            if (DialogResult.DialogResultOK == sender.DialogResult)
            {
                OpenFile(((FileChooser)sender).SelectedFilePath);
            }
        }

        /// <summary>
        /// Handles window close event.
        /// </summary>
        /// <param name="sender">clicked menu item</param>
        /// <param name="args">arguments</param>
        private void DesignWindowCloseClicked(MenuItem sender, EventArgs args)
        {
            Close();

            if (null != this.Engine.ScriptsHandler)
            {
                this.Engine.ScriptsHandler.OnUIEvent("uidesigner.exit", this);
            }
        }

        /// <summary>
        /// Handles delete selected element event.
        /// </summary>
        /// <param name="sender">clicked button</param>
        /// <param name="e">arguments</param>
        private void DeleteButtonClicked(Button sender, EventArgs e)
        {
            if (null != this.activeControl)
            {
                if (this.activeControl != this.activeWindow)
                {
                    this.activeControl.Parent.RemoveControl(this.activeControl);

                    SetControl(null, true);
                }
            }
        }

        /// <summary>
        /// Overrides mouse movement. Updates cursor based control under edit state.
        /// </summary>
        /// <param name="X">cursor position X</param>
        /// <param name="Y">cursor position Y</param>
        protected override void OnMouseMove(int x, int y)
        {
			base.OnMouseMove(x, y);

			if (null != this.activeControl)
			{
				int cx = this.Bounds.X;
				int cy = this.activeWindow.TopOffset + this.Bounds.Y;
				Control control = this.activeControl;

                for (Control i = this.activeControl.Parent; i != null; i = i.Parent)
				{
					Rectangle bounds = i.Bounds;

					cx += bounds.X;
					cy += bounds.Y;
				}

				if (this.activeWindow == this.activeControl)
				{
					cy -= this.activeWindow.TopOffset;
				}

				if (true == control.IsOnBorder(ContentAlignment.MiddleCenter, x - cx, y - cy))
				{
                    UpdateCursor(MousePointers.PointerMove);
				}
                else if (true == control.IsOnBorder(ContentAlignment.BottomLeft, x - cx, y - cy))
				{
                    UpdateCursor(MousePointers.PointerResize1);
				}
                else if (true == control.IsOnBorder(ContentAlignment.TopLeft, x - cx, y - cy))
				{
                    UpdateCursor(MousePointers.PointerResize2);
				}
                else if (true == control.IsOnBorder(ContentAlignment.TopRight, x - cx, y - cy))
				{
                    UpdateCursor(MousePointers.PointerResize1);
				}
                else if (true == control.IsOnBorder(ContentAlignment.BottomRight, x - cx, y - cy))
				{
                    UpdateCursor(MousePointers.PointerResize2);
				}
                else if (true == control.IsOnBorder(ContentAlignment.MiddleLeft, x - cx, y - cy))
				{
                    UpdateCursor(MousePointers.PointerHResize);
				}
                else if (true == control.IsOnBorder(ContentAlignment.MiddleRight, x - cx, y - cy))
				{
                    UpdateCursor(MousePointers.PointerHResize);
				}
                else if (true == control.IsOnBorder(ContentAlignment.TopCenter, x - cx, y - cy))
				{
                    UpdateCursor(MousePointers.PointerVResize);
				}
                else if (true == control.IsOnBorder(ContentAlignment.BottomCenter, x - cx, y - cy))
				{
					UpdateCursor(MousePointers.PointerVResize);
				}

				if ((this.designControlState & ControlState.ControlDragging) > 0)
				{
					this.activeControl.Bounds = new Rectangle(this.designControlBounds.X + (x - this.designMousePress.X), this.designControlBounds.Y + (y - this.designMousePress.Y), this.designControlBounds.Width, this.designControlBounds.Height);
					RefreshData();
				}
                else if ((this.designControlState & ControlState.ControlResizing) > 0)
				{
					int dx = (this.designMousePress.X - x);
					int dy = (this.designMousePress.Y - y);
					int nw = this.designControlBounds.Width - dx;
					int nh = this.designControlBounds.Height - dy;
					int nx = this.activeControl.Bounds.X;
					int ny = this.activeControl.Bounds.Y;

                    if ((this.designControlState & ControlState.ControlResizeHoriz) == 0)
					{
						nw = this.designControlBounds.Width;
					}

                    if ((this.designControlState & ControlState.ControlResizeVert) == 0)
					{
						nh = this.designControlBounds.Height;
					}

                    if ((this.designControlState & ControlState.ControlResizeLeft) > 0)
					{
						nx = this.designControlBounds.X - dx;
						nw = this.designControlBounds.Width + dx;

						if (nw < this.activeControl.MinSize.Width)
						{
							nx -= this.activeControl.MinSize.Width - nw;
							nw = this.activeControl.MinSize.Width;
						}
					}

                    if ((this.designControlState & ControlState.ControlResizeTop) > 0)
					{
						ny = this.designControlBounds.Y - dy;
						nh = this.designControlBounds.Height + dy;

						if (nh < this.activeControl.MinSize.Height)
						{
							ny -= this.activeControl.MinSize.Height - nh;
							nh = this.activeControl.MinSize.Height;
						}
					}

					if (nw < this.activeControl.MinSize.Width)
					{
						nw = this.activeControl.MinSize.Width;
					}

					if (nh < this.activeControl.MinSize.Height)
					{
						nh = this.activeControl.MinSize.Height;
					}

					this.activeControl.Bounds = new Rectangle(nx, ny, nw, nh);
					
                    RefreshData();
				}
			}
        }

        /// <summary>
        /// Mouse press event handler in design window.
        /// </summary>
        /// <param name="X">mouse press X coordinate.</param>
        /// <param name="Y">mouse press Y coordinate.</param>
        protected override void OnMousePressed(int x, int y)
        {
			base.OnMousePressed(x, y);

			// Search for pressed control
			Control selectedControl = FindControl(x, y);

            if (this.designPanel == selectedControl)
			{
				if ((null != this.controlToCreateType) && (this.controlToCreateType.Length > 0))
				{
					CreateDesignControl(x, y);
				}
				else
				{
					MousePressDesign(x, y);
				}

				this.controlProperties.ClearFocus();
			}
			else if (null != selectedControl)
			{
                this.pressedControl = selectedControl;

                if (selectedControl != this)
                {
                    this.pressedControl.MousePressedInternal(x, y);
                }
			}
        }

        /// <summary>
        /// Handles mouse release event in design window.
        /// </summary>
        /// <param name="X">mouse release X coordinate</param>
        /// <param name="Y">mouse release Y coordinate</param>
        protected override void OnMouseReleased(int x, int y)
        {
            base.OnMouseReleased(x, y);

            if (null != this.activeWindow)
            {
                Control control = this.activeWindow.FindControl(x, y);

                if ((null != control) && ((control.CreationFlag & CreationFlag.SelectableInDesigner) == 0))
                {
                    control.OnClickInternal(x, y);
                }
            }

            this.designControlState = ControlState.None;
        }

        /// <summary>
        /// Renders design window and editable window.
        /// </summary>
        /// <param name="graphics">graphics to render to</param>
        /// <param name="X">rendering X position</param>
        /// <param name="Y">rendering Y position</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
			base.Render(graphics, x, y);

			if (null != this.activeWindow)
			{
				// draw outline (windows border is too thick to default outline to be visible)
				if (this.activeControl == this.activeWindow)
				{
                    graphics.SetColor(this.Desktop.Theme.Colors.HighlightBorder, this.Opacity);
					graphics.DrawRectangle(x + this.Bounds.X - 4 + this.activeWindow.Bounds.X, y + this.Bounds.Y - 4 + this.activeWindow.Bounds.Y, this.activeWindow.Bounds.Width + 8, this.activeWindow.Bounds.Height + 8);
				}

				this.activeWindow.RenderInternal(graphics, x + this.Bounds.X, y + this.Bounds.Y);
			}
        }

        /// <summary>
        /// Releases control, as well as clears active control.
        /// </summary>
        /// <param name="control">contro lto remove</param>
        internal override void DestroyControl(Control control)
        {
			if (control == this.activeControl)
			{
				this.activeControl = null;
			}

			base.DestroyControl(control);
        }

        /// <summary>
        /// Updates property grid data for active control.
        /// </summary>
        private void RefreshData()
        {
			if (null != this.controlsList)
			{
				this.controlsList.ClearItems();

				if (null != this.activeWindow)
				{
					this.controlsList.AddItem(this.activeWindow.Name + " : " + this.activeWindow.Type, this.activeWindow.Name + " : " + this.activeWindow.Type, "", this.activeWindow);

					foreach (Control control in this.activeWindow.WindowControls)
					{
						if ((control.CreationFlag & CreationFlag.SelectableInDesigner) > 0)
						{
							this.controlsList.AddItem(control.Name + " : " + control.Type, control.Name + " : " + control.Type, "", control);
						}
					}
				}

				foreach (ComboBoxItem comboBoxItem in this.controlsList.Items)
				{
					if (comboBoxItem.Tag == this.activeControl)
					{
						this.controlsList.SelectedItem = comboBoxItem;
						
                        break;
					}
				}
			}
        }

        /// <summary>
        /// Sets editable window.
        /// </summary>
        /// <param name="window">windwo to edi</param>
        private void SetWindow(Window window)
        {
            if ((this != window) && (window != this.activeWindow))
            {
                this.activeWindow = window;

                RefreshData();
            }
        }

        /// <summary>
        /// Sets active control. Displays its' properties.
        /// </summary>
        /// <param name="control">control under edi</param>
        /// <param name="refresh">shoud the properties list be refreshed</param>
        private void SetControl(Control control, bool refresh)
        {
			if (null != this.activeWindow)
			{
				foreach (Control windowControl in this.activeWindow.WindowControls)
				{
                    windowControl.DrawOutline = false;
				}

				this.activeWindow.DrawOutline = false;
			}

			if (control != this.activeControl)
			{
				if (null != control)
				{
					this.controlProperties.UpdateValue();
					this.controlProperties.ClearControls();
					this.controlProperties.SetProperties(control.Properties);
				}
				else
				{
					this.controlProperties.ClearControls();
				}
			}

			this.activeControl = control;

			if (null != this.activeControl)
			{
				this.activeControl.DrawOutline = true;
			}

			if (true == refresh)
			{
				RefreshData();
			}
        }

        /// <summary>
        /// Control type selection event handler.
        /// </summary>
        /// <param name="controlTypeToolBoxButton">selected control button</param>
        private void SelectControlType(Control controlTypeToolBoxButton)
        {
			if (null != controlTypeToolBoxButton)
			{
				this.controlToCreateType = controlTypeToolBoxButton.Name;
			}
			else
			{
				this.controlToCreateType = "";
			}

			if (null != this.toolBox)
			{
                foreach (Control control in this.toolBox.Controls)
                {
                    control.DrawOutline = (control == controlTypeToolBoxButton);
                }
			}
        }

        /// <summary>
        /// Creates control at X, Y coordinates.
        /// </summary>
        /// <param name="X">X coordiante.</param>
        /// <param name="Y">Y coordiante.</param>
        private void CreateDesignControl(int x, int y)
        {
			Control control = FindDesignControl(x, y);

			if (null != control)
			{
				if (null != this.activeWindow)
				{
					String name = this.controlToCreateType;
					
                    uint i = 0;

					if (false == control.CanContainControl(name))
					{
						return;
					}

					while (null != control.FindControl(name))
					{
						name = this.controlToCreateType + "_" + i;
						
                        i++;
					}

                    Control newControl = this.activeWindow.CreateControl(this.controlToCreateType, CreationFlag.NeedSaving | CreationFlag.NeedLoading | CreationFlag.SelectableInDesigner);

					if (null != newControl)
					{
						int cx = this.Bounds.X;
						int cy = this.activeWindow.TopOffset + this.Bounds.Y;

						for (Control pp = control; pp != null; pp = pp.Parent)
						{
							cx += pp.Bounds.X;
                            cy += pp.Bounds.Y;
						}

						newControl.Bounds = new Rectangle(x - cx, y - cy, 95, 24);
						newControl.Name = name;

						control.AddControl(newControl);

						SetControl(newControl, true);
					}
				}
			}

			this.SelectControlType(null);
        }

        /// <summary>
        /// Handles mouse press event in design window.
        /// </summary>
        /// <param name="X">X mouse press coordinate.</param>
        /// <param name="Y">Y mouse press coordinate.</param>
        private void MousePressDesign(int x, int y)
        {
            Control control = FindDesignControl(x, y);

            SetControl(control, true);

            if (null != control)
            {
                int cx = this.Bounds.X;
                int cy = this.activeWindow.TopOffset + this.Bounds.Y;

                for (Control i = control.Parent; i != null; i = i.Parent)
                {
                    cx += i.Bounds.X;
                    cy += i.Bounds.Y;
                }

                if (this.activeWindow == this.activeControl)
                {
                    cy -= this.activeWindow.TopOffset;
                }

                if (true == control.IsOnBorder(ContentAlignment.MiddleCenter, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlBounds = control.Bounds.Clone();
                    this.designControlState = ControlState.ControlDragging;
                }
                else if (true == control.IsOnBorder(ContentAlignment.BottomLeft, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlBounds = control.Bounds.Clone();
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeLeft;
                }
                else if (true == control.IsOnBorder(ContentAlignment.TopLeft, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlBounds = control.Bounds.Clone();
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeTop | ControlState.ControlResizeLeft;
                }
                else if (true == control.IsOnBorder(ContentAlignment.TopRight, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlBounds = control.Bounds.Clone();
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeTop;
                }
                else if (true == control.IsOnBorder(ContentAlignment.BottomRight, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlBounds = control.Bounds.Clone();
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz;
                }
                else if (true == control.IsOnBorder(ContentAlignment.MiddleLeft, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlBounds = control.Bounds.Clone();
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeHoriz | ControlState.ControlResizeLeft;
                }
                else if (true == control.IsOnBorder(ContentAlignment.MiddleRight, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlBounds = control.Bounds.Clone();
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeHoriz;
                }
                else if (true == control.IsOnBorder(ContentAlignment.TopCenter, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlBounds = control.Bounds.Clone();
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeTop;
                }
                else if (true == control.IsOnBorder(ContentAlignment.BottomCenter, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlBounds = control.Bounds.Clone();
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert;
                }
            }
        }

        /// <summary>
        /// Opens file.
        /// </summary>
        /// <param name="fullPath">full path of file name to open.</param>
        private void OpenFile(String fullPath)
        {
			NewWindow();

            using (IXmlReader reader = this.Engine.OpenXmlFile(fullPath))
            {
                if (null != reader)
                {
                    this.activeWindow.Load(reader);
                }
            }

			this.activeFileName = fullPath;
        }

        /// <summary>
        /// Saves window.
        /// </summary>
        /// <param name="fullPath">file name to save to.</param>
        private void SaveFile(String fullPath)
        {
			String extension = ".window.xml";
			String name = fullPath;

			if (fullPath.Length > extension.Length)
			{
				if (fullPath.Substring(fullPath.Length - extension.Length) != extension)
				{
					name = name + extension;
				}
			}
			else
			{
				name = name + extension;
			}

			if (null != this.activeWindow)
			{
				this.activeWindow.Save(name);
			}

			this.activeFileName = name;
        }

        /// <summary>
        /// Creates new window.
        /// </summary>
        private void NewWindow()
        {
			if (null != this.activeWindow)
			{
				this.activeWindow.ClearControls();
				this.activeWindow.Bounds = new Rectangle(165, 70, 300, 300);
				this.activeWindow.Text = "Window";
				this.activeWindow.Border = BorderStyle.BorderRaisedDouble;
				this.activeWindow.Opacity = 1.0f;
                this.activeWindow.BackColor = this.Desktop.Theme.Colors.Control;
				SetControl(null, true);
			}
			else
			{
                Window window = new Window(this.Desktop, CreationFlag.SelectableInDesigner | CreationFlag.NeedLoading | CreationFlag.NeedSaving, "");
				window.Bounds = new Rectangle(165, 70, 300, 300);
				SetWindow(window);
			}

			this.activeFileName = "";
        }

        /// <summary>
        /// Saves window.
        /// </summary>
        private void SaveWindow()
        {
            if (0 != this.activeFileName.Length)
            {
                SaveFile(this.activeFileName);
            }
            else
            {
                SaveAsWindow();
            }
        }

        /// <summary>
        /// Shows save as window.
        /// </summary>
        private void SaveAsWindow()
        {
			List<String> filter = new List<String>();
			filter.Add(".window.xml");

			FileChooser window = (FileChooser)(this.Desktop.NewRegisteredWindow(FileChooser.TypeName));
			window.ActiveFilter = window.AddFilter("Windows (*.window.xml)", filter);
			window.FileMustExist = false;
			window.Text = "Save as...";
			window.SelectButtonText = "Save";
            window.Closing += this.SaveAsWindowClosed;
        }

        /// <summary>
        /// Save as window closed event handler.
        /// </summary>
        /// <param name="sender">closed window</param>
        private void SaveAsWindowClosed(Window sender, EventArgs args)
        {
            if (DialogResult.DialogResultOK == sender.DialogResult)
            {
                SaveFile(((FileChooser)sender).SelectedFilePath);
            }
        }

        /// <summary>
        /// Find control in desing window at mouse coordinates.
        /// </summary>
        /// <param name="X">coordiante X to find control under</param>
        /// <param name="Y">coordiante Y to find control under</param>
        /// <returns></returns>
        private Control FindDesignControl(int x, int y)
        {
            if (null != this.activeWindow)
            {
                Control control = this.activeWindow.FindControl(x, y);

                while ((null != control) && ((control.CreationFlag & CreationFlag.SelectableInDesigner) == 0))
                {
                    control = control.Parent;
                }

                if (null == control)
                {
                    if (true == this.activeWindow.IsInside(x - this.Bounds.X, y - this.Bounds.Y))
                    {
                        control = this.activeWindow;
                    }
                }

                return control;
            }

            return null;
        }

        /// <summary>
        /// Design window type name
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "uiDesigner";
            }
        }

        private ComboBox controlsList = null;
        private PropertyGrid controlProperties = null;
        private Panel designPanel = null;
		private Control activeControl = null;
        private ScrollPanel toolBox = null;
        private Point2D designMousePress = new Point2D();
        private Rectangle designControlBounds = new Rectangle();
		private Window activeWindow = null;
		private String controlToCreateType = "";
		private String activeFileName = "";
		private ControlState designControlState = ControlState.None;
	}
}
