using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Values
{
    public abstract class Font
    {
        public abstract SKTypeface GetTypeface(FontStyle fontStyle, int weight);        
    }
}
