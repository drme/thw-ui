using System;

namespace ThW.UI.Utils
{
    /// <summary>
    /// The interface for rendering User interface objects
    /// </summary>
    public interface IRender
    {
        /// <summary>
        /// Draws a square image.
        /// </summary>
        /// <param name="X">top left coordinate X</param>
        /// <param name="Y">top left coordinate Y</param>
        /// <param name="Width">Width</param>
        /// <param name="Height">Height</param>
        /// <param name="image">image to draw</param>
        /// <param name="us">top left texture coordinate X</param>
        /// <param name="vs">top left texture coordinate Y</param>
        /// <param name="ue">bottom right texture coordinate X</param>
        /// <param name="ve">bottom right texture coordinate Y</param>
        /// <param name="color">image color</param>
        /// <param name="outLineOnly">render wireframe</param>
        void DrawImage(int x, int y, int w, int h, IImage image, float us, float vs, float ue, float ve, Color color, bool outLineOnly);
        /// <summary>
        /// Creates the image object form file bytes
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="fileName">file name of the file thats contents are passed as byte array.</param>
        /// <returns></returns>
        IImage CreateImage(byte[] fileBytes, String fileName);
        /// <summary>
        /// Creates the image object from a file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IImage CreateImage(String fileName);
        /// <summary>
        /// Creates the image object from loaded pixels
        /// Its intended use is for creating fonts letters.
        /// The UI library will call this function by passing texture data for render in order to create real texture and return it as an IImage object.
        /// </summary>
        /// <param name="Width">image Width</param>
        /// <param name="Height">image Height</param>
        /// <param name="imageBytes">actual image data (Width * Height * 4) 32-bits RGBA image data</param>
        /// <returns></returns>
        IImage CreateImage(int w, int h, byte[] imageBytes);
        /// <summary>
        /// Draw line
        /// </summary>
        /// <param name="x1">start position</param>
        /// <param name="y1">start position</param>
        /// <param name="x2">end position</param>
        /// <param name="y2">end position</param>
        /// <param name="color">line color</param>
        void DrawLine(int x1, int y1, int x2, int y2, Color color);
    }
}
