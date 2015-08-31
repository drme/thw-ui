using System;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Display progress information in scale from 0.0 to 1.0.
    /// </summary>
	public class ProgressBar : Control
	{
        public ProgressBar(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
            this.Border = BorderStyle.Lowered;
            this.ProgressBarColor = window.Desktop.Theme.Colors.Highlight;
        }

        /// <summary>
        /// Adds progress bar properties.
        /// </summary>
        protected override void AddProperties()
        {
            base.AddProperties();

            const String groupName = "ProgressBar";

            AddProperty(new PropertyFloat(this.ProgressValue, "value", groupName, "value", (x) => { this.ProgressValue = x; }, () => { return this.ProgressValue; }));
            AddProperty(new PropertyColor(this.ProgressBarColor, "barColor", groupName, "barColor", (x) => { this.ProgressBarColor = x; }, () => { return this.ProgressBarColor; }));
        }

        /// <summary>
        /// Renders control.
        /// </summary>
        /// <param name="graphics">graphics to render to</param>
        /// <param name="X">X screen offset</param>
        /// <param name="Y">Y screen offset</param>
        protected override void RenderControls(Graphics graphics, int x, int y)
        {
            int borderSize = 2;

            if (BorderStyle.None == this.Border)
            {
                borderSize = 0;
            }

            int tx = x + this.Bounds.X + borderSize;
            int ty = y + this.Bounds.Y + borderSize;

            int tw = (int)((float)(this.Bounds.Width - borderSize * 2) * this.ProgressValue);

            graphics.SetColor(this.ProgressBarColor);
            graphics.DrawRectangle(tx, ty, tw, this.Bounds.Height - borderSize * 2);
        }

        /// <summary>
        /// Progress bar value in the range [0; 1]
        /// </summary>
		public float ProgressValue
        {
            set
            {
                if (value >= 1.0f)
                {
                    this.progressValue = 1.0f;
                }
                else if (value <= 0.0f)
                {
                    this.progressValue = 0.0f;
                }
                else
                {
                    this.progressValue = value;
                }
            }
            get
            {
                return this.progressValue;
            }
        }

        /// <summary>
        /// Progress bar color.
        /// </summary>
        public Color ProgressBarColor
        {
            set
            {
                this.barColor = value;
            }
            get
            {
                return this.barColor;
            }
        }


        /// <summary>
        /// Progress bar type name
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "progressBar";
            }
        }

        private Color barColor = Colors.White;
        private float progressValue = 0.5f;
	}
}
