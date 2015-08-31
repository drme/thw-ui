using Microsoft.Xna.Framework.Graphics;
using System.IO;
using ThW.UI.Utils;

namespace ThW.UI.Sample.Renderers.XNA
{
	class XNAImage : IImage
	{
		public XNAImage(GraphicsDevice device, int w, int h, byte[] imageBytes) : base()
		{
			this.width = w;
			this.height = h;
			this.texture = new Texture2D(device, this.width, this.height, true, SurfaceFormat.Color);
			this.texture.SetData<byte>(imageBytes);
		}

		public XNAImage(Texture2D texture)
		{
			this.width = texture.Width;
			this.height = texture.Height;
			this.texture = texture;
		}

		public XNAImage(GraphicsDevice device, byte[] fileBytes)
		{
			using (MemoryStream data = new MemoryStream(fileBytes))
			{
				this.texture = Texture2D.FromStream(device, data);
				this.width = this.texture.Width;
				this.height = this.texture.Height;
			}
		}

		public override int Width
		{
			get
			{
				return this.width;
			}
		}

		public override int Height
		{
			get
			{
				return this.height;
			}
		}

		public override bool Loaded
		{
			get
			{
				return true;
			}
		}

		public override bool LoadFailed
		{
			get
			{
				return false;
			}
		}

		public Texture2D Texture
		{
			get
			{
				return this.texture;
			}
		}

		public override void Dispose()
		{
			if (null != this.texture)
			{
				this.texture.Dispose();
				this.texture = null;
			}
		}

		private int width;
		private int height;
		private Texture2D texture = null;
	}
}
