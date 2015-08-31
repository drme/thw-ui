using System;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Mouse pointer renderer
    /// </summary>
	internal class MousePointer : UIObject
	{
        /// <summary>
        /// Constructs mouse poointer object.
        /// </summary>
        /// <param name="engine">ui engine for allocating image resources.</param>
		internal MousePointer(UIEngine engine) : base("mousePointer")
        {
            this.engine = engine;

			for (uint i = 0; i < pointersCount; i++)
			{
				this.textures[i] = null;
			}
        }

		~MousePointer()
        {
			for (uint i = 0; i < pointersCount; i++)
			{
				this.engine.DeleteImage(ref this.textures[i]);
			}
        }

        /// <summary>
        /// Render cursor at the specified position
        /// </summary>
        internal void Render(Graphics render, int x, int y, Theme theme)
        {
			render.SetColor(white);

			if (null == this.textures[0])
			{
                String themeFolder = theme.ThemeFolder + "/images/cursor_";

                this.textures[(int)MousePointers.PointerStandard] = this.engine.CreateImage(themeFolder + "default");
                this.textures[(int)MousePointers.PointerWait] = this.engine.CreateImage(themeFolder + "clock");
                this.textures[(int)MousePointers.PointerMove] = this.engine.CreateImage(themeFolder + "move");
                this.textures[(int)MousePointers.PointerHResize] = this.engine.CreateImage(themeFolder + "hsize");
                this.textures[(int)MousePointers.PointerVResize] = this.engine.CreateImage(themeFolder + "vsize");
                this.textures[(int)MousePointers.PointerResize1] = this.engine.CreateImage(themeFolder + "resize1");
                this.textures[(int)MousePointers.PointerResize2] = this.engine.CreateImage(themeFolder + "resize2");
                this.textures[(int)MousePointers.PointerText] = this.engine.CreateImage(themeFolder + "text");
                this.textures[(int)MousePointers.PointerHand] = this.engine.CreateImage(themeFolder + "hand");
			}

            if (null != this.textures[(int)this.activeCursor])
            {
                if (MousePointers.PointerStandard == this.activeCursor)
                {
                    render.DrawImage(x, y, 32, 32, this.textures[(int)this.activeCursor]);
                }
                else
                {
                    render.DrawImage(x - 16, y - 16, this.textures[(int)this.activeCursor].Width, this.textures[(int)this.activeCursor].Height, this.textures[(int)this.activeCursor]);
                }
            }
        }

        /// <summary>
        /// Curosr to display
        /// </summary>
        public MousePointers ActiveCursor
        {
            set
            {
                this.activeCursor = value;
            }
            get
            {
                return this.activeCursor;
            }
        }

        private UIEngine engine = null;
		private static Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		private	static uint pointersCount = 9;
		private MousePointers activeCursor = MousePointers.PointerStandard;
		private	IImage[] textures = new IImage[pointersCount];
	}
}
