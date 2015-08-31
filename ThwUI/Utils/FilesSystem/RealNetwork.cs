using System;
using System.Collections.Generic;

namespace ThW.UI.Utils.FilesSystem
{
	internal class RealNetwork : RealFolder
	{
		internal RealNetwork(IRealFile desktop) : base("Network", desktop, "", FileTypes.Network)
        {
        }

        public override List<IVirtualFile> ChildFiles
        {
            get
            {
                if (false == this.childsLoaded)
                {
                    foreach (String domain in FileUtils.Platform.GetDomains())
                    {
                        AddFile(new RealDomain(domain, this));
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
