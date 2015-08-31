using System;
using System.Collections.Generic;
using System.IO;
using ThW.UI.Controls;
using ThW.UI.Design;
using ThW.UI.Fonts;
using ThW.UI.Utils;
using ThW.UI.Utils.FilesSystem;
using ThW.UI.Utils.Themes;
using ThW.UI.Windows;

namespace ThW.UI
{
    public sealed class UIEngine : UIObject
    {
        public UIEngine() : base("uiengine")
        {
			this.Logger = new InternalLogger();
			this.imagesCache = new ImagesCache(this);

            RegisterControlsCreator(new ControlsCreator<TextBox>((x, y) => { return new TextBox(x, y); }), TextBox.TypeName, typeof(TextBox));
            RegisterControlsCreator(new ControlsCreator<Button>((x, y) => { return new Button(x, y); }), Button.TypeName, typeof(Button));
            RegisterControlsCreator(new ControlsCreator<Label>((x, y) => { return new Label(x, y); }), Label.TypeName, typeof(Label));
            RegisterControlsCreator(new ControlsCreator<Panel>((x,y)=>{return new Panel(x,y);}), Panel.TypeName, typeof(Panel));
            RegisterControlsCreator(new ControlsCreator<SplitPanel>((x, y) => { return new SplitPanel(x,y); }), SplitPanel.TypeName, typeof(SplitPanel));
            RegisterControlsCreator(new ControlsCreator<ScrollBar>((x, y) => { return new ScrollBar(x, y); }), ScrollBar.TypeName, typeof(ScrollBar));
            RegisterControlsCreator(new ControlsCreator<ScrollPanel>((x, y) => { return new ScrollPanel(x, y); }), ScrollPanel.TypeName, typeof(ScrollPanel));
            RegisterControlsCreator(new ControlsCreator<TabControl>((x, y) => { return new TabControl(x, y); }), TabControl.TypeName, typeof(TabControl));
            RegisterControlsCreator(new ControlsCreator<TabPage>((x, y) => { return new TabPage(x, y); }), TabPage.TypeName, typeof(TabPage));
            RegisterControlsCreator(new ControlsCreator<ListBox>((x, y) => { return new ListBox(x, y); }), ListBox.TypeName, typeof(ListBox));
            RegisterControlsCreator(new ControlsCreator<ListItem>((x, y) => { return new ListItem(x, y); }), ListItem.TypeName, typeof(ListItem));
            RegisterControlsCreator(new ControlsCreator<Picture>((x, y) => { return new Picture(x, y); }), Picture.TypeName, typeof(Picture));
            RegisterControlsCreator(new ControlsCreator<InfoPanel>((x, y) => { return new InfoPanel(x, y); }), InfoPanel.TypeName, typeof(InfoPanel));
            RegisterControlsCreator(new ControlsCreator<Menu>((x, y) => { return new Menu(x, y); }), Menu.TypeName, typeof(Menu));
            RegisterControlsCreator(new ControlsCreator<ComboBox>((x, y) => { return new ComboBox(x, y); }), ComboBox.TypeName, typeof(ComboBox));
            RegisterControlsCreator(new ControlsCreator<PropertyGrid>((x, y) => { return new PropertyGrid(x, y); }), PropertyGrid.TypeName, typeof(PropertyGrid));
            RegisterControlsCreator(new ControlsCreator<PropertyGroup>((x, y) => { return new PropertyGroup(x, y); }), PropertyGroup.TypeName, typeof(PropertyGroup));
            RegisterControlsCreator(new ControlsCreator<PropertyRow>((x, y) => { return new PropertyRow(x, y); }), PropertyRow.TypeName, typeof(PropertyRow));
            RegisterControlsCreator(new ControlsCreator<ProgressBar>((x, y) => { return new ProgressBar(x, y); }), ProgressBar.TypeName, typeof(ProgressBar));
            RegisterControlsCreator(new ControlsCreator<ToolBar>((x, y) => { return new ToolBar(x, y); }), ToolBar.TypeName, typeof(ToolBar));
            RegisterControlsCreator(new ControlsCreator<StatusBar>((x, y) => { return new StatusBar(x, y); }), StatusBar.TypeName, typeof(StatusBar));
            RegisterControlsCreator(new ControlsCreator<TrackBar>((x, y) => { return new TrackBar(x, y); }), TrackBar.TypeName, typeof(TrackBar));
            RegisterControlsCreator(new ControlsCreator<ColorPicker>((x, y) => { return new ColorPicker(x,y); }, false), ColorPicker.TypeName, typeof(ColorPicker));
            RegisterControlsCreator(new ControlsCreator<FilePicker>((x, y) => { return new FilePicker(x,y); }, false), FilePicker.TypeName, typeof(FilePicker));
            RegisterControlsCreator(new ControlsCreator<CheckBox>((x, y) => { return new CheckBox(x, y); }), CheckBox.TypeName, typeof(CheckBox));
            RegisterControlsCreator(new ControlsCreator<AnchorPicker>((x, y) => { return new AnchorPicker(x,y); }, false), AnchorPicker.TypeName, typeof(AnchorPicker));
            RegisterControlsCreator(new ControlsCreator<ScrollPanelInternal>((x, y) => { return new ScrollPanelInternal(x,y); }, false), ScrollPanelInternal.TypeName, typeof(ScrollPanelInternal));
            RegisterControlsCreator(new ControlsCreator<RadioButton>((x, y) => { return new RadioButton(x, y); }), RadioButton.TypeName, typeof(RadioButton));
            RegisterWindowType((x) => { return new FileChooser(x); }, FileChooser.TypeName);
            RegisterWindowType((x) => { return new DesignWindow(this, x); }, DesignWindow.TypeName);
			RegisterWindowType((x) => { return new ColorChooser(x); }, ColorChooser.TypeName);
			RegisterWindowType((x) => { return new AnchorChooser(x); }, AnchorChooser.TypeName);
			RegisterWindowType((x) => { return new AboutWindow(x); }, AboutWindow.TypeName);
			RegisterWindowType((x) => { return new MessageBox(x); }, MessageBox.TypeName);
        }

