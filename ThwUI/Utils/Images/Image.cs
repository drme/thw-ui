using System;

namespace ThW.UI.Utils.Images
{
	public class Image
	{
		public Image(String fileName, uint size, uint width, uint height, uint bitsPerPixel)
        {
			this.data = new byte[size];
			this.width = width;
			this.height = height;
			this.IsLoaded = false;
			this.bitsPerPixel = bitsPerPixel;
			this.fileName = fileName;
        }

		public Image(String fileName, byte[] bytes, uint width, uint height, uint bitsPerPixel)
        {
			this.data = bytes;
			this.width = width;
			this.height = height;
			this.IsLoaded = false;
			this.bitsPerPixel = bitsPerPixel;
			this.fileName = fileName;
        }

		public bool IsLoaded
		{
			get;
			set;
		}

        /// <summary>
        /// Image file name, the image was loaded from.
        /// </summary>
        public String FileName
        {
            get
            {
                return this.fileName;
            }
        }

        /// <summary>
        /// Image Width.
        /// </summary>
        public uint Width
        {
            get
            {
                return this.width;
            }
        }

        /// <summary>
        /// Image Height.
        /// </summary>
        public uint Height
        {
            get
            {
                return this.height;
            }
        }

        /// <summary>
        /// Image bits per pixel.
        /// </summary>
        public uint BitsPerPixel
        {
            get
            {
                return this.bitsPerPixel;
            }
        }

