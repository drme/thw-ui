using System;
using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Demo
{
	class MenuRibbon : Panel
	{
		public MenuRibbon(Window window, CreationFlag creationFlags) : base(window, creationFlags)
        {
			this.Type = TypeName;

			this.menuButton = CreateControl<Button>();

			this.menuButton.Height = 28;
			this.menuButton.Text = "Menu";
			this.menuButton.FontInfo.Name = "Calibri";
			this.menuButton.FontInfo.Bold = true;
			this.menuButton.FontInfo.Size = 20;
			this.menuButton.TextColor = Colors.White;
			this.menuButton.BackColor = Colors.None;
			this.menuButton.Border = BorderStyle.None;
			this.menuButton.Clicked += (x, y) => { this.Active = !this.Active; };

			AddControl(this.menuButton);

			this.BackColor = Colors.None;
        }

		protected override void Render(Graphics graphics, int x, int y)
		{
			this.menuButton.Width = this.Width - 80;
			this.menuButton.X = 40;
			this.menuButton.Y = this.Height - 100;

			int y1 = this.Height - 150;

			foreach (var button in this.Controls)
			{
				if ((button != this.menuButton))
				{
					button.Width = this.Width-80;
					button.Height = 28;
					button.FontInfo.Name = "Calibri";
					button.FontInfo.Bold = true;
					button.FontInfo.Size = 18;
					button.TextColor = Colors.White;
					button.BackColor = Colors.None;
					button.Border = BorderStyle.None;
					button.X = 40;
					button.Y = y1;
					button.Visible = this.Active;

					y1 -= 50;
				}
			}

			graphics.SetColor(Colors.Red);

			if (this.Active == true)
			{
				var rx = (int)(5.0 * Math.Sin(Math.PI * 2 * DateTime.Now.Millisecond / 1000));
				var rw = this.Width - 20 + (int)(5.0 * Math.Cos(Math.PI * 2 * DateTime.Now.Millisecond / 1000));

				graphics.DrawBox(this.Bounds.X + x + rx, this.Bounds.Y + y + 0, rw, this.Height);
			}
			else
			{
				var rx = (this.Width - 15) / 2 + (int)(5.0 * Math.Sin(Math.PI * 2 * DateTime.Now.Millisecond / 1000));
				var rw = 15 + (int)(5.0 * Math.Cos(Math.PI * 2 * DateTime.Now.Millisecond / 1000));

				graphics.DrawBox(this.Bounds.X + x + rx, this.Bounds.Y + y + 0, rw, this.Height);
			}

			base.Render(graphics, x, y);
		}

		public override String Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = "";
				this.menuButton.Text = value;
			}
		}

		internal static String TypeName
		{
			get
			{
				return "menuRibbon";
			}
		}

		public bool Active
		{
			get
			{
				return this.active;
			}
			set
			{
				this.active = value;

				if (null != this.ActiveChanged)
				{
					this.ActiveChanged(this, null);
				}
			}
		}

		private bool active = false;
		private Button menuButton;
		public event EventHandler ActiveChanged;
	}
}
