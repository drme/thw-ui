using System;

namespace ThW.UI.Utils
{
	/// <summary>
	/// Interface for saving and loading window runtime properties.
	/// </summary>
	public interface IConfig
	{
		void SaveValue(String name, String value);
		String LoadValue(String name, String defaultValue);
	};
}
