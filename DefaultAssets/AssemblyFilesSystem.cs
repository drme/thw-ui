using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ThW.UI.Utils;
using ThW.UI.Utils.FilesSystem;

namespace ThW.UI.Sample
{
    public class AssemblyFilesSystem : IVirtualFileSystem
    {
        public bool OpenFile(String fileName, out byte[] fileBuffer, out uint fileSize, out Object fileHandle)
        {
            fileName = fileName.Replace("//", ".").Replace("/", ".");

            foreach (String name in this.GetType().Assembly.GetManifestResourceNames())
            {
                if (name.EndsWith(fileName))
                {
                    Stream stream = Assembly.GetAssembly(GetType()).GetManifestResourceStream(name);

                    if (null != stream)
                    {
                        byte[] buffer = new byte[16 * 1024];

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            int read;
                            
							while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                memoryStream.Write(buffer, 0, read);
                            }
                            
							fileBuffer = memoryStream.ToArray();
                            fileSize = (uint)fileBuffer.Length;
                            fileHandle = fileBuffer;

                            return true;
                        }
                    }
                }
            }

            fileHandle = null;
            fileBuffer = null;
            fileSize = 0;

            return false;
        }

        public void CloseFile(Object fileHandle)
        {
        }

        public bool CreateFile(string fileName, byte[] dataBuffer, uint bufferSize)
        {
            return false;
        }

        public IVirtualFile RootFile
        {
			get
			{
				return null;
			}
        }

        public void ReleaseRootFile(IVirtualFile rootFile)
        {
        }

        public void GetFiles(String strFolder, List<String> files)
        {
        }
    }
}
