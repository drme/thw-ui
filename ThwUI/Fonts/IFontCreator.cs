using System;
using System.Collections.Generic;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Fonts
{
    /// <summary>
    /// Interface for fonts creator.
    /// </summary>
	public interface IFontCreator
	{
        /// <summary>
        /// Creates specified font. (no caching here needed. Fonts factory will be performing caching). 
        /// </summary>
        /// <param name="fontName">font name</param>
        /// <param name="size">font size in pixels</param>
        /// <param name="bold">font is bold</param>
        /// <param name="italic">font is italic</param>
        /// <returns>on success returns pointer to IFont interface, otherwise returns NULL</returns>
        IFont GetFont(String fontName, int size, bool bold, bool italic, UIEngine engine, Theme theme);

        /// <summary>
        /// Fills list with supported font names in lower case (for example "arial")
        /// </summary>
        /// <param name="availableFonts">list to fill with availabe fonts names</param>
        void GetFonts(List<String> availableFonts, UIEngine engine, Theme theme);
	}
}
