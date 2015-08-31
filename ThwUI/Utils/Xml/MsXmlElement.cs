using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace ThW.UI.Utils
{
	internal class MsXmlElement : IXmlElement
	{
        public MsXmlElement(XElement node)
        {
            this.node = node;
            this.name = node.Name.ToString();
        }

		public override List<IXmlElement> Elements
        {
            get
            {
                if (false == this.loaded)
                {
                    foreach (XNode node in this.node.Nodes())
                    {
                        if (XmlNodeType.Element == node.NodeType)
                        {
                            XElement element = (XElement)node;

                            if (null != element)
                            {
                                this.elements.Add(new MsXmlElement(element));
                            }
                        }
                    }

                    this.loaded = true;
                }

                return this.elements;
            }
        }

        public override String GetAttributeValue(String name, String defaultValue)
        {
            XAttribute attribute = this.node.Attribute(XName.Get(name));

            if (null == attribute)
            {
                return defaultValue;
            }

            if (null == attribute.Value)
            {
                return defaultValue;
            }
            else
            {
                return attribute.Value;
            }
        }

		public override String Name
        {
            get
            {
                return this.name;
            }
        }

		private List<IXmlElement> elements = new List<IXmlElement>();
		private bool loaded = false;
        private XElement node = null;
		private String name = "";
	}
}