        /// <summary>
        /// Creates new desktop. User is responsible of freeing it by calling its Release metdod.
        /// </summary>
        /// <param name="fileName">specifies from which XML file to load desktop. If fileName is empty string, empty desktop is created.</param>
        /// <param name="themeFolder">specifies theme folder, the one that contains theme.xml and all related theme files. This theme is applied to all windows on this desktop.</param>
        /// <returns>created desktop object</returns>
        public Desktop NewDesktop(String fileName, String themeFolder)
        {
            Theme theme = null;

            if ((null == themeFolder) || (themeFolder.Length <= 0))
            {
                theme = new Theme("ui/themes/generic/", this);
            }
            else
            {
                theme = new Theme(themeFolder, this);
            }

        	return new Desktop(this, theme, fileName);
        }

		public IConfig Config
		{
			set;
			get;
		}

        /// <summary>
        /// Specifies implementation of texts translation object.
        /// </summary>
        public ILanguage Language
        {
            set
            {
                this.language = value;
            }
            get
            {
                if (null == this.language)
                {
                    return this.defaultLanguage;
                }
                else
                {
                    return this.language;
                }
            }
        }

		/// <summary>
		/// Scripting handler.
		/// </summary>
        public IScriptsHandler ScriptsHandler
        {
			get
			{
				return this.scriptsHandler;
			}
			set
			{
				this.scriptsHandler = value;
			}
        }

		/// <summary>
		/// Gets fonts creator.
		/// </summary>
		/// <param name="theme"></param>
		/// <returns></returns>
        internal FontsFactory GetFontsFactory(Theme theme)
        {
            if (null == this.fontsFactory)
			{
				this.fontsFactory = new FontsFactory(this, theme);
			}

			return this.fontsFactory;
        }

        /// <summary>
        /// Active render
        /// </summary>
        public IRender Render
        {
            get
            {
                return this.render;
            }
            set
            {
                this.render = value;

                if (null == this.graphics)
                {
                    this.graphics = new Graphics();
                }

                this.graphics.SetRender(value);
            }
        }

