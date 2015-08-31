using System;
using System.Collections.Generic;

namespace ThW.UI.Utils.FilesSystem
{
    /// <summary>
    /// Interface for file choser window files
    /// </summary>
    public interface IVirtualFile
    {
        /// <summary>
        /// If this file is folder, retuns a list of files in the folder
        /// </summary>
        List<IVirtualFile> ChildFiles
        {
            get;
        }

        /// <summary>
        /// File name (name + extension)
        /// </summary>
        String Name
        {
            get; 
        }

        /// <summary>
        /// The parent folder where this file resides
        /// </summary>
        IVirtualFile Parent
        {
            get;
        }

        /// <summary>
        /// File type
        /// </summary>
        FileTypes Type
        {
            get;
        }

        /// <summary>
        /// The full path of file/folder/network share, etc...
        /// </summary>
        String FullPath
        {
            get;
        }
    }
}
