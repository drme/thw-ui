using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Reads Xml document. Something like DOM wrapper
    /// </summary>
	internal interface IXmlReader : IDisposable
	{
        /// <summary>
        /// Opens file for parsing
        /// </summary>
		void OpenFile(String fileName);
        /// <summary>
        /// Gets the root element of parsed file.
		/// The pointer will be deleted while releasing this parser.
        /// </summary>
        IXmlElement RootElement { get; }
	}
}
