using Microsoft.Xna.Framework.Audio;
using ThW.UI.Utils;

namespace ThW.UI.Sample.Renderers.XNA
{
	class XNASoundEffect : ISoundEffect
	{
		public XNASoundEffect(SoundEffect effect)
		{
			this.effect = effect;
		}

		public void Play()
		{
			if (null != this.effect)
			{
				this.effect.Play();
			}
		}

		private SoundEffect effect = null;
	}
}
