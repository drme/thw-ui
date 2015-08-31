using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Base class for all objects
    /// </summary>
    public class UIObject
    {
        /// <summary>
        /// Constructs object.
        /// </summary>
        /// <param name="type">object type.</param>
        public UIObject(String type)
        {
            this.objectType = type;
        }

        /// <summary>
        /// Constructs object.
        /// </summary>
        /// <param name="type">object type.</param>
        /// <param name="name">object name.</param>
        public UIObject(String type, String name) : this(type)
        {
            this.objectName = name;
        }

        /// <summary>
        /// UI object name
        /// </summary>
        public String Name
        {
            get
            {
                return this.objectName;
            }
            set
            {
                this.objectName = value;
            }
        }

        /// <summary>
        /// UI object type
        /// </summary>
        public String Type
        {
            get
            {
                return this.objectType;
            }
            set
            {
                this.objectType = value;
            }
        }

        private String objectType = null;
        private String objectName = null;
    }
}
