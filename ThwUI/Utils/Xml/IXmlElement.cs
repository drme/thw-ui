using System;
using System.Collections.Generic;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Represents Xml documents node element actually it's <node lialia="value" /> element.
    /// </summary>
	public abstract class IXmlElement
	{
        /// <summary>
        /// Element's name
        /// </summary>
        public abstract String Name
        {
            get;
        }

        /// <summary>
        /// List of child's nodes. Actually only nodes of type "Element" is required, thus attributes, text etc.. are not required to be returned by this method.
		/// The created list of Elements has to be destroyed in this class destructor.
        /// </summary>
        public abstract List<IXmlElement> Elements
        {
            get;
        }

        /// <summary>
        /// Returns attributes value. If attribute is nor found, the strDefaultValue has to be returned.
        /// </summary>
        public abstract String GetAttributeValue(String name, String defaultValue);
        /// <summary>
        /// Returns attributes value. If attribute is not found, the null has to be returned.
        /// </summary>
        public String GetAttributeValue(String name)
        {
            return GetAttributeValue(name, null);
        }
    }
}
