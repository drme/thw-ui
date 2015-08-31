using System;
using System.Collections.Generic;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;
using ThW.UI.Windows;

namespace ThW.UI
{
    /// <summary>
    /// Groups windows. Has wallpaper. Performs windows management (z-order, events handling)
    /// </summary>
	public class Desktop : UIObject
	{
        /// <summary>
        /// Constructs desktop object.
        /// </summary>
        /// <param name="engine">ui engine</param>
        /// <param name="theme">desktop theme</param>
        /// <param name="fileName">filename to load dekstop from, or empty for default desktop.</param>
        internal Desktop(UIEngine engine, Theme theme, String fileName) : base("desktop")
        {
            this.engine = engine;
            this.theme = theme;
            this.mousePointer = new MousePointer(this.engine);

            if (null != fileName)
            {
                using (IXmlReader reader = this.engine.OpenXmlFile(fileName))
                {
                    if (null != reader)
                    {
                        Load(reader);
                    }
                }
            }
        }

        /// <summary>
        /// Releases desktop. Cleans its images.
        /// </summary>
        ~Desktop()
        {
            this.engine.DeleteImage(ref this.wallpaper);

            DeleteWindows();
        }

        /// <summary>
        /// Renders desktop.
        /// </summary>
        /// <param name="dt">time passed since last frame (in seconds). Used for animations timing.</param>
        public void Render(float dt)
        {
            Render(this.engine.GraphicsInternal, dt);
        }

