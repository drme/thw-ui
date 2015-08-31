using System.Collections.Generic;

namespace ThW.UI.Utils.FilesSystem
{
	internal class RealDesktop : RealFolder
	{
        internal RealDesktop() : base("Desktop", null, FileUtils.GetSystemFolder(FileTypes.Desktop), FileTypes.Desktop)
        {
        }

        public override List<IVirtualFile> ChildFiles
        {
            get
            {
                if ((false == this.childsLoaded) || (this.childFiles.Count == 0))
                {
                    AddFile(new RealFolder("My Documents", this, FileUtils.GetSystemFolder(FileTypes.MyDocuments), FileTypes.MyDocuments));
                    AddFile(new RealMyComputer(this));
                    AddFile(new RealNetwork(this));

                    return base.ChildFiles;
                }
                else
                {
                    return base.ChildFiles;
                }
            }
        }
	}
}
