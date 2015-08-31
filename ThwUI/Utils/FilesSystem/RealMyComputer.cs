using System.Collections.Generic;

namespace ThW.UI.Utils.FilesSystem
{
	internal class RealMyComputer : RealFolder
	{
 		internal RealMyComputer(IRealFile desktop) : base("My Computer", desktop, "", FileTypes.MyComputer)
        {
        }

        public override List<IVirtualFile> ChildFiles
        {
            get
            {
                if (false == this.childsLoaded)
                {
                    foreach (RealFolder folder in FileUtils.Platform.GetDrives(this))
                    {
                        AddFile(folder);
                    }

                    this.childsLoaded = true;

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