        internal Graphics GraphicsInternal
        {
            get
            {
                return this.graphics;
            }
        }

		/// <summary>
		/// Registers property creator. Use IPropertyCreatorTemplate class.
		/// </summary>
		/// <param name="creator">creator.</param>
		/// <param name="type">property type.</param>
        public void RegisterPropertyCreator(IPropertyCreator creator, String type)
        {
            IPropertyCreator ex = null;

            if (true == this.propertyCreators.TryGetValue(type, out ex))
			{
				throw new Exception("Property creator already exists");
			}

			if (null != creator)
			{
				this.propertyCreators[type] = creator;
			}
        }

		/// <summary>
		/// Registers control creator. Use ControlsCreatorTemplate class.
		/// </summary>
		/// <param name="creator">control creator.</param>
		/// <param name="controlType">control type name.</param>
		/// <param name="type">control type.</param>
        public void RegisterControlsCreator(IControlsCreator creator, String controlType, Type type)
        {
            KeyValuePair<Type, IControlsCreator> ex;

			if (true == this.controlCreators.TryGetValue(controlType, out ex))
			{
				throw new Exception("Controls creator already exists");
			}

			if (null != creator)
			{
				this.controlCreators[controlType] = new KeyValuePair<System.Type,IControlsCreator>(type, creator);
			}
        }

        /// <summary>
        /// The list of registered control types.
        /// </summary>
        public IEnumerable<KeyValuePair<String, KeyValuePair<Type, IControlsCreator>>> ControlTypes
        {
            get
            {
                return this.controlCreators;
            }
        }

		/// <summary>
		/// Registers windows type. Use IWindowCreatorTemplate.
		/// </summary>
		/// <param name="windowCreator">window creator factory.</param>
		/// <param name="typeName">window type name.</param>
        public void RegisterWindowType(WindowCreator windowCreator, String typeName)
        {
			this.windowCreators[typeName] = windowCreator;
        }

		/// <summary>
		/// Logger to log all UI library messages.
		/// </summary>
		public ILogger Logger
		{
			get;
			set;
		}

		/// <summary>
		/// Virtual file system interface. By default library works by using
        /// System API functions for reading files.
        /// If it is required to use files from rar, zip and so on archives, they could be accessed using this interface.
		/// </summary>
		public IVirtualFileSystem VirtualFileSystem
		{
			set;
			get;
		}

		/// <summary>
		/// Sets working folder (used only built-in files access).
		/// </summary>
        public String WorkingFolder
        {
			set
			{
				this.workingFolder = value;
			}
        }

        internal void CreateFile(String fileName, byte[] bytes, uint bufferSize)
        {
            if (null != this.VirtualFileSystem)
            {
                if (true == this.VirtualFileSystem.CreateFile(fileName, bytes, bufferSize))
                {
                    return;
                }
            }
            //else
            {
#if WINDOWS
                FileStream writer = FileOpenWrite(this.workingFolder + "/" + fileName);

                if (null == writer)
                {
                    writer = FileOpenWrite(fileName);
                }

                if (null != writer)
                {
                    writer.Write(bytes, 0, bytes.Length);

                    writer.Close();
                }
#endif
            }
        }

        internal bool OpenFile(String fileName, out byte[] fileBytes, out uint size, out Object fileHandle)
        {
            fileHandle = null;

            if (null != this.VirtualFileSystem)
            {
                if (true == this.VirtualFileSystem.OpenFile(fileName, out fileBytes, out size, out fileHandle))
                {
                    return true;
                }
                else
                {
                    fileHandle = null;
                    fileBytes = null;
                    size = 0;
                }
            }

            byte[] fileData = FileUtils.Platform.ReadFile(fileName, this.workingFolder);

            if ((null != fileData) && (fileData.Length > 0))
            {
                fileBytes = fileData;

                size = (uint)fileBytes.Length;

                fileHandle = fileBytes;

                this.defaultOpenedFiles.Add(fileHandle);

                return true;
            }
            else
            {
                fileHandle = null;
                fileBytes = null;
                size = 0;

                return false;
            }
        }

