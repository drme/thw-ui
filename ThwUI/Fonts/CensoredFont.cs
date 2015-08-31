using System;
using System.Diagnostics;
using ThW.UI.Utils;

namespace ThW.UI.Fonts
{
    /// <summary>
    /// Fallback font that renders censored squares instead of letters.
    /// </summary>
    internal class CensoredFont : IFont
    {
        public CensoredFont(String name, int size, bool bold, bool italic) : base(name, size, bold, italic)
        {
            Debug.WriteLine("Creating cesored font: " + name);
        }

        public override void DrawText(Graphics graphics, int x, int y, string text, int start, int stop)
        {
            if ((null == text) || (text.Length == 0))
            {
                return;
            }

            int renderX = x;

            if (stop < 0)
            {
                stop = (int)(text.Length);
            }

            if (stop > text.Length)
            {
                stop = text.Length;
            }

            if (start < 0)
            {
                start = 0;
            }

            for (int i = start; i < stop; i++)
            {
                graphics.DrawBox(renderX, y, this.size / 2, this.size);

                renderX += this.size / 2 + 2;
            }
        }

        public override int TextLength(String text, int start, int stop)
        {
            if ((null == text) || (text.Length == 0))
            {
                return 0;
            }

            int length = 0;

            if ((stop < 0) || (stop > text.Length))
            {
                stop = (int)(text.Length);
            }

            if (start < 0)
            {
                start = 0;
            }

            for (int i = start; i < stop; i++)
            {
                length += this.size / 2 + 2;
            }

            return length;
        }
    }
}
