using System;

namespace ThW.UI.Utils
{
    public interface ILogger
    {
        void WriteLine(String message);
		void WriteLine(LogLevel level, String message);
    }
}
