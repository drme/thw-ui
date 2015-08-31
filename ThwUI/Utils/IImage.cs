using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Virtual image objects interface. Has to be implemented besides IRender interface.
	/// Usually holds pointer to internal Texture object.
    /// </summary>
    public abstract class IImage : UIObject, IDisposable
    {
        public IImage() : base("image")
        {
        }

        /// <summary>
        /// Image Width
        /// </summary>
        public abstract int Width
        {
            get;
        }

        /// <summary>
        /// Image Height
        /// </summary>
        public abstract int Height
        {
            get;
        }

        internal int AddRef()
        {
            this.referenceCount++;

            return this.referenceCount;
        }

        internal int Release()
        {
            this.referenceCount--;

            return this.referenceCount;
        }

        /// <summary>
        /// If texture uses delayed laoding, this flag specifies if texture is alredy loaded.
        /// If its false renderer uses temporary loading texture.
        /// </summary>
        public abstract bool Loaded
        {
            get;
        }

        /// <summary>
        /// If image texture loading is delayed and it fails, this flag marks loading failure
        /// and engine uses missing texture substitute for rendering this image.
        /// </summary>
        public abstract bool LoadFailed
        {
            get;
        }

        public abstract void Dispose();

        internal int referenceCount = 0;
    }
}
