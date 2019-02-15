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
        /// Get the placeholder to use while the image is loading.
        /// </summary>
        /// <param name="width">The width where the image is going to render.</param>
        /// <param name="height">The height where the image is going to render.</param>
        /// <param name="placeholder">The bitmap placeholder to use.</param>
        /// <returns>false if not placeholder is using or true if the placeholder is returned.</returns>
        public abstract bool TryGetImagePlaceholder(int width, int height, out SKBitmap placeholder);

        /// <summary>
        /// Get the image bitmap to render.
        /// </summary>
        /// <param name="width">The width where the image is going to render.</param>
        /// <param name="height">The height where the image is going to render.</param>
        /// <returns></returns>
        public abstract Task<SKBitmap> GetImage(int width, int height);
    }
}
