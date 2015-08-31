using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Picture control. Displays iamge.
    /// </summary>
	public class Picture : Control
	{
        /// <summary>
        /// Creates picture control.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
		public Picture(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
        }

        /// <summary>
        /// Reference to back image object.
        /// </summary>
        public IImage BackPicture
        {
            get
            {
                if (null != this.backgroundImage)
                {
                    return this.backgroundImage.Image;
                }
                else
                {
                    return null;
                }
            }
        } 

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "picture";
            }
        }
	}
}
