using System;

namespace ThW.UI.Utils.FilesSystem
{
    /// <summary>
    /// File types used in files chooser window.
    /// </summary>
    public enum FileTypes : int
    {
        Folder = 0,
        File = 1,
        Drive = 5,
        MyComputer = 4,
        MyDocuments = 3,
        Network = 2,
        Domain = 6,
        Desktop = 7,
        Server = 8,
        Share = 9,
        Floppy = 12,
        Disk = 11,
        DVD = 10,
        VirtualFolder = 21,
        VirtualFile = 22,
    }
}
