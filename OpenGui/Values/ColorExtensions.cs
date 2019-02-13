using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace OpenGui.Values
{
    public static class ColorExtensions
    {
        public static SKColor ToSkiaColor(this Color color)
        {
            return new SKColor(color.R, color.G, color.B, color.A);
        }
    }
}
