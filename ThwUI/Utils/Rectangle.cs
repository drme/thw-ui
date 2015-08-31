using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// 2D rectangle bounding box
    /// </summary>
    public class Rectangle
    {
        public Rectangle() : this(0, 0, 0, 0)
        {
        }

		public Rectangle(int x, int y, int w, int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }
					
        public bool IsInside(int x, int y)
        {
            if ((x >= this.X) && (x <= this.X + this.Width) && (y >= this.Y) && (y <= this.Y + this.Height))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Contains(int x, int y)
        {
            return IsInside(x, y);
        }

        public Rectangle Clone()
        {
            return new Rectangle(this.X, this.Y, this.Width, this.Height);
        }

        public int X2
        {
            get
            {
                return this.X + this.Width;
            }
            set
            {
                this.Width = value - this.X;
            }
        }

        public int Y2
        {
            get
            {
                return this.Y + this.Height;
            }
            set
            {
                this.Height = value - this.Y;
            }
        }

        /// <summary>
        /// Rectangles' Width.
        /// </summary>
		public int Width
		{
			get;
			set;
		}

        /// <summary>
        /// Rectangles' Height.
        /// </summary>
		public int Height
		{
			get;
			set;
		}

        /// <summary>
        /// X coordiante.
        /// </summary>
		public int X
		{
			get;
			set;
		}

        /// <summary>
        /// Y coordinate.
        /// </summary>
		public int Y
		{
			get;
			set;
		}

        internal void UpdateSize(int x, int y, int w, int h)
        {
            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }
    }
}
