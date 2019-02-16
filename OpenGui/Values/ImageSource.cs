using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenGui.Values
{
    public abstract class ImageSource
    {
        /// <summary>
        /// Get the image bitmap to render.
        /// </summary>
        /// <param name="width">The width where the image is going to render.</param>
        /// <param name="height">The height where the image is going to render.</param>
        /// <returns></returns>
        public abstract Task<SKBitmap> GetImage(int width, int height);
    }
}
