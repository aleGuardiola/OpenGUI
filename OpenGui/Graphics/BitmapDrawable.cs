using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace OpenGui.Graphics
{
    public class BitmapDrawable : Drawable
    {
        /// <summary>
        /// Get the bitmap used to draw
        /// </summary>
        public SKBitmap Bitmap { get; private set; }

        public BitmapDrawable(SKBitmap bitmap)
        {
            Bitmap = bitmap;
        }

        public override void Draw(int width, int height, SKCanvas canvas)
        {
            using (var paint = new SKPaint())
            {
                canvas.DrawBitmap(Bitmap, 0, 0, paint);
            }
                
        }
    }
}
