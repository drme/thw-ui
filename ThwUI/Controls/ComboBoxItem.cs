using System;
using System.Collections.Generic;
using System.Text;
using ThW.UI.Utils;

namespace ThW.UI.Controls
{
	public class ComboBoxItem : UIObject
	{
		internal ComboBoxItem(UIEngine engine) : base("comboBoxItem")
        {
            this.engine = engine;
        }

		~ComboBoxItem()
        {
            SetIcon((IImage)null);
        }

		public String Icon
        {
            set
            {
                this.icon = value;
            }
            get
            {
                return this.icon;
            }
        }

		public String Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }


        public Object Tag
        {
            get
            {
                return this.tag;
            }
            set
            {
                this.tag = value;
            }
        }

        internal void SetIcon(IImage pImage)
        {
			this.engine.DeleteImage(ref this.image);
			this.image = pImage;
        }

        internal void LoadAttributes(IXmlElement element)
        {
			base.Name = element.GetAttributeValue("name", base.Name);
			this.text = element.GetAttributeValue("text", this.text);
			this.Icon = element.GetAttributeValue("icon", this.Icon);
        }

        internal void WriteAttributes(IXmlWriter serializer)
        {
			serializer.WriteAttribute("name", this.Name);
			serializer.WriteAttribute("text", this.text);
			serializer.WriteAttribute("icon", this.Icon);
        }

        private UIEngine engine = null;
		private String text = "";
		private String icon = "";
        private Object tag = null;
        internal IImage image = null;
	}
}
