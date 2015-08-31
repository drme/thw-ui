using System;
using System.Collections.Generic;
using ThW.UI.Utils;
using ThW.UI.Utils.Themes;

namespace ThW.UI.Design
{
    /// <summary>
    /// Event handler for property set value methods.
    /// </summary>
    /// <typeparam name="Type">property type.</typeparam>
    /// <param name="value">new value.</param>
    public delegate void SetValueHandler<Type>(Type value);
    /// <summary>
    /// Event handler for property get value methods.
    /// </summary>
    /// <typeparam name="Type">property type.</typeparam>
    /// <returns>property value.</returns>
    public delegate Type GetValueHandler<Type>();

    /// <summary>
    /// Control properties base class.
    /// </summary>
	public abstract class Property : UIObject
	{
        /// <summary>
        /// Constructs property with specified name, assigns it to the group and sets initial value.
        /// </summary>
    	protected Property(String propertyName, String propertyGroup, String description, String propertyEditControlType) : this()
        {
            this.Name = propertyName;
            this.Group = propertyGroup;
            this.ControlType = propertyEditControlType;
        }

        protected Property() : base("property")
        {
        }

        /// <summary>
        /// Converts property value to string.
        /// </summary>
        public override String ToString()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads property value from string.
        /// </summary>
        public abstract void FromString(String value, Theme theme);

        /// <summary>
        /// Returns list of supported values for this property.
		/// If list is apty any value is acceptable.
        /// </summary>
        public virtual List<String> GetAcceptableValues(Theme theme)
        {
            return acceptableValues;
        }

        /// <summary>
        /// Is current value held by property is default value.
		/// Checked when saving window, control to XML file. if value is default property is not saved.
        /// </summary>
        /// <returns>true if current value equals default value.</returns>
		public virtual bool IsDefault()
        {
            return false;
        }

        /// <summary>
        /// Return properties display name.
        /// </summary>
		public String Text
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// Return properties description.
        /// </summary>
        public String Description
        {
            get
            {
                return this.propertyDescription;
            }
            set
            {
                this.propertyDescription = value;
            }
        }

        /// <summary>
        /// Returns by what control to display this property in PropertyGrid, for example TextBox.
        /// </summary>
		public String ControlType
        {
            get
            {
                return this.propertyEditControlType;
            }
            set
            {
                this.propertyEditControlType = value;
            }
        }

        /// <summary>
        /// Gets properties' group.
        /// </summary>
		public String Group
        {
            get
            {
                return this.propertyGroup;
            }
            set
            {
                this.propertyGroup = value;
            }
        }

        /// <summary>
        /// Notifies subsribers that property value has changed.
        /// </summary>
		protected void RaiseChangeEvent()
        {
            if (null != this.ValueChanged)
            {
                this.ValueChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Property value casted to Object.
        /// </summary>
        public abstract Object Value
        {
            get;
            set;
        }

        /// <summary>
        /// Property value has changed throug property interface.
        /// Using from String method.
        /// </summary>
        public event UIEventHandler<Property> ValueChanged = null;

        private String propertyGroup = "";
        private String propertyDescription = "";
		private String propertyEditControlType = "";
		protected static List<String> acceptableValues = new List<String>();
	}

    /// <summary>
    /// Parametrized property base class.
    /// </summary>
    /// <typeparam name="T">property type</typeparam>
    public abstract class Property<T> : Property
    {
        /// <summary>
        /// Constructs property object.
        /// </summary>
        /// <param name="defaultValue">default value.</param>
        /// <param name="name">property name.</param>
        /// <param name="group">propertry group.</param>
        /// <param name="description">property description.</param>
        /// <param name="setter">property setter function.</param>
        /// <param name="getter">property getter function.</param>
        /// <param name="propertyEditControlType">control type for editing this property</param>
        protected Property(T defaultValue, String name, String group, String description, SetValueHandler<T> setter, GetValueHandler<T> getter, String propertyEditControlType) : base(name, group, description, propertyEditControlType)
        {
            this.defaultValue = defaultValue;
            this.setter = setter;
            this.getter = getter;
        }

        /// <summary>
        /// Converts property value to string. By default invokes getter and calls ToString on returned value.
        /// </summary>
        /// <returns>property valeu as a string.</returns>
        public override String ToString()
        {
            T value = this.getter();

            if (null != value)
            {
                return this.getter().ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Property value casted to Object.
        /// </summary>
        public override Object Value
        {
            get
            {
                return this.getter();
            }
            set
            {
                if (null != value)
                {
                    this.setter((T)value);
                }
            }
        }

        /// <summary>
        /// Is current value held by property is default value.
        /// Checked when saving window, control to XML file. if value is default property is not saved.
        /// </summary>
        /// <returns>true if current value equals default value.</returns>
        public override bool IsDefault()
        {
            if ((null != this.defaultValue) && (null != this.getter()))
            {
                return this.defaultValue.Equals(this.getter());
            }
            else
            {
                return ((null == this.defaultValue) && (null == this.getter()));
            }
        }

        protected readonly T defaultValue = default(T);
        protected readonly SetValueHandler<T> setter = null;
        protected readonly GetValueHandler<T> getter = null;
    }
}
