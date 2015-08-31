using System;
using System.IO;
using System.Xml.Linq;

namespace ThW.UI.Utils
{
	internal class MsXmlReader : IXmlReader
	{
		public MsXmlReader(UIEngine engine)
        {
            this.engine = engine;
        }

		public virtual void OpenFile(String fileName)
        {
			CloseFile();

			if (0 != fileName.Length)
			{
				byte[] fileBytes = null;
				uint fileSize = 0;
				Object fileHandle = null;

				if (false == this.engine.OpenFile(fileName, out fileBytes, out fileSize, out fileHandle))
				{
					return;
				}

                using (MemoryStream mem = new MemoryStream(fileBytes))
                {
                    this.engine.CloseFile(ref fileHandle);

                    this.xmlDocument = XDocument.Load(mem);
                }
			}
        }

        private void CloseFile()
        {
			this.xmlDocument = null;
			this.rootElement = null;
        }

        public IXmlElement RootElement
        {
            get
            {
                if (null != this.rootElement)
                {
                    return this.rootElement;
                }

                if (null != this.xmlDocument)
                {
                    this.rootElement = new MsXmlElement(this.xmlDocument.Document.Root);
                }

                return this.rootElement;
            }
        }

        public void Dispose()
        {
            CloseFile();
        }

        private UIEngine engine = null;
        private XDocument xmlDocument = null;
        private MsXmlElement rootElement = null;
    }
}
