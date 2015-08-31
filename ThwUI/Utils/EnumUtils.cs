using System;
using System.Reflection;

namespace ThW.UI.Utils
{
    internal class EnumUtils
    {
        public static String ToString(Enum value)
        {
#if WINDOWS
            FieldInfo fi = value.GetType().GetField(value.ToString());

            NameAttribute[] attributes = (NameAttribute[])fi.GetCustomAttributes(typeof(NameAttribute), false);
            
            if (attributes.Length > 0)
            {
                return attributes[0].Name;
            }
            else
#endif
            {
                return value.ToString();
            }
        }

/*        public static Object FromString(string value, Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (stringValueOf((Enum)Enum.Parse(enumType, name)).Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }
            }

            throw new ArgumentException("The string is not a description or value of the specified enum.");
        } */
    }
}
