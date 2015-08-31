using ThW.UI.Controls;
using ThW.UI.Utils;
using ThW.UI.Windows;

namespace ThW.UI.Design
{
    /// <summary>
    /// Interface for control creation factory
    /// </summary>
	public interface IControlsCreator
	{
        /// <summary>
        /// Creates control, assigns window window to it and passes creation parameters.
        /// </summary>
        /// <param name="window">window the control belongs to</param>
        /// <param name="creationFlags">creation flags</param>
        /// <returns>created control</returns>
		Control CreateControl(Window window, CreationFlag creationFlags);
        /// <summary>
        /// Is control visible in a designer.
        /// </summary>
        bool ShowInDesigner
        {
            get;
        }
	};
}
