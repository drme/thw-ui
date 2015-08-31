using System;

namespace ThW.UI.Utils
{
    public class Size2d
    {
        public Size2d(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }

        public Size2d() : this(0, 0)
        {
        }

        /// <summary>
        /// Updates size object with new values.
        /// </summary>
        /// <param name="Width">Width</param>
        /// <param name="Height">Height</param>
        internal void SetSize(int w, int h)
        {
            this.Width = w;
            this.Height = h;
        }

		public int Width
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}
    }
}
