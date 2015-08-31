using ThW.UI.Windows;

namespace ThW.UI.Utils
{
	/// <summary>
	/// Constructs window object, assigngs desktop to it.
	/// </summary>
	/// <param name="desktop">the desktop to that created window belongs</param>
	/// <returns>created window</returns>
	public delegate Window WindowCreator(Desktop desktop);
}
