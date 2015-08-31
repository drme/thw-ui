using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Panel displayed at the bottom of window.
    /// </summary>
    public class StatusBar : Panel
    {
        /// <summary>
        /// Creates status bar object.
        /// </summary>
        /// <param name="window">window it belongs to.</param>
        /// <param name="creationFlags">creation flags.</param>
        public StatusBar(Window window, CreationFlag creationFlags) : base(window, creationFlags)
        {
            this.Type = TypeName;
        }

        /// <summary>
        /// Controls name as serialized in a xml file.
        /// </summary>
        internal new static String TypeName
        {
            get
            {
                return "statusBar";
            }
        }
    }
}
