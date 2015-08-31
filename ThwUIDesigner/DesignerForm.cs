using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ThW.UI.Controls;
using ThW.UI.Design;
using ThW.UI.Designer;
using ThW.UI.Sample;
using ThW.UI.Sample.Renderers.GDI;
using ThW.UI.Windows;

namespace ThW.UI.Utils.Designer
{
    /// <summary>
    /// User interface designer form.
    /// </summary>
    public partial class DesignerForm : Form
    {
        /// <summary>
        /// Constructs designer form.
        /// </summary>
        public DesignerForm()
        {
            InitializeComponent();
        }

        private void RenderPanelMouseMove(object sender, MouseEventArgs e)
        {
            if (null != this.desktop)
            {
                this.desktop.MouseMove(e.X, e.Y);
            }

            MouseMove1(e.X, e.Y);

            if (this.render is GDIRender)
            {
                this.renderPanel.Invalidate();
                this.renderPanel.Update();
            }
        }

        private void ApplicationIdle(object sender, EventArgs e)
        {
            if (e is PaintEventArgs)
            {
                this.render.BeginRender(((PaintEventArgs)e).Graphics);
            }
            else
            {
                this.render.BeginRender(null);
            }

            this.desktop.Render(float.MaxValue);
            this.render.EndRender();
        }

        private void InitControls(object sender, EventArgs e)
        {
            this.render = new GDIRender(this.renderPanel.Handle.ToInt32(), this.uiEngine);
            //this.render = new OpenGLRender(this.renderPanel.Handle.ToInt32());
            //this.render = new D3DRender(this.renderPanel.Handle.ToInt32());
            this.uiEngine.Render = this.render;
//            this.uiEngine.SetWorkingFolder(@"E:\1\");
            this.uiEngine.VirtualFileSystem = new AssemblyFilesSystem();
            this.desktop = this.uiEngine.NewDesktop(null, "ui/themes/generic/");
            this.desktop.DrawCursor = false;
            this.hasChanges = false;
            NewWindowClick(sender, e);
            this.hasChanges = true;
            FillProperties();
            this.render.SetBackColor(0f, 0f, 0f, 1f);

            Application.Idle += new EventHandler(ApplicationIdle);
            this.renderPanel.SizeChanged += (s, args) => { this.render.SetViewSize(0, 0, this.renderPanel.Width, this.renderPanel.Height); };
            this.renderPanel.Paint += (s, args) => { ApplicationIdle(s, args); };
            this.renderPanel.MouseMove += new MouseEventHandler(RenderPanelMouseMove);
            this.renderPanel.MouseDown += (s, args) => { MousePress(args.X, args.Y); };
            this.renderPanel.MouseUp += (s, args) => { MouseRelease(args.X, args.Y); };
            this.propertyGrid = new PropertyGridEx();
            this.propertiesPanel.Controls.Add(this.propertyGrid);
            this.propertyGrid.Dock = DockStyle.Fill;

            this.deleteButton.Image = DesignerUtils.GetSystemIcon(131).ToBitmap();
            this.newButton.Image = DesignerUtils.GetSystemIcon(100).ToBitmap();
            this.openButton.Image = DesignerUtils.GetSystemIcon(126).ToBitmap();

            this.controlsPanel.Controls.Clear();

            foreach (KeyValuePair<String, KeyValuePair<Type, IControlsCreator>> controlType in this.uiEngine.ControlTypes)
            {
                KeyValuePair<Type, IControlsCreator> creator = controlType.Value;

                if (true == creator.Value.ShowInDesigner)
                {
                    System.Windows.Forms.RadioButton button = new System.Windows.Forms.RadioButton();

                    button.Dock = DockStyle.Top;
                    button.Text = controlType.Key.ToUpper()[0] + controlType.Key.Substring(1);
                    button.Tag = controlType.Key;

                    this.controlsPanel.Controls.Add(button);
                    this.designControls.Add(button);
                }
            }
            
            this.controlsPanel.Controls.Add(this.radioButtonPointer);
            this.radioButtonPointer.Dock = DockStyle.Top;
            this.render.SetViewSize(0, 0, this.renderPanel.Width, this.renderPanel.Height);
        }

        private void DeleteActiveControl()
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

