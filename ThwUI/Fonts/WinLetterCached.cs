using ThW.UI.Utils;

namespace ThW.UI.Fonts
{
    /// <summary>
    /// Windows font letter laoded from cached bitmap.
    /// </summary>
    internal class WinLetterCached
    {
        /// <summary>
        /// Creates winletter cached object.
        /// </summary>
        /// <param name="engine">ui engine.</param>
        public WinLetterCached(UIEngine engine)
        {
            this.engine = engine;
        }

        /// <summary>
        /// Releases cached iamge.
        /// </summary>
        ~WinLetterCached()
        {
            this.Image = null;
        }

        /// <summary>
        /// Sets letter iamge data. assigns bitmap and position where on it resides cached letter.
        /// </summary>
        /// <param name="image">image containing several rendered letter bitmaps.</param>
        /// <param name="us">us texture coordinate for this letter.</param>
        /// <param name="vs">vs texture coordinate for this letter.</param>
        /// <param name="ue">ue texture coordinate for this letter.</param>
        /// <param name="ve">ve texture coordinate for this letter.</param>
        /// <param name="Width">letter Width.</param>
        /// <param name="offX">letter X offset.</param>
        /// <param name="offY">letter Y offset.</param>
        internal void SetCachedData(IImage image, int us, int vs, int ue, int ve, int w, int offX, int offY)
        {
            SetCachedData(image, us, vs, ue, ve);
            this.width = w;
            this.offsetX = offX;
            this.offsetY = offY;
        }

        /// <summary>
        /// Sets letter iamge data. assigns bitmap and position where on it resides cached letter.
        /// </summary>
        /// <param name="image">image containing several rendered letter bitmaps.</param>
        /// <param name="us">us texture coordinate for this letter.</param>
        /// <param name="vs">vs texture coordinate for this letter.</param>
        /// <param name="ue">ue texture coordinate for this letter.</param>
        /// <param name="ve">ve texture coordinate for this letter.</param>
        internal void SetCachedData(IImage image, int us, int vs, int ue, int ve)
        {
            this.loaded = true;
            this.Image = image;
//            this.internalImage = false;
            this.uv[0] = us;
            this.uv[1] = vs;
            this.uv[2] = ue;
            this.uv[3] = ve;
            this.textureWidth = this.uv[2] - this.uv[0];
            this.textureHeight = this.uv[3] - this.uv[1];
            this.uvs[0] = (float)this.uv[0] / (float)WinFontCached.cacheTextureSize;//(float)image.Width;
            this.uvs[1] = (float)this.uv[1] / (float)WinFontCached.cacheTextureSize;//(float)image.Height;
            this.uvs[2] = (float)this.uv[2] / (float)WinFontCached.cacheTextureSize;//(float)image.Width;
            this.uvs[3] = (float)this.uv[3] / (float)WinFontCached.cacheTextureSize;//(float)image.Height;
        }

        /// <summary>
        /// Letter Width
        /// </summary>
        internal int Width
        {
            get
            {
                if (false == this.loaded) // branch prediction will do the job.
                {
                    Load(false);
                }

                return this.width;
            }
        }

        /// <summary>
        /// Loads letter.
        /// </summary>
        /// <param name="cache">should be cahced to tga and xml files.</param>
        protected internal virtual LetterInfo Load(bool cache)
        {
            return null;
        }

        /// <summary>
        /// Render letter
        /// </summary>
        /// <param name="render">graphics to render to.</param>
        /// <param name="X">X position.</param>
        /// <param name="Y">Y position.</param>
        /// <returns></returns>
        public int Render(Graphics render, int x, int y)
        {
            if (false == this.loaded) // branch prediction will do the job.
            {
                Load(false);
            }

            if (null != this.image)
            {
                render.DrawImage(x + this.offsetX, y + this.offsetY, this.textureWidth, this.textureHeight, this.image, this.uvs);
            }

            return this.width;
        }

        /// <summary>
        /// Holds bitmap image for several letters.
        /// </summary>
        internal IImage Image
        {
            set
            {
          //      if (true == this.internalImage)
            //    {
                    this.engine.DeleteImage(ref this.image);
//                }
  //              else
    //            {
      //              this.image = null;
        //        }

				this.image = value;

                if (null != this.image)
                {
                    this.image.AddRef();
                }
            }
        }

        protected int width = 0;
        protected int offsetX = 0;
        protected int offsetY = 0;
        protected int textureWidth = 0;
        protected int textureHeight = 0;
        protected int[] uv = new int[4];
        protected float[] uvs = new float[] { 0.0f, 0.0f, 1.0f, 1.0f };
//        protected bool internalImage = false;
        protected bool loaded = false;
        protected UIEngine engine = null;
        protected IImage image = null;
    }
}
