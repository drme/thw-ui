using System;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Utils.FilesSystem;

namespace ThW.UI.Windows
{
    /// <summary>
    /// File item displayed in file chooser window.
    /// </summary>
	internal class FileListItem : ListItem
	{
        /// <summary>
        /// Creates file list item object.
        /// </summary>
        /// <param name="file">actual file it presents.</param>
        /// <param name="window">window it belongs to.</param>
		public FileListItem(IVirtualFile file, Window window) : base(window, CreationFlag.FlagsNone)
        {
            if (null == file)
            {
                throw new Exception("file can not be null");
            }

            this.file = file;
			this.NeedTranslation = false;
			this.Text = file.Name;
			this.Name = file.Name;
			this.ListStyle = ListStyle.Details;
			this.BackColor = Colors.None;

            SetFileIcon();
        }

        /// <summary>
        /// Loads system icon for file if possible.
        /// </summary>
		private void SetFileIcon()
        {
            switch (this.file.Type)
            {
                case FileTypes.MyComputer:
                    this.icon = new ImageObject(FileUtils.Platform.GetMyComputerIcon(this.ListStyle == ListStyle.LargeIcons, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
                    break;
                case FileTypes.Network:
                    this.icon = new ImageObject(FileUtils.Platform.GetNetworkIcon(this.ListStyle == ListStyle.LargeIcons, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
                    break;
                case FileTypes.Domain:
                    this.icon = new ImageObject(FileUtils.Platform.GetDomainIcon(this.ListStyle == ListStyle.LargeIcons, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
                    break;
                case FileTypes.MyDocuments:
                    this.icon = new ImageObject(FileUtils.Platform.GetMyDocumentsIcon(this.ListStyle == ListStyle.LargeIcons, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
                    break;
                case FileTypes.Share:
                    this.icon = new ImageObject(FileUtils.Platform.GetShareIcon(this.file.FullPath, this.ListStyle == ListStyle.LargeIcons, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
                    break;
                default:
					this.icon = new ImageObject(FileUtils.Platform.GetFileIcon(this.file.FullPath, this.ListStyle == ListStyle.LargeIcons, this.Engine, this.Window.Desktop.Theme), this.Engine, null);
                    break;
            }
        }

        /// <summary>
        /// File presented by list item.
        /// </summary>
		public IVirtualFile File
        {
            get
            {
                return this.file;
            }
        }

        private IVirtualFile file = null;
	}
}
