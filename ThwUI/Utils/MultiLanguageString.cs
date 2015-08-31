using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThW.UI.Utils
{
    internal class MultiLanguageString
    {
        /// <summary>
        /// Display text string.
        /// </summary>
        public String Text
        {
            get
            {
                if (null == this.text)
                {
                    return this.ReferenceText;
                }

                return this.text;
            }
			set
			{
				this.text = value;
			}
        }

		public bool NeedTranslation
		{
			get;
			set;
		}

        /// <summary>
        /// Key text string. In reference language usally.
        /// </summary>
        public String ReferenceText
        {
            get;
            set;
        }

		private String text;
	}
}
