using System;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Slider control with selectable position in the range [0; 1].
    /// </summary>
	public class TrackBar : Control
	{
        /// <summary>
        /// Creates track bar control.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
		public TrackBar(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.backPanel = CreateControl<Panel>(CreationFlag.InternalControl);
			this.sliderButton = CreateControl<Button>(CreationFlag.InternalControl);

            this.sliderButton.MousePress += (x, y) => { this.trackerDragging = true; };
            this.sliderButton.MouseRelease += (x, y) => { this.trackerDragging = false; };
            this.sliderButton.MouseMoved += this.SliderButtonMouseMoved;

			this.backPanel.Border = BorderStyle.Lowered;
			this.BackColor = Colors.None;

			AddControl(this.backPanel);
			AddControl(this.sliderButton);
        }

        /// <summary>
        /// Handles mouse move event on tracker button.
        /// </summary>
        /// <param name="sender">tracker button.</param>
        /// <param name="args">arguments.</param>
        private void SliderButtonMouseMoved(Button sender, EventArgs args)
        {
            if (true == this.trackerDragging)
            {
                Window window = null;

                for (Control control = this; control != null; control = control.Parent)
                {
                    if (null == control.Parent)
                    {
                        window = (Window)control;
                    }
                }

                Desktop desktop = window.Desktop;

                if (null != desktop)
                {
                    if (true == this.vertical)
                    {
                        int y = desktop.MouseY;
                        int sy = this.TranslateY(y);

                        if ((sy >= 0) && (sy <= this.bounds.Height - this.buttonSize))
                        {
                            this.Position = (float)(sy - this.buttonSize) / (float)(this.bounds.Height - this.buttonSize);
                        }
                        else if (sy < 0.0f)
                        {
                            this.Position = 0.0f;
                        }
                        else if (sy > this.bounds.Height - this.buttonSize)
                        {
                            this.Position = 1.0f;
                        }
                    }
                    else
                    {
                        int x = desktop.MouseX;
                        int sx = this.TranslateX(x);

                        if ((sx >= 0) && (sx <= this.bounds.Width - this.buttonSize))
                        {
                            this.Position = (float)(sx) / (float)(this.bounds.Width - this.buttonSize);
                        }
                        else if (sx < 0.0f)
                        {
                            this.Position = 0.0f;
                        }
                        else if (sx > this.Bounds.Width - this.buttonSize)
                        {
                            this.Position = 1.0f;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Track bar value in the range [0; 1];
        /// </summary>
        public float Position
        {
            get
            {
                return this.position;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value > 1.0f)
                {
                    value = 1.0f;
                }

                if (this.position != value)
                {
                    this.position = value;

                    RaiseValueChangedEvent();
                }
            }
        }

        /// <summary>
        /// Renders control at X, Y position
        /// </summary>
        /// <param name="graphics">graphic to render to</param>
        /// <param name="X">X coordinate</param>
        /// <param name="Y">Y coordinate</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
			if (false == this.vertical)
			{
				this.backPanel.Bounds.UpdateSize(0, this.bounds.Height / 2 - 2, this.bounds.Width, 4);
				int w = this.Bounds.Width - this.buttonSize;
                this.sliderButton.Bounds.UpdateSize((int)(w * this.position), this.bounds.Height / 2 - this.buttonSize, this.buttonSize, this.buttonSize * 2);
			}
			else
			{
                this.backPanel.Bounds.UpdateSize(this.bounds.Width / 2 - 2, 0, 4, this.bounds.Height);
				int h = this.Bounds.Height - this.buttonSize;
                this.sliderButton.Bounds.UpdateSize(this.bounds.Width / 2 - this.buttonSize, (int)(h * this.position), this.buttonSize * 2, this.buttonSize);
			}

			base.Render(graphics, x, y);
        }

        /// <summary>
        /// Renders control background at X, Y position.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        protected override void RenderBackground(Graphics graphics, int x, int y)
        {
			base.RenderBackground(graphics, x, y);

			if ( (true == this.TicksVisible) && (0 != this.TicksCount) )
			{
				if (true == this.Vertical)
				{
					int w = this.Bounds.Width / 2 + this.Bounds.X + x;

					if (true == this.BottomTicksVisible)
					{
						w += 4;
					}
					else
					{
						w -= 10 + 4;
					}

                    if (this.TicksCount > 0)
                    {
                        int h = this.Bounds.Height - 1;

                        graphics.SetColor(this.TextColor);

                        float step = 1.0f / this.ticksCount;

                        for (float i = 0; i < 1.001f; i += step)
                        {
                            graphics.DrawRectangle(w, (int)(this.Bounds.Y + y + h * i), 5, 1);
                        }
                    }
				}
				else
				{
					int h = this.Bounds.Height / 2 + this.Bounds.Y + y;

					if (true == this.BottomTicksVisible)
					{
						h += 4;
					}
					else
					{
						h -= 10 + 4;
					}

                    if (this.TicksCount > 0)
                    {
                        int w = this.Bounds.Width - 1;

                        graphics.SetColor(this.TextColor);

                        float step = 1.0f / this.TicksCount;

                        for (float i = 0; i < 1.001f; i += step)
                        {
                            graphics.DrawRectangle((int)(this.Bounds.X + x + w * i), h, 1, 5);
                        }
                    }
				}
			}

            if ((false == this.Vertical) && (null != this.FontInfo.Font))
			{
				int h = this.Bounds.Height / 2 + this.Bounds.Y + y;

				if (true == this.BottomTicksVisible)
				{
					h += 4 + 5;
				}
				else
				{
                    h -= 4 + 5 + this.FontInfo.Font.TextHeight(this.maxText) + 7;
				}

                this.FontInfo.Font.DrawText(graphics, this.Bounds.X + x, h, this.minText);
                this.FontInfo.Font.DrawText(graphics, this.Bounds.X + x + this.Bounds.Width - this.FontInfo.Font.TextLength(this.maxText), h, this.maxText);
			}
        }

        /// <summary>
        /// Handles language changed event.
        /// Refreshes texts based on language.
        /// </summary>
		protected override void OnLanguageChanged()
        {
			base.OnLanguageChanged();

			if (false == this.NeedTranslation)
			{
				this.minText = this.minTextReference;
				this.maxText = this.maxTextReference;

				return;
			}

			if (null != this.Engine.Language)
			{
				String group = (null == this.Window) ? this.Name : this.Window.Name;

                this.minText = this.Engine.Language.Translate("window." + group, this.minTextReference);
                this.maxText = this.Engine.Language.Translate("window." + group, this.maxTextReference);
			}
			else
			{
				this.minText = this.minTextReference;
				this.maxText = this.maxTextReference;
			}
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
		protected override void AddProperties()
        {
			base.AddProperties();

            String group = "trackBar";

            AddProperty(new PropertyBoolean(this.Vertical, "vertical", group, "vertical", (x) => { this.Vertical = x; }, () => { return this.Vertical; }));
            AddProperty(new PropertyBoolean(this.TicksVisible, "showTick", group, "showTick", (x) => { this.TicksVisible = x; }, () => { return this.TicksVisible; }));
            AddProperty(new PropertyBoolean(this.BottomTicksVisible, "showBottom", group, "showTickBottom", (x) => { this.BottomTicksVisible = x; }, () => { return this.BottomTicksVisible; }));
            AddProperty(new PropertyInteger(this.TicksCount, "ticksCount", group, "ticksCount", (x) => { this.TicksCount = x; }, () => { return this.TicksCount; }));
            AddProperty(new PropertyString(this.MinText, "minText", group, "minText", (x) => { this.MinText = x; }, () => { return this.MinText; }));
            AddProperty(new PropertyString(this.MaxText, "maxText", group, "maxText", (x) => { this.MaxText = x; }, () => { return this.MaxText; }));
            AddProperty(new PropertyFloat(this.Position, "value", group, "value", (x) => { this.Position = x; }, () => { return this.Position; }));
        }

        /// <summary>
        /// Does not contain any other controls.
        /// </summary>
        /// <returns>always false</returns>
		public override bool CanContainControl(String typeName)
        {
            return false;
        }

        /// <summary>
        /// Control name
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "trackBar";
            }
        }

        /// <summary>
        /// Rendered ticks count.
        /// </summary>
        public int TicksCount
        {
            get
            {
                return this.ticksCount;
            }
            set
            {
                this.ticksCount = value;
            }
        }

        /// <summary>
        /// Ticks are rendered.
        /// </summary>
        public bool TicksVisible
        {
            get
            {
                return this.ticksVisible;
            }
            set
            {
                this.ticksVisible = value;
            }
        }
        
        /// <summary>
        /// Are ticks at the bottom (or at right of vertical one) of the control are visible.
        /// </summary>
        public bool BottomTicksVisible
        {
            get
            {
                return this.showTickBottom;
            }
            set
            {
                this.showTickBottom = value;
            }
        }

        /// <summary>
        /// Is control vertical. Othervise it's horizontal.
        /// </summary>
        public bool Vertical
        {
            get
            {
                return this.vertical;
            }
            set
            {
                this.vertical = value;
            }
        }

        /// <summary>
        /// Text displayed new max position.
        /// </summary>
        public String MaxText
        {
            get
            {
                return this.maxTextReference;
            }
            set
            {
                this.maxTextReference = value;
                OnLanguageChanged();
            }
        }

        /// <summary>
        /// Text displayed near min position
        /// </summary>
        public String MinText
        {
            get
            {
                return this.minTextReference;
            }
            set
            {
                this.minTextReference = value;
                OnLanguageChanged();
            }
        }

        private Panel backPanel = null;
        private Button sliderButton = null;
        private int buttonSize = 7;
        private bool trackerDragging = false;
        private int ticksCount = 20;
        private String minText = "";
        private String maxText = "";
        private String minTextReference = "";
        private String maxTextReference = "";
        private bool vertical = false;
        private bool ticksVisible = true;
        private bool showTickBottom = true;
		private float position = 0.5f;
	}
}
