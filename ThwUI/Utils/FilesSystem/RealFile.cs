using System;
using System.Collections.Generic;

namespace ThW.UI.Utils.FilesSystem
{
	internal class RealFile : IRealFile
	{
		internal RealFile(String name, IRealFile parent, String fullPath, FileTypes fileType)
        {
			this.parent = parent;
			this.name = name;
			this.fullPath = fullPath;
			this.fileType = fileType;
        }

		public virtual String Name
        {
            get
            {
                return this.name;
            }
        }

		public virtual String FullPath
        {
            get
            {
                return this.fullPath;
            }
        }

		public virtual List<IVirtualFile> ChildFiles
        {
            get
            {
                if (false == this.sorted)
                {
                    SortFiles();
                    this.sorted = true;
                }

                return this.childFiles;
            }
        }

		public virtual FileTypes Type
        {
            get
            {
                return this.fileType;
            }
        }

		public virtual IVirtualFile Parent
        {
            get
            {
                return this.parent;
            }
        }

		protected virtual void ReleaseFiles()
        {
			this.childFiles.Clear();
			this.childsLoaded = false;
			this.sorted = false;
        }

		protected void AddFile(IRealFile pFile)
        {
			if (null != pFile)
			{
				this.childFiles.Add(pFile);
			}
        }

        protected void SortFiles()
        {
            this.childFiles.Sort(new FilesSort());
        }

		protected IRealFile parent = null;
		protected String name = null;
		protected String fullPath = null;
		protected FileTypes fileType = FileTypes.File;
		protected bool childsLoaded = false;
		protected List<IVirtualFile> childFiles = new List<IVirtualFile>();
		protected bool sorted = false;
	}
}
