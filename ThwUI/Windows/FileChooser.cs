using System;
using System.Collections.Generic;
using System.Diagnostics;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Utils.FilesSystem;

namespace ThW.UI.Windows
{
    /// <summary>
    /// File chooser window.
    /// </summary>
	public class FileChooser : Window
	{
        /// <summary>
        /// Creates files chooser window.
        /// </summary>
        /// <param name="desktop">desktop it belongs to.</param>
        public FileChooser(Desktop desktop) : base(desktop, CreationFlag.FlagsNone, "")
        {
            this.filesListListBox = CreateControl<ListBox>();
            this.filtersComboBox = CreateControl<ComboBox>();
            this.fileNameTextBox = CreateControl<TextBox>();
            this.desktopItem = CreateControl<ListItem>();
            this.myDocumentsItem = CreateControl<ListItem>();
            this.myComputerItem = CreateControl<ListItem>();
            this.NetworkItem = CreateControl<ListItem>();
            this.virtualFileSystemItem = CreateControl<ListItem>();
            this.drivesComboBox = CreateControl<ComboBox>();
            this.upButton = CreateControl<Button>();
            this.selectButton = CreateControl<Button>();
            this.filesScrollPanel = CreateControl<ScrollPanel>();

            this.BackColor = this.Desktop.Theme.Colors.Control;
            this.MinSize = new Size2d(563, 412);
            this.Text = "Select File";
            this.Opacity = 1.0f;
            this.Modal = true;
            this.Bounds = new Rectangle((desktop.Width - this.MinSize.Width) / 2, (desktop.Height - this.MinSize.Height) / 2, this.MinSize.Width, this.MinSize.Height);
            this.HasShadow = true;

            this.selectButton.Anchor = AnchorStyle.AnchorBottom | AnchorStyle.AnchorRight;
            this.selectButton.Text = "Open";
            this.selectButton.Bounds = new Rectangle(477, 330, 75, 23);
            this.selectButton.DialogResult = DialogResult.DialogResultOK;
            AddControl(this.selectButton);

            Button cancelButton = CreateControl<Button>();
            cancelButton.Anchor = AnchorStyle.AnchorBottom | AnchorStyle.AnchorRight;
            cancelButton.Text = "Cancel";
            cancelButton.Bounds = new Rectangle(477, 357, 75, 23);
            cancelButton.DialogResult = DialogResult.DialogResultCancel;
            AddControl(cancelButton);

            Label lookInLabel = CreateControl<Label>();
            lookInLabel.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorTop;
            lookInLabel.Text = "Look in:";
            lookInLabel.Bounds = new Rectangle(10, 10, 87, 23);
            AddControl(lookInLabel);

            Label fileNameLabel = CreateControl<Label>();
            fileNameLabel.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorBottom;
            fileNameLabel.Text = "File name:";
            fileNameLabel.Bounds = new Rectangle(103, 331, 97, 23);
            AddControl(fileNameLabel);

            Label filterLabel = CreateControl<Label>();
            filterLabel.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorBottom;
            filterLabel.Text = "Files of type:";
            filterLabel.Bounds = new Rectangle(103, 359, 97, 23);
            AddControl(filterLabel);

            ListBox sideListBox = CreateControl<ListBox>();
            sideListBox.Opacity = 0.5f;
            sideListBox.Border = BorderStyle.BorderLoweredDouble;
            sideListBox.BackColor = this.Desktop.Theme.Colors.ControlDark;
            sideListBox.AutoSize = false;
            sideListBox.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorTop | AnchorStyle.AnchorBottom;
            sideListBox.Bounds = new Rectangle(10, 39, 87, 338);
            AddControl(sideListBox);

            this.desktopItem.Bounds = new Rectangle(2, 2, 82, 60);
            this.desktopItem.Text = "Desktop";
            this.desktopItem.Clicked += (sender, args) => { FillList(this.desktopFolder); };
            sideListBox.AddControl(this.desktopItem);

            this.myDocumentsItem.Bounds = new Rectangle(2, 2, 62, 60);
            this.myDocumentsItem.Text = "My Documents";
            this.myDocumentsItem.Clicked += (sender, args) => { FillList(GetFile(this.desktopFolder, FileTypes.MyDocuments)); };
            sideListBox.AddControl(this.myDocumentsItem);

            this.myComputerItem.Bounds = new Rectangle(2, 2, 122, 60);
            this.myComputerItem.Text = "My Computer";
            this.myComputerItem.Clicked += (sender, args) => { FillList(GetFile(this.desktopFolder, FileTypes.MyComputer)); };
            sideListBox.AddControl(this.myComputerItem);

            this.NetworkItem.Bounds = new Rectangle(2, 2, 182, 60);
            this.NetworkItem.Text = "Network";
            this.NetworkItem.Clicked += (sender, args) => { FillList(GetFile(this.desktopFolder, FileTypes.Network)); };
            sideListBox.AddControl(this.NetworkItem);

            this.virtualFileSystemItem.Bounds = new Rectangle(2, 2, 242, 60);
            this.virtualFileSystemItem.Text = "Virtual FS";
            this.virtualFileSystemItem.Clicked += this.VirtualFileSystemClicked;
            sideListBox.AddControl(this.virtualFileSystemItem);

            this.filesScrollPanel.Border = BorderStyle.BorderLoweredDouble;
            this.filesScrollPanel.BackColor = this.Window.Desktop.Theme.Colors.Window;
            this.filesScrollPanel.Anchor = AnchorStyle.AnchorBottom | AnchorStyle.AnchorLeft | AnchorStyle.AnchorTop | AnchorStyle.AnchorRight;
            this.filesScrollPanel.Opacity = 0.5f;
            this.filesScrollPanel.Bounds = new Rectangle(103, 39, 450, 283);
            AddControl(this.filesScrollPanel);

            this.filesListListBox.Border = BorderStyle.None;
            this.filesListListBox.BackColor = Colors.None;
            this.filesListListBox.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorTop;
            this.filesListListBox.Bounds = new Rectangle(0, 0, 430, 0);
            this.filesListListBox.ListStyle = ListStyle.Details;
            this.filesListListBox.AutoSize = true;
            this.filesScrollPanel.AddControl(this.filesListListBox);

            this.fileNameTextBox.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorBottom;
            this.fileNameTextBox.Bounds = new Rectangle(199, 332, 266, 22);
            AddControl(this.fileNameTextBox);

            this.drivesComboBox.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
            this.drivesComboBox.Bounds = new Rectangle(103, 10, 421, 22);
            this.drivesComboBox.SelectedItemChanged += new UIEventHandler<ComboBox>(DrivesSelectedItemChanged);
            this.drivesComboBox.NeedTranslation = false;
            AddControl(this.drivesComboBox);

            this.filtersComboBox.Anchor = AnchorStyle.AnchorLeft | AnchorStyle.AnchorRight | AnchorStyle.AnchorBottom;
            this.filtersComboBox.Bounds = new Rectangle(199, 359, 266, 22);
            this.filtersComboBox.SelectedItemChanged += (sender, args) => { FillList(this.activeVirtualFolder); };
            AddControl(this.filtersComboBox);

            this.upButton.Anchor = AnchorStyle.AnchorRight | AnchorStyle.AnchorTop;
            this.upButton.Bounds = new Rectangle(528, 10, 23, 23);
            this.upButton.Icon = (this.Window.Desktop.Theme.ThemeFolder + "icons/button_up");
            this.upButton.Clicked += this.UpButtonClicked;
            AddControl(this.upButton);

            IVirtualFile myComputer = GetFile(this.desktopFolder, FileTypes.MyComputer);

            if (null != myComputer)
            {
                foreach (IVirtualFile file in myComputer.ChildFiles)
                {
                    ComboBoxItem item = this.drivesComboBox.AddItem(file.Name, file.Name, "", file);

                    item.SetIcon(FileUtils.Platform.GetFileIcon(file.Name, false, this.Engine, this.Window.Desktop.Theme));
                }
            }

            this.desktopItem.icon = new ImageObject(FileUtils.Platform.GetDesktopIcon(true, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
            this.myDocumentsItem.icon = new ImageObject(FileUtils.Platform.GetMyDocumentsIcon(true, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
            this.myComputerItem.icon = new ImageObject(FileUtils.Platform.GetMyComputerIcon(true, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
            this.NetworkItem.icon = new ImageObject(FileUtils.Platform.GetNetworkIcon(true, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
			this.virtualFileSystemItem.icon = new ImageObject(FileUtils.Platform.GetIcon(4, true, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
              
            List<String> extensions = new List<String>();

            extensions.Add("");
            AddFilter("All files (*.*)", extensions);

            extensions = new List<String>();
            extensions.Add(".avi");
            extensions.Add(".omg");
            extensions.Add(".mpg");
            AddFilter("Video files (*.avi; *.omg; *.mpg)", extensions);

            extensions = new List<String>();
            extensions.Add(".mp3");
            extensions.Add(".ogg");
            extensions.Add(".wav");
            AddFilter("Audio files (*.mp3; *.ogg; *.wav)", extensions);

            extensions = new List<String>();
            extensions.Add(".txt");
            extensions.Add(".doc");
            extensions.Add(".pdf");
            AddFilter("Documents (*.txt; *.doc; *.pdf)", extensions);

            extensions = new List<String>();
            extensions.Add(".png");
            extensions.Add(".jpg");
            extensions.Add(".tga");
            AddFilter("Images (*.png; *.jpg; *.tga)", extensions);

            FillList(this.desktopFolder);
        }

        private void UpButtonClicked(Control sender, EventArgs args)
        {
            if (null != this.activeVirtualFolder)
            {
                FillList(this.activeVirtualFolder.Parent);
            }
        }

        private void VirtualFileSystemClicked(Control sender, EventArgs args)
        {
            if (null == this.VirtualFileSystemRoot)
            {
                if (null != this.Engine.VirtualFileSystem)
                {
                    this.VirtualFileSystemRoot = this.Engine.VirtualFileSystem.RootFile;
                }
            }

            if (null != this.VirtualFileSystemRoot)
            {
                FillList(this.VirtualFileSystemRoot);
            }
        }

        private void DrivesSelectedItemChanged(ComboBox sender, EventArgs args)
        {
            ComboBoxItem selectedItem = this.drivesComboBox.SelectedItem;

            if (null != selectedItem)
            {
                FillList((IRealFile)selectedItem.Tag);
            }
        }

		~FileChooser()
        {
			if (null != this.VirtualFileSystemRoot)
			{
                if (null != this.Engine.VirtualFileSystem)
				{
                    this.Engine.VirtualFileSystem.ReleaseRootFile(this.VirtualFileSystemRoot);
					this.VirtualFileSystemRoot = null;
				}
				else
				{
                    Debug.WriteLine("Leaked vfs object");
				}
			}
        }

        /// <summary>
        /// Renders window.
        /// </summary>
        /// <param name="graphics">graphics to render to</param>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
			if (this.filesToLoad.Count > 0)
			{
				IVirtualFile fileToLoad = this.filesToLoad[0];

				if (true == FilterFile(fileToLoad))
				{
					FileListItem fileListItem = new FileListItem(fileToLoad, this);
					this.filesListListBox.AddControl(fileListItem);
					this.WindowControls.Add(fileListItem);
                    fileListItem.Clicked += this.FileItemClicked;
				}

                this.filesToLoad.Remove(fileToLoad);
			}

			this.filesListListBox.Width = this.filesScrollPanel.Bounds.Width - 20;

			base.Render(graphics, x, y);
        }

        private void FileItemClicked(Control sender, EventArgs args)
        {
            FileListItem listItem = (FileListItem)sender;

            if (null != listItem)
            {
                IVirtualFile file = listItem.File;

                if (null != file)
                {
                    if (FileTypes.File != file.Type)
                    {
                        FillList(file);

                        this.selectedVirtualFile = null;
                    }
                    else
                    {
                        this.fileNameTextBox.Text = file.Name;
                        this.selectedVirtualFile = file;
                    }
                }
            }
        }

        /// <summary>
        /// Closes window.
        /// </summary>
		public override void Close()
        {
			if ((DialogResult.DialogResultOK != this.DialogResult) || (false == this.FileMustExist))
			{
				base.Close();
				
                return;
			}

			if (true == this.selectFolder)
			{
				if (null == this.activeVirtualFolder)
				{
					ShowFileMustExist(true);
					return;
				}

				if (this.fileNameTextBox.Text.Length == 0)
				{
					base.Close();
					return;
				}

				if (true == FileExistsInFolder(this.activeVirtualFolder, this.fileNameTextBox.Text, true))
				{
					base.Close();
					return;
				}
				else
				{
					ShowFileMustExist(true);
					return;
				}
			}

			if (null == this.selectedVirtualFile)
			{
				if (null == this.activeVirtualFolder)
				{
					ShowFileMustExist(false);
					return;
				}

				if (true == FileExistsInFolder(this.activeVirtualFolder, this.fileNameTextBox.Text, false))
				{
					base.Close();
					return;
				}
				else
				{
					ShowFileMustExist(false);
					return;
				}
			}

			if (this.selectedVirtualFile.Name == this.fileNameTextBox.Text)
			{
				base.Close();
				return;
			}
			else
			{
				if (null == this.activeVirtualFolder)
				{
					ShowFileMustExist(false);
					return;
				}

				if (true == FileExistsInFolder(this.activeVirtualFolder, this.fileNameTextBox.Text, false))
				{
					base.Close();
					return;
				}
				else
				{
					ShowFileMustExist(false);
					return;
				}
			}
        }

        /// <summary>
        /// Adds filter to the filters list.
        /// </summary>
        /// <param name="filterName">filter name</param>
        /// <param name="allowedExtensions">allowed extensions list</param>
        /// <returns></returns>
		public int AddFilter(String filterName, List<String> allowedExtensions)
        {
			this.filters.Add(allowedExtensions);

			if (null != this.filtersComboBox)
			{
				this.filtersComboBox.AddItem(filterName, filterName, "", null);
			}

			return (this.filters.Count) - 1;
        }

        /// <summary>
        /// Active filter position.
        /// </summary>
		public int ActiveFilter
        {
            set
            {
                if ((value >= 0) && (value < (this.filters.Count)))
                {
                    if (null != this.filtersComboBox)
                    {
                        this.filtersComboBox.SelectedItemIndex = value;
                    }
                }
            }
        }

        /// <summary>
        /// Selected file path
        /// </summary>
        public String SelectedFilePath
        {
            get
            {
                if (true == this.selectFolder)
                {
                    if (null != this.activeVirtualFolder)
                    {
                        if (this.fileNameTextBox.Text.Length > 0)
                        {
                            return this.activeVirtualFolder.FullPath + "/" + this.fileNameTextBox.Text;
                        }
                        else
                        {
                            return this.activeVirtualFolder.FullPath;
                        }
                    }
                    else
                    {
                        return "";
                    }
                }

                if (null != this.selectedVirtualFile)
                {
                    if (null != this.fileNameTextBox)
                    {
                        if (this.fileNameTextBox.Text == this.selectedVirtualFile.Name)
                        {
                            return this.selectedVirtualFile.FullPath;
                        }
                        else
                        {
                            if (null != this.activeVirtualFolder)
                            {
                                return this.activeVirtualFolder.FullPath + this.fileNameTextBox.Text;
                            }
                        }
                    }
                    else
                    {
                        return this.selectedVirtualFile.FullPath;
                    }
                }
                else
                {
                    if (null != this.activeVirtualFolder)
                    {
                        if (null != this.fileNameTextBox)
                        {
                            return this.activeVirtualFolder.FullPath + this.fileNameTextBox.Text;
                        }
                        else
                        {
                            return this.activeVirtualFolder.FullPath;
                        }
                    }
                    else
                    {
                        if (null != this.fileNameTextBox)
                        {
                            return this.fileNameTextBox.Text;
                        }
                    }
                }

                return "";
            }
        }

        /// <summary>
        /// Select button text. Like open ir save.
        /// </summary>
        public String SelectButtonText
        {
            set
            {
                this.selectButton.Text = value;
            }
        }

        /// <summary>
        /// File must exists.
        /// Dialog will not accept close if selected file is not available.
        /// </summary>
        public bool FileMustExist
        {
            set
            {
                this.fileMustExist = value;
            }
            get
            {
                return this.fileMustExist;
            }
        }

        /// <summary>
        /// Can the folder be selected.
        /// </summary>
		public bool SelectFolder
        {
            set
            {
                this.selectFolder = value;
            }
        }

		private void FillList(IVirtualFile file)
        {
            if ((null == file) || (null == this.filesListListBox))
            {
                return;
            }

            this.activeVirtualFolder = file;

            this.filesToLoad = new List<IVirtualFile>();
                
            foreach (IVirtualFile virtualFile in this.activeVirtualFolder.ChildFiles)
            {
                this.filesToLoad.Add(virtualFile);
            }                

            if (null != this.filesListListBox)
            {
                this.filesListListBox.ClearControls();
            }
        }

		private IVirtualFile GetFile(IVirtualFile folder, FileTypes fileType)
        {
			if (null == folder)
			{
				return null;
			}

			foreach (IVirtualFile file in folder.ChildFiles)
			{
				if (file.Type == fileType)
				{
					return file;
				}
			}

			return null;
        }

		private bool FilterFile(IVirtualFile file)
        {
			if ( (file.Type != FileTypes.File) || (null == this.filtersComboBox) )
			{
				return true;
			}

			int index = this.filtersComboBox.SelectedItemIndex;

            if ((index < 0) || ((index) >= this.filters.Count))
            {
                return true;
            }

			if (this.filters[index].Count <= 0)
			{
				return true;
			}

			String fileName = file.Name;

			foreach (String extension in this.filters[index])
			{
				if (fileName.Length > extension.Length)
				{
					if (fileName.Substring(fileName.Length - extension.Length) == extension)
					{
						return true;
					}
				}
			}

			return false;
        }

		private void ShowFileMustExist(bool folder)
        {
            MessageBox messageBox = (MessageBox)this.Desktop.NewRegisteredWindow(MessageBox.TypeName);

			if (null != messageBox)
			{
				messageBox.Text = "Error";
				
				if (true == folder)
				{
					messageBox.MessageLine1 = "Selected folder name does not exist";
					messageBox.YesButtonText = "OK";
				}
				else
				{
					messageBox.MessageLine1 = "Selected file name does not exist";
					messageBox.YesButtonText = "OK";
				}
			}
        }

        private bool FileExistsInFolder(IVirtualFile folder, String fileName, bool isFolder)
        {
            if ((null == folder) || (fileName.Length == 0))
            {
                return false;
            }

            foreach (IVirtualFile virtualFile in this.activeVirtualFolder.ChildFiles)
			{
				if (this.fileNameTextBox.Text == virtualFile.Name)
				{
					if (virtualFile.Type == FileTypes.File)
					{
						if (true == isFolder)
						{
							return false;
						}
						else
						{
							return true;
						}
					}
					else
					{
						if (true == isFolder)
						{
							return true;
						}
						else
						{
							return false;
						}
					}
				}
			}

			return false;
        }

        /// <summary>
        /// File chooser window type name
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "ui.fileChooser";
            }
        }

        private ListBox filesListListBox = null;
        private ComboBox filtersComboBox = null;
        private TextBox fileNameTextBox = null;
        private ListItem desktopItem = null;
        private ListItem myDocumentsItem = null;
        private ListItem myComputerItem = null;
        private ListItem NetworkItem = null;
        private ListItem virtualFileSystemItem = null;
        private ComboBox drivesComboBox = null;
        private Button upButton = null;
        private Button selectButton = null;
        private ScrollPanel filesScrollPanel = null;
		private IVirtualFile selectedVirtualFile = null;
		private IVirtualFile activeVirtualFolder = null;
		private IVirtualFile VirtualFileSystemRoot = null;
		private List<IVirtualFile> filesToLoad = new List<IVirtualFile>();
		private List<List<String> > filters = new List<List<String>>();
		private bool fileMustExist = false;
		private bool selectFolder = false;
        private IVirtualFile desktopFolder = new RealDesktop();
	}
}
