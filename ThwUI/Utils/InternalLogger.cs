using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThW.UI.Utils;

namespace ThW.UI.Utils
{
	/// <summary>
	/// Internal logger for debugging.
	/// </summary>
	internal class InternalLogger : ILogger
	{
		public void WriteLine(String message)
		{
			System.Diagnostics.Debug.WriteLine(message);
		}

		public void WriteLine(LogLevel level, String message)
		{
			WriteLine("INFO: " + message);
		}
	}
}