        /// <summary>
        /// Renders Desktop and all its windows using graphics interface.
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="dt">time elapsed since last frame (used for animations)</param>
        protected virtual void Render(Graphics graphics, float dt)
        {
            lock (this)
            {
                DeleteWindows();

                if (null != this.wallpaper)
                {
                    graphics.SetColor(Colors.White);
                    graphics.DrawImage(0, 0, this.desktopSize.Width, this.desktopSize.Height, this.wallpaper);
                }

                if (this.windows.Count > 0)
                {
                    for (int i = this.windows.Count - 1; i > 0; i--)
                    {
                        //if (false == this->m_lstWindows[i]->IsAlwaysOnTop())
                        {
                            this.windows[i].RenderInternal(graphics, dt);

                            Rectangle r = this.windows[i].Bounds;

                            if ((null != this.tempWindow) && (i == 1))
                            {
                            }
                            else
                            {
                                graphics.SetColor(disabled);
                                graphics.DrawRectangle(r.X, r.Y, r.Width, r.Height);
                                graphics.SetColor(Colors.White);
                            }
                        }
                    }

                    //for (int i = this->m_lstWindows.size() - 1; i > 0; i--)
                    //{
                    //	if (true == this->m_lstWindows[i]->IsAlwaysOnTop())
                    //	{
                    //		this->m_lstWindows[i]->Render(this->render);
                    //		Rectangle& r = this->m_lstWindows[i]->GetSize();
                    //		static Color disabled(0.5f, 0.5f, 0.5f, 0.5f);
                    //		this->render->SetColor(disabled);
                    //		this->render->DrawRectangle(r.X, r.Y, r.Width, r.Height);
                    //		this->render->SetColor(Colors::White);
                    //	}
                    //}

                    this.windows[0].RenderInternal(graphics, dt);
                }

                if (true == this.drawCursorInternally)
                {
                    this.mousePointer.Render(graphics, this.mousePosition.X, this.mousePosition.Y, this.Theme);
                }
                else
                {
                    if (null != this.CursorChanged)
                    {
                        this.CursorChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Sets desktop's size
        /// </summary>
        /// <param name="Width">desktop Width.</param>
        /// <param name="Height">desktop Height.</param>
		public void SetSize(int width, int height)
        {
            if ((this.desktopSize.Width != width) || (this.desktopSize.Height != height))
            {
                this.desktopSize.SetSize(width, height);

                if (null != this.Resized)
                {
                    this.Resized(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Sets desktops' wallpaper
        /// </summary>
		public String Wallpaper
        {
            set
            {
                this.engine.DeleteImage(ref this.wallpaper);

                if ((null != value) && (0 != value.Length))
                {
                    this.wallpaper = this.engine.CreateImage(value);
                }
                else
                {
                    this.wallpaper = null;
                }
            }
            get
            {
                return (null != this.wallpaper) ? this.wallpaper.Name : null;
            }
        }

		/// <summary>
		/// Creates window of registered type. Window creator (type has to be registered using
		/// UIEngine.RegisterWindowType method, in order to use this method.
		/// </summary>
		public Window NewRegisteredWindow(String windowType)
        {
            Window window = this.engine.NewWindow(this, windowType, CreationFlag.NeedLoading, "");

			AddWindow(window);

			return window;
        }

        /// <summary>
        /// Creates window loading it from file. If file not found, default empty window is created.
        /// </summary>
		public Window NewWindow(CreationFlag creationFlags, String fileName)
        {
            Window window = this.engine.NewWindow(this, "", creationFlags, fileName);

            lock (this)
            {
                this.windows.Insert(0, window);
            }

			return window;
        }

        /// <summary>
        /// Creates window by loading it from file. The created window is added to the desktop.
        /// If file not found, default empty window is created.
        /// </summary>
        /// <param name="windowFileName">window file name to load (ex.: loading.window.xml)</param>
        /// <returns>Loaded and added to the desktop window</returns>
        public Window NewWindow(String windowFileName)
        {
            return NewWindow(CreationFlag.NeedLoading, windowFileName);
        }

        /// <summary>
        /// Handles mouse move event, passes to active window.
        /// </summary>
        /// <param name="X">X mouse position.</param>
        /// <param name="Y">Y mouse position.</param>
		public void MouseMove(int x, int y)
        {
			this.mousePosition.SetCoords(x, y);

			SetMouseCursor(MousePointers.PointerStandard);

            lock (this)
            {
                Window window = this.ActiveWindow;

                if (null != window)
                {
                    window.MouseMoveInternal(x, y);
                }
            }
        }

        /// <summary>
        /// Handles mouse press event, passes to active window.
        /// </summary>
        /// <param name="X">X mouse position.</param>
        /// <param name="Y">Y mouse position.</param>
        public void MousePress(int x, int y)
        {
			MouseMove(x, y);

            lock (this)
            {
                if ((null != this.tempWindow) && (false == this.tempWindow.IsInside(x, y)))
                {
                    this.tempWindow.Close();
                    DeleteWindow(this.tempWindow);
                    this.tempWindow = null;

                    return;
                }

                if (this.windows.Count > 0)
                {
                    if (true == this.windows[0].Modal)
                    {
                        if (this.windows[0].IsInside(x, y))
                        {
                            this.windows[0].MousePressedInternal(x, y);
                        }

                        return;
                    }

                    foreach (Window window in this.windows)
                    {
                        if ((true == window.AlwaysOnTop) && (true == window.IsInside(x, y)))
                        {
                            this.windows.Remove(window);
                            this.windows.Insert(0, window);

                            window.MousePressedInternal(x, y);

                            return;
                        }
                    }

                    if (this.windows[0].IsInside(x, y))
                    {
                        this.windows[0].MousePressedInternal(x, y);

                        return;
                    }

                    foreach (Window window in this.windows)
                    {
                        if ((false == window.AlwaysOnTop) && (true == window.IsInside(x, y)))
                        {
                            this.windows.Remove(window);
                            this.windows.Insert(0, window);

                            window.MousePressedInternal(x, y);

                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles mouse release event, passes to active window.
        /// </summary>
        /// <param name="X">X mouse position.</param>
        /// <param name="Y">Y mouse position.</param>
        public void MouseRelease(int x, int y)
        {
            MouseMove(x, y);

            lock (this)
            {
                if (this.windows.Count > 0)
                {
                    this.windows[0].MouseReleasedInternal(x, y);
                }
            }
        }

        /// <summary>
        /// Handles mouse wheel up event, passes to active window.
        /// </summary>
        public void MouseWheelUp()
        {
            lock (this)
            {
                Window window = this.ActiveWindow;

                if (null != window)
                {
                    window.MouseWheelUpInternal(0, 0, 0, 0);
                }
            }
        }

        /// <summary>
        /// Handles mouse wheel down event, passes to active window.
        /// </summary>
        public void MouseWheelDown()
        {
            lock (this)
            {
                Window window = ActiveWindow;

                if (null != window)
                {
                    window.MouseWheelDownInternal(0, 0, 0, 0);
                }
            }
        }

        /// <summary>
        /// Adds window to the desktop. DEsktop manages this window.
        /// Window is released on desktop release.
        /// </summary>
        /// <param name="window">window to add to this desktop.</param>
		public void AddWindow(Window window)
        {
			if (null != window)
			{
                lock (this)
                {
                    this.windows.Insert(0, window);
                }
			}
        }

        /// <summary>
        /// Removes window from desktop, without deleting it.
        /// </summary>
        public void RemoveWindow(Window window)
        {
			if (null == window)
			{
				return;
			}

            lock (this)
            {
                foreach (Window it in this.windows)
                {
                    if (it == window)
                    {
                        this.windows.Remove(it);

                        if (window == this.tempWindow)
                        {
                            this.tempWindow = null;
                        }

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Handles key press event, passes to active window.
        /// </summary>
		public void KeyPress(char c, Key key)
        {
            lock (this)
            {
                Window window = this.ActiveWindow;

                if (null != window)
                {
                    window.KeyPressInternal(c, key);
                }
            }
        }

        /// <summary>
        /// Returns desktop's Width.
        /// </summary>
		public int Width
        {
            get
            {
                return this.desktopSize.Width;
            }
        }

        /// <summary>
        /// Returns desktop's Height.
        /// </summary>
		public int Height
        {
            get
            {
                return this.desktopSize.Height;
            }
        }

        /// <summary>
        /// Searches for window with specified name. If not found, returns NULL.
        /// </summary>
		public Window FindWindow(String windowName)
        {
            lock (this)
            {
                foreach (Window window in this.windows)
                {
                    if (windowName == window.Name)
                    {
                        return window;
                    }
                }
            }

			return null;
        }

        /// <summary>
        /// To draw or not mouse cursor.
        /// </summary>
		public bool DrawCursor
        {
            set
            {
                this.drawCursorInternally = value;
            }
            get
            {
                return this.drawCursorInternally;
            }
        }

        /// <summary>
        /// Sets desktop cursor.
        /// </summary>
        /// <param name="cursor">desktop cursor.</param>
		internal void SetMouseCursor(MousePointers cursor)
        {
			this.mousePointer.ActiveCursor = cursor;
        }

        /// <summary>
        /// Removes windows.
        /// </summary>
        private void DeleteWindows()
        {
            lock (this)
            {
                this.windowsToDelete.Clear();
            }
        }

        /// <summary>
        /// Loads desktop from xml file. Creates define windows as well.
        /// </summary>
        /// <param name="reader">xml document to load from.</param>
		private void Load(IXmlReader reader)
        {
			IXmlElement root = reader.RootElement;

			if ( (null != root) && (root.Name == "desktop") )
			{
				this.desktopSize.Width = UIUtils.FromString(root.GetAttributeValue("Width", ""), this.desktopSize.Width);
				this.desktopSize.Height = UIUtils.FromString(root.GetAttributeValue("Height", ""), this.desktopSize.Height);
                this.Wallpaper = root.GetAttributeValue("wallpaper", this.Wallpaper);

                foreach (IXmlElement element in root.Elements)
				{
					if (element.Name == "window")
					{
						String type = element.GetAttributeValue("type", "");

						if (type.Length > 0)
						{
							NewRegisteredWindow(type);
						}
						else
						{
							NewWindow(CreationFlag.NeedLoading, element.GetAttributeValue("name", ""));
						}

                        if (null != this.Resized)
                        {
                            this.Resized(this, EventArgs.Empty);
                        }
					}
				}
			}
        }

        /// <summary>
        /// Active window.
        /// </summary>
        private Window ActiveWindow
        {
            get
            {
                if (this.windows.Count > 0)
                {
                    return this.windows[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Creates temporary window, window type is selected by it's name. If window is not found by its name
        /// the default window is created.
        /// </summary>
        /// <param name="name">window name</param>
        /// <returns>created window</returns>
        internal Window NewTempWindow(String name)
        {
			DeleteWindow(this.tempWindow);

			this.tempWindow = NewWindow(CreationFlag.FlagsNone, name);
			this.tempWindow.AlwaysOnTop = true;

			return this.tempWindow;
        }
	
        /// <summary>
        /// Sets temporary window. Used for comboboxes and menus.
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        internal Window NewTempWindow(Window window)
        {
            DeleteWindow(this.tempWindow);

			this.tempWindow = window;
			this.tempWindow.AlwaysOnTop = true;
			this.tempWindow.SetDesktop(this);

            lock (this)
            {
                this.windows.Insert(0, window);
            }

			return this.tempWindow;
        }

        /// <summary>
        /// Releases window resources.
        /// </summary>
        /// <param name="window">window to release.</param>
		internal void DeleteWindow(Window window)
        {
			if (null == window)
			{
				return;
			}

            lock (this)
            {
                foreach (Window it in this.windows)
                {
                    if (it == window)
                    {
                        this.windows.Remove(it);

                        if (window == this.tempWindow)
                        {
                            this.tempWindow = null;
                        }

                        if (true == window.releaseOnClose)
                        {
                            this.windowsToDelete.Add(window);
                        }

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Mouse Y position.
        /// </summary>
		internal int MouseY
        {
            get
            {
                return this.mousePosition.Y;
            }
        }

        /// <summary>
        /// Mouse X position.
        /// </summary>
		internal int MouseX
        {
            get
            {
                return this.mousePosition.X;
            }
        }

        /// <summary>
        /// UI Engine associated with this desktop.
        /// </summary>
        internal UIEngine Engine
        {
            get
            {
                return this.engine;
            }
        }

        /// <summary>
        /// Selected theme for this desktop.
        /// </summary>
        public Theme Theme
        {
            get
            {
                return this.theme;
            }
        }

        /// <summary>
        /// The current mosue cursor.
        /// </summary>
        public MousePointers CurrentCursor
        {
            get
            {
                return this.mousePointer.ActiveCursor;
            }
        }

        /// <summary>
        /// The overlay color that is rendered over inactive window.
        /// Usually with alpha value making window a little bit darker.
        /// </summary>
        public Color InactiveWindowColor
        {
            get
            {
                return this.disabled;
            }
            set
            {
                this.disabled = value;
            }
        }

        /// <summary>
        /// Mouse cursor change on desktop event.
        /// </summary>
        public event UIEventHandler<Desktop> CursorChanged = null;

        /// <summary>
        /// Desktop size change event.
        /// </summary>
        public event UIEventHandler<Desktop> Resized = null;

        private Theme theme = null;
		private	Size2d desktopSize = new Size2d(1024, 768);
		private	Point2D mousePosition = new Point2D(0, 0);
		private	bool drawCursorInternally = true;
		private	IImage wallpaper = null;
		private	List<Window> windows = new List<Window>();
		private	List<Window> windowsToDelete = new List<Window>();
		private	MousePointer mousePointer = null;
		private	Window tempWindow = null;
        private Color disabled = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        private UIEngine engine = null;
	}
}
