using System;

namespace ThW.UI.Utils.Images
{
    /// <summary>
    /// Images laoder interface
    /// </summary>
	internal interface IImageLoader
	{
        /// <summary>
        /// Loads image from file. Loaded images is 32-bits RGBA
        /// </summary>
        /// <param name="fileName">image file name (could be with out extension, exension is added by loader itself)</param>
        /// <returns></returns>
		Image CreateImage(String fileName);
	}
}
