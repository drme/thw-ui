using System;
using System.Collections.Generic;

namespace ThW.UI.Utils
{
	/// <summary>
	/// Interface for User interface texts translation.
	/// </summary>
	public class ILanguage
	{
		/// <summary>
		/// Translates text from reference tlanguage to some other.
		/// </summary>
		/// <param name="group">language group.</param>
		/// <param name="textInReferenceLanguage">reference text.</param>
		/// <returns></returns>
		public virtual String Translate(String group, String textInReferenceLanguage)
        {
    		return textInReferenceLanguage;
        }

		/// <summary>
		/// Calls all registered event handlers notifying about language change.
		/// </summary>
		public virtual void RaiseLanguageChanged()
		{
			if (null != this.LanguageChanged)
			{
				this.LanguageChanged();
			}
        }

		public event LanguageChangedHandler LanguageChanged;
	}
}
