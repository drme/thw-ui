using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThW.UI.Utils;
using ThW.UI.Utils.FilesSystem;

namespace ThW.UI.Designer
{
	public partial class FontsExportForm : Form
	{
		public FontsExportForm(Form parent, HashSet<Font> fonts) : base()
		{
			InitializeComponent();

			foreach (var font in fonts)
			{
				this.listBoxFonts.Items.Add(new FontToExport(font));
			}
		}

		private void SelectFolderClick(Object sender, EventArgs e)
		{
			if (DialogResult.OK == this.folderBrowserDialog.ShowDialog(this))
			{
				this.textBoxSaveLocation.Text = this.folderBrowserDialog.SelectedPath;
				this.buttonExport.Enabled = true;
			}
		}

		private void AddFontClick(Object sender, EventArgs e)
		{
			if (DialogResult.OK == this.fontDialog.ShowDialog(this))
			{
				this.listBoxFonts.Items.Add(new FontToExport(this.fontDialog.Font));
			}
		}

		private void RemoveFontClick(Object sender, EventArgs e)
		{
			var selectedItems = new List<Object>();

			foreach (var item in this.listBoxFonts.SelectedItems)
			{
				selectedItems.Add(item);
			}

			foreach (var item in selectedItems)
			{
				this.listBoxFonts.Items.Remove(item);
			}
		}

		private void ExportFontsClick(Object sender, EventArgs e)
		{
			foreach (FontToExport font in this.listBoxFonts.Items)
			{
				UIEngine engine = new UIEngine();
				engine.Render = null;
				engine.VirtualFileSystem = new FilesSystem(this.textBoxSaveLocation.Text);

				engine.CacheFont(font.Font.Name, (int)font.Font.Size, font.Font.Bold, font.Font.Italic, "/");
			}

			MessageBox.Show(this, "Exported", "Export fonts");
		}
	}

	class FilesSystem : IVirtualFileSystem
	{
		public FilesSystem(String root)
		{
			this.root = root;
		}

		public bool OpenFile(String fileName, out byte[] buffer, out uint size, out Object fileNandle)
		{
			buffer = null;
			size = 0;
			fileNandle = null;

			return false;
		}

		public void CloseFile(Object fileHandle)
		{
		}

		public bool CreateFile(String fileName, byte[] buffer, uint size)
		{
			String fullPath = this.root + fileName;

			fullPath = fullPath.Replace('\\', '/').Replace("//", "/");

			String[] dirs = fullPath.Split('/');

			String folder = "";

			for (int i = 0; i < dirs.Length - 1; i++)
			{
				folder += dirs[i] + "/";

				if (false == Directory.Exists(folder))
				{
					Directory.CreateDirectory(folder);
				}
			}

			using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(buffer, 0, (int)size);
			}

			return true;
		}

		public Utils.FilesSystem.IVirtualFile RootFile
		{
			get
			{
				return null;
			}
		}

		public void ReleaseRootFile(IVirtualFile rootFile)
		{
		}

		public void GetFiles(String folder, List<String> files)
		{
		}

		private String root;
	}
}
