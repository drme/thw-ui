using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using ThW.UI.Utils;

namespace ThW.UI.Sample.Renderers.XNA
{
	public class XNAAudio : IAudio
	{
		public XNAAudio(ContentManager content)
		{
			this.content = content;
		}

		public ISoundEffect LoadSound(String name)
		{
			try
			{
				return new XNASoundEffect(this.content.Load<SoundEffect>(name));
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);

				return null;
			}
		}

		public void PlaySound(ISoundEffect sound)
		{
			if (null != sound)
			{
				((XNASoundEffect)sound).Play();
			}
		}

		private ContentManager content = null;
	}
}
