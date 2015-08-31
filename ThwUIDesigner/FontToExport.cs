using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThW.UI.Designer
{
	class FontToExport
	{
		public FontToExport(System.Drawing.Font font)
		{
			this.Font = font;
		}

		public override string ToString()
		{
			return this.Font.Name + " " + this.Font.Size + "pt " + (this.Font.Italic ? "Italic" : "") + " " + (this.Font.Bold ? "Bold" : "");
		}

		public System.Drawing.Font Font
		{
			get;
			private set;
		}
	}
}
