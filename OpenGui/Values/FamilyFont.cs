using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;

namespace OpenGui.Values
{
    public class FamilyFont : Font
    {
        string _fontFamily;

        SKTypeface cachedTypeface = null;

        public FamilyFont(string fontFamily)
        {
            _fontFamily = fontFamily ?? throw new ArgumentNullException(nameof(fontFamily));
        }

        public override SKTypeface GetTypeface(FontStyle fontStyle, int weight)
        {
            if (!(cachedTypeface != null && cachedTypeface.FontSlant == fontStyle.ToSKFontStyleSlant() && cachedTypeface.FontWeight == weight))
            {
                cachedTypeface = SKTypeface.FromFamilyName(_fontFamily, weight, 0, fontStyle.ToSKFontStyleSlant());
            }

            return cachedTypeface;
        }
    }
}