        internal void CloseFile(ref Object fileHandle)
        {
            if (null == fileHandle)
            {
                return;
            }

            if (true != this.defaultOpenedFiles.Contains(fileHandle))
            {
                this.defaultOpenedFiles.Remove(fileHandle);
            }
            else
            {
                if (null != this.VirtualFileSystem)
                {
                    this.VirtualFileSystem.CloseFile(fileHandle);
                }
                else
                {
                    // leaked...
                }
            }

            fileHandle = null;
        }

        internal void DeleteImage(ref IImage image)
        {
            this.imagesCache.DeleteImage(ref image);
            image = null;
        }

        internal IImage CreateImage(String fileName)
        {
            return this.imagesCache.CreateImage(fileName, this, this.GraphicsInternal);
        }

        internal IImage CreateImage(String uniqueName, int w, int h, byte[] bytes)
        {
            return this.imagesCache.CreateImage(uniqueName, w, h, bytes, this.GraphicsInternal);
        }

        internal IXmlReader OpenXmlFile(String fileName)
        {
            IXmlReader reader = null;

            if (null != this.xmlFactory)
            {
                reader = this.xmlFactory.CreateXmlReader(this);
            }

            if (null != reader)
            {
                reader.OpenFile(fileName);

                return reader;
            }

            return null;
        }

        /// <summary>
        /// Creates registered control by its name. Uses registered cotnrol createor.
        /// If creator is not found null is returned.
        /// </summary>
        /// <param name="window">window the control belongs to. PAssed to control creation cosntructor.</param>
        /// <param name="controlTypeName">control name.</param>
        /// <param name="creationFlags">creations flags.</param>
        /// <returns>created control.</returns>
        internal Control CreateControl(Window window, String controlTypeName, CreationFlag creationFlags)
        {
            KeyValuePair<Type, IControlsCreator> creator;

            this.controlCreators.TryGetValue(controlTypeName, out creator);

            if (null == creator.Value)
            {
                return null;
            }

            Control control = creator.Value.CreateControl(window, creationFlags);

            if (null != control)
            {
                control.Type = controlTypeName;
                control.SetEngine(this);
            }

            return control;
        }

        /// <summary>
        /// Creates registered control by its type. Uses registered cotnrol createor.
        /// If creator is not found null is returned.
        /// </summary>
        /// <typeparam name="ControlType">control type.</typeparam>
        /// <param name="window">window the control belongs to. PAssed to control creation cosntructor.</param>
        /// <param name="creationFlags">creations flags.</param>
        /// <returns>created control.</returns>
        internal ControlType CreateControl<ControlType>(Window window, CreationFlag creationFlags)
        {
            IControlsCreator creator = null;
            String controlType = null;

            foreach (KeyValuePair<String, KeyValuePair<Type, IControlsCreator>> c in this.controlCreators)
            {
                if (c.Value.Key == typeof(ControlType))
                {
                    creator = c.Value.Value;
                    controlType = c.Key;
                    
                    break;
                }
            }

            if (null == creator)
            {
                return default(ControlType);
            }

            Control control = creator.CreateControl(window, creationFlags);

            if ((null != control) && (control is ControlType))
            {
                control.Type = controlType;
                control.SetEngine(this);

                return (ControlType)(Object)control;
            }
            else
            {
                return default(ControlType);
            }
        }

        internal IXmlWriter CreateXmlFile(String fileName)
        {
            IXmlWriter writer = null;

            if (null != this.xmlFactory)
            {
                writer = this.xmlFactory.CreateXmlWriter(this);
            }

            if (null != writer)
            {
                writer.OpenFile(fileName);

                return writer;
            }

            return null;
        }

