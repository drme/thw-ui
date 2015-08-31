using System;
using System.Collections.Generic;

namespace ThW.UI.Utils.FilesSystem
{
	internal class RealDomain : RealFolder
	{
		internal RealDomain(String name, IRealFile network) : base(name, network, "", FileTypes.Domain)
        {
        }

        public override List<IVirtualFile> ChildFiles
        {
            get
            {
                if (false == this.childsLoaded)
                {
                    foreach (String domain in FileUtils.Platform.GetServers(this.Name))
                    {
                        AddFile(new RealComputer(domain, this));
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
