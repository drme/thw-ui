using System;

namespace ThW.UI.Utils.Images
{
    /// <summary>
    /// Load image from Targa file.
    /// </summary>
	internal class TgaImageLoader : IImageLoader
	{
        public TgaImageLoader(UIEngine engine)
        {
            this.engine = engine;
        }

		public virtual Image CreateImage(String fileName)
        {
            Object imageFile = null;
			uint size = 0;
			byte[] data = null;

            try
            {
                this.engine.OpenFile(fileName + ".tga", out data, out size, out imageFile);

                if (null == imageFile)
                {
                    return null;
                }

                if (size < 19)
                {
                    this.engine.CloseFile(ref imageFile);

                    return null;
                }

                uint width;
                uint height;
                byte pixelSize;

                byte[] pixels = LoadTGA((int)size, data, fileName, out width, out height, out pixelSize);

                Image img = new Image(fileName, pixels, width, height, pixelSize);

                if (pixelSize != 32)
                {
                    return Make32RGBA(img);
                }
                else
                {
                    return img;
                }
            }
            finally
            {
                this.engine.CloseFile(ref imageFile);
            }
        }

        private byte[] LoadTGA(int fileSize, byte[] tgaBytes, String fileName, out uint width, out uint height, out byte pixelSize)
        {
            if (fileSize < 19)
            {
                throw new Exception("TEXTURE: corrupted TGA file");
            }

            byte idLength = tgaBytes[0];
            byte colormapType = tgaBytes[1];
            byte imageType = tgaBytes[2];
            pixelSize = tgaBytes[16];
            width = ToUInt16(tgaBytes, 12);
            height = ToUInt16(tgaBytes, 14);
            byte descriptor = tgaBytes[17];

            if ((width <= 0) || (height <= 0) || ((pixelSize != 24) && (pixelSize != 32)))
            {
                throw new Exception("TEXTURE: Only 32 and 24 bit TGA images are supported");
            }

            if (imageType == 2)
            {
                if (fileSize < (int)((width * height * pixelSize / 8) + 18 + idLength))
                {
                    throw new Exception("TEXTURE: corrupted TGA file");
                }

                byte[] dst = new byte[width * height * pixelSize / 8];

                int imageDataOffset = 18 + idLength;
                int bpp = pixelSize / 8;
                bool needFlip = ((descriptor & 32) == 0);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        long dstPos;
                        long srcPos = imageDataOffset + (y * width + x) * bpp;

                        if (true == needFlip)
                        {
                            dstPos = ((height - y - 1) * width + x) * bpp;
                        }
                        else
                        {
                            dstPos = (y * width + x) * bpp;
                        }

                        dst[dstPos + 0] = tgaBytes[srcPos + 2];
                        dst[dstPos + 1] = tgaBytes[srcPos + 1];
                        dst[dstPos + 2] = tgaBytes[srcPos + 0];

                        if (4 == bpp)
                        {
                            dst[dstPos + 3] = tgaBytes[srcPos + 3];
                        }
                    }
                }

                return dst;
            }
            else if (imageType == 10)
            {
                byte[] imageBytes = new byte[width * height * pixelSize / 8];

                LoadCompressedTGA(tgaBytes, width, height, (uint)pixelSize / 8, imageBytes, 18 + (uint)idLength);

                if ((descriptor & 32) == 0)
                {
                    imageBytes = (Flip(width * pixelSize / 8, height, imageBytes));
                }

                return imageBytes;
            }

            throw new Exception("TEXTURE: unsupported TGA file type");
        }

        private byte[] Flip(uint width, uint height, byte[] imageBytes)
        {
            byte[] destinationBuffer = new byte[imageBytes.Length];

            for (int i = 0; i < height; i++)
            {
                Array.Copy(imageBytes, (int)(i * width), destinationBuffer, (int)((height - i - 1) * width), (int)width);
            }

            return destinationBuffer;
        }

        private void LoadCompressedTGA(byte[] compressedBuffer, uint width, uint height, uint bytesPerPixel, byte[] decompressedBuffer, uint imageDataOffset)
        {
            uint pixelCount = width * height;
            uint currentPixel = 0;
            uint currentByte = 0;
            uint index = imageDataOffset;

            do
            {
                byte chunkHeader = compressedBuffer[0 + index];
                index++;

                if (chunkHeader < 128)
                {
                    chunkHeader++;

                    for (short i = 0; i < chunkHeader; i++)
                    {
                        decompressedBuffer[currentByte + 0] = compressedBuffer[index + 2];
                        decompressedBuffer[currentByte + 1] = compressedBuffer[index + 1];
                        decompressedBuffer[currentByte + 2] = compressedBuffer[index + 0];

                        if (bytesPerPixel == 4)
                        {
                            decompressedBuffer[currentByte + 3] = compressedBuffer[index + 3];
                        }

                        index += bytesPerPixel;
                        currentByte += bytesPerPixel;
                        currentPixel++;

                        if (currentPixel > pixelCount)
                        {
                            throw new Exception("TEXTURE: Too many pixels read while decompressing TGA image");
                        }
                    }
                }
                else
                {
                    chunkHeader -= 127;

                    for (short i = 0; i < chunkHeader; i++)
                    {
                        decompressedBuffer[currentByte + 0] = compressedBuffer[index + 2];
                        decompressedBuffer[currentByte + 1] = compressedBuffer[index + 1];
                        decompressedBuffer[currentByte + 2] = compressedBuffer[index + 0];

                        if (bytesPerPixel == 4)
                        {
                            decompressedBuffer[currentByte + 3] = compressedBuffer[index + 3];
                        }

                        currentByte += bytesPerPixel;
                        currentPixel++;

                        if (currentPixel > pixelCount)
                        {
                            throw new Exception("TEXTURE: Too many pixels read while decompressing TGA image");
                        }
                    }

                    index += bytesPerPixel;
                }
            }
            while (currentPixel < pixelCount);
        }

        private Image Make32RGBA(Image img)
        {
            if (img.BitsPerPixel == 32)
            {
                return img;
            }
            else if (img.BitsPerPixel == 24)
            {
                Image dst = new Image(img.FileName, img.Width * img.Height * 4, img.Width, img.Height, 32);
                byte[] bytes = dst.Bytes;

                for (int y = 0; y < img.Height; y++)
                {
                    for (int x = 0; x < img.Width; x++)
                    {
                        bytes[y * img.Width * 4 + x * 4 + 0] = img.Bytes[y * img.Width * 3 + x * 3 + 0];
                        bytes[y * img.Width * 4 + x * 4 + 1] = img.Bytes[y * img.Width * 3 + x * 3 + 1];
                        bytes[y * img.Width * 4 + x * 4 + 2] = img.Bytes[y * img.Width * 3 + x * 3 + 2];
                        bytes[y * img.Width * 4 + x * 4 + 3] = 0xff;
                    }
                }

                return dst;
            }
            else
            {
                return null;
            }
        }

        private uint ToUInt16(byte[] data, int offset)
        {
            if (false == BitConverter.IsLittleEndian)
            {
                byte a = data[offset];
                data[offset] = data[offset + 1];
                data[offset + 1] = a;
            }

            return BitConverter.ToUInt16(data, offset);
        }

        private UIEngine engine = null;
    }
}
