using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Demo
{
	class AcButton : Button
	{
		public AcButton(Window window, CreationFlag creationFlags)
			: base(window, creationFlags)
        {
			this.Type = TypeName;
			this.TextAlignment = ContentAlignment.BottomRight;
        }

		protected override void Render(Graphics graphics, int x, int y)
		{
			bool isOver = (((true == this.isMouseOver) && (this.backColor.A > 0.0f) && (true == this.RenderSelectionOverlay)) || (true == this.HasFocus));

			if (true == isOver)
			{
				x -= 5;
				y -= 5;
				this.Width += 10;
				this.Height += 10;
			}

			base.Render(graphics, x, y);

			if (true == isOver)
			{
				x += 5;
				y += 5;
				this.Width -= 10;
				this.Height -= 10;
			}
		}


		internal static String TypeName
		{
			get
			{
				return "acButton";
			}
		}
	}
}
