using System;
using ThW.UI.Design;

namespace ThW.UI.Utils
{
	/// <summary>
	/// Interface for creating oprerties objects. While registering to UIEngine use IPropertyCreatorTemplate class.
	/// </summary>
	public interface IPropertyCreator
	{
		/// <summary>
		/// Creates property.
		/// </summary>
		/// <param name="name">property name.</param>
		/// <param name="group">property group.</param>
		/// <param name="text">property label.</param>
		/// <returns>created property.</returns>
		Property CreateProperty(String name, String group, String text);
	}
}
