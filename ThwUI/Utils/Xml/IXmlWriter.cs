using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Writes data to Xml file.
    /// </summary>
	public interface IXmlWriter : IDisposable
	{
        /// <summary>
        /// Creates file for xml writing. If file already exits, it has to be overwritten.
        /// </summary>
		void OpenFile(String strFileName);
        /// <summary>
        /// Writes opening tag. Like "<hello ".
        /// </summary>
        /// <param name="name"></param>
		void OpenTag(String name);
        /// <summary>
        /// Closes opened tag. Writes " </hello>" or " />".
        /// </summary>
		void CloseTag();
        /// <summary>
        /// Writes attribute to currently opened tag.
        /// </summary>
		void WriteAttribute(String name, String value);
	}
}
