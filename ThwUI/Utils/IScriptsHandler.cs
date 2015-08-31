using System;
using ThW.UI.Controls;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Global handler for User interface events.
    /// </summary>
	public interface IScriptsHandler
	{
        /// <summary>
        /// Called then control in user interfase is pressed and has some OnClick property value.
        /// </summary>
        /// <param name="script">clicked controls onclick value.</param>
        /// <param name="control">clicked control.</param>
		void OnUIEvent(String script, Control control);
	}
}
