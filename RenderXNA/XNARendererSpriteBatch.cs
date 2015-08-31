using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ThW.UI.Utils;

namespace ThW.UI.Sample.Renderers.XNA
{
	public class XNARendererSpriteBatch : XNARenderer
	{
		public XNARendererSpriteBatch(GraphicsDevice device, SpriteBatch spriteBatch, ContentManager content) : base(device, content)
		{
			this.spriteBatch = spriteBatch;
		}

		public override void DrawImage(int x, int y, int w, int h, IImage image, float us, float vs, float ue, float ve, Color color, bool outLineOnly)
		{
			Texture2D texture = ((XNAImage)image).Texture;
			Microsoft.Xna.Framework.Rectangle destinationRectangle = new Microsoft.Xna.Framework.Rectangle(x, y, w, h);
			Microsoft.Xna.Framework.Color imageColor = new Microsoft.Xna.Framework.Color(color.R, color.G, color.B, color.A);
			Microsoft.Xna.Framework.Rectangle sourceRectangle = new Microsoft.Xna.Framework.Rectangle((int)(us * texture.Width), (int)(ue * texture.Height), (int)(vs * texture.Width), (int)(ve * texture.Height));

			this.spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, imageColor);
		}

		private SpriteBatch spriteBatch;
	}
}
