using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Xml Factory
    /// </summary>
	internal interface IXmlFactory
	{
        IXmlReader CreateXmlReader(UIEngine engine);
        IXmlWriter CreateXmlWriter(UIEngine engine);
	}
}
