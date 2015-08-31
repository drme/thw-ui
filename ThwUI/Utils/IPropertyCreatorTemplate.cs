using System;
using ThW.UI.Design;

namespace ThW.UI.Utils
{
	/// <summary>
	/// Default implmenetation for IPropertyCreator interface.
	/// </summary>
	/// <typeparam name="PropertyType">Property type to create.</typeparam>
    public class IPropertyCreatorTemplate<PropertyType> : IPropertyCreator where PropertyType : new()
	{
		public Property CreateProperty(String name, String group, String text)
		{
			Object property = new PropertyType();

            Property p = (Property)property;

            p.Name = name;
            p.Text = text;
            p.Group = group;

            return p;
		}
	}
}
