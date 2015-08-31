using System;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Utils
{
    /// <summary>
    /// RGB color.
    /// </summary>
    public class Color
    {
        /// <summary>
        /// Constructs whire color object.
        /// </summary>
        public Color()
        {
        }

        /// <summary>
        /// Builds color object from it's string presentation.
        /// </summary>
        /// <param name="value">color value in #r,g,b,a format</param>
        /// <returns>color object.</returns>
        internal Color (String value)
        {
            int r = 255;
            int g = 255;
            int b = 255;
            int a = 255;

            String[] values = value.Split(',');

            if (values.Length > 0)
            {
                r = int.Parse(values[0].Replace("#", ""));
            }

            if (values.Length > 1)
            {
                g = int.Parse(values[1].Replace("#", ""));
            }

            if (values.Length > 2)
            {
                b = int.Parse(values[2].Replace("#", ""));
            }

            if (values.Length > 3)
            {
                a = int.Parse(values[3].Replace("#", ""));
            }

            this.colorArray[0] = (float)r / 255.0f;
            this.colorArray[1] = (float)g / 255.0f;
            this.colorArray[2] = (float)b / 255.0f;
            this.colorArray[3] = (float)a / 255.0f;
            
			this.name = "";
        }

        public Color(float r, float g, float b, float a, String name)
        {
            this.colorArray[0] = r;
            this.colorArray[1] = g;
            this.colorArray[2] = b;
            this.colorArray[3] = a;
            this.name = name;
        }

        public	Color(int r, int g, int b, int a, String name)
        {
            this.colorArray[0] = (float)r / 255.0f;
            this.colorArray[1] = (float)g / 255.0f; ;
            this.colorArray[2] = (float)b / 255.0f; ;
            this.colorArray[3] = (float)a / 255.0f; ;
            this.name = name;
        }

        public Color(float r, float g, float b, float a) : this(r, g, b, a, "")
        {
        }

        public Color(float r, float g, float b) : this(r, g, b, 1.0f)
        {
        }

        public Color(int r, int g, int b, int a) : this(r, g, b, a, "")
        {
        }

        public Color(int r, int g, int b) : this(r, g, b, 255)
        {
        }

		public Color(uint value) : this(0, 0, 0, 0)
		{
			uint a = (uint)(value & 0xff000000) >> 24;
			uint r = (uint)(value & 0x00ff0000) >> 16;
			uint g = (uint)(value & 0x0000ff00) >> 8;
			uint b = (uint)(value & 0x000000ff);

			this.R = (float)r / 255.0f;
			this.G = (float)g / 255.0f;
			this.B = (float)b / 255.0f;
			this.A = (float)a / 255.0f;
		}

        public float R
        {
            get
            {
                return this.colorArray[0];
            }
            set
            {
                this.colorArray[0] = value;
            }
        }

        public float G
        {
            get
            {
                return this.colorArray[1];
            }
            set
            {
                this.colorArray[1] = value;
            }
        }

        public float B
        {
            get
            {
                return this.colorArray[2];
            }
            set
            {
                this.colorArray[2] = value;
            }
        }

        public float A
        {
            get
            {
                return this.colorArray[3];
            }
            set
            {
                this.colorArray[3] = value;
            }
        }

        /// <summary>
        /// R G B A
        /// </summary>
        public float[] Values
        {
            get
            {
                return this.colorArray;
            }
        }

        public uint GetColorValue()
        {
			return (uint)(this.colorArray[2] * 256) + ((uint)(this.colorArray[1] * 256) << 8) + ((uint)(this.colorArray[0] * 256) << 16) + ((uint)(this.colorArray[3] * 256) << 24);
		}

        /// <summary>
        /// Color name.
        /// </summary>
        public String Name
        {
            get
            {
                if ( (null == this.name) || (0 == this.name.Length) )
				{
					String buf = "";
					
                    buf += "#" + (int)(255.0f * this.colorArray[0]) + ",";
                    buf += "#" + (int)(255.0f * this.colorArray[1]) + ",";
                    buf += "#" + (int)(255.0f * this.colorArray[2]) + ",";
                    buf += "#" + (int)(255.0f * this.colorArray[3]) + "";

                    return buf;
				}
				else
				{
					return this.name;
				}
            }
        }

        internal void UpdateColor(String name, ThemeColors colors)
        {
            if ((null == name) || (0 == name.Length))
            {
                return;
            }

            if (true == name.StartsWith("#"))
            {
                Color color = new Color(name.Substring(1));

                this.colorArray[0] = color.colorArray[0];
                this.colorArray[1] = color.colorArray[1];
                this.colorArray[2] = color.colorArray[2];
                this.colorArray[3] = color.colorArray[3];
            }
            else
            {
                foreach (Color color in colors.SystemColors)
                {
                    if (true == name.Equals(color.Name))
                    {
                        this.colorArray[0] = color.colorArray[0];
                        this.colorArray[1] = color.colorArray[1];
                        this.colorArray[2] = color.colorArray[2];
                        this.colorArray[3] = color.colorArray[3];

                        return;
                    }
                }
            }
        }

        public static bool operator == (Color l, Color r)
        {
            if (Object.ReferenceEquals(l, r))
            {
                return true;
            }

            if (((object)l == null) || ((object)r == null))
            {
                return false;
            }

            return ((l.colorArray[0] == r.colorArray[0]) && (l.colorArray[1] == r.colorArray[1]) && (l.colorArray[2] == r.colorArray[2]) && (l.colorArray[3] == r.colorArray[3]));
        }

        public static bool operator != (Color l, Color r)
        {
            if (Object.ReferenceEquals(l, r))
            {
                return false;
            }

            if (((object)l == null) || ((object)r == null))
            {
                return true;
            }

            return ((l.colorArray[0] != r.colorArray[0]) || (l.colorArray[1] != r.colorArray[1]) || (l.colorArray[2] != r.colorArray[2]) || (l.colorArray[3] != r.colorArray[3]));
        }

        public override bool Equals(object obj)
        {
            if (obj is Color)
            {
                Color r = (Color)obj;

                return (this.colorArray[0] == r.colorArray[0]) && (this.colorArray[1] == r.colorArray[1]) && (this.colorArray[2] == r.colorArray[2]) && (this.colorArray[3] == r.colorArray[3]);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (int)this.GetColorValue();
        }

        /// <summary>
        /// 0xAARRGGBB
        /// </summary>
        public uint Value
        {
            get
            {
				uint a = (byte)(this.A * 255);
				uint r = (byte)(this.R * 255);
				uint g = (byte)(this.G * 255);
				uint b = (byte)(this.B * 255);

                return (uint)(a << 24 | r << 16 | g << 8 | b);
            }
        }

		public static Color FromArgb(byte a, byte r, byte g, byte b)
		{
			return new Color(r, g, b, a);
		}

		public static Color FromArgb(byte a, int r, int g, int b)
		{
			return new Color(r, g, b, (int)a);
		}

        private String name = "";
        private float[] colorArray = { 1.0f, 1.0f, 1.0f, 1.0f };
	}
}
