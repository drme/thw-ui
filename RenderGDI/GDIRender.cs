using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ThW.UI.Utils;

namespace ThW.UI.Sample.Renderers.GDI
{
	public class GDIRender : IRender
	{
		public GDIRender(int ignored, UIEngine engine)
		{
		}

		public void EndRender()
		{
			this.graphics = null;
		}

		public void DrawImage(int x, int y, int w, int h, IImage image, float us, float vs, float ue, float ve, ThW.UI.Utils.Color color, bool outLineOnly)
		{
			if ((null != this.graphics) && (null != image))
			{
				if (true == outLineOnly)
				{
					Pen pen = new Pen(System.Drawing.Color.FromArgb((int)(color.A * 0xff), (int)(color.R * 0xff), (int)(color.G * 0xff), (int)(color.B * 0xff)));

					this.graphics.DrawRectangle(pen, x, y, w, h);
				}
				else
				{
					Image bmp = ((GDIImage)image).Image;

					System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, w, h);

					ImageAttributes imageAttributes = new ImageAttributes();

					float[][] pointsArray =
                    {
                        new float[] { color.R,  0,  0,  0, 0 },
                        new float[] { 0,  color.G,  0,  0, 0 },
                        new float[] { 0,  0,  color.B,  0, 0 },
                        new float[] { 0,  0,  0,  color.A, 0 },
                        new float[] { 0, 0, 0, 0, 1}
                    };

					try
					{
						ColorMatrix clrMatrix = new ColorMatrix(pointsArray);
						imageAttributes.SetColorMatrix(clrMatrix, ColorMatrixFlag.Default, ColorAdjustType.Default);
						imageAttributes.SetWrapMode(System.Drawing.Drawing2D.WrapMode.Tile);
						this.graphics.DrawImage(bmp, rectangle, us * bmp.Width, vs * bmp.Height, bmp.Width * (ue - us), bmp.Height * (ve - vs), GraphicsUnit.Pixel, imageAttributes);
					}
					catch (Exception ex)
					{
						Debug.WriteLine(ex.Message);
					}
				}
			}
		}

		public IImage CreateImage(byte[] fileBytes, String fileName)
		{
			if (fileName.ToLower().EndsWith(".tga"))
			{
				return null;
			}

			using (MemoryStream memoryStream = new MemoryStream(fileBytes))
			{
				try
				{
					Image bmp = Bitmap.FromStream(memoryStream, false, true);

					if (null == bmp)
					{
						return null;
					}

					return new GDIImage(bmp);
				}
				catch (Exception ex)
				{
					Debug.WriteLine(ex.Message);

					return null;
				}
			}
		}

		public IImage CreateImage(String fileName)
		{
			return null;
		}

		public IImage CreateImage(int w, int h, byte[] bytes)
		{
			Bitmap bmp = new Bitmap(w, h);

			for (int j = 0; j < h; j++)
			{
				for (int i = 0; i < w; i++)
				{
					bmp.SetPixel(i, j, System.Drawing.Color.FromArgb(bytes[j * w * 4 + i * 4 + 3], bytes[j * w * 4 + i * 4 + 2], bytes[j * w * 4 + i * 4 + 1], bytes[j * w * 4 + i * 4 + 0]));
				}
			}

			return new GDIImage(bmp);
		}

		public void DrawLine(int x1, int y1, int x2, int y2, ThW.UI.Utils.Color color)
		{
			if (null != this.graphics)
			{
				Pen pen = new Pen(System.Drawing.Color.FromArgb((int)(color.A * 0xff), (int)(color.R * 0xff), (int)(color.G * 0xff), (int)(color.B * 0xff)));

				this.graphics.DrawLine(pen, x1, y1, x2, y2);
			}
		}

		public void SetViewSize(int x, int y, int w, int h)
		{
		}

		public void BeginRender(System.Drawing.Graphics graphics)
		{
			this.graphics = graphics;

			if (null != this.graphics)
			{
				this.graphics.Clear(this.backColor);
			}
		}

		public void SetBackColor(float r, float g, float b, float a)
		{
			this.backColor = System.Drawing.Color.FromArgb((int)(a * 255), (int)(r * 255), (int)(g * 255), (int)(b * 255));
		}

		private System.Drawing.Graphics graphics = null;
		private System.Drawing.Color backColor = new System.Drawing.Color();
	}
}
