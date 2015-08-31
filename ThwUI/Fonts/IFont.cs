using System;
using ThW.UI.Utils;

namespace ThW.UI.Fonts
{
    /// <summary>
    /// Font interface.
    /// </summary>
	public abstract class IFont : UIObject
	{
        /// <summary>
        /// Sets initial properties.
        /// </summary>
        /// <param name="name">Font name</param>
        /// <param name="size">Font size</param>
        /// <param name="bold">is the font bold</param>
        /// <param name="italic">is the font italic</param>
		protected IFont(String name, int size, bool bold, bool italic) : base("font", name)
        {
			this.size = size;
			this.bold = bold;
			this.italic = italic;
        }

        /// <summary>
        /// Renders text (substring from start till end) at specified location using active color.
        /// </summary>
        /// <param name="graphics">rendering object</param>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position </param>
        /// <param name="text">text to render</param>
        /// <param name="start">starting symbol of the text</param>
        /// <param name="stop">ending symbol of the text</param>
        public abstract void DrawText(Graphics graphics, int x, int y, String text, int start, int stop);

        /// <summary>
        /// Renders text at specified location using active color.
        /// </summary>
        /// <param name="graphics">rendering object</param>
        /// <param name="X">X position</param>
        /// <param name="Y">Y position </param>
        /// <param name="text">text to render</param>
        public virtual void DrawText(Graphics graphics, int x, int y, String text)
        {
            DrawText(graphics, x, y, text, 0, -1);
        }

        /// <summary>
        /// Returns text length in pixels.
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="start">text start char</param>
        /// <param name="stop">text end char</param>
        /// <returns>text length in pixels</returns>
		public virtual int TextLength(String text, int start, int stop)
        {
            return 0;
        }

        /// <summary>
        /// Returns text length in pixels.
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>text length in pixels</returns>
		public virtual int TextLength(String text)
        {
            return TextLength(text, 0, -1);
        }
		
        /// <summary>
        /// Returns text Height in pixels.
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="start">text start char</param>
        /// <param name="stop">text end char</param>
        /// <returns>text Height in pixels</returns>
		public virtual int TextHeight(String text, int start, int stop)
        {
        	return this.size;
        }

        /// <summary>
        /// Returns text Height in pixels.
        /// </summary>
        /// <returns>text Height in pixels</returns>
        public virtual int TextHeight(String text)
        {
        	return TextHeight(text, 0, -1);
        }

        /// <summary>
        /// Is font bold.
        /// </summary>
		public virtual bool Bold
        {
            get
            {
                return this.bold;
            }
        }

        /// <summary>
        /// Is font italic.
        /// </summary>
		public virtual bool Italic
        {
            get
            {
                return this.italic;
            }
        }

        /// <summary>
        /// Returns fonts size.
        /// </summary>
		public virtual int Size
        {
            get
            {
                return this.size;
            }
        }

        internal void AddRef()
        {
            this.refCount++;
        }

        internal int Release()
        {
            this.refCount--;

            return this.refCount;
        }

		protected int size;
		protected bool bold;
	    protected bool italic;
        internal int refCount = 0;
    }
}
