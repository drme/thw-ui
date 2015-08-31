using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Label with some text
    /// </summary>
	public class Label : Control
	{
        /// <summary>
        /// Creates label control.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
		public Label(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
			this.BackColor = Colors.None;
			this.TextAlignment = ContentAlignment.MiddleLeft;
			this.Border = BorderStyle.None;
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal static String TypeName
        {
            get
            {
                return "label";
            }
        }
	}
}
