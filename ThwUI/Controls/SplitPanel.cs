using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Renders 2 contros and split between them.
    /// </summary>
    public class SplitPanel : Panel
    {
        /// <summary>
        /// Creates split panel.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
        public SplitPanel(Window window, CreationFlag creationFlags) : base(window, creationFlags)
        {
            this.Type = TypeName;
        }

        /// <summary>
        /// Renders 2 controls ant a split between them.
        /// </summary>
        /// <param name="graphics">graphis to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
            Control c1 = this.Controls.GetEnumerator().Current;
            
            if (true == this.Controls.GetEnumerator().MoveNext())
            {
                Control c2 = this.Controls.GetEnumerator().Current;

                if (null != c1)
                {
                    c1.X = 0;
                    c1.Y = 0;
                    c1.Height = this.Height;
                    c1.Width = (int)(this.Width * this.splitPosition) - this.splitWidth;

                    if (null != c2)
                    {
                        c2.X = c1.Width + this.splitWidth;
                        c2.Y = 0;
                        c2.Height = this.Height;
                        c2.Width = this.Width - c2.X;
                    }
                }
            }

            base.Render(graphics, x, y);
        }

        /// <summary>
        /// Split position. It is in the range [0; 1]; there 0.5 two eal panels would be visible.
        /// </summary>
        public float SplitPosition
        {
            get
            {
                return this.splitPosition;
            }
            set
            {
                this.splitPosition = value;
            }
        }

        /// <summary>
        /// Split separator Width.
        /// </summary>
        public int SplitWidth
        {
            get
            {
                return this.splitWidth;
            }
            set
            {
                this.splitWidth = value;
            }
        }

        /// <summary>
        /// Control name.
        /// </summary>
        internal new static String TypeName
        {
            get
            {
                return "splitPanel";
            }
        }

        private float splitPosition = 0.5f;
        private int splitWidth = 3;
    }
}
