using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ThW.UI.Utils;

namespace ThW.UI.Sample.Renderers.XNA
{
	public class XNARenderer : IRender
	{
		public XNARenderer(GraphicsDevice graphicsDevice, ContentManager content)
		{
			this.device = graphicsDevice;

			this.effect2d = new BasicEffect(this.device);
			this.effect2d.VertexColorEnabled = true;
			this.effect2d.TextureEnabled = true;

			this.blendState2d.ColorDestinationBlend = Blend.InverseSourceAlpha;
			this.blendState2d.AlphaDestinationBlend = Blend.InverseSourceAlpha;
			this.blendState2d.ColorSourceBlend = Blend.SourceAlpha;
			this.blendState2d.AlphaSourceBlend = Blend.SourceAlpha;

			this.depthStencilStateOn.DepthBufferEnable = true;
			this.depthStencilStateOn.DepthBufferFunction = CompareFunction.LessEqual;
			this.depthStencilStateOn.DepthBufferWriteEnable = true;

			this.rasterizerState.CullMode = CullMode.None;// CullMode.CullCounterClockwiseFace;
			this.rasterizerState.FillMode = FillMode.Solid;

			this.content = content;
		}

		public IImage CreateImage(byte[] fileBytes, String fileName)
		{
			if (fileName.ToLower().EndsWith(".tga"))
			{
				return null;
			}

			return new XNAImage(this.device, fileBytes);
		}

		public IImage CreateImage(String fileName)
		{
			if (this.missingTextures.Contains(fileName))
			{
				return null;
			}

			try
			{
				return new XNAImage(this.content.Load<Texture2D>(fileName));
			}
			catch (Exception)
			{
				this.missingTextures.Add(fileName);

				return null;
			}
		}

		public IImage CreateImage(int w, int h, byte[] imageBytes)
		{
			return new XNAImage(this.device, w, h, imageBytes);
		}

		public virtual void DrawImage(int x, int y, int w, int h, IImage image, float us, float vs, float ue, float ve, ThW.UI.Utils.Color color, bool outLineOnly)
		{
			float d = -0.5f;

			XNAVertex[] v = new XNAVertex[4];
			v[0].Position.X = (float)x + d;
			v[0].Position.Y = (float)y + d;
			v[0].Position.Z = 0.0f;
			v[0].TextureCoordinate.X = us;
			v[0].TextureCoordinate.Y = vs;
			v[0].Color.R = (byte)(255 * color.R);
			v[0].Color.G = (byte)(255 * color.G);
			v[0].Color.B = (byte)(255 * color.B);
			v[0].Color.A = (byte)(255 * color.A);

			v[1].Position.X = (float)x + (float)w + d;
			v[1].Position.Y = (float)y + d;
			v[1].Position.Z = 0.0f;
			v[1].TextureCoordinate.X = ue;
			v[1].TextureCoordinate.Y = vs;
			v[1].Color.R = (byte)(255 * color.R);
			v[1].Color.G = (byte)(255 * color.G);
			v[1].Color.B = (byte)(255 * color.B);
			v[1].Color.A = (byte)(255 * color.A);

			v[2].Position.X = (float)x + d;
			v[2].Position.Y = (float)y + (float)h + d;
			v[2].Position.Z = 0.0f;
			v[2].TextureCoordinate.X = us;
			v[2].TextureCoordinate.Y = ve;
			v[2].Color.R = (byte)(255 * color.R);
			v[2].Color.G = (byte)(255 * color.G);
			v[2].Color.B = (byte)(255 * color.B);
			v[2].Color.A = (byte)(255 * color.A);

			v[3].Position.X = (float)x + (float)w + d;
			v[3].Position.Y = (float)y + (float)h + d;
			v[3].Position.Z = 0.0f;
			v[3].TextureCoordinate.X = ue;
			v[3].TextureCoordinate.Y = ve;
			v[3].Color.R = (byte)(255 * color.R);
			v[3].Color.G = (byte)(255 * color.G);
			v[3].Color.B = (byte)(255 * color.B);
			v[3].Color.A = (byte)(255 * color.A);

			short[] indx = { 0, 1, 3, 3, 2, 0 };

			this.effect2d.Texture = ((XNAImage)image).Texture;

			foreach (EffectPass pass in this.effect2d.CurrentTechnique.Passes)
			{
				pass.Apply();

				device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, v, 0, 4, indx, 0, 2, this.vertexShaderDeclaration);
			}
		}

		public void DrawLine(int x1, int y1, int x2, int y2, ThW.UI.Utils.Color color)
		{
			throw new NotImplementedException();
		}

		public void Init2d()
		{
			this.device.RasterizerState = this.rasterizerState;
			this.device.DepthStencilState = this.depthStencilStateOn;
			this.device.BlendState = this.blendState2d;

			Matrix world = Matrix.Identity;
			Matrix view = Matrix.CreateLookAt(new Microsoft.Xna.Framework.Vector3(0.0f, 0.0f, 0.001f), Microsoft.Xna.Framework.Vector3.Zero, Microsoft.Xna.Framework.Vector3.Up);
			Matrix projection = Matrix.CreateOrthographicOffCenter(0, (float)this.device.Viewport.Width, (float)this.device.Viewport.Height, 0, -10.0f, 1000.0f);

			this.effect2d.World = world;
			this.effect2d.View = view;
			this.effect2d.Projection = projection;
		}

		private BasicEffect effect2d;
		private BlendState blendState2d = new BlendState();
		private VertexDeclaration vertexShaderDeclaration = new VertexDeclaration(XNAVertex.VertexElements);
		private DepthStencilState depthStencilStateOn = new DepthStencilState();
		private RasterizerState rasterizerState = new RasterizerState();
		private GraphicsDevice device;
		private ContentManager content;
		private HashSet<String> missingTextures = new HashSet<String>();
	}

	[StructLayout(LayoutKind.Sequential)]
	struct XNAVertex : IVertexType
	{
		public Microsoft.Xna.Framework.Vector3 Position;
		public Microsoft.Xna.Framework.Color Color;
		public Microsoft.Xna.Framework.Vector3 TextureCoordinate;
		public static readonly VertexElement[] VertexElements;

		static XNAVertex()
		{
			VertexElements = new VertexElement[]
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0), 
                new VertexElement(12, VertexElementFormat.Color, VertexElementUsage.Color, 0), 
                new VertexElement(16, VertexElementFormat.Vector3, VertexElementUsage.TextureCoordinate, 0)
            };
		}

		public VertexDeclaration VertexDeclaration
		{
			get
			{
				return new VertexDeclaration(XNAVertex.VertexElements);
			}
		}
	}
}
