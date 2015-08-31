using System;
using System.Collections.Generic;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Fonts
{
    internal class BitmapFontCreator : IFontCreator
    {
        public BitmapFontCreator(UIEngine engine)
        {
            this.engine = engine;

            List<String> files = new List<String>();

            engine.GetFiles(fontsFolder, files);

            foreach (String fileName in files)
            {
                if (true == fileName.EndsWith(".font.xml"))
                {
                    String fontName = GetFontName(fontsFolder + fileName, engine);

                    if (null != fontName)
                    {
                        this.availableFonts[fontName.ToLower()] = fileName;
                    }
                }
            }
        }

        public IFont GetFont(string name, int size, bool bold, bool italic, UIEngine engine, Theme theme)
        {
            String fileName = null;

            if (true == this.availableFonts.TryGetValue(name.ToLower(), out fileName))
            {
                BitmapFont font = new BitmapFont(this.engine, fontsFolder + fileName, name, size, bold, italic);

                if (false == font.Loaded)
                {
                    return null;
                }
                else
                {
                    return font;
                }
            }
            else
            {
                return null;
            }
        }

        public void GetFonts(List<String> availableFonts, UIEngine engine, Theme theme)
        {
            foreach (String font in this.availableFonts.Keys)
            {
                availableFonts.Add(font);
            }
        }

        private String GetFontName(String fileName, UIEngine engine)
        {
            using (IXmlReader reader = this.engine.OpenXmlFile(fileName))
            {
                if (null != reader)
                {
                    IXmlElement root = reader.RootElement;
                    String fontName = "";

                    if ("font" == root.Name)
                    {
                        fontName = root.GetAttributeValue("name", "");
                    }

                    if ("" == fontName)
                    {
                        return null;
                    }
                    else
                    {
                        return fontName;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private UIEngine engine = null;
        private static String fontsFolder = "ui/fonts/";
        private Dictionary<String, String> availableFonts = new Dictionary<String, String>();
    }
}
