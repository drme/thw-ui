using System;
using System.Collections.Generic;

namespace ThW.UI.Utils.FilesSystem
{
	internal class RealComputer : RealFolder
	{
		internal RealComputer(String name, IRealFile domain) : base(name, domain, "\\\\" + name, FileTypes.Server)
        {
        }

		public override List<IVirtualFile> ChildFiles
        {
            get
            {
                if (false == this.childsLoaded)
                {
                    foreach (String strName in FileUtils.Platform.GetShares(this.Name))
                    {
                        int len = strName.Length;

                        if (strName[len - 1] != '$')
                        {
                            AddFile(new RealFolder(strName, this, this.FullPath + "\\" + strName + "\\", FileTypes.Share));
                        }
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
