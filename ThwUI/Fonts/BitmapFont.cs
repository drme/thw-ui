using System;
using System.Collections.Generic;
using ThW.UI.Utils;

namespace ThW.UI.Fonts
{
    internal class BitmapFont : IFont
    {
        public BitmapFont(UIEngine engine, String fileName, string fontName, int size, bool bold, bool italic) : base(fontName, size, bold, italic)
        {
            this.engine = engine;

            using (IXmlReader reader = this.engine.OpenXmlFile(fileName))
            {
                if (null != reader)
                {
                    IXmlElement root = reader.RootElement;

                    if ("font" == root.Name)
                    {
                        String name = root.GetAttributeValue("name", "");
                        String textureName = root.GetAttributeValue("texture", "");
                        int rowWidth = int.Parse(root.GetAttributeValue("rowWidth", "0"));
                        int rowHeight = int.Parse(root.GetAttributeValue("rowHeight", "0"));
                        float scaleW = float.Parse(root.GetAttributeValue("scaleW", "1"));
                        float scaleH = float.Parse(root.GetAttributeValue("scaleH", "1"));

                        int[] widths = new int[rowWidth * rowHeight];

                        for (int i = 0; i < widths.Length; i++)
                        {
                            widths[i] = -1;
                        }

                        foreach (IXmlElement child in root.Elements)
                        {
                            if ("letter" == child.Name)
                            {
                                String c = child.GetAttributeValue("char");
                                String code = child.GetAttributeValue("code");

                                Object cc = null;

                                if ((null != c) && (c.Length == 1))
                                {
                                    cc = c[0];
                                }
                                else if (null != code)
                                {
                                    cc = (char)int.Parse(code);
                                }

                                if ((null != cc) && ((int)(char)cc < widths.Length))
                                {
                                    int width = int.Parse(child.GetAttributeValue("Width", "-1"));

                                    if (width >= 0)
                                    {
                                        widths[(int)(char)cc] = width;
                                    }
                                }
                            }
                        }

                        IImage image = this.engine.CreateImage(textureName);

                        for (int y = 0; y < rowHeight; y++)
                        {
                            for (int x = 0; x < rowWidth; x++)
                            {
                                float w = (int)(image.Width / rowWidth);

                                int w1 = (int)w;

                                if (-1 != widths[y * rowWidth + x])
                                {
                                    w1 = widths[y * rowWidth + x];
                                }

                                float h = (int)(image.Height / rowHeight);
                                float s1 = (float)(x * w) / (float)image.Width;
                                float s2 = (float)((x + 1) * w) / (float)image.Width;
                                float t1 = (float)(y * h) / (float)image.Height;
                                float t2 = (float)((y + 1) * h) / (float)image.Height;

                                this.letters.Add(new BitmapLetter(image, (int)(w * scaleW), (int)(h * scaleH), s1, t1, s2, t2, w1));
                            }
                        }
                    }

                    this.loaded = true;
                }
            }
        }

        public override void DrawText(Graphics graphics, int x, int y, string strText, int start, int stop)
        {
            if (false == this.loaded)
            {
                return;
            }

            int renderX = x;

            if (stop < 0)
            {
                stop = (int)(strText.Length);
            }

            if (start < 0)
            {
                start = 0;
            }

            for (int i = start; i < stop; i++)
            {
                if (strText[i] < this.letters.Count)
                {
                    renderX += this.letters[strText[i]].Render(graphics, renderX, y);
                }
            }
        }

        internal bool Loaded
        {
            get
            {
                return this.loaded;
            }
        }

        public override int TextLength(String text, int start, int stop)
        {
            if (false == this.loaded)
            {
                return 0;
            }

            int nLength = 0;

            if (stop < 0)
            {
                stop = (int)(text.Length);
            }

            if (start < 0)
            {
                start = 0;
            }

            for (int i = start; i < stop; i++)
            {
                if (text[i] < this.letters.Count)
                {
                    nLength += this.letters[text[i]].Width;
                }
            }

            return nLength;
        }

        private UIEngine engine = null;
        private bool loaded = false;
        private List<BitmapLetter> letters = new List<BitmapLetter>();
    }
}
