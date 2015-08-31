using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Template IXmlFactory implementation.
    /// </summary>
    internal class IXmlFactoryImpl : IXmlFactory
    {
        public virtual IXmlReader CreateXmlReader(UIEngine engine)
        {
            return new MsXmlReader(engine);
        }

        public virtual IXmlWriter CreateXmlWriter(UIEngine engine)
        {
            return new TextXmlWriter(engine);
        }
    }
}
