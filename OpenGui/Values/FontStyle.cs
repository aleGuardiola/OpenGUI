using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Values
{
    public enum FontStyle
    {
        Upright = 0,        
        Italic = 1,        
        Oblique = 2
    }

    public static class FontStyleExtensions
    {
        public static SKFontStyleSlant ToSKFontStyleSlant(this FontStyle fontStyle)
        {
            return (SKFontStyleSlant)(int)fontStyle;
        }
    }

}
