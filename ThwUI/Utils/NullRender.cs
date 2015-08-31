using System;
using ThW.UI.Utils.Images;

namespace ThW.UI.Utils
{
    internal class NullRender : IRender
    {
        public void DrawImage(int x, int y, int w, int h, IImage image, float us, float vs, float ue, float ve, Color color, bool outLineOnly)
        {
        }

        public IImage CreateImage(byte[] fileBytes, String fileName)
        {
			return new NullImage();
		}

        public IImage CreateImage(String fileName)
        {
			return new NullImage();
		}

        public IImage CreateImage(int w, int h, byte[] bytes)
        {
			return new NullImage();
		}

        public void DrawLine(int x1, int y1, int x2, int y2, Color color)
        {
        }
    }

	class NullImage : IImage
	{
		public override int Width
		{
			get
			{
				return 2;
			}
		}

		public override int Height
		{
			get
			{
				return 2;
			}
		}

		public override bool Loaded
		{
			get
			{
				return true;
			}
		}

		public override bool LoadFailed
		{
			get
			{
				return false;
			}
		}

		public override void Dispose()
		{
		}
	}
}
