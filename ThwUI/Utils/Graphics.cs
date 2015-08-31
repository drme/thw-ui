using System;
using System.Collections.Generic;

namespace ThW.UI.Utils
{
    public sealed class Graphics
    {
        /// <summary>
        /// Creates imge by passing loaded image file as bytes array.
        /// </summary>
        public IImage CreateImage(byte[] fileBytes, String fileName)
        {
            return this.render.CreateImage(fileBytes, fileName);
        }

        /// <summary>
        /// Creates image by passing file name.
        /// </summary>
        public IImage CreateImage(String fileName)
        {
           return this.render.CreateImage(fileName);
        }

        /// <summary>
        /// Sets active rendering color.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="alpha"></param>
        public void SetColor(Color color, float alpha)
        {
            if (1.0f != alpha)
            {
                this.activeColor.Values[0] = color.R;
                this.activeColor.Values[1] = color.G;
                this.activeColor.Values[2] = color.B;
                this.activeColor.Values[3] = alpha;
            }
            else
            {
                this.activeColor.Values[0] = color.R;
                this.activeColor.Values[1] = color.G;
                this.activeColor.Values[2] = color.B;
                this.activeColor.Values[3] = color.A;
            }
        }

        /// <summary>
        /// Draws line using the active color.
        /// </summary>
        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            this.render.DrawLine(x1, y1, x2, y2, this.activeColor);
        }

        /// <summary>
        /// Sets active drawing color.
        /// </summary>
        public void SetColor(Color color)
        {
            SetColor(color, 1.0f);
        }

        /// <summary>
        /// Draws image.
        /// </summary>
        public void DrawImage(int x, int y, int w, int h, IImage image)
        {
            DrawImage(x, y, w, h, image, 1.0f, 1.0f);
        }

        /// <summary>
        /// Creates IImage from memory data.
        /// Intended use for creating fonts letters.
        /// UI library will call by passing texture data for render in order to create real texture and return it as an IImage object.
        /// </summary>
        /// <param name="width">image Width</param>
        /// <param name="height">image Height</param>
        /// <param name="bytes">32-bit RGBA image data</param>
        /// <returns></returns>
        public IImage CreateImage(int width, int height, byte[] bytes)
        {
            return this.render.CreateImage(width, height, bytes);
        }

        /// <summary>
        /// Specifies bounding box of region to which perform rendering.
        /// </summary>
        public void SetRegion(int x, int y, int w, int h)
        {
            this.clipViews.Add(this.clipView);

            int x1 = x;
            int y1 = y;
            int x2 = x + w;
            int y2 = y + h;

            if (null != this.clipView)
            {
                if (x1 < this.clipView.X)
                {
                    x1 = this.clipView.X;
                }

                if (y1 < this.clipView.Y)
                {
                    y1 = this.clipView.Y;
                }

                if (x2 > this.clipView.X2)
                {
                    x2 = this.clipView.X2;
                }

                if (y2 > this.clipView.Y2)
                {
                    y2 = this.clipView.Y2;
                }
            }

            this.clipView = new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        /// <summary>
        /// Clears the previous region.
        /// </summary>
        public void ClearRegion()
        {
            this.clipView = this.clipViews[clipViews.Count - 1];
            this.clipViews.RemoveAt(this.clipViews.Count - 1);
        }

        /// <summary>
        /// Draws box using active color.
        /// </summary>
        public void DrawBox(int x, int y, int w, int h)
        {
            DrawImage(x, y, w, h, this.whiteImage, 0.0f, 0.0f, 1.0f, 1.0f, true);
        }

        /// <summary>
        /// Draws image.
        /// </summary>
        /// <param name="u">u texture coord value</param>
        /// <param name="v">v texture coord value</param>
        public void DrawImage(int x, int y, int w, int h, IImage image, float u, float v)
        {
            DrawImage(x, y, w, h, image, 0.0f, 0.0f, u, v, false);
        }

        /// <summary>
        /// Fills rectangle with active color
        /// </summary>
        public void DrawRectangle(int x, int y, int w, int h)
        {
            DrawImage(x, y, w, h, this.whiteImage);
        }

        public void DrawImage(int x, int y, int w, int h, IImage image, float[] uvs)
        {
            DrawImage(x, y, w, h, image, uvs[0], uvs[1], uvs[2], uvs[3], false);
        }

        public void DrawImage(int x, int y, int w, int h, IImage image, float u0, float v0, float u, float v, bool outLineOnly)
        {
           // X = (int)((float)X * 1.5f);
           // Y = (int)((float)Y * 1.5f);
         //   Width = (int)((float)Width * 1.5f);
       //     Height = (int)((float)Height * 1.5f);

            if (null != this.clipView)
            {
                if (y > this.clipView.Y + this.clipView.Height)
                {
                    return;
                }

                if (x > this.clipView.X + this.clipView.Width)
                {
                    return;
                }

                if (x + w < this.clipView.X)
                {
                    return;
                }

                if (y + h < this.clipView.Y)
                {
                    return;
                }

                int h_ = h;

                if ((y <= this.clipView.Y + this.clipView.Height) && (y + h >= this.clipView.Y + this.clipView.Height))
                {
                    v = v * (float)(this.clipView.Y + this.clipView.Height - y) / (float)h_;
                    h = this.clipView.Y + this.clipView.Height - y;
                }

                if ((y <= this.clipView.Y) && (y + h >= this.clipView.Y))
                {
                    v0 = 0.0f + (float)(this.clipView.Y - y) / (float)h_;
                    h = y - this.clipView.Y + h;
                    y = this.clipView.Y;
                }

                if ((x <= this.clipView.X + this.clipView.Width) && (x + w >= this.clipView.X + this.clipView.Width))
                {
                    u = u * (float)(this.clipView.X + this.clipView.Width - x) / (float)w;
                    w = this.clipView.X + this.clipView.Width - x;
                }

                if ((x <= this.clipView.X) && (x + w >= this.clipView.X))
                {
                    u0 = 0.0f + (float)(this.clipView.X - x) / (float)w;
                    w = x - this.clipView.X + w;
                    x = this.clipView.X;
                }

            }

            this.render.DrawImage(x, y, w, h, image, u0, v0, u, v, this.activeColor, outLineOnly);
        }

        internal void SetRender(IRender render)
        {
            if (null != render)
            {
                this.render = render;
                this.whiteImage = this.render.CreateImage(2, 2, new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff } );
            }
            else
            {
                this.render = new NullRender();
                this.whiteImage = null;
            }
        }

        public Color ActiveColor
        {
            get
            {
                return this.activeColor;
            }
        }

        private IImage whiteImage = null;
        private IRender render = new NullRender();
        private Rectangle clipView = null;
        private Color activeColor = new Color(0xff, 0xff, 0xff, 0xff);
        private List<Rectangle> clipViews = new List<Rectangle>();
    }
}
