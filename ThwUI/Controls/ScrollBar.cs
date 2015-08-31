using System;
using ThW.UI.Design;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Scroll bar control. Contains adjustable slider for marking current position.
    /// </summary>
	public class ScrollBar : Control
	{
        /// <summary>
        /// Creates scrollbar control.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation falgs.</param>
		public ScrollBar(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
            this.increaseButton = CreateControl<Button>(CreationFlag.InternalControl);
            this.decreaseButton = CreateControl<Button>(CreationFlag.InternalControl);
            this.sliderButton = CreateControl<Button>(CreationFlag.InternalControl);

			AddControl(this.increaseButton);
			AddControl(this.decreaseButton);
			AddControl(this.sliderButton);

            this.increaseButton.Clicked += (x, Y) => { this.Position += this.stepSize; };
            this.decreaseButton.Clicked += (x, y) => { this.Position -= this.stepSize; };
            this.sliderButton.MouseMoved += new UIEventHandler<Button>(SliderMouseMoved);
            this.sliderButton.MousePress += (x, y) => { this.sliderDragging = true; };
            this.sliderButton.MouseRelease += (x, y) => { this.sliderDragging = false; };

			this.Vertical = true;
        }

        /// <summary>
        /// Handles slider mouse move event.
        /// </summary>
        /// <param name="sender">slider button</param>
        /// <param name="args">arguments</param>
        private void SliderMouseMoved(Button sender, EventArgs args)
        {
            if (true == this.sliderDragging)
            {
                Desktop desktop = this.Window.Desktop;

                if (null != desktop)
                {
                    if (true == this.vertical)
                    {
                        int y = desktop.MouseY;
                        int sy = this.TranslateY(y);

                        if ((sy >= this.buttonSize) && (sy <= this.bounds.Height - this.buttonSize))
                        {
                            this.Position = (float)this.maxSize * (float)(sy - this.buttonSize) / (float)(this.bounds.Height - this.buttonSize * 2);
                        }
                        else if (sy < this.buttonSize)
                        {
                            this.Position = 0;
                        }
                        else if (sy > this.bounds.Height - this.buttonSize)
                        {
                            this.Position = (float)(this.maxSize);
                        }
                    }
                    else
                    {
                        int x = desktop.MouseX;
                        int sx = this.TranslateX(x);

                        if ((sx >= 16) && (sx <= this.bounds.Width - this.buttonSize))
                        {
                            this.Position = (float)this.maxSize * (float)(sx - this.buttonSize) / (float)(this.bounds.Width - this.buttonSize * 2);
                        }
                        else if (sx < this.buttonSize)
                        {
                            this.Position = 0;
                        }
                        else if (sx > this.bounds.Width - this.buttonSize)
                        {
                            this.Position = (float)(this.maxSize);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets control orientation. True for vertical, false for horizontal.
        /// </summary>
		public bool Vertical
        {
            set
            {
                if (this.vertical == value)
                {
                    return;
                }

                this.vertical = value;

                String folder = this.Window.Desktop.Theme.ThemeFolder + "/images/scroll_bar_";

                if (true == this.vertical)
                {
                    this.increaseButton.Icon = folder + "down";
                    this.decreaseButton.Icon = folder + "up";
                    this.BackImage = folder + "back_vertical";
                    this.BackImageLayout = ImageLayout.ImageLayoutStretch;
                }
                else
                {
                    this.increaseButton.Icon = folder + "right";
                    this.decreaseButton.Icon = folder + "left";
                    this.BackImage = folder + "back_horizontal";
                    this.BackImageLayout = ImageLayout.ImageLayoutStretch;
                }
            }
            get
            {
                return this.vertical;
            }
        }

        /// <summary>
        /// Maximum position. If current position is larger that max position, the current value is adjsuted to matct max size.
        /// </summary>
		public int MaxSize
        {
            set
            {
                this.maxSize = value;

                if (this.Position > value)
                {
                    this.Position = 0.0f;
                }

                if (this.maxSize <= 0)
                {
                    this.sliderButton.Visible = false;
                    this.Position = 0.0f;
                }
                else
                {
                    this.sliderButton.Visible = true;
                }
            }
            get
            {
                return this.maxSize;
            }
        }

        /// <summary>
        /// Step size. The position is adjusted by specified amount then pressing up/down/left/right buttons.
        /// </summary>
		public int StepSize
        {
            set
            {
                this.stepSize = value;
            }
            get
            {
                return this.stepSize;
            }
        }

        /// <summary>
        /// Slider position.
        /// </summary>
		public float Position
        {
            set
            {
                this.position = value;

                if (this.position > this.MaxSize)
                {
                    this.position = (float)this.MaxSize;
                }

                if (this.position < 0)
                {
                    this.position = 0;
                }
            }
            get
            {
                return this.position;
            }
        }

        /// <summary>
        /// How large is the displayed slider button.
        /// </summary>
		internal int ButtonSize
        {
            get
            {
                return this.buttonSize;
            }
        }

        /// <summary>
        /// Renders scroolbar at specified position.
        /// </summary>
        /// <param name="graphics">graphics to render to.</param>
        /// <param name="X">X coordinate.</param>
        /// <param name="Y">Y coordinate.</param>
        protected override void Render(Graphics graphics, int x, int y)
        {
			// for properties updating....
			this.Vertical = this.vertical;
			this.MaxSize = this.maxSize;
			this.Position = this.position;

			if (true == this.vertical)
			{
				int buttonHeight = 16;

				if (this.Bounds.Height < 40)
				{
					buttonHeight = this.Bounds.Height / 4;
				}

				this.decreaseButton.Bounds.UpdateSize(this.Bounds.Width - this.buttonSize, 0, this.buttonSize, buttonHeight);
                this.increaseButton.Bounds.UpdateSize(this.Bounds.Width - this.buttonSize, this.Bounds.Height - buttonHeight, this.buttonSize, buttonHeight);

				FixSlider(buttonHeight);
			}
			else
			{
				int buttonHeight = 16;

				if (this.Bounds.Width < 40)
				{
					buttonHeight = this.Bounds.Width / 4;
				}

                this.decreaseButton.Bounds.UpdateSize(0, this.Bounds.Height - this.buttonSize, this.buttonSize, this.buttonSize);
                this.increaseButton.Bounds.UpdateSize(this.Bounds.Width - this.buttonSize, this.bounds.Height - this.buttonSize, this.buttonSize, this.buttonSize);

				FixSlider(buttonHeight);
			}

			this.decreaseButton.Opacity = this.Opacity;
            this.increaseButton.Opacity = this.Opacity;
            this.sliderButton.Opacity = this.Opacity;

			base.Render(graphics, x, y);
        }

        /// <summary>
        /// Handles mouse wheel up event.
        /// </summary>
        /// <returns></returns>
        protected override bool OnMouseWheelUp(int x, int y, int dx, int dy)
        {
            MoveUp();

            return true;
        }

        /// <summary>
        /// Handles mouse wheel down event.
        /// </summary>
        /// <returns></returns>
        protected override bool OnMouseWheelDown(int x, int y, int dx, int dy)
        {
            MoveDown();

            return true;
        }

        /// <summary>
        /// Adds control properties.
        /// </summary>
        protected override void AddProperties()
        {
			base.AddProperties();

            AddProperty(new PropertyBoolean(this.Vertical, "vertical", "scrollBar", "vertical", (x) => { this.Vertical = x; }, () => { return this.Vertical; }));
            AddProperty(new PropertyInteger(this.StepSize, "stepSize", "scrollBar", "stepSize", (x) => { this.StepSize = x; }, () => { return this.StepSize; }));
            AddProperty(new PropertyInteger(this.MaxSize, "maxSize", "scrollBar", "maxSize", (x) => { this.MaxSize = x; }, () => { return this.MaxSize; }));
            AddProperty(new PropertyFloat(this.Position, "position", "scrollBar", "position", (x) => { this.Position = x; }, () => { return this.Position; }));
        }

        /// <summary>
        /// Moves slider up by 1 step.
        /// </summary>
        protected void MoveUp()
        {
            this.Position = this.position - this.stepSize;
        }

        /// <summary>
        /// Moves slider down bt 1 step.
        /// </summary>
        protected void MoveDown()
        {
            this.Position = this.position + this.stepSize;
        }

        /// <summary>
        /// Updates slider position on the screen.
        /// </summary>
        /// <param name="buttonHeight">slider  button Height.</param>
        protected void FixSlider(int buttonHeight)
        {
            if (true == this.vertical)
            {
                float len = (float)(this.bounds.Height - (buttonHeight * 2) - this.sliderSize);
                int h = buttonHeight + (int)((float)(len * this.position) / (float)(this.maxSize));
                this.sliderButton.Bounds.UpdateSize(this.bounds.Width - this.buttonSize, h, this.buttonSize, this.sliderSize);
            }
            else
            {
                float len = (float)(this.bounds.Width - (buttonHeight * 2) - this.sliderSize);
                float w = buttonHeight + len * this.position / (float)this.maxSize;
                this.sliderButton.Bounds.UpdateSize((int)(w), this.bounds.Height - this.buttonSize, this.sliderSize, this.buttonSize);
            }
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "scrollBar";
            }
        }

		private Button increaseButton = null;
        private Button decreaseButton = null;
        private Button sliderButton = null;
        private bool vertical = false;
        private int maxSize = 100;
        private float position = 0;
        private int stepSize = 10;
        private int buttonSize = 16;
        private bool sliderDragging = false;
        private int sliderSize = 16;
	}
}