        private void OpenWindow(object sender, EventArgs e)
        {
            if (true == this.hasChanges)
            {
                switch (System.Windows.Forms.MessageBox.Show("Save changes", "Unsaved window", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case System.Windows.Forms.DialogResult.Cancel:
                        return;
                    case System.Windows.Forms.DialogResult.No:
                        break;
                    case System.Windows.Forms.DialogResult.Yes:
                        SaveClick(sender, e);
                        break;
                    default:
                        return;
                }
            }

            if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (null != this.activeWindow)
                {
                    this.activeWindow.Close();
                    SetEditWindow(null);
                }

                if (null != this.desktop)
                {
                    SetEditWindow(this.desktop.NewWindow(CreationFlag.SelectableInDesigner | CreationFlag.NeedLoading | CreationFlag.NeedSaving, this.openFileDialog1.FileName));
                }

                this.activeFileName = this.openFileDialog1.FileName;
            }
        }

        private void OptionsClick(object sender, EventArgs e)
        {
            if (this.colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.render.SetBackColor((float)colorDialog1.Color.R / 255f, (float)colorDialog1.Color.G / 255f, (float)colorDialog1.Color.B / 255f, 1.0f);
            }
        }

        private void SetEditWindow(Window window)
        {
            this.activeWindow = window;
            SetEditControl(window);
            FillControls();
        }

        private void SetEditControl(ThW.UI.Controls.Control control)
        {
            this.activeControl = control;
            FillProperties();
        }

        private void FillProperties()
        {
            if (null != propertyGrid)
            {
                this.propertyGrid.SelectedObject = this.activeControl;
                this.propertyGrid.Refresh();
            }
        }

        private void ExitClick(object sender, EventArgs e)
        {
            Close();
        }

        private void FillControls()
        {
            this.comboBox1.Items.Clear();

            if (null != this.activeWindow)
            {
                this.comboBox1.Items.Add(this.activeWindow);

                foreach (ThW.UI.Controls.Control c in this.activeWindow.WindowControls)
                {
                    this.comboBox1.Items.Add(c);
                }
            }
        }

        private void ControlSelectedIndexChanged(object sender, EventArgs e)
        {
            SetEditControl((ThW.UI.Controls.Control)this.comboBox1.SelectedItem);
            SetControl((ThW.UI.Controls.Control)this.comboBox1.SelectedItem, false);
        }

        private void SaveAsClick(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveFile(this.saveFileDialog1.FileName);
            }
        }

        private void SaveFile(String strFullPath)
        {
            String ext = ".window.xml";
            String strName = strFullPath;

            if (strFullPath.Length > ext.Length)
            {
                if (strFullPath.Substring(strFullPath.Length - ext.Length) != ext)
                {
                    strName = strName + ext;
                }
            }
            else
            {
                strName = strName + ext;
            }

            if (null != this.activeWindow)
            {
                this.activeWindow.Save(strName);
            }

            this.activeFileName = strName;
        }