        internal Window NewWindow(Desktop desktop, String type, CreationFlag creationFlags, String fileName)
        {
            if (type.Length > 0)
            {
                WindowCreator creator = null;

                this.windowCreators.TryGetValue(type, out creator);

                if (null != creator)
                {
					Window window = creator(desktop);
                    //window.SetType(type);

                    return window;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                CreationFlag flags = creationFlags;

                if (fileName.Length > 0)
                {
                    flags |= CreationFlag.NeedLoading;
                }

                return new Window(desktop, creationFlags, fileName);
            }
        }

        internal void GetFiles(String folder, List<String> files)
        {
            if (null != this.VirtualFileSystem)
            {
                this.VirtualFileSystem.GetFiles(folder, files);
            }

            FileUtils.Platform.FillFiles(this.workingFolder + "/" + folder, files);
        }

        internal Property CreateProperty(String type, String name, String group, String text)
        {
            IPropertyCreator creator = null;

            this.propertyCreators.TryGetValue(type, out creator);

            if (null == creator)
            {
                return null;
            }

            return creator.CreateProperty(name, group, text);
        }

#if WINDOWS
        internal FileStream FileOpenRead(String fileName)
        {
#if WINDOWS
            try
            {
                if (true == File.Exists(fileName))
                {
                    FileStream fileStream = File.OpenRead(fileName);

                    return fileStream;
                }

                if (true == File.Exists(this.workingFolder + "/" + fileName))
                {
                    FileStream fs = File.OpenRead(this.workingFolder + "/" + fileName);

                    return fs;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.WriteLine(LogLevel.Warning, ex.Message);

                return null;
            }
#else
            return null;
#endif
        }

        internal static FileStream FileOpenWrite(String fileName)
        {
            try
            {
                FileStream fs = File.Create(fileName);

                return fs;
            }
            catch (Exception)
            {
                return null;
            }
        }
#endif
        public IImage GetIcon(int id, bool large, Theme theme)
        {
            return FileUtils.Platform.GetIcon(id, large, this, theme);
        }

		internal String GeneratedCachedFontsFolder
		{
			get;
			private set;
		}

        /// <summary>
        /// Gets sound effect by name. Searches for it in cache.
        /// If found retuns cached one, otherwise creates a new one.
        /// </summary>
        /// <param name="name">sound effect name.</param>
        /// <returns>cached or newly created sound effect object.</returns>
        internal SoundObject GetSound(String name)
        {
            if ((null == name) || (name == ""))
            {
                return null;
            }

            lock (this.sounds)
            {
                SoundObject value = null;

                this.sounds.TryGetValue(name, out value);

                if (null == value)
                {
                    value = new SoundObject(this, name);

                    this.sounds.Add(name, value);
                }

                value.AddRef();

                return value;
            }
        }

        internal void RemoveSound(SoundObject value)
        {
            lock (this.sounds)
            {
                if (null != value)
                {
                    if (0 == value.Release())
                    {
                        this.sounds.Remove(value.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Interface for playing and managing sound effects.
        /// </summary>
        public IAudio Audio
        {
            set
            {
                this.audio = value;
            }
            internal get
            {
                return this.audio;
            }
        }

#if WINDOWS
		public void CacheFont(String fontName, int size, bool bold, bool italic, String cacheFolder)
		{
			this.GeneratedCachedFontsFolder = cacheFolder;

			WinFont winFont = new WinFont(this, fontName, size, bold, italic);
		}
#endif

        private IAudio audio = null;
        private List<Object> defaultOpenedFiles = new List<object>();
        private IRender render = null;
        private FontsFactory fontsFactory = null;
        private ILanguage language = null;
        private ILanguage defaultLanguage = new ILanguage();
        private IScriptsHandler scriptsHandler = null;
        private Dictionary<String, KeyValuePair<Type, IControlsCreator>> controlCreators = new Dictionary<String, KeyValuePair<Type, IControlsCreator>>();
        private Dictionary<String, WindowCreator> windowCreators = new Dictionary<String, WindowCreator>();
        private List<String> keyNames = new List<String>();
        private List<String> mouseKeyNames = new List<string>();
        private String workingFolder = "";
        private Dictionary<String, IPropertyCreator> propertyCreators = new Dictionary<String, IPropertyCreator>();
        private ImagesCache imagesCache;
        private IXmlFactory xmlFactory = new IXmlFactoryImpl();
        private Graphics graphics = null;
        private Dictionary<String, SoundObject> sounds = new Dictionary<String, SoundObject>();
    }
}