        /// <summary>
        /// Image raw pixels.
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                return this.data;
            }
			set
			{
				this.data = value;
			}
        }

        /// <summary>
        /// Convert image to grayscale.
        /// </summary>
		public void MakeGrayScale()
        {
    		uint size = this.width * this.height;
		
			switch (this.bitsPerPixel)
			{
				case 32:
					{
						for (uint i = 0; i < size; i++)
						{
                            long sum = this.data[i * 4 + 0] + this.data[i * 4 + 1] + this.data[i * 4 + 2];
							byte avg = (byte)(sum /3);

                            this.data[i * 4 + 0] = avg;
                            this.data[i * 4 + 1] = avg;
                            this.data[i * 4 + 2] = avg;
						}
					}
					break;
				case 24:
					{
						for (uint i = 0; i < size; i++)
						{
                            long sum = this.data[i * 3 + 0] + this.data[i * 3 + 1] + this.data[i * 3 + 2];
							byte avg = (byte)(sum /3);

                            this.data[i * 3 + 0] = avg;
                            this.data[i * 3 + 1] = avg;
                            this.data[i * 3 + 2] = avg;
						}
					}
					break;
				case 8:
					// allready grayscale
					break;
				default:
                    throw new NotSupportedException();
			}
        }

        /// <summary>
        /// Scales image.
        /// </summary>
        /// <param name="newWidth">new image Width<./param>
        /// <param name="newHeight">new image Height.</param>
        /// <returns>Scaled image.</returns>
		public Image Scale(uint newWidth, uint newHeight)
        {
			Image scaledImages = new Image(this.fileName, newWidth * newHeight * this.bitsPerPixel / 8, newWidth, newHeight, this.bitsPerPixel);
		
			Scale(this, scaledImages.Bytes, newWidth, newHeight, this.bitsPerPixel / 8);
		
			return scaledImages;
        }

        /// <summary>
        /// Scales image.
        /// </summary>
        /// <param name="sourceImage">source image.</param>
        /// <param name="targetImageBytes">scaled image bytes to fill.</param>
        /// <param name="targetWidth">scaled image Width.</param>
        /// <param name="targetHeight">scaled image Height.</param>
        /// <param name="bytesPerPixel">source image bits per pixel.</param>
        public static void Scale(Image sourceImage, byte[] targetImageBytes, uint targetWidth, uint targetHeight, uint bytesPerPixel)
        {
			float sx = 0.0F;
			float sy = 0.0F;
			uint sourceWidth = sourceImage.Width;
			uint sourceHeight = sourceImage.Height;
			byte[] sourceBytes = sourceImage.Bytes;
		
			if (targetWidth  > 1)
			{
				sx = (float) (sourceWidth - 1) / (float) (targetWidth - 1);
			}
			else
			{
				sx = (float) (sourceWidth - 1);
			}
		
			if (targetHeight > 1)	
			{
				sy = (float) (sourceHeight - 1) / (float) (targetHeight - 1);
			}
			else
			{
				sy = (float) (sourceHeight - 1);
			}
		
    		if ( (sx < 1.0) && (sy < 1.0) )
			{
                uint i0, i1, j0, j1;
                float alpha, beta;
                uint src00, src01, src10, src11;
                float s1, s2;
                byte[] dst = targetImageBytes;
		
				for (uint i = 0; i < targetHeight; i++)
				{
					i0 = (uint)(i * sy);
					i1 = i0 + 1;
		
					if (i1 >= sourceHeight)
					{
						i1 = sourceHeight - 1;
					}
		
					alpha = i * sy - i0;
		
					for (uint j = 0; j < targetWidth; j++)
					{
						j0 = (uint)(j * sx);
						j1 = j0 + 1;
		
						if (j1 >= sourceWidth)
						{
							j1 = sourceWidth - 1;
						}
		
						beta = j * sx - j0;
		
						src00 =   (i0 * sourceWidth + j0) * bytesPerPixel;
						src01 =   (i0 * sourceWidth + j1) * bytesPerPixel;
						src10 =   (i1 * sourceWidth + j0) * bytesPerPixel;
						src11 =   (i1 * sourceWidth + j1) * bytesPerPixel;
		
                        uint dst_id = (i * targetWidth + j) * bytesPerPixel;
						//dst = pOut + (i * uOutWidth + j) * bytesPerPixel;
		
						for (int k = 0; k < bytesPerPixel; k++)
						{
							s1 = (float)(sourceBytes[src00++] * (1.0 - beta) + sourceBytes[src01++] * beta);
							s2 = (float)(sourceBytes[src10++] * (1.0 - beta) + sourceBytes[src11++] * beta);
							//*dst++ = (unsigned char)(s1 * (1.0 - alpha) + s2 * alpha);
							dst[dst_id + k] = (byte)(s1 * (1.0 - alpha) + s2 * alpha);
						}
					}
				}
			}
			else
			{
                uint i0, i1;
                uint j0, j1;
                uint ii, jj;
                float sum;
                byte[] dst = targetImageBytes;
		
				for (uint i = 0; i < targetHeight; i++)
				{
					i0 = (uint)(i * sy);
					i1 = i0 + 1;
		
					if (i1 >= sourceHeight)
					{
						i1 = sourceHeight - 1;
					}
		
					for (uint j = 0; j < targetWidth; j++)
					{
						j0 = (uint)(j * sx);
						j1 = j0 + 1;
		
						if (j1 >= sourceWidth)
						{
							j1 = sourceWidth - 1;
						}
		
                        uint dst_id = (i * targetWidth + j) * bytesPerPixel;
    					//dst = pOut + (i * uOutWidth + j) * bytesPerPixel;

                        for (int k = 0; k < bytesPerPixel; k++)
						{
							sum = 0; //0.0;
		
							for (ii = i0; ii <= i1; ii++)
							{
								for (jj = j0; jj <= j1; jj++)
								{
									//sum += *(pIn + (ii * uInWidth + jj) * bytesPerPixel + k);
                                    sum += sourceBytes[(ii * sourceWidth + jj) * bytesPerPixel + k];
								}
							}
		
							sum /= (j1 - j0 + 1) * (i1 - i0 + 1);
							//*dst++ = (unsigned char)sum;
                            dst[dst_id + k] = (byte)sum;
						}
					}
				}
			}
        }

        /// <summary>
        /// Gets image as Targa image.
        /// </summary>
        /// <param name="Width">source image Width.</param>
        /// <param name="Height">source image Height.</param>
        /// <param name="imageData">source image bytes.</param>
        /// <param name="bitsPerPixel">source iamge bits per pixel.</param>
        /// <returns>Targa image bytes.</returns>
        public static byte[] GetTGA(uint width, uint height, byte[] imageData, uint bitsPerPixel)
        {
            int off = 18;

            byte[] buffer = new byte[width * height * 4 + off];

            buffer[0] = 0;
            buffer[1] = 0;
            buffer[2] = 2;
            buffer[3] = 0;
            buffer[4] = 0;
            buffer[5] = 0;
            buffer[6] = 0;
            buffer[7] = 0;
            buffer[8] = 0;
            buffer[9] = 0;
            buffer[10] = 0;
            buffer[11] = 0;
            buffer[12] = BitConverter.GetBytes(width)[0];
            buffer[13] = BitConverter.GetBytes(width)[1];
            buffer[14] = BitConverter.GetBytes(height)[0];
            buffer[15] = BitConverter.GetBytes(height)[1];
            buffer[16] = 32;
            buffer[17] = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (24 == bitsPerPixel)
                    {
                        buffer[off + ((height - y - 1) * width + x) * 4 + 2] = imageData[(y * width + x) * 3 + 0];
                        buffer[off + ((height - y - 1) * width + x) * 4 + 1] = imageData[(y * width + x) * 3 + 1];
                        buffer[off + ((height - y - 1) * width + x) * 4 + 0] = imageData[(y * width + x) * 3 + 2];
                        buffer[off + ((height - y - 1) * width + x) * 4 + 3] = 0xff;
                    }
                    else if (32 == bitsPerPixel)
                    {
                        buffer[off + ((height - y - 1) * width + x) * 4 + 2] = imageData[(y * width + x) * 4 + 0];
                        buffer[off + ((height - y - 1) * width + x) * 4 + 1] = imageData[(y * width + x) * 4 + 1];
                        buffer[off + ((height - y - 1) * width + x) * 4 + 0] = imageData[(y * width + x) * 4 + 2];
                        buffer[off + ((height - y - 1) * width + x) * 4 + 3] = imageData[(y * width + x) * 4 + 3];
                    }
                    else
                    {
                        throw new Exception("Unsupported bits per pixel " + bitsPerPixel);
                    }
                }
            }

            return buffer;
        }

		private byte[] data = null;
        private uint width = 0;
        private uint height = 0;
        private uint bitsPerPixel = 0;
        private String fileName = "";
	}
}
