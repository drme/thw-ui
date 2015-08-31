using System;
using ThW.UI.Controls;

namespace ThW.UI.Utils
{
    class ImageObject : UIObject
    {
        public ImageObject(UIEngine engine, String name) : base("ImageObject", name)
        {
            this.engine = engine;
            this.missing = null == name || name.Length == 0;
        }

		public ImageObject(IImage image, UIEngine engine, String name) : base("ImageObject", name)
		{
			this.engine = engine;
			this.missing = false;
			this.image = image;
		}

        ~ImageObject()
        {
			if (null != this.image)
			{
				this.engine.DeleteImage(ref this.image);
			}
        }

        public void Render(Graphics graphics, int x, int y, int w, int h, Color color, float opacity, ImageLayout layout)
        {
            if (false == this.missing)
            {
                if (null == this.image)
                {
                    this.image = this.engine.CreateImage(this.Name);

                    if (null == this.image)
                    {
                        this.missing = true;
                        
                        return;
                    }
                }

                if ((0.0f == this.image.Width) || (0.0f == this.image.Height))
                {
                    return;
                }

                if ((null == color) || (opacity <= 0.0f) || (opacity == 1.0f && (color.A <= 0.0f)))
                {
                    return;
                }

                graphics.SetColor(color, opacity);

                switch (layout)
                {
                    case ImageLayout.ImageLayoutNone: // +
                        {
                            graphics.SetRegion(x, y, w, h);
                            graphics.DrawImage(x, y, this.image.Width, this.image.Height, this.image);
                            graphics.ClearRegion();
                        }
                        break;
                    case ImageLayout.ImageLayoutCenter: // +
                        {
                            int offX = (w - this.image.Width) / 2;
                            int offY = (h - this.image.Height) / 2;
                            graphics.SetRegion(x, y, w, h);
                            graphics.DrawImage(x + offX, y + offY, this.image.Width, this.image.Height, this.image);
                            graphics.ClearRegion();
                        }
                        break;
                    case ImageLayout.ImageLayoutStretch:
                        graphics.DrawImage(x, y, w, h, this.image);
                        break;
                    case ImageLayout.ImageLayoutTile: // +
                        {
                            float u = (float)(w) / (float)(this.image.Width);
                            float v = (float)(h) / (float)(this.image.Height);
                            graphics.DrawImage(x, y, w, h, this.image, u, v);
                        }
                        break;
                    case ImageLayout.ImageLayoutZoom: // +
                        {
                            float ri = (float)(this.image.Width) / (float)(this.image.Height);

                            int newW = w;
                            int newH = (int)((float)(newW) / ri);
                            int px = x;
                            int py = y + (h - newH) / 2;

                            if (newH > h)
                            {
                                newH = h;
                                newW = (int)((float)(newH) * ri);
                                px = x + (w - newW) / 2;
                                py = y;
                            }

                            graphics.DrawImage(px, py, newW, newH, this.image);
                        }
                        break;
                    case ImageLayout.ImageLayoutFillWidth:
                        {
                            int dy = (int)((w - h) / 2);

                            graphics.SetRegion(x, y, w, h);

                            graphics.DrawImage(x, y - dy, w, h + dy * 2, this.image);

                            graphics.ClearRegion();
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public int Width
        {
            get
            {
                return null != this.image ? this.image.Width : 0;
            }
        }

        public int Height
        {
            get
            {
                return null != this.image ? this.image.Height : 0;
            }
        }

        internal IImage Image
        {
            get
            {
                return this.image;
            }
        }

        private bool missing = false;
        private UIEngine engine;
        private IImage image = null;
    }
}
