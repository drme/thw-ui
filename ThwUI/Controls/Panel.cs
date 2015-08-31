using System;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Controls
{
    /// <summary>
    /// Panel control for placing other controls on it
    /// </summary>
	public class Panel : Control
	{
        public Panel(Window window, CreationFlag creationFlags) : base(window, creationFlags, TypeName)
        {
        }

        internal static String TypeName
        {
            get
            {
                return "panel";
            }
        }
    }
}
