using System;
using System.Collections.Generic;
using ThW.UI.Design;
using ThW.UI.Fonts;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Events handler for key pressed events received by control
    /// </summary>
    /// <param name="sender">control that has received key press event</param>
    /// <param name="c">character</param>
    /// <param name="key">pressed key</param>
    public delegate void KeyPressedHandler(Control sender, char c, Key key);

    /// <summary>
    /// Base class for all controls. All controls lifetime is managed by window object.
    /// </summary>
    public class Control : UIObject
	{
        protected Control(Window window, CreationFlag creationFlags, String type) : this(null, window, creationFlags, type)
        {
        }

        protected Control(Desktop desktop, Window window, CreationFlag creationFlags, String type) : base(type)
        {
            if (null != desktop)
            {
                this.uiEngine = desktop.Engine;
            }

            if (null != window)
            {
                this.uiEngine = window.Engine;
            }

            Theme theme = null;

            if (null != desktop)
            {
                theme = desktop.Theme;
            }
            else if (null != window)
            {
                theme = window.Desktop.Theme;
            }
            else
            {
                theme = this.Window.Desktop.Theme;
            }

            this.window = window;
            this.creationFlags = creationFlags;
            this.borderColor = theme.Colors.Window;

            ControlSettings settings = theme.GetControlSettings(this.Type);

            this.backColor = settings.ColorBack;
            this.borderColorDark1 = settings.ColorBorderDark1;
            this.borderColorDark2 = settings.ColorBorderDark2;
            this.borderColorLight1 = settings.ColorBorderLight1;
            this.borderColorLight2 = settings.ColorBorderLight2;
            this.skinned = settings.Skinned;

            if (settings.BackImage.Length > 0)
            {
                BackImage = theme.ThemeFolder + "/" + settings.BackImage;
            }

            this.font = new FontInfo(this.uiEngine, this.Window, settings, theme.DefaultFontName, theme.DefaultFontSize);

			this.uiEngine.Language.LanguageChanged += this.OnLanguageChanged;

            OnLanguageChanged();
        }

        ~Control()
        {
            this.uiEngine.DeleteImage(ref this.borderImage);

			if (null != this.font)
			{
				this.font.ClearFont();
			}
        }

        #region Container

        /// <summary>
        /// Adds child control to this control.
        /// </summary>
        /// <param name="childControl">control to add</param>
        public virtual void AddControl(Control control)
        {
            lock (this.childControls)
            {
                this.childControls.Add(control);
            }

			control.SetZOrder(this.zOrder + 1);
			control.SetParent(this);
        }

        /// <summary>
        /// Removes child control. Clears all its childs.
        /// </summary>
        /// <param name="childControl">child control to remove.</param>
        public virtual void RemoveControl(Control control)
        {
            lock (this.childControls)
            {
                control.ClearControls();

                foreach (Control it in this.childControls)
                {
                    if (it == control)
                    {
                        this.childControls.Remove(it);

                        this.Window.DestroyControl(control);

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Removes control from this control.
        /// </summary>
        /// <param name="control">control to remove.</param>
        protected void Deatach(Control control)
        {
            this.childControls.Remove(control);
        }

        /// <summary>
        /// Removes all child controls
        /// </summary>
        public virtual void ClearControls()
        {
            lock (this.childControls)
            {
                Window window = this.Window;

                if (null != window)
                {
                    foreach (Control it in this.childControls)
                    {
                        it.ClearControls();
                        window.DestroyControl(it);
                    }

                    this.childControls.Clear();
                }
            }
        }

        /// <summary>
        /// Finds child control by name. Searches control in all window it belongs to.
        /// </summary>
        /// <param name="controlName">control name.</param>
        /// <returns>discovered control or null if not found.</returns>
        public virtual Control FindControl(String controlName)
        {
            return this.Window.FindControl(controlName);
        }

        /// <summary>
        /// Searches for a child control of specified type and name on a window.
        /// </summary>
        /// <typeparam name="ControlType">control type</typeparam>
        /// <param name="controlName">control name</param>
        /// <returns>reference to the found control, or null if not found</returns>
        public ControlType FindControl<ControlType>(String controlName)
        {
            Control control = FindControl(controlName);

            if ((null != control) && (control is ControlType))
            {
                return (ControlType)Convert.ChangeType(control, typeof(ControlType), null);
            }
            else
            {
                return default(ControlType);
            }
        }

        /// <summary>
        /// Can control contain control of specifeid type.
        /// </summary>
        /// <param name="controlType">control type.</param>
        /// <returns>can control by added as a child</returns>
        public virtual bool CanContainControl(String controlType)
        {
            if ((controlType == PropertyRow.TypeName) || (controlType == PropertyGroup.TypeName) || (controlType == ListItem.TypeName))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Creates control by its type name.
        /// </summary>
        /// <param name="controlType">control type</param>
        /// <param name="creationFlags">creation flags.</param>
        /// <returns>created controlm or null if type is unknow.</returns>
        public virtual Control CreateControl(String controlType, CreationFlag creationFlags)
        {
            return this.Window.CreateControl(controlType, creationFlags);
        }

        /// <summary>
        /// Creates control and adds to the window. Contorl is not added as a child for this control.
        /// Windows maintains lifecyscle of created control.
        /// </summary>
        /// <typeparam name="ControlType">control type</typeparam>
        /// <param name="creationFlags">creation flags</param>
        /// <returns>created control</returns>
        public virtual ControlType CreateControl<ControlType>(CreationFlag creationFlags)
        {
            return this.Window.CreateControl<ControlType>(creationFlags);
        }

        /// <summary>
        /// Creates control and adds to the window. Contorl is not added as a child for this control.
        /// Windows maintains lifecyscle of created control.
        /// </summary>
        /// <typeparam name="ControlType">control type</typeparam>
        /// <returns>created control</returns>
        public ControlType CreateControl<ControlType>()
        {
            return CreateControl<ControlType>(CreationFlag.FlagsNone);
        }

        /// <summary>
        /// Assigns Z order for a control.
        /// </summary>
        /// <param name="zOrder">z order</param>
        private void SetZOrder(int zOrder)
        {
            this.zOrder = zOrder;

            for (int i = 0; i < this.childControls.Count; i++)
            {
                this.childControls[i].SetZOrder(this.zOrder + 1);
            }
        }

        /// <summary>
        /// Assigns parent control.
        /// </summary>
        /// <param name="parentControl">parent control.</param>
        internal void SetParent(Control parentControl)
        {
            this.parent = parentControl;

            if (null != this.parent)
            {
                this.parentControlSize.SetSize(this.parent.Bounds.Width, this.parent.Bounds.Height);
            }
        }

        /// <summary>
        /// Assigns engine for control.
        /// </summary>
        /// <param name="engine">UI engine</param>
        internal void SetEngine(UIEngine engine)
        {
            this.uiEngine = engine;
        }

        #endregion

        #region sizes handling

        /// <summary>
        /// Helper function for setting control bounds.
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        internal void SetSize(int x, int y, int width, int height)
        {
            this.bounds.X = x;
            this.bounds.Y = y;

            if (width > 3)
            {
                this.bounds.Width = width;
            }

            if (height > 3)
            {
                this.bounds.Height = height;
            }
        }

        internal virtual void UpdateSize()
        {
            UpdateSizeSelf();
            UpdateSizeControls();
        }

        protected virtual void UpdateSizeSelf()
        {
            // anchor stuff
            if (null != this.parent)
            {
                //if (this.m_vBottomRightDif.X == 0)
                //{
                //	this.m_vBottomRightDif.X = this.m_pParent.bounds.Width - this.bounds.Width - this.bounds.X;
                //	this.m_vBottomRightDif.Y = this.m_pParent.bounds.Height - this.bounds.Height - this.bounds.Y;
                //}

                if (this.parentControlSize.Width == 0) // first time
                {
                    this.parentControlSize.Width = this.parent.bounds.Width;
                    this.parentControlSize.Height = this.parent.bounds.Height;
                }
                else
                {
                    int dx = this.parent.bounds.Width - this.parentControlSize.Width;
                    int dy = this.parent.bounds.Height - this.parentControlSize.Height;

                    if ((this.anchor & AnchorStyle.AnchorRight) > 0)
                    {
                        if ((this.anchor & AnchorStyle.AnchorLeft) > 0)
                        {
                            this.bounds.Width += dx;
                        }
                        else
                        {
                            this.bounds.X += dx;
                        }
                    }

                    if ((this.anchor & AnchorStyle.AnchorBottom) > 0)
                    {
                        if ((this.anchor & AnchorStyle.AnchorTop) > 0)
                        {
                            this.bounds.Height += dy;
                        }
                        else
                        {
                            this.bounds.Y += dy;
                        }
                    }

                    this.parentControlSize.Width = this.parent.bounds.Width;
                    this.parentControlSize.Height = this.parent.bounds.Height;
                }
            }

            if ((true == this.fillParent) && (null != this.parent))
            {
                SetSize(0, 0, this.parent.Bounds.Width, this.parent.Bounds.Height);
            }
        }

        protected virtual void UpdateSizeControls()
        {
            lock (this.childControls)
            {
                foreach (Control it in this.childControls)
                {
                    it.UpdateSize();
                }
            }
        }

		#endregion

		#region Focus handling

        /// <summary>
        /// Removes focus from this control.
        /// </summary>
        public virtual void ClearFocus()
        {
            if (true == this.HasFocus)
            {
                if (null != this.LostFocus)
                {
                    this.LostFocus(this, EventArgs.Empty);
                }
            }

            this.hasFocus = false;
            this.isMouseOver = false;

            lock (this.childControls)
            {
                foreach (Control control in this.childControls)
                {
                    if (null != control)
                    {
                        control.ClearFocus();
                    }
                }
            }

            this.drawOutline = false;
        }

        public virtual void OnFocus()
        {
            this.hasFocus = true;

            if (null != this.GotFocus)
            {
                this.GotFocus(this, EventArgs.Empty);
            }
        }

        #endregion

        #region I18N

        protected virtual void OnLanguageChanged()
        {
			if (false == this.needTranslation)
			{
				this.controlText = this.textReference;

				return;
			}

            if (null != this.uiEngine.Language)
			{
				String group = (null == this.Window) ? this.Name : this.Window.Name;

                this.controlText = this.uiEngine.Language.Translate("window." + group, this.textReference);
			}
			else
			{
				this.controlText = this.textReference;
			}
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Sets what mouse cursotr to draw at this moment.
        /// </summary>
        /// <param name="cursor">cursor type.</param>
        internal void UpdateCursor(MousePointers cursor)
        {
            if (false == this.IsInside(this.mousePosition.X, this.mousePosition.Y))
            {
                return;
            }

            this.Window.Desktop.SetMouseCursor(cursor);
        }

        /// <summary>
        /// Checks if coordinates are on control border.
        /// </summary>
        /// <param name="border">border corner of interest.</param>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        /// <returns>if control is on specfied border side.</returns>
        public virtual bool IsOnBorder(ContentAlignment border, int x, int y)
        {
            //int borderSize = 4;
            int bs = 2;

            switch (border)
            {
                case ContentAlignment.BottomCenter:
                    return (x >= this.bounds.X + bs) && (x <= this.bounds.X + this.bounds.Width - bs) && (y >= this.bounds.Y + this.bounds.Height - bs) && (y <= this.bounds.Y + this.bounds.Height);
                case ContentAlignment.BottomLeft:
                    return (x >= this.bounds.X) && (x <= this.bounds.X + bs) && (y >= this.bounds.Y + this.bounds.Height - bs) && (y <= this.bounds.Y + this.bounds.Height);
                case ContentAlignment.BottomRight:
                    return (x >= this.bounds.X + this.bounds.Width - bs) && (x <= this.bounds.X + this.bounds.Width) && (y >= this.bounds.Y + this.bounds.Height - bs) && (y <= this.bounds.Y + this.bounds.Height);
                case ContentAlignment.MiddleLeft:
                    return (x >= this.bounds.X) && (x <= this.bounds.X + bs) && (y >= this.bounds.Y + bs) && (y <= this.bounds.Y + this.bounds.Height - bs);
                case ContentAlignment.MiddleRight:
                    return (x >= this.bounds.X + this.bounds.Width - bs) && (x <= this.bounds.X + this.bounds.Width) && (y >= this.bounds.Y + bs) && (y <= this.bounds.Y + this.bounds.Height - bs);
                case ContentAlignment.TopCenter:
                    return (x >= this.bounds.X + bs) && (x <= this.bounds.X + this.bounds.Width - bs) && (y >= this.bounds.Y) && (y <= this.bounds.Y + bs);
                case ContentAlignment.TopLeft:
                    return (x >= this.bounds.X) && (x <= this.bounds.X + bs) && (y >= this.bounds.Y) && (y <= this.bounds.Y + bs);
                case ContentAlignment.TopRight:
                    return (x >= this.bounds.X + this.bounds.Width - bs) && (x <= this.bounds.X + this.bounds.Width) && (y >= this.bounds.Y) && (y <= this.bounds.Y + bs);
                case ContentAlignment.MiddleCenter:
                    return (x >= this.bounds.X + bs) && (x <= this.bounds.X + this.bounds.Width - bs) && (y >= this.bounds.Y + bs) && (y <= this.bounds.Y + ((this.bounds.Height > 20) ? 20 : this.bounds.Height) - bs);
                default:
                    return this.bounds.IsInside(x, y);
            }
        }

        /// <summary>
        /// Changes mouse cursor based on it's position over control borders.
        /// </summary>
        /// <param name="X">X mouse position.</param>
        /// <param name="Y">Y mouse position.</param>
        private void SetCursorByBorder(int x, int y)
        {
            if (true == IsOnBorder(ContentAlignment.BottomCenter, x, y))
            {
                UpdateCursor(MousePointers.PointerVResize);
            }
            else if (true == IsOnBorder(ContentAlignment.TopCenter, x, y))
            {
                UpdateCursor(MousePointers.PointerVResize);
            }
            else if (true == IsOnBorder(ContentAlignment.TopRight, x, y))
            {
                UpdateCursor(MousePointers.PointerResize1);
            }
            else if (true == IsOnBorder(ContentAlignment.BottomRight, x, y))
            {
                UpdateCursor(MousePointers.PointerResize2);
            }
            else if (true == IsOnBorder(ContentAlignment.BottomLeft, x, y))
            {
                UpdateCursor(MousePointers.PointerResize1);
            }
            else if (true == IsOnBorder(ContentAlignment.MiddleLeft, x, y))
            {
                UpdateCursor(MousePointers.PointerHResize);
            }
            else if (true == IsOnBorder(ContentAlignment.MiddleRight, x, y))
            {
                UpdateCursor(MousePointers.PointerHResize);
            }
            else if (true == IsOnBorder(ContentAlignment.TopLeft, x, y))
            {
                UpdateCursor(MousePointers.PointerResize2);
            }
            else if (true == IsOnBorder(ContentAlignment.MiddleCenter, x, y))
            {
                UpdateCursor(MousePointers.PointerMove);
            }
        }

        /// <summary>
        /// Returns if point is inside control.
        /// </summary>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        /// <returns>is inside</returns>
        internal virtual bool IsInside(int x, int y)
        {
            if (null == this.window)
            {
                return this.bounds.IsInside(x, y);
            }
            else
            {
                if (true == this.bounds.IsInside(x, y))
                {
                    if (null != this.parent)
                    {
                        return this.parent.IsInside(x + this.parent.Bounds.X, y + this.parent.Bounds.Y + this.parent.topOffset);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Transform screen coordinates to window coordinates. the distance from top left corner of window.
        /// </summary>
        /// <param name="screenX">X coordinate on screen.</param>
        /// <returns>translated coordiante</returns>
        internal virtual int TranslateX(int screenX)
        {
            if (null != this.parent)
            {
                return this.parent.TranslateX(screenX - this.bounds.X);
            }

            return screenX;
        }

        /// <summary>
        /// Transform screen coordinates to window coordinates. the distance from top left corner of window.
        /// </summary>
        /// <param name="screenY">Y coordinate on screen.</param>
        /// <returns>translated coordiante</returns>
        internal virtual int TranslateY(int screenY)
        {
            if (null != this.parent)
            {
                return this.parent.TranslateY(screenY - this.bounds.Y);
            }

            return screenY;
        }

        /// <summary>
        /// Prints control name and type.
        /// </summary>
        /// <returns>control name and type</returns>
        public override string ToString()
        {
			return this.Name + ": " + this.Type + ": " + this.Text;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Registers controls' properties.
        /// </summary>
        protected virtual void AddProperties()
        {
            const String groupBackground = "Background";

            AddProperty(new PropertyString(this.ClickScript, "onClick", "Scripting", "onClick", (x) => { this.ClickScript = x; }, () => { return this.ClickScript; }));
            AddProperty(new PropertyString(this.Name, "name", "Scripting", "name", (x) => { this.Name = x; }, () => { return this.Name; }));
            AddProperty(new PropertyInteger(this.Bounds.X, "x", "Size", "X", (x) => { this.Bounds.X = x; }, () => { return this.Bounds.X; }));
            AddProperty(new PropertyInteger(this.Bounds.Y, "y", "Size", "Y", (y) => { this.Bounds.Y = y; }, () => { return this.Bounds.Y; }));
            AddProperty(new PropertyInteger(this.Bounds.Width, "width", "Size", "Width", (w) => { this.Bounds.Width = w; }, () => { return this.Bounds.Width; }));
            AddProperty(new PropertyInteger(this.Bounds.Height, "height", "Size", "Height", (h) => { this.Bounds.Height = h; }, () => { return this.Bounds.Height; }));
            AddProperty(new PropertyInteger(this.MinSize.Width, "minWidth", "Size", "minWidth", (x) => { this.MinSize.Width = x; }, () => { return this.MinSize.Width; }));
            AddProperty(new PropertyInteger(this.MinSize.Height, "minHeight", "Size", "minHeight", (x) => { this.MinSize.Height = x; }, () => { return this.MinSize.Height; }));
            AddProperty(new PropertyAnchor(this.Anchor, "anchor", "Size", "anchor", (x) => { this.Anchor = x; }, () => { return this.Anchor; }));
            AddProperty(new PropertyBoolean(this.FillParent, "fillParent", "Size", "fillParent", (x) => { this.FillParent = x; }, () => { return this.FillParent; }));

            AddProperty(new PropertyString(this.Text, "text", "Text", "text", (x) => { this.Text = x; }, () => { return this.Text; }));

            AddFontProperties();

            AddProperty(new PropertyBoolean(this.NeedTranslation, "needTranslation", "Text", "needTranslation", (x) => { this.NeedTranslation = x; }, () => { return this.NeedTranslation; }));
            AddProperty(new PropertyColor(this.TextColor, "text.color", "Text", "text.color", (x) => { this.TextColor = x; }, () => { return this.TextColor; }));
            AddProperty(new PropertyColor(this.SelectedTextColor, "text.color.selected", "Text", "text.color.selected", (x) => { this.SelectedTextColor = x; }, () => { return this.SelectedTextColor; }));
            AddProperty(new PropertyList<ContentAlignment>(this.TextAlignment, "text.align", "Text", "text.align", (x) => { this.TextAlignment = x; }, () => { return this.TextAlignment; }));
            AddProperty(new PropertyInteger(this.TextOffset.X, "text.shift.x", "Text", "text.shift.X", (x) => { this.TextOffset.X = x; }, () => { return this.TextOffset.X; }));
            AddProperty(new PropertyInteger(this.TextOffset.Y, "text.shift.y", "Text", "text.shift.Y", (y) => { this.TextOffset.Y = y; }, () => { return this.TextOffset.Y; }));

            AddProperty(new PropertyColor(this.BorderColor, "borderColor", "Background", "borderColor", (x) => { this.BorderColor = x; }, () => { return this.BorderColor; }));
            AddProperty(new PropertyColor(this.BorderColorDark1, "borderColorDark1", "Background", "borderColorDark1", (x) => { this.BorderColorDark1 = x; }, () => { return this.BorderColorDark1; }));
            AddProperty(new PropertyColor(this.BorderColorDark2, "borderColorDark2", "Background", "borderColorDark2", (x) => { this.BorderColorDark2 = x; }, () => { return this.BorderColorDark2; }));
            AddProperty(new PropertyColor(this.BorderColorLight1, "borderColorLight1", "Background", "borderColorLight1", (x) => { this.BorderColorLight1 = x; }, () => { return this.BorderColorLight1; }));
            AddProperty(new PropertyColor(this.BorderColorLight2, "borderColorLight2", "Background", "borderColorLight2", (x) => { this.BorderColorLight2 = x; }, () => { return this.BorderColorLight2; }));
            AddProperty(new PropertyColor(this.BackColor, "backColor", "Background", "backColor", (x) => { this.BackColor = x; }, () => { return this.BackColor; }));
            AddProperty(new PropertyColor(this.SelectedBackgroundColor, "backColor.selected", "Background", "backColor.selected", (x) => { this.SelectedBackgroundColor = x; }, () => { return this.SelectedBackgroundColor; }));
            AddProperty(new PropertyFloat(this.Opacity, "opacity", groupBackground, "opacity", (x) => { this.Opacity = x; }, () => { return this.Opacity; }));
            AddProperty(new PropertyList<BorderStyle>(this.Border, "borderStyle", "Background", "borderStyle", (x) => { this.Border = x; }, () => { return this.Border; }));
            AddProperty(new PropertyImage(this.BackImage, "backImage", "Background", "backImage", (x) => { this.BackImage = x; }, () => { return this.BackImage; }));
            AddProperty(new PropertyColor(this.BackImageColor, "backImageColor", "Background", "backImageColor", (x) => { this.BackImageColor = x; }, () => { return this.BackImageColor; }));
            AddProperty(new PropertyList<ImageLayout>(this.BackImageLayout, "backImageLayout", "Background", "backImageLayout", (x) => { this.BackImageLayout = x; }, () => { return this.BackImageLayout; }));

            AddIconProperties();

            AddProperty(new PropertyList<MousePointers>(this.Cursor, "cursor", "Mouse", "cursor", (x) => { this.Cursor = x; }, () => { return this.Cursor; }));
            AddProperty(new PropertyBoolean(this.Skinned, "skinned", "Rendering", "skinned", (x) => { this.Skinned = x; }, () => { return this.Skinned; }));

            AddProperty(new PropertyBoolean(this.TabStop, "tabStop", "Navigation", "tabStop", (x) => { this.TabStop = x; }, () => { return this.TabStop; }));
            AddProperty(new PropertyInteger(this.TabIndex, "tabIndex", "Navigation", "tabIndex", (x) => { this.TabIndex = x; }, () => { return this.TabIndex; }));

            this.propertiesAreLoaded = true;
        }

        /// <summary>
        /// Add controls text rendering font properties.
        /// </summary>
        protected virtual void AddFontProperties()
        {
            String group = "Text";

            AddProperty(new PropertyFont(this.Engine, this.FontInfo.Name, "font.name", group, "font.name", (x) => { this.FontInfo.Name = x; }, () => { return this.FontInfo.Name; }));
            AddProperty(new PropertyBoolean(this.FontInfo.Bold, "font.bold", group, "font.bold", (x) => { this.FontInfo.Bold = x; }, () => { return this.FontInfo.Bold; }));
            AddProperty(new PropertyBoolean(this.FontInfo.Italic, "font.italic", group, "font.italic", (x) => { this.FontInfo.Italic = x; }, () => { return this.FontInfo.Italic; }));
            AddProperty(new PropertyInteger(this.FontInfo.Size, "font.size", group, "font.size", (x) => { this.FontInfo.Size = x; }, () => { return this.FontInfo.Size; }));
        }

        /// <summary>
        /// Adds icons' properties.
        /// </summary>
        protected virtual void AddIconProperties()
        {
            const String groupIcon = "Icon";

            AddProperty(new PropertyList<IconSize>(this.IconSize, "icon.size", "Icon", "icon.size", (x) => { this.IconSize = x; }, () => { return this.IconSize; }));
            AddProperty(new PropertyList<ContentAlignment>(this.IconAlignment, "icon.align", "Icon", "icon.align", (x) => { this.IconAlignment = x; }, () => { return this.IconAlignment; }));
            AddProperty(new PropertyInteger(this.IconImageOffset.X, "icon.offset.x", groupIcon, "icon.offset.X", (x) => { this.IconImageOffset.X = x; }, () => { return this.IconImageOffset.X; }));
            AddProperty(new PropertyInteger(this.IconImageOffset.Y, "icon.offset.y", groupIcon, "icon.offset.Y", (y) => { this.IconImageOffset.Y = y; }, () => { return this.IconImageOffset.Y; }));
            AddProperty(new PropertyImage(this.Icon, "icon", groupIcon, "icon displayed on the control", (x) => { this.Icon = x; }, () => { return this.Icon; }));
            AddProperty(new PropertyImage(this.IconSelected, "icon.selected", groupIcon, "icon.selected", (x) => { this.IconSelected = x; }, () => { return this.IconSelected; }));
            AddProperty(new PropertyColor(this.IconColor, "icon.color", groupIcon, "icon.color", (x) => { this.IconColor = x; }, () => { return this.IconColor; }));
            AddProperty(new PropertyFloat(this.IconScale, "icon.scale", groupIcon, "enlarges icon size", (x) => { this.IconScale = x; }, () => { return this.IconScale; }));
        }

        /// <summary>
        /// Adds property in controls properties list.
        /// </summary>
        /// <param name="property">property to add.</param>
        protected void AddProperty(Property property)
        {
			if (null != property)
			{
				this.properties.Add(property);
			}
        }

        /// <summary>
        /// Control properties list.
        /// </summary>
        public IEnumerable<Property> Properties
        {
            get
            {
                if (false == this.propertiesAreLoaded)
                {
                    AddProperties();

                    this.propertiesAreLoaded = true;
                }

                return this.properties;
            }
        }

        /// <summary>
        /// Removes control properties objects. Called after loading from xml files is finised.
        /// </summary>
        protected void ClearProperties()
        {
			this.properties.Clear();
        }

        #endregion

        #region Rendering

        /// <summary>
        /// Renders control at the specified location
        /// </summary>
        protected virtual void Render(Graphics graphics, int x, int y)
        {
            if (true == this.visible)
            {
                RenderBackground(graphics, x, y);
                RenderBorder(graphics, x, y);
                RenderIcon(graphics, x, y);
                RenderText(graphics, x, y);
                RenderControls(graphics, x, y);
            }
        }

        internal virtual void RenderInternal(Graphics graphics, int x, int y)
        {
            Render(graphics, x, y);
        }

        /// <summary>
        /// Renders control border.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X offset.</param>
        /// <param name="Y">Y offset.</param>
        protected virtual void RenderBorder(Graphics graphics, int x, int y)
        {
            if (false == this.skinned)
            {
                RenderBorderXYWH(graphics, x + this.Bounds.X, y + this.Bounds.Y, this.Bounds.Width, this.Bounds.Height, this.Border);
            }
            else if (BorderStyle.None != this.borderStyle)
            {
                if (null == this.borderImage)
                {
                    ControlSettings settings = this.Window.Desktop.Theme.GetControlSettings(this.Type);

                    String borderImageFileName = this.Window.Desktop.Theme.ThemeFolder + settings.BordersFilePrefix + "border_" + EnumUtils.ToString(this.borderStyle);

                    this.borderImageWidth = settings.BorderWidth;

                    this.borderImage = this.Engine.CreateImage(borderImageFileName);
                }

                if (null != this.borderImage)
                {
                    int px = x + this.Bounds.X;
                    int py = y + this.Bounds.Y;
                    int bs = this.borderImageWidth;
                    float w = (float)this.borderImage.Width;
                    float h = (float)this.borderImage.Height;
                    float ss_h = (float)this.borderImageWidth / h;
                    float ss_w = (float)this.borderImageWidth / w;
                    int of = 2;

                    graphics.SetColor(this.BorderColor);

                    graphics.DrawImage(px - bs + of, py - bs + of, bs, bs, this.borderImage, 0.0f, 0.0f, ss_w, ss_h, false);
                    graphics.DrawImage(px - bs + of, py + of, bs, this.bounds.Height - 2 * of, this.borderImage, 0.0f, ss_h, ss_w, 1.0f - ss_h, false);
                    graphics.DrawImage(px - bs + of, py - of + this.bounds.Height, bs, bs, this.borderImage, 0.0f, 1.0f - ss_h, ss_w, 1.0f, false);
                    graphics.DrawImage(px + of, py - bs + of, this.bounds.Width - 2 * of, bs, this.borderImage, ss_w, 0.0f, 1.0f - ss_w, ss_h, false);
                    graphics.DrawImage(px + of, py - of + this.bounds.Height, this.bounds.Width - 2 * of, bs, this.borderImage, ss_w, 1.0f - ss_h, 1.0f - ss_w, 1.0f, false);
                    graphics.DrawImage(px + this.bounds.Width - of, py - bs + of, bs, bs, this.borderImage, 1.0f - ss_w, 0.0f, 1.0f, ss_h, false);
                    graphics.DrawImage(px + this.bounds.Width - of, py - of + this.bounds.Height, bs, bs, this.borderImage, 1.0f - ss_w, 1.0f - ss_h, 1.0f, 1.0f, false);
                    graphics.DrawImage(px - of + this.bounds.Width, py + of, bs, this.bounds.Height - 2 * of, this.borderImage, 1.0f - ss_w, ss_h, 1.0f, 1.0f - ss_h, false);
                }
            }
        }

        internal virtual void RenderBorderVertexXYWH(Graphics graphics, int x, int y, int w, int h, bool bDouble, Color colorLight1, Color colorLight2, Color colorDark1, Color colorDark2)
        {
            graphics.SetColor(colorLight1);
            graphics.DrawRectangle(x, y, w - 1, 1);
            graphics.DrawRectangle(x, y, 1, h - 1);
            graphics.SetColor(colorDark1);
            graphics.DrawRectangle(x + w - 1, y, 1, h);
            graphics.DrawRectangle(x, y + h - 1, w, 1);

            if (true == bDouble)
            {
                graphics.SetColor(colorLight2);
                graphics.DrawRectangle(x + 1, y + 1, w - 1 - 2, 1);
                graphics.DrawRectangle(x + 1, y + 1, 1, h - 1 - 2);
                graphics.SetColor(colorDark2);
                graphics.DrawRectangle(x - 1 + w - 1, y + 1, 1, h - 2);
                graphics.DrawRectangle(x + 1, y - 1 + h - 1, w - 2, 1);
            }
        }

        internal virtual void RenderBackgroundXYWH(Graphics graphics, int x, int y, int w, int h)
        {
            if (true == this.drawOutline)
            {
                graphics.SetColor(this.Window.Desktop.Theme.Colors.HighlightBorder, this.Opacity);
                graphics.DrawRectangle(x - 2, y - 2, w + 4, h + 4);
            }

            if ((true == this.isMouseOver) && (Colors.None != this.colorBackSelected))
            {
                graphics.SetColor(this.colorBackSelected, this.Opacity);

                if (!(this.colorBackSelected == Colors.None))
                {
                    graphics.DrawRectangle(x, y, w, h);
                }
            }
            else
            {
                if (null != this.BackColor)
                {
                    graphics.SetColor(this.BackColor, this.Opacity);

                    if (!(this.backColor == Colors.None))
                    {
                        graphics.DrawRectangle(x, y, w, h);
                    }
                }
            }

            if (null != this.backgroundImage)
            {
                this.backgroundImage.Render(graphics, x, y, w, h, this.BackColor, this.Opacity, this.BackImageLayout);
            }
        }

        protected virtual void RenderControls(Graphics graphics, int x, int y)
        {
            RenderControls(graphics, x, y, false);
        }

        protected void RenderControls(Graphics graphics, int x, int y, bool clip)
        {
            lock (this.childControls)
            {
                foreach (Control control in this.childControls)
                {
                    if (true == clip)
                    {
                        if (control.X > this.Width)
                        {
                            continue;
                        }

                        if (control.Y > this.Height)
                        {
                            continue;
                        }

                        if (control.X + control.Width < this.X)
                        {
                            continue;
                        }

                        if (control.Y + control.Height < this.Y)
                        {
                            continue;
                        }
                    }

                    control.Render(graphics, x + this.bounds.X, y + this.bounds.Y + this.topOffset);
                }
            }
        }

        protected virtual void RenderSelection(Graphics graphics, int x, int y)
        {
            if (Colors.None != this.Window.Desktop.Theme.Colors.Highlight)
            {
                graphics.SetColor(this.Window.Desktop.Theme.Colors.Highlight, 0.5f);
                graphics.DrawRectangle(x + this.Bounds.X + 1, y + this.Bounds.Y + 1, this.Bounds.Width - 2, this.Bounds.Height - 2);
            }

            if (Colors.None != this.Window.Desktop.Theme.Colors.HighlightBorder)
            {
                graphics.SetColor(this.Window.Desktop.Theme.Colors.HighlightBorder, 0.5f);
                graphics.DrawBox(x + this.Bounds.X, y + this.Bounds.Y, this.Bounds.Width, this.Bounds.Height);
            }
        }

        protected virtual void RenderBackground(Graphics graphics, int x, int y)
        {
            RenderBackgroundXYWH(graphics, x + this.bounds.X, y + this.bounds.Y, this.bounds.Width, this.bounds.Height);
        }

        protected virtual void RenderText(Graphics graphics, int x, int y)
        {
            RenderText(graphics, x, y, this.textAlignment, this.Text);
        }

        protected virtual void RenderText(Graphics graphics, int x, int y, ContentAlignment textAlignment, String text)
        {
            if ((null != this.font.Font) && (null != text) && (0 != text.Length))
            {
                int len = text.Length;

                if (this.bounds.Width - this.borderSize < 5)
                {
                    return;
                }

                while (this.font.Font.TextLength(text, 0, len) > this.bounds.Width - this.borderSize)
                {
                    len--;

                    if (len < 0)
                    {
                        return;
                    }
                }

                int tw = this.font.Font.TextLength(text, 0, len);
                int th = this.font.Font.TextHeight(text, 0, len);
                int w = this.bounds.Width - 4;
                int h = this.bounds.Height - 2;

                int ofX = 2 + this.textOffset.X;
                int ofY = 1 + this.textOffset.Y;

                switch (textAlignment)
                {
                    case ContentAlignment.BottomCenter:
                        ofX += (w - tw) / 2;
                        ofY += (h - th);
                        break;
                    case ContentAlignment.BottomLeft:
                        ofY += (h - th);
                        break;
                    case ContentAlignment.BottomRight:
                        ofX += w - tw;
                        ofY += (h - th);
                        break;
                    case ContentAlignment.MiddleCenter: // +
                        ofX += (w - tw) / 2;
                        ofY += (h - th) / 2;
                        break;
                    case ContentAlignment.MiddleLeft: // +
                        ofY += (h - th) / 2;
                        break;
                    case ContentAlignment.MiddleRight:
                        ofY += (h - th) / 2;
                        ofX += w - tw;
                        break;
                    case ContentAlignment.TopCenter:
                        ofX += (w - tw) / 2;
                        break;
                    case ContentAlignment.TopLeft:
                        break;
                    case ContentAlignment.TopRight:
                        ofX += w - tw;
                        break;
                    default:
                        break;
                }

                if ((true == this.isMouseOver) && (Colors.None != this.colorTextSelected))
                {
                    graphics.SetColor(this.colorTextSelected, 1.0f);
                }
                else
                {
                    graphics.SetColor(this.textColor, 1.0f);
                }

                this.font.Font.DrawText(graphics, ofX + x + this.bounds.X, ofY + y + this.bounds.Y, text, 0, len);
            }
        }

        protected internal IImage GetImage(ref String imageName, ref IImage image)
        {
            if (null != image)
            {
                return image;
            }

            if ((null == imageName) || (0 == imageName.Length))
            {
                return null;
            }

            image = this.uiEngine.CreateImage(imageName);

            if (null == image)
            {
                imageName = "";
            }

            return image;
        }

        protected virtual void RenderIcon(Graphics graphics, int x, int y)
        {
            ImageObject image = this.icon;

            if ((true == this.isMouseOver) && (null != this.selectedIcon))
            {
                image = this.selectedIcon;
            }

            if (null == image)
            {
                return;
            }

            int offX = this.iconImageOffset.X;
            int offY = this.iconImageOffset.Y;

            int w = image.Width;
            int h = image.Height;

            if (IconSize.IconSmall == this.iconSize)
            {
                w = 16;
                h = 16;
            }

            w = (int)(w * this.iconScale);
            h = (int)(h * this.iconScale);

            switch (this.iconAlignment)
            {
                case ContentAlignment.BottomCenter:
                    offX += (this.bounds.Width - w) / 2;
                    break;
                case ContentAlignment.BottomLeft:
                    break;
                case ContentAlignment.BottomRight:
                    break;
                case ContentAlignment.MiddleCenter:
                    offX += (this.bounds.Width - w) / 2;
                    offY += (this.bounds.Height - h) / 2;
                    break;
                case ContentAlignment.MiddleLeft:
                    offY += (this.bounds.Height - h) / 2;
                    break;
                case ContentAlignment.MiddleRight:
                    offY += (this.bounds.Height - h) / 2;
                    break;
                case ContentAlignment.TopCenter:
                    offX += (this.bounds.Width - w) / 2;
                    break;
                case ContentAlignment.TopLeft:
                    break;
                case ContentAlignment.TopRight:
                    break;
                default:
                    break;
            }

            image.Render(graphics, offX + this.Bounds.X + x, offY + this.Bounds.Y + y + this.topOffset, w, h, this.iconColor, this.Opacity, ImageLayout.ImageLayoutNone);
        }

        /// <summary>
        /// Renders control border at X, Y position with specified bounds.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X offset.</param>
        /// <param name="Y">Y offset.</param>
        /// <param name="Width">border Width.</param>
        /// <param name="Height">border Height.</param>
        /// <param name="borderStyle">border style.</param>
        protected void RenderBorderXYWH(Graphics graphics, int x, int y, int width, int height, BorderStyle borderStyle)
        {
			if (false == this.skinned)
			{
				switch (borderStyle)
				{
                    case BorderStyle.Flat:
						graphics.SetColor(this.borderColorDark2);
						graphics.DrawBox(x, y, width, height);
						break;
                    case BorderStyle.BorderFlatDouble:
						graphics.SetColor(this.borderColorDark2);
						graphics.DrawBox(x, y + y, this.bounds.Width, height);
						graphics.DrawBox(x + 1, y + 1, width - 2, height - 2);
						break;
                    case BorderStyle.BorderRaised:
						RenderBorderVertexXYWH(graphics, x, y, width, height, false, this.borderColorLight1, this.borderColorLight2, this.borderColorDark1, this.borderColorDark2);
						break;
                    case BorderStyle.Lowered:
						RenderBorderVertexXYWH(graphics, x, y, width, height, false, this.borderColorDark2, Colors.None, Colors.None, this.borderColorLight1);
						break;
                    case BorderStyle.BorderRaisedDouble:
						RenderBorderVertexXYWH(graphics, x, y, width, height, true, this.borderColorLight1, this.borderColorLight2, this.borderColorDark1, this.borderColorDark2);
						break;
                    case BorderStyle.BorderLoweredDouble:
						RenderBorderVertexXYWH(graphics, x, y, width, height, true, this.borderColorDark2, this.borderColorDark1, this.borderColorLight2, this.borderColorLight1);
						break;
					default:
						break;
				}

				return;
			}
            else  if (BorderStyle.None != borderStyle)
			{
/*				if (null == this.m_pBorders[0])
				{
					ControlSettings settings = UIEngine.GetInstance().GetThemes().GetControlSettings(this.m_type);

                    String folder = null;

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
				render.DrawImage(X + 2, Y - m_pBorders[3].GetHeight() + 2, Width - 4, m_pBorders[3].GetHeight(), m_pBorders[3]);
				render.DrawImage(X + 2, Y - 2 + Height, Width - 4, m_pBorders[4].GetHeight(), m_pBorders[4]);
				render.DrawImage(X - 2 + Width, Y + 2, m_pBorders[7].GetWidth(), Height - 4, m_pBorders[7]);
				render.DrawImage(X - m_pBorders[0].GetWidth() + 2, Y - m_pBorders[0].GetHeight() + 2, m_pBorders[0].GetWidth(), m_pBorders[0].GetHeight(), m_pBorders[0]);
				render.DrawImage(X - m_pBorders[2].GetWidth() + 2, Y - 2 + Height, m_pBorders[2].GetWidth(), m_pBorders[2].GetHeight(), m_pBorders[2]);
				render.DrawImage(X + Width - 2, Y - m_pBorders[5].GetHeight() + 2, m_pBorders[5].GetWidth(), m_pBorders[5].GetHeight(), m_pBorders[5]);
				render.DrawImage(X + Width - 2, Y - 2 + Height, m_pBorders[6].GetWidth(), m_pBorders[6].GetHeight(), m_pBorders[6]); */

                if (null == this.borderImage)
                {
                    ControlSettings settings = this.Window.Desktop.Theme.GetControlSettings(this.Type);

                    String borderFileName = this.Window.Desktop.Theme.ThemeFolder + settings.BordersFilePrefix + "border_" + EnumUtils.ToString(this.borderStyle);

                    this.borderImageWidth = settings.BorderWidth;

                    this.borderImage = this.uiEngine.CreateImage(borderFileName);
                }

                if (null != this.borderImage)
                {

                    int px = x;
                    int py = y;
                    int bs = this.borderImageWidth;
                    float w = (float)this.borderImage.Width;
                    float h = (float)this.borderImage.Height;
                    float ss_h = (float)this.borderImageWidth / h;
                    float ss_w = (float)this.borderImageWidth / w;
                    int of = 2;

                    graphics.SetColor(this.borderColor);

                    graphics.DrawImage(px - bs + of, py - bs + of, bs, bs, this.borderImage, 0.0f, 0.0f, ss_w, ss_h, false);
                    graphics.DrawImage(px - bs + of, py + of, bs, width - 2 * of, this.borderImage, 0.0f, ss_h, ss_w, 1.0f - ss_h, false);
                    graphics.DrawImage(px - bs + of, py - of + height, bs, bs, this.borderImage, 0.0f, 1.0f - ss_h, ss_w, 1.0f, false);
                    graphics.DrawImage(px + of, py - bs + of, width - 2 * of, bs, this.borderImage, ss_w, 0.0f, 1.0f - ss_w, ss_h, false);
                    graphics.DrawImage(px + of, py - of + height, width - 2 * of, bs, this.borderImage, ss_w, 1.0f - ss_h, 1.0f - ss_w, 1.0f, false);
                    graphics.DrawImage(px + width - of, py - bs + of, bs, bs, this.borderImage, 1.0f - ss_w, 0.0f, 1.0f, ss_h, false);
                    graphics.DrawImage(px + width - of, py - of + height, bs, bs, this.borderImage, 1.0f - ss_w, 1.0f - ss_h, 1.0f, 1.0f, false);
                    graphics.DrawImage(px - of + width, py + of, bs, height - 2 * of, this.borderImage, 1.0f - ss_w, ss_h, 1.0f, 1.0f - ss_h, false);
                }
			}
        }

        #endregion

        #region Persistence

        /// <summary>
        /// Persistence. Saves object info into xml stream.
        /// </summary>
        /// <param name="writer">stream to write to</param>
        protected virtual void WriteBegin(IXmlWriter writer)
        {
            writer.OpenTag(this.Type);
        }

        /// <summary>
        /// Persistence. Saves object info into xml stream.
        /// </summary>
        /// <param name="writer">stream to write to</param>
        protected virtual void WriteEnd(IXmlWriter writer)
        {
            writer.CloseTag();
        }

        /// <summary>
        /// Persistence. Saves object info into xml stream.
        /// </summary>
        /// <param name="writer">stream to write to</param>
        protected virtual void WriteAttributes(IXmlWriter writer)
        {
            foreach (Property property in this.Properties)
            {
                if (false == property.IsDefault())
                {
                    writer.WriteAttribute(property.Name, property.ToString());
                }
            }
        }

        /// <summary>
        /// Persistence. Saves object info into xml stream.
        /// </summary>
        /// <param name="writer">stream to write to.</param>
        /// <param name="controls">controls to save.</param>
        protected void WriteControls(IXmlWriter writer, IEnumerable<Control> controls)
        {
            foreach (Control control in controls)
            {
                if ((control.CreationFlag & CreationFlag.InternalControl) == 0)
                {
                    control.WriteBegin(writer);
                    control.WriteAttributes(writer);
                    control.WriteControls(writer);
                    control.WriteEnd(writer);
                }
            }
        }

        /// <summary>
        /// Persistence. Saves object info into xml stream.
        /// </summary>
        /// <param name="writer">stream to write to</param>
        protected virtual void WriteControls(IXmlWriter writer)
        {
            WriteControls(writer, this.Controls);
        }

        /// <summary>
        /// Loads control from xml file.
        /// </summary>
        /// <param name="rootElement">parent xml element.</param>
        protected virtual void LoadControls(IXmlElement rootElement)
        {
            foreach (IXmlElement element in rootElement.Elements)
            {
                String strControlType = element.Name;

                if (true == CanContainControl(strControlType))
                {
                    Control control = CreateControl(strControlType, this.CreationFlag | CreationFlag.NeedLoading);

                    if (null != control)
                    {
                        control.LoadBegin(element);
                        control.LoadAttributes(element);
                        control.LoadControls(element);
                        control.LoadEnd(element);

                        AddControl(control);
                    }
                }
            }
        }

        /// <summary>
        /// Loads controls' attributes from xml document.
        /// </summary>
        /// <param name="element">xml element with control attributes.</param>
        protected virtual void LoadAttributes(IXmlElement element)
        {
            foreach (Property property in this.Properties)
            {
                property.FromString(element.GetAttributeValue(property.Name, property.ToString()), this.Window.Desktop.Theme);
            }
        }

        /// <summary>
        /// Finishes loading control from xml file.
        /// </summary>
        /// <param name="rootElement">parent xml element.</param>
        protected virtual void LoadEnd(IXmlElement rootElement)
        {
            OnLanguageChanged();

            if ((this.CreationFlag & CreationFlag.NeedSaving) == 0)
            {
                ClearProperties();
            }
        }

        /// <summary>
        /// Begins loading control properties from xml.
        /// </summary>
        /// <param name="rootElement">control rool xml element.</param>
        protected virtual void LoadBegin(IXmlElement rootElement)
        {
        }

        #endregion

        #region Mouse handling

        /// <summary>
        /// Handles click on control event.
        /// </summary>
        /// <param name="X">mouse click X position.</param>
        /// <param name="Y">mouse click Y position.</param>
        protected virtual void OnClick(int x, int y)
        {
            IScriptsHandler scriptsHandler = this.uiEngine.ScriptsHandler;

            if ((null != this.ClickScript) && (0 != this.ClickScript.Length) && (null != scriptsHandler))
            {
                // little preprocessing
                if ((this.ClickScript.Length > 24) && (this.ClickScript.StartsWith("thw.uiengine.loadWindow ") == true))
                {
                    this.Window.Desktop.NewWindow(CreationFlag.NeedLoading | CreationFlag.NeedSaving, this.ClickScript.Substring(24));
                }
                else if ((this.ClickScript.Length > 34) && (this.ClickScript.StartsWith("thw.uiengine.loadRegisteredWindow ") == true))
                {
                    this.Window.Desktop.NewRegisteredWindow(this.ClickScript.Substring(34));
                }
                else
                {
                    scriptsHandler.OnUIEvent(this.ClickScript, this);
                }
            }
        }

        /// <summary>
        /// Handles click on control event, passed from window.
        /// </summary>
        /// <param name="X">mouse click X position.</param>
        /// <param name="Y">mouse click Y position.</param>
        internal void OnClickInternal(int x, int y)
        {
            OnClick(x, y);
        }

        /// <summary>
        /// Handles mouse move over control event.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        protected virtual void OnMouseMove(int x, int y)
        {
			this.mousePosition.SetCoords(x, y);

			if (true == this.moveable)
			{
				SetCursorByBorder(x, y);
			}
			else
			{
				if ( (x >= this.bounds.X) && (x <= this.bounds.X + this.bounds.Width) && (y >= this.bounds.Y) && (y <= this.bounds.Y + this.bounds.Height) )
				{
					this.isMouseOver = true;
					UpdateCursor(this.cursor);
				}
				else
				{
					this.isMouseOver = false;
				}
			}

            if (((this.controlState & ControlState.ControlDragging) == 0) && ((this.controlState & ControlState.ControlResizing) == 0))
			{
				OnMouseMoveControls(x, y);
			}

            if ((this.controlState & ControlState.ControlDragging) != 0)
			{
				SetSize(this.originalBounds.X + (x - this.mousePressPosition.X), this.originalBounds.Y + (y - this.mousePressPosition.Y), this.bounds.Width, this.bounds.Height);
			}
            else if ((this.controlState & ControlState.ControlResizing) != 0)
			{
				int dx = (this.mousePressPosition.X - x);
				int dy = (this.mousePressPosition.Y - y);

				int nW = this.originalBounds.Width - dx;
				int nH = this.originalBounds.Height - dy;
				int nX = this.bounds.X;
				int nY = this.bounds.Y;

                if ((this.controlState & ControlState.ControlResizeHoriz) == 0)
				{
					nW = this.originalBounds.Width;
				}

                if ((this.controlState & ControlState.ControlResizeVert) == 0)
				{
					nH = this.originalBounds.Height;
				}

                if ((this.controlState & ControlState.ControlResizeLeft) != 0)
				{
					nX = this.originalBounds.X - dx;
					nW = this.originalBounds.Width + dx;

					if (nW < this.minSize.Width)
					{
						nX -= (this.minSize.Width - nW);
						nW = this.minSize.Width;
					}
				}

                if ((this.controlState & ControlState.ControlResizeTop) != 0)
				{
					nY = this.originalBounds.Y - dy;
					nH = this.originalBounds.Height + dy;

					if (nH < this.minSize.Height)
					{
						nY -= (this.minSize.Height - nH);
						nH = this.minSize.Height;
					}
				}

				if (nW < this.minSize.Width)
				{
					nW = this.minSize.Width;
				}

				if (nH < this.minSize.Height)
				{
					nH = this.minSize.Height;
				}

				SetSize(nX, nY, nW, nH);
			}

            if ((this.controlState & ControlState.ControlDragging) != 0)
			{
                UpdateCursor(MousePointers.PointerMove);
			}
            else if ((this.controlState & ControlState.ControlResizing) != 0)
			{
				switch(this.controlState)
				{
                    case ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeLeft:
						UpdateCursor(MousePointers.PointerResize1);//BottomLeft
						break;
                    case ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeTop | ControlState.ControlResizeLeft:
                        UpdateCursor(MousePointers.PointerResize2);//TopLeft
						break;
                    case ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeTop:
                        UpdateCursor(MousePointers.PointerResize1);//TopRight
						break;
                    case ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz:
                        UpdateCursor(MousePointers.PointerResize2);//BottomRight
						break;
                    case ControlState.ControlResizing | ControlState.ControlResizeHoriz | ControlState.ControlResizeLeft:
                        UpdateCursor(MousePointers.PointerHResize);//MiddleLeft
						break;
                    case ControlState.ControlResizing | ControlState.ControlResizeHoriz:
                        UpdateCursor(MousePointers.PointerHResize);//MiddleRight
						break;
                    case ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeTop:
                        UpdateCursor(MousePointers.PointerVResize);//TopCenter
						break;
                    case ControlState.ControlResizing | ControlState.ControlResizeVert:
                        UpdateCursor(MousePointers.PointerVResize);//BottomCenter
						break;
					default:
                        UpdateCursor(MousePointers.PointerStandard);
						break;
				}
			}
        }

        /// <summary>
        /// Passes mouse move event to child controls.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        protected virtual void OnMouseMoveControls(int x, int y)
        {
			//if (true == this.bounds.IsInside(X, Y))
			{
				int nx = x - this.bounds.X;
				int ny = y - this.bounds.Y - this.topOffset;

                lock (this.childControls)
                {
                    foreach (Control it in this.childControls)
                    {
                        it.OnMouseMove(nx, ny);
                    }
                }
			}
        }

        /// <summary>
        /// Handles mouse press event.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        protected virtual void OnMousePressed(int x, int y)
        {
			this.mousePosition.SetCoords(x, y);

			if (true == this.sizeable)
			{
                if (true == IsOnBorder(ContentAlignment.BottomLeft, x, y))
				{
                    this.originalBounds = this.bounds.Clone();
					this.mousePressPosition.SetCoords(x,y);
                    this.controlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeLeft;
				}
                else if (true == IsOnBorder(ContentAlignment.TopLeft, x, y))
				{
                    this.originalBounds = this.bounds.Clone();
					this.mousePressPosition.SetCoords(x,y);
                    this.controlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeTop | ControlState.ControlResizeLeft;
				}
                else if (true == IsOnBorder(ContentAlignment.TopRight, x, y))
				{
                    this.originalBounds = this.bounds.Clone();
					this.mousePressPosition.SetCoords(x,y);
                    this.controlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz | ControlState.ControlResizeTop;
				}
                else if (true == IsOnBorder(ContentAlignment.BottomRight, x, y))
				{
                    this.originalBounds = this.bounds.Clone();
					this.mousePressPosition.SetCoords(x,y);
                    this.controlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeHoriz;
				}
                else if (true == IsOnBorder(ContentAlignment.MiddleLeft, x, y))
				{
                    this.originalBounds = this.bounds.Clone();
					this.mousePressPosition.SetCoords(x,y);
                    this.controlState = ControlState.ControlResizing | ControlState.ControlResizeHoriz | ControlState.ControlResizeLeft;
				}
                else if (true == IsOnBorder(ContentAlignment.MiddleRight, x, y))
				{
                    this.originalBounds = this.bounds.Clone();
					this.mousePressPosition.SetCoords(x,y);
                    this.controlState = ControlState.ControlResizing | ControlState.ControlResizeHoriz;
				}
                else if (true == IsOnBorder(ContentAlignment.TopCenter, x, y))
				{
                    this.originalBounds = this.bounds.Clone();
					this.mousePressPosition.SetCoords(x,y);
                    this.controlState = ControlState.ControlResizing | ControlState.ControlResizeVert | ControlState.ControlResizeTop;
				}
				else if (true == IsOnBorder(ContentAlignment.BottomCenter, x, y))
				{
                    this.originalBounds = this.bounds.Clone();
					this.mousePressPosition.SetCoords(x,y);
                    this.controlState = ControlState.ControlResizing | ControlState.ControlResizeVert;
				}
			}
            if ((true == this.moveable) && (true == IsOnBorder(ContentAlignment.MiddleCenter, x, y)))
			{
                this.originalBounds = this.bounds.Clone();
				this.mousePressPosition.SetCoords(x,y);
				this.controlState = ControlState.ControlDragging;
			}

            foreach (Control control in this.childControls)
            {
                control.OnMousePressed(x - this.bounds.X, y - this.bounds.Y - this.topOffset);
            }
        }

        /// <summary>
        /// Handles mouse release event.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        protected virtual void OnMouseReleased(int x, int y)
        {
            OnMouseMove(x, y);

			this.mousePosition.SetCoords(x, y);

			this.controlState = ControlState.None;

			for (int i = 0; i < this.childControls.Count; i++)
			{
                this.childControls[i].OnMouseReleased(x - this.bounds.X, y - this.bounds.Y - this.topOffset);
			}
        }

        /// <summary>
        /// Handles mouse wheel event.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        /// <param name="dx">scrolled wheel by X axis.</param>
        /// <param name="dy">scrolled wheel by Y azis.</param>
        protected virtual bool OnMouseWheelUp(int x, int y, int dx, int dy)
        {
            return false;
        }

        /// <summary>
        /// Handles mouse wheel event.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        /// <param name="dx">scrolled wheel by X axis.</param>
        /// <param name="dy">scrolled wheel by Y azis.</param>
        protected virtual bool OnMouseWheelDown(int x, int y, int dx, int dy)
        {
            return false;
        }

        /// <summary>
        /// Handles mouse released event passed from window.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        internal void MouseReleasedInternal(int x, int y)
        {
            OnMouseReleased(x, y);
        }

        /// <summary>
        /// Handles mouse pressed event passed from window.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        internal void MousePressedInternal(int x, int y)
        {
            OnMousePressed(x, y);
        }

        /// <summary>
        /// Handles mouse wheel event passed from window.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        /// <param name="dx">scrolled wheel by X axis.</param>
        /// <param name="dy">scrolled wheel by Y azis.</param>
        internal bool MouseWheelUpInternal(int x, int y, int dx, int dy)
        {
            return OnMouseWheelUp(x, y, dx, dy);
        }

        /// <summary>
        /// Handles mouse wheel event.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        /// <param name="dx">scrolled wheel by X axis.</param>
        /// <param name="dy">scrolled wheel by Y azis.</param>
        internal bool MouseWheelDownInternal(int x, int y, int dx, int dy)
        {
            return OnMouseWheelDown(x, y, dx, dy);
        }

        /// <summary>
        /// Handles mouse move event from window.
        /// </summary>
        /// <param name="X">mouse X position.</param>
        /// <param name="Y">mouse Y position.</param>
        internal void MouseMoveInternal(int x, int y)
        {
            OnMouseMove(x, y);
        }
        
        #endregion

        #region Keyboard handling

		/// <summary>
        /// Handles key press event.
        /// By default raises KeyPressedEvent
        /// </summary>
        /// <param name="c">char code</param>
        /// <param name="modifier">key modifier, -1 if regular key</param>
        protected virtual void KeyPress(char c, Key modifier)
        {
            if (null != this.KeyPressed)
            {
                this.KeyPressed(this, c, modifier);
            }
        }

        /// <summary>
        /// Handles key press event. Passed from window.
        /// By default raises KeyPressedEvent
        /// </summary>
        /// <param name="c">char code</param>
        /// <param name="modifier">key modifier, -1 if regular key</param>
        internal void KeyPressInternal(char c, Key modifier)
        {
            KeyPress(c, modifier);
        }

        #endregion

        #region Fields

        /// <summary>
        /// Controls' creation flags.
        /// </summary>
		public CreationFlag CreationFlag
		{
			get
			{
				return this.creationFlags;
			}
		}

        /// <summary>
        /// Is this control accessible by keyboard
        /// </summary>
        public bool TabStop
        {
            get
            {
                return this.tabStop;
            }
            set
            {
                this.tabStop = value;
            }
        }

        /// <summary>
        /// The control order on the window (keybord navigation moves acording this order)
        /// </summary>
        public int TabIndex
        {
            get
            {
                return this.tabIndex;
            }
            set
            {
                this.tabIndex = value;
            }
        }

        /// <summary>
        /// Text displayed on control. If controls text is langauge dependant, the translated text is returned.
        /// </summary>
        public virtual String Text
        {
            get
            {
                if (true == this.needTranslation)
                {
                    return this.textReference;
                }
                else
                {
                    return this.controlText;
                }
            }
            set
            {
                this.textReference = value;

                OnLanguageChanged();
            }
        }

        /// <summary>
        /// Control bounds
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return this.bounds;
            }
            set
            {
                SetSize(value.X, value.Y, value.Width, value.Height);
            }
        }

        /// <summary>
        /// UI Engine.
        /// </summary>
        protected UIEngine Engine
        {
            get
            {
                return this.uiEngine;
            }
        }

        /// <summary>
        /// Icon color. The icon image is multiplied with specified color.
        /// </summary>
        public Color IconColor
        {
            get
            {
                return this.iconColor;
            }
            set
            {
                this.iconColor = value;
            }
        }

        /// <summary>
        /// Selected text color.
        /// </summary>
        public Color SelectedTextColor
        {
            get
            {
                return this.colorTextSelected;
            }
            set
            {
                this.colorTextSelected = value;
            }
        }

        /// <summary>
        /// Controls' bacgroud color that it is selected.
        /// </summary>
        public Color SelectedBackgroundColor
        {
            get
            {
                return this.colorBackSelected;
            }
            set
            {
                this.colorBackSelected = value;
            }
        }

        /// <summary>
        /// Back image color. Multiplies background image with specified color.
        /// </summary>
        public Color BackImageColor
        {
            get
            {
                return this.backImageColor;
            }
            set
            {
                this.backImageColor = value;
            }
        }

        /// <summary>
        /// Icon scale.
        /// </summary>
        public float IconScale
        {
            get
            {
                return this.iconScale;
            }
            set
            {
                this.iconScale = value;
            }
        }

        /// <summary>
        /// Some special value attached to the control. Usually null.
        /// </summary>
        public virtual Object Value
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        /// <summary>
        /// Border color.
        /// </summary>
        public Color BorderColor
        {
            get
            {
                return this.borderColor;
            }
            set
            {
                this.borderColor = value;
            }
        }

        /// <summary>
        /// Controls border color, the first dark one
        /// </summary>
        public Color BorderColorDark1
        {
            get
            {
                return this.borderColorDark1;
            }
            set
            {
                this.borderColorDark1 = value;
            }
        }

        /// <summary>
        /// Controls border color, the second dark one
        /// </summary>
        public Color BorderColorDark2
        {
            get
            {
                return this.borderColorDark2;
            }
            set
            {
                this.borderColorDark2 = value;
            }
        }

        /// <summary>
        /// Controls border color, the first light one
        /// </summary>
        public Color BorderColorLight1
        {
            get
            {
                return this.borderColorLight1;
            }
            set
            {
                this.borderColorLight1 = value;
            }
        }

        /// <summary>
        /// Controls border color, the second light one
        /// </summary>
        public Color BorderColorLight2
        {
            get
            {
                return this.borderColorLight2;
            }
            set
            {
                this.borderColorLight2 = value;
            }
        }

        /// <summary>
        /// Does the control have text that needs translation (I18N).
        /// </summary>
        public bool NeedTranslation
        {
            set
            {
                this.needTranslation = value;

                OnLanguageChanged();
            }
            get
            {
                return this.needTranslation;
            }
        }

        /// <summary>
        /// Window this control belongs to.
        /// </summary>
        public Window Window
        {
            get
            {
                if ((null == this.window) && (this is Window))
                {
                    return (Window)this;
                }

                return this.window;
            }
        }

        /// <summary>
        /// Information about font.
        /// </summary>
        public FontInfo FontInfo
        {
            get
            {
                return this.font;
            }
        }

        /// <summary>
        /// Does the control extends to fill all parent area.
        /// </summary>
        public bool FillParent
        {
            get
            {
                return this.fillParent;
            }
            set
            {
                this.fillParent = value;
            }
        }

        /// <summary>
        /// Can this control be resized.
        /// </summary>
        public bool Sizeable
        {
            set
            {
                this.sizeable = value;
            }
            get
            {
                return this.sizeable;
            }
        }

        /// <summary>
        /// Can control be moved.
        /// </summary>
        public bool Moveable
        {
            get
            {
                return this.moveable;
            }
            set
            {
                this.moveable = value;
            }
        }

        /// <summary>
        /// Control has bitmap skin, or is rendered by vertex graphics.
        /// </summary>
        public bool Skinned
        {
            get
            {
                return this.skinned;
            }
            set
            {
                this.skinned = value;
            }
        }

        /// <summary>
        /// Opacity in [0; 1] range. 1 - fully solid, 0 - fully transparent.
        /// Exceeding range values are clamped.
        /// </summary>
        public virtual float Opacity
        {
            set
            {
                this.opacity = value;

                if (this.opacity < 0.0f)
                {
                    this.opacity = 0.0f;
                }

                if (this.opacity > 1.0f)
                {
                    this.opacity = 1.0f;
                }
            }
            get
            {
                return this.opacity;
            }
        }

        /// <summary>
        /// Minimum controls' Width and Height.
        /// </summary>
        public Size2d MinSize
        {
            set
            {
                this.minSize.Width = value.Width;
                this.minSize.Height = value.Height;
            }
            get
            {
                return this.minSize;
            }
        }

        /// <summary>
        /// Is outline is rendered around the control. Used at the design time.
        /// </summary>
        public bool DrawOutline
        {
            set
            {
                this.drawOutline = value;
            }
        }

        /// <summary>
        /// Icon display size.
        /// </summary>
        public IconSize IconSize
        {
            set
            {
                this.iconSize = value;
            }
            get
            {
                return this.iconSize;
            }
        }

        /// <summary>
        /// Is this control visible.
        /// </summary>
        public bool Visible
        {
            set
            {
                this.visible = value;
            }
            get
            {
                return this.visible;
            }
        }

        /// <summary>
        /// Is this control in focus
        /// </summary>
        public bool HasFocus
        {
            get
            {
                return this.hasFocus;
            }
        }

        /// <summary>
        /// Tag object associated with this control.
        /// </summary>
        public Object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }

        /// <summary>
        /// Parent control, this control belongs to.
        /// </summary>
        public Control Parent
        {
            get
            {
                return this.parent;
            }
        }

        /// <summary>
        /// Controls z order.
        /// </summary>
        public int ZOrder
        {
            get
            {
                return this.zOrder;
            }
        }

        /// <summary>
        /// Controls Width.
        /// </summary>
        public int Width
        {
            set
            {
                this.bounds.Width = value;
            }
            get
            {
                return this.bounds.Width;
            }
        }

        /// <summary>
        /// Controls X position.
        /// </summary>
        public int X
        {
            set
            {
                this.bounds.X = value;
            }
            get
            {
                return this.bounds.X;
            }
        }

        /// <summary>
        /// Controls Y position.
        /// </summary>
        public int Y
        {
            set
            {
                this.bounds.Y = value;
            }
            get
            {
                return this.bounds.Y;
            }
        }

        /// <summary>
        /// Controls Height
        /// </summary>
        public int Height
        {
            get
            {
                return this.bounds.Height;
            }
            set
            {
                this.bounds.Height = value;
            }
        }

        /// <summary>
        /// Some sctipt that is executed then control is clicked.
        /// thw.uiengine.loadWindow windowFileName - loads and displays window from xml file.
        /// thw.uiengine.loadRegisteredWindow windowName - loads and displays registered window.
        /// IF UIEngine has registered script processor script is passed to it.
        /// </summary>
        public String ClickScript
        {
            get
            {
                return this.clickScript;
            }
            set
            {
                this.clickScript = value;
            }
        }

        /// <summary>
        /// Controls placed on this control
        /// </summary>
        public virtual IList<Control> Controls
        {
            get
            {
                return this.childControls;
            }
        }

        /// <summary>
        /// The mouse cursor that is displayed than mouse moves over this control.
        /// </summary>
        public virtual MousePointers Cursor
        {
            set
            {
                this.cursor = value;
            }
            get
            {
                return this.cursor;
            }
        }

        /// <summary>
        /// Control position anchoring to its' parent.
        /// </summary>
        public AnchorStyle Anchor
        {
            set
            {
                this.anchor = value;
            }
            get
            {
                return this.anchor;
            }

        }

        /// <summary>
        /// Controls icon.
        /// </summary>
        public virtual String Icon
        {
            get
            {
                return this.icon != null ? this.icon.Name : null;
            }
            set
            {
                this.icon = new ImageObject(this.Engine, value);
            }
        }

        /// <summary>
        /// Text aligment. Specifies to which control border the text should be aligned.
        /// </summary>
        public ContentAlignment TextAlignment
        {
            set
            {
                this.textAlignment = value;
            }
            get
            {
                return this.textAlignment;
            }

        }

        /// <summary>
        /// Text offset. Used if there is a need to relocate text by several pixels.
        /// </summary>
        public Point2D TextOffset
        {
            get
            {
                return this.textOffset;
            }
        }

        /// <summary>
        /// Text font color.
        /// </summary>
        public Color TextColor
        {
            set
            {
                this.textColor = value;
            }
            get
            {
                return this.textColor;
            }
        }

        /// <summary>
        /// Relocate icon image by some pixels.
        /// </summary>
        public Point2D IconImageOffset
        {
            get
            {
                return this.iconImageOffset;
            }
        }

        /// <summary>
        /// Icon aligment.
        /// </summary>
        public ContentAlignment IconAlignment
        {
            set
            {
                this.iconAlignment = value;
            }
            get
            {
                return this.iconAlignment;
            }
        }

        /// <summary>
        /// Control border style.
        /// </summary>
        public BorderStyle Border
        {
            set
            {
                this.uiEngine.DeleteImage(ref this.borderImage);

                this.borderStyle = value;

                if ((this.borderStyle == BorderStyle.BorderRaised) || (this.borderStyle == BorderStyle.Lowered) || (this.borderStyle == BorderStyle.Flat))
                {
                    this.borderSize = 1;
                }
                else if ((this.borderStyle == BorderStyle.BorderLoweredDouble) || (this.borderStyle == BorderStyle.BorderRaisedDouble) || (this.borderStyle == BorderStyle.BorderFlatDouble))
                {
                    this.borderSize = 2;
                }
                else
                {
                    this.borderSize = 0;
                }
            }
            get
            {
                return this.borderStyle;
            }
        }

        /// <summary>
        /// Then contorol is selected this icon is displayed
        /// </summary>
        public virtual String IconSelected
        {
            set
            {
                if ((null == value) || (value == ""))
                {
                    this.selectedIcon = null;
                }
                else
                {
                    this.selectedIcon = new ImageObject(this.Engine, value);
                }
            }
            get
            {
                return null != this.selectedIcon ? this.selectedIcon.Name : null;
            }
        }

        /// <summary>
        /// Controls background image.
        /// </summary>
        public String BackImage
        {
            set
            {
                this.backgroundImage = new ImageObject(this.Engine, value);
            }
            get
            {
                return null != this.backgroundImage ? this.backgroundImage.Name : null;
            }
        }

        /// <summary>
        /// Back image drawing style
        /// </summary>
        public ImageLayout BackImageLayout
        {
            set
            {
                this.backImageLayout = value;
            }
            get
            {
                return this.backImageLayout;
            }
        }

        /// <summary>
        /// Controls' background color.
        /// </summary>
        public Color BackColor
        {
            set
            {
                this.backColor = value;
            }
            get
            {
                return this.backColor;
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises Value property change event.
        /// </summary>
        protected void RaiseValueChangedEvent()
        {
            if (null != this.ValueChanged)
            {
                this.ValueChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Control has lost focus.
        /// </summary>
        public event UIEventHandler<Control> LostFocus = null;

        /// <summary>
        /// Control has gained focus.
        /// </summary>
        public event UIEventHandler<Control> GotFocus = null;

        /// <summary>
        /// If the value changed for input control this event is raised.
        /// </summary>
        public event UIEventHandler<Control> ValueChanged = null;

        /// <summary>
        /// Key press on control event.
        /// </summary>
        public event KeyPressedHandler KeyPressed = null;

        #endregion

        #region Attributes

        private bool drawOutline = false;
        private bool visible = true;
        private bool sizeable = false;
        protected bool isMouseOver = false;
        private bool moveable = false;
        private bool hasFocus = false;
        private bool needTranslation = true;
        private bool skinned = false;
        protected bool propertiesAreLoaded = false;
        private bool fillParent = false;
        private AnchorStyle anchor = AnchorStyle.AnchorTop | AnchorStyle.AnchorLeft;
        private CreationFlag creationFlags = CreationFlag.FlagsNone;
        private ControlState controlState = ControlState.None;
        internal ImageObject backgroundImage = null;
        protected String textReference = "";
        protected String controlText = "";
        private String clickScript = "";
        internal ImageObject icon = null;
        private ImageObject selectedIcon = null;
        protected Rectangle bounds = new Rectangle();
        private Rectangle originalBounds = new Rectangle();
        protected Color backColor = Colors.None;
        protected Color textColor = Colors.Black;
        protected Color borderColor = Colors.White;
        private Color borderColorDark1 = new Color();
        private Color borderColorDark2 = new Color();
        private Color borderColorLight1 = new Color();
        private Color borderColorLight2 = new Color();
        private Size2d parentControlSize = new Size2d();
        protected Point2D iconImageOffset = new Point2D();
        protected Point2D mousePosition = new Point2D();
        private Point2D mousePressPosition = new Point2D();
        private FontInfo font = null;
        private ImageLayout backImageLayout = ImageLayout.ImageLayoutStretch;
        private BorderStyle borderStyle = BorderStyle.None;
        private Window window = null;
        private ContentAlignment textAlignment = ContentAlignment.MiddleCenter;
        private ContentAlignment iconAlignment = ContentAlignment.MiddleCenter;
        private MousePointers cursor = MousePointers.PointerStandard;
        private Control parent = null;
        private Object tag = null;
        private List<Control> childControls = new List<Control>();
        private List<Property> properties = new List<Property>();
        protected IImage borderImage = null;
        private int borderImageWidth = 0;
        private Point2D textOffset = new Point2D();
        private Size2d minSize = new Size2d();
        private IconSize iconSize = IconSize.IconLarge;
        private bool tabStop = false;
        private int tabIndex = -1;
        protected int topOffset = 0;
        private int zOrder = 0;
        protected int borderSize = 0;
        private Color colorBackSelected = Colors.None;
        private Color colorTextSelected = Colors.None;
        private float opacity = 1.0f;
        private UIEngine uiEngine = null;
        private Color iconColor = Colors.White;
        private Color backImageColor = Colors.White;
        private float iconScale = 1.0f;
        
        #endregion
    }
}
