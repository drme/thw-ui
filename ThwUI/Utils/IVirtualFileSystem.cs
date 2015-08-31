using System;
using System.Collections.Generic;
using ThW.UI.Utils.FilesSystem;

namespace ThW.UI.Utils
{
	public interface IVirtualFileSystem
	{
		bool OpenFile(String fileName, out byte[] buffer, out uint size, out Object fileNandle);
		void CloseFile(Object fileHandle);
		bool CreateFile(String fileName, byte[] buffer, uint size);
		IVirtualFile RootFile { get; }
		void ReleaseRootFile(IVirtualFile rootFile);
		void GetFiles(String folder, List<String> files);
	}
}
