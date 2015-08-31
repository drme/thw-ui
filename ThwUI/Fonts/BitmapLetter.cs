using ThW.UI.Utils;

namespace ThW.UI.Fonts
{
    internal class BitmapLetter
    {
        public BitmapLetter(IImage image, int width, int height, float s1, float t1, float s2, float t2, int letterWidth)
        {
            this.letterWidth = letterWidth;
            this.texture = image;
            this.width = width;
            this.height = height;
            this.texCoords[0] = s1;
            this.texCoords[1] = t1;
            this.texCoords[2] = s2;
            this.texCoords[3] = t2;
        }

        public int Render(Graphics graphics, int x, int y)
        {
            graphics.DrawImage(x, y, this.width, this.height, this.texture, this.texCoords);

            return this.letterWidth;
        }

        public int Width
        {
            get
            {
                return this.letterWidth;
            }
        }

        private int letterWidth = 0;
        private int width = 0;
        private int height = 0;
        internal IImage texture = null;
        private float[] texCoords = new float[4];
    }
}
