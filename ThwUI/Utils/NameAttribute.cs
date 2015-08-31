using System;

namespace ThW.UI.Utils
{
    public class NameAttribute : System.Attribute
    {
        public NameAttribute(String name)
        {
            this.name = name;
        }

        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        private String name = "";
    }
}
