using System;
using System.Collections.Generic;

namespace ThW.UI.Utils
{
	internal class TextXmlWriter : IXmlWriter
	{
		public TextXmlWriter(UIEngine engine)
        {
            this.engine = engine;
        }

		public void Dispose()
        {
			CloseFile();
        }

		public virtual void OpenFile(String fileName)
        {
			CloseFile();

			this.fileName = fileName;

			WriteString("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>\n");
        }

		private void CloseFile()
        {
			if (this.fileName.Length > 0)
			{
				String strXml = this.xml;

                this.engine.CreateFile(this.fileName, System.Text.Encoding.GetEncoding("UTF-8").GetBytes(strXml), 0);
			}

			this.fileName = "";
			this.hasInside = false;
			this.level = 0;
			
			while (this.tags.Count > 0)
			{
                this.tags.Clear();
			}

			while (this.inside.Count > 0)
			{
                this.inside.Clear();
			}
        }

		public virtual void OpenTag(String name)
        {
			if ((this.tags.Count > 0) && (this.hasInside == false))
			{
				WriteString(" >\n");
			}

			WriteTabs();

			WriteString("<" + (name));

			this.level++;

			this.tags.Add(name);
			this.inside.Add(true);

			this.hasInside = false;
        }

		public virtual void CloseTag()
        {
			this.level--;

			if (false == this.hasInside)
			{
				WriteString(" />\n");
			}
			else
			{
				WriteTabs();

				WriteString(("</") + (this.tags[this.tags.Count - 1]) + ">\n");
			}

            this.tags.RemoveAt(this.tags.Count - 1);

            this.hasInside = this.inside[this.inside.Count - 1];

            this.inside.RemoveAt(this.inside.Count - 1);
        }

		public virtual void WriteAttribute(String name, String value)
        {
			WriteString(" " + (name) + "=\"" + (value) + "\"");
        }

		public virtual void Release()
        {
            CloseFile();
        }

		private void WriteTabs()
        {
			for (int i = 0; i < this.level; i++)
			{
				WriteString("\t");
			}
        }

		private void WriteString(String text)
        {
            this.xml += text;
        }

        private UIEngine engine = null;
		private bool hasInside = false;
        private List<String> tags = new List<String>();
		private List<bool> inside = new List<bool>();
		private int level = 0;
		private String xml = "";
		private String fileName = "";
	}
}
