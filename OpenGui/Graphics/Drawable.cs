using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Graphics
{
    /// <summary>
    /// Represent an object that can be draw.
    /// </summary>
    public abstract class Drawable
    {
        /// <summary>
        /// Draw the object in the canvas
        /// </summary>
        /// <param name="width">Width of the target</param>
        /// <param name="height">Height of the target</param>
        /// <param name="canvas">The canvas to draw</param>
        public abstract void Draw(int width, int height, SKCanvas canvas);        
    }
}
