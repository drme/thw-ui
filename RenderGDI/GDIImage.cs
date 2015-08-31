using ThW.UI.Utils;

namespace ThW.UI.Sample.Renderers.GDI
{
	class GDIImage : IImage
	{
		public GDIImage(System.Drawing.Image bmp) : base()
		{
			this.Image = bmp;
		}

		public System.Drawing.Image Image
		{
			get;
			private set;
		}

		public override int Height
		{
			get
			{
				return this.Image.Height;
			}
		}

		public override bool LoadFailed
		{
			get
			{
				return false;
			}
		}

		public override bool Loaded
		{
			get
			{
				return true;
			}
		}

		public override int Width
		{
			get
			{
				return this.Image.Width;
			}
		}

		public override void Dispose()
		{
			if (null != this.Image)
			{
				this.Image.Dispose();
				this.Image = null;
			}
		}
	}
}
