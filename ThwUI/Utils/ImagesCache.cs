using System;
using System.Collections.Generic;
using System.Diagnostics;
using ThW.UI.Utils.Images;

namespace ThW.UI.Utils
{
    /// <summary>
    /// Images cache.
	/// Manages all images.
    /// In order to save memory. if two images required has the same name, the reference to the same image
	/// is returned
    /// </summary>
	internal class ImagesCache
	{
		public ImagesCache(UIEngine engine)
		{
			this.engine = engine;
		}

		~ImagesCache()
        {
            ILogger logger = this.engine.Logger;

            foreach (IImage image in this.cachedImages.Values)
            {
                String message = "Images: Leaked image " + image.Name + " (" + image.referenceCount + ")";

                if (null != logger)
                {
                    logger.WriteLine(message);
                }
                else
                {
                    Debug.WriteLine(message);
                }
            }
        }

        /// <summary>
        /// Creates requested image. If image file not found creates dotted texture.
        /// </summary>
		internal IImage CreateImage(String fileName, UIEngine engine, Graphics graphics)
        {
			if (null == fileName)
			{
				var img = new Image(fileName, new byte[2 * 2 * 4], 2, 2, 32);

				return graphics.CreateImage((int)img.Width, (int)img.Height, img.Bytes);
			}

            IImage image = null;
            Object file = null;

            try
            {
                this.cachedImages.TryGetValue(fileName, out image);

                if (null != image)
                {
                    return image;
                }

                if (true == fileName.StartsWith("#shell32,"))
                {
                    image = engine.GetIcon(int.Parse(fileName.Substring("#shell32,".Length)), true, null);

                    if (null != image)
                    {
                        return image;
                    }
                }

                image = graphics.CreateImage(fileName);

                if (null != image)
                {
                    return image;
                }

                byte[] imageData = null;
                uint size = 0;


                if (true == engine.OpenFile(fileName, out imageData, out size, out file))
                {
                    image = graphics.CreateImage(imageData, fileName);
                }
                else if (true == engine.OpenFile(fileName + ".png", out imageData, out size, out file))
                {
                    image = graphics.CreateImage(imageData, fileName + ".png");
                }
                else if (true == engine.OpenFile(fileName + ".jpg", out imageData, out size, out file))
                {
                    image = graphics.CreateImage(imageData, fileName + ".jpg");
                }
                else if (true == engine.OpenFile(fileName + ".tga", out imageData, out size, out file))
                {
                    image = graphics.CreateImage(imageData, fileName + ".tga");

                    if (null == image)
                    {
                        IImageLoader imageLoader = new TgaImageLoader(engine);

                        Image img = imageLoader.CreateImage(fileName);

                        if (null != img)
                        {
                            image = graphics.CreateImage((int)img.Width, (int)img.Height, img.Bytes);
                        }
                    }
                }

                return image;
            }
            finally
            {
                if (null != image)
                {
                    image.Name = fileName;
                    
                    image.AddRef();

                    this.cachedImages[fileName] = image;
                }

                if (null != file)
                {
                    engine.CloseFile(ref file);
                }
            }
        }

        internal IImage CreateImage(String uniqueName, int w, int h, byte[] bytes, Graphics graphics)
        {
            if (true == this.cachedImages.ContainsKey(uniqueName))
            {
                throw new Exception("Image already exists: " + uniqueName);
            }

            IImage image = graphics.CreateImage(w, h, bytes);

            if (null != image)
            {
                image.AddRef();
                
                image.Name = uniqueName;

                this.cachedImages[uniqueName] = image;

                return image;
            }

            return null;
        }

        /// <summary>
        /// Deletes image.
        /// </summary>
		internal void DeleteImage(ref IImage image)
        {
            if (null == image)
			{
                return;
            }

            if (image.Release() <= 0)
            {
                this.cachedImages.Remove(image.Name);
                image.Dispose();
            }
        }

		private IDictionary<String, IImage> cachedImages = new Dictionary<String, IImage>();
		private UIEngine engine;
	}
}