        private void NewWindowClick(object sender, EventArgs e)
        {
            if (true == this.hasChanges)
            {
                if (true == this.hasChanges)
                {
                    switch (System.Windows.Forms.MessageBox.Show("Save changes", "Unsaved window", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case System.Windows.Forms.DialogResult.Cancel:
                            return;
                        case System.Windows.Forms.DialogResult.No:
                            break;
                        case System.Windows.Forms.DialogResult.Yes:
                            SaveClick(sender, e);
                            break;
                        default:
                            return;
                    }
                }
            }

            if (null != this.activeWindow)
            {
                this.activeWindow.ClearControls();
                this.activeWindow.Bounds = new Rectangle(10, 10, 300, 300);
                this.activeWindow.Text = "Window";
                this.activeWindow.Border = ThW.UI.Controls.BorderStyle.BorderRaisedDouble;
                this.activeWindow.Opacity = 1.0f;
                this.activeWindow.BackColor = this.activeWindow.Desktop.Theme.Colors.Control;
            }
            else
            {
                Window window = new Window(this.desktop, CreationFlag.SelectableInDesigner | CreationFlag.NeedLoading | CreationFlag.NeedSaving, "");
                window.Bounds = new Rectangle(10, 10, 300, 300);
                SetEditWindow(window);
                this.desktop.AddWindow(window);
            }

            this.activeFileName = "";
        }

        private void MouseMove1(int x, int y)
        {
            UpdateCursor(MousePointers.PointerStandard);

            if (null != this.activeControl)
            {
                int cx = 0;
                int cy = this.activeWindow.TopOffset;
                ThW.UI.Controls.Control c = this.activeControl;

                for (ThW.UI.Controls.Control pp = c.Parent; pp != null; pp = pp.Parent)
                {
                    ThW.UI.Utils.Rectangle r = pp.Bounds;

                    cx += r.X;
                    cy += r.Y;
                }

                if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.BottomLeft, x - cx, y - cy))
                {
                    UpdateCursor(MousePointers.PointerResize1);
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.TopLeft, x - cx, y - cy))
                {
                    UpdateCursor(MousePointers.PointerResize2);
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.TopRight, x - cx, y - cy))
                {
                    UpdateCursor(MousePointers.PointerResize1);
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.BottomRight, x - cx, y - cy))
                {
                    UpdateCursor(MousePointers.PointerResize2);
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.MiddleLeft, x - cx, y - cy))
                {
                    UpdateCursor(MousePointers.PointerHResize);
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.MiddleRight, x - cx, y - cy))
                {
                    UpdateCursor(MousePointers.PointerHResize);
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.TopCenter, x - cx, y - cy))
                {
                    UpdateCursor(MousePointers.PointerVResize);
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.BottomCenter, x - cx, y - cy))
                {
                    UpdateCursor(MousePointers.PointerVResize);
                }
                else if (true == c.Bounds.IsInside(x - cx, y - cy))
                {
                    UpdateCursor(MousePointers.PointerMove);
                }

                if ((this.designControlState & ControlState.ControlDragging) > 0)
                {
                    int dx = x - this.designMousePress.X;
                    int dy = y - this.designMousePress.Y;

                    this.activeControl.X += dx;
                    this.activeControl.Y += dy;

                    this.designMousePress.SetCoords(x, y);
                }
                else if ((this.designControlState & ControlState.ControlResizing) > 0)
                {
                    int dx = x - this.designMousePress.X;
                    int dy = y - this.designMousePress.Y;

                    int nW = this.activeControl.Width + dx;
                    int nH = this.activeControl.Height + dy;
                    int nX = this.activeControl.X;
                    int nY = this.activeControl.Y;

                    this.designMousePress.SetCoords(x, y);

                    if ((this.designControlState & ControlState.ControlResizeHoriz) == 0)
                    {
                        nW -= dx;
                    }

                    if ((this.designControlState & ControlState.ControlResizeVert) == 0)
                    {
                        nH -= dy;
                    }

                    if ((this.designControlState & ControlState.ControlResizeLeft) > 0)
                    {
                        nX = this.activeControl.X + dx;
                        nW = this.activeControl.Width - dx;

                        if (nW < this.activeControl.MinSize.Width)
                        {
                            nX -= (this.activeControl.MinSize.Width - nW);
                            nW = this.activeControl.MinSize.Width;
                            this.designMousePress.X -= dx;
                        }
                    }

                    if ((this.designControlState & ControlState.ControlResizeTop) > 0)
                    {
                        nY = this.activeControl.Y + dy;
                        nH = this.activeControl.Height - dy;

                        if (nH < this.activeControl.MinSize.Height)
                        {
                            nY -= (this.activeControl.MinSize.Height - nH);
                            nH = this.activeControl.MinSize.Height;
                            this.designMousePress.Y -= dy;
                        }
                    } 

                    if (nW < this.activeControl.MinSize.Width)
                    {
                        nW = this.activeControl.MinSize.Width;
                        this.designMousePress.X -= dx;
                    }

                    if (nH < this.activeControl.MinSize.Height)
                    {
                        nH = this.activeControl.MinSize.Height;
                        this.designMousePress.Y -= dy;
                    }
                    
                    this.activeControl.Bounds = new Rectangle(nX, nY, nW, nH);
                }
            }
        }

        private void UpdateCursor(MousePointers pointer)
        {
            //this.activeWindow.UpdateCursor(p);
            //

            switch (pointer)
            {
                case MousePointers.PointerStandard:
                    Cursor.Current = Cursors.Arrow;
                    break;
                case MousePointers.PointerWait:
                    Cursor.Current = Cursors.WaitCursor;
                    break;
                case MousePointers.PointerMove:
                    Cursor.Current = Cursors.SizeAll;
                    break;
                case MousePointers.PointerHResize:
                    Cursor.Current = Cursors.SizeWE;
                    break;
                case MousePointers.PointerVResize:
                    Cursor.Current = Cursors.SizeNS;
                    break;
                case MousePointers.PointerResize1:
                    Cursor.Current = Cursors.SizeNESW;
                    break;
                case MousePointers.PointerResize2:
                    Cursor.Current = Cursors.SizeNWSE;
                    break;
                case MousePointers.PointerText:
                    Cursor.Current = Cursors.IBeam;
                    break;
                case MousePointers.PointerHand:
                    Cursor.Current = Cursors.Hand;
                    break;
                default:
                    Cursor.Current = Cursors.Arrow;
                    break;
            }
        }

        private void MouseRelease(int x, int y)
        {
            if (null != this.activeWindow)
            {
                ThW.UI.Controls.Control c = this.activeWindow.FindControl(x, y);

                if (null != c)
                {
                    if (false == (c is ThW.UI.Controls.Button))
                    {
//                        c.OnClick(x, y);
                    }

                    while ((c.CreationFlag & CreationFlag.SelectableInDesigner) == 0)
                    {
                        c = c.Parent;

                        if (null == c)
                        {
                            break;
                        }
                    }
                }

                this.activeControl = c;

                this.comboBox1.SelectedItem = c;

                if (null != c)
                {
                    c.DrawOutline = true;

                    foreach (ThW.UI.Controls.Control control in this.activeWindow.WindowControls)
                    {
                        if (c != control)
                        {
                            control.DrawOutline = false;
                        }
                    }

                    if (c != this.activeWindow)
                    {
                        this.activeWindow.DrawOutline = false;
                    }

                }
            }

            this.designControlState = 0;
        }

        private void MousePress(int x, int y)
        {
            foreach (System.Windows.Forms.RadioButton button in this.designControls)
            {
                if (button.Checked == true)
                {
                    CreateDesignControl((String)button.Tag, x, y);
                    this.radioButtonPointer.Checked = true;
                    
                    return;
                }
            }
                
            MousePressDesign(x, y);
        }

        private void MousePressDesign(int x, int y)
        {
            ThW.UI.Controls.Control c = FindDesignControl(x, y);

            SetControl(c, true);

            if (null != c)
            {
                int cx = 0;
                int cy = this.activeWindow.TopOffset;

                for (ThW.UI.Controls.Control pp = c.Parent; pp != null; pp = pp.Parent)
                {
                    ThW.UI.Utils.Rectangle r = pp.Bounds;

                    cx += r.X;
                    cy += r.Y;
                }

                if (this.activeWindow == this.activeControl)
                {
                    cy -= this.activeWindow.TopOffset;
                }

                if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.BottomLeft, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeLeft;
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.TopLeft, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeTop | ControlState.ControlResizeLeft;
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.TopRight, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeTop;
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.BottomRight, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz;
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.MiddleLeft, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeHoriz | ControlState.ControlResizeLeft;
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.MiddleRight, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeHoriz;
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.TopCenter, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeTop;
                }
                else if (true == c.IsOnBorder(ThW.UI.Controls.ContentAlignment.BottomCenter, x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlState = ControlState.ControlResizing | ControlState.ControlResizeVert;
                }
                else if (true == c.Bounds.Contains(x - cx, y - cy))
                {
                    this.designMousePress.SetCoords(x, y);
                    this.designControlState = ControlState.ControlDragging;
                }
            }
        }

        private ThW.UI.Controls.Control FindDesignControl(int x, int y)
        {
            if (null != this.activeWindow)
            {
                ThW.UI.Controls.Control c = this.activeWindow.FindControl(x, y);

                while ((null != c) && ((c.CreationFlag & CreationFlag.SelectableInDesigner) == 0))
                {
                    c = c.Parent;
                }

                if (null == c)
                {
                    c = this.activeWindow;
                }

                return c;
            }

            return null;
        }

        private void SetControl(ThW.UI.Controls.Control selectedControl, bool refreshProperties)
        {
            this.activeControl = selectedControl;
            this.comboBox1.SelectedItem = selectedControl;
            this.propertyGrid.SelectedObject = selectedControl;

            if (null != selectedControl)
            {
                selectedControl.DrawOutline = true;

                foreach (ThW.UI.Controls.Control control in this.activeWindow.WindowControls)
                {
                    if (selectedControl != control)
                    {
                        control.DrawOutline = false;
                    }
                }

                if (selectedControl != this.activeWindow)
                {
                    this.activeWindow.DrawOutline = false;
                }
            }

            this.designControlState = 0;       

            if (true == refreshProperties)
            {
                FillControls();
            }
        }

        private void CreateDesignControl(String controlType, int x, int y)
        {
            ThW.UI.Controls.Control parentControl = FindDesignControl(x, y);

            if ((null != parentControl) && (null != this.activeWindow))
            {
                String controlName = controlType;

                if (false == parentControl.CanContainControl(controlName))
                {
                    return;
                }

                int i = 0;
                
                while (null != this.activeWindow.FindControl(controlName))
                {
                    controlName = controlType + "_" + i;
                    i++;
                }

                ThW.UI.Controls.Control newControl = this.activeWindow.CreateControl(controlType, CreationFlag.NeedSaving | CreationFlag.NeedLoading | CreationFlag.SelectableInDesigner);

                if (null != newControl)
                {
                    int cx = 0;
                    int cy = this.activeWindow.TopOffset + 0;

                    for (ThW.UI.Controls.Control pp = parentControl; pp != null; pp = pp.Parent)
                    {
                        cx += pp.X;
                        cy += pp.Y;
                    }

                    newControl.Bounds = new Rectangle(x - cx, y - cy, 95, 24);
                    newControl.Name = controlName;

                    parentControl.AddControl(newControl);

                    SetControl(newControl, true);
                }
            }
        }
        
        private void DeleteButtonClick(object sender, EventArgs e)
        {
            DeleteActiveControl();
        }

        private void SaveClick(object sender, EventArgs e)
        {
            if ((this.activeFileName != null) && (this.activeFileName.Length > 0))
            {
                SaveFile(this.activeFileName);
            }
            else
            {
                SaveAsClick(sender, e);
            }
        }

        private void ClosingForm(object sender, FormClosingEventArgs e)
        {
            if (true == this.hasChanges)
            {
                switch (System.Windows.Forms.MessageBox.Show("There are unsaved changes, save?", "Quit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case System.Windows.Forms.DialogResult.No:
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                    case System.Windows.Forms.DialogResult.Yes:
                        SaveClick(sender, e);
                        return;
                    default:
                        return;
                }
            }
        }

		private void CollectFonts(ThW.UI.Controls.Control control, HashSet<Font> fonts)
		{
			if (null == control)
			{
				return;
			}

			FontStyle style = FontStyle.Regular;

			if (control.FontInfo.Bold)
			{
				style |= FontStyle.Bold;
			}

			if (control.FontInfo.Italic)
			{
				style |= FontStyle.Italic;
			}

			fonts.Add(new Font(control.FontInfo.Name, control.FontInfo.Size, style));

			foreach (var child in control.Controls)
			{
				CollectFonts(child, fonts);
			}
		}

		private void CacheFontClicked(Object sender, EventArgs e)
		{
			HashSet<Font> fonts = new HashSet<Font>();

			CollectFonts(this.activeWindow, fonts);

			FontsExportForm form = new FontsExportForm(this, fonts);
			form.ShowDialog(this);
		}

        private UIEngine uiEngine = new UIEngine();
        private ControlState designControlState = ControlState.None;
        private Point2D designMousePress = new Point2D();
        private GDIRender render = null;
        private Desktop desktop = null;
        private Window activeWindow = null;
        private ThW.UI.Controls.Control activeControl = null;
        private System.Windows.Forms.PropertyGrid propertyGrid = null;
        private List<System.Windows.Forms.RadioButton> designControls = new List<System.Windows.Forms.RadioButton>();
        private bool hasChanges = true;
        private String activeFileName = null;
    }
}
