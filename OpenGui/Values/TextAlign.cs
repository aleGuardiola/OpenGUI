using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Values
{
    public enum TextAlign
    {
        /// <summary>
        /// Left align the text.
        /// </summary>
        Left = 0,
        /// <summary>
        /// Center the text.
        /// </summary>        
        Center = 1,
        /// <summary>
        /// Right align the text.
        /// </summary>
        Right = 2
    }

    public static class TextAlignExtensions
    {
        public static SKTextAlign ToSkiaTextAlign(this TextAlign textAlign)
        {
            return (SKTextAlign)(int)textAlign;
        }
    }

}
