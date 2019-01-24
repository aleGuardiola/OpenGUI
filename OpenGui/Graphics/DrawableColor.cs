using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using SkiaSharp;

namespace OpenGui.Graphics
{
    /// <summary>
    /// Represent a single drawable color
    /// </summary>
    public class DrawableColor : Drawable
    {
        SKColor _skColor;
        Color _color;

        Color Color
        {
            get => _color;
        }

        /// <summary>
        /// Create a drawable color.
        /// </summary>
        /// <param name="color">The color to draw</param>
        public DrawableColor(Color color)
        {
            _color = color;
            _skColor = new SKColor(color.R, color.G, color.B, color.A);
        }

        public override void Draw(int width, int height, SKCanvas canvas)
        {
            canvas.Clear(_skColor);
        }
    }
}
