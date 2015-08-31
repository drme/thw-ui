using System;
using System.Collections.Generic;

namespace ThW.UI.Utils.FilesSystem
{
	internal class RealFolder : RealFile
	{
		internal RealFolder(String name, IRealFile parent, String fullPath, FileTypes fileType) : base(name, parent, fullPath, fileType)
        {
        }

		public override List<IVirtualFile> ChildFiles
        {
            get
            {
                if (true == this.childsLoaded)
                {
                    return base.ChildFiles;
                }
                else
                {
                    List<String> lstFiles = new List<String>();

                    FileUtils.Platform.FillFiles(this.FullPath, lstFiles);

                    foreach (String fileName in lstFiles)
                    {
                        if (fileName[fileName.Length - 1] == '/')
                        {
                            AddFile(new RealFolder(fileName.Substring(0, fileName.Length - 1), this, this.FullPath + "/" + fileName.Substring(0, fileName.Length - 1), FileTypes.Folder));
                        }
                        else
                        {
                            AddFile(new RealFile(fileName, this, this.FullPath + "/" + fileName, FileTypes.File));
                        }
                    }

                    this.childsLoaded = true;

                    return base.ChildFiles;
                }
            }
        }
	}
}
