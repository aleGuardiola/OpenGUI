using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using SkiaSharp;

namespace OpenGui.Values
{
    public class EmbededFont : Font
    {
        Assembly _assembly;
        string _resource;

        SKTypeface cachedTypeface = null;

        public EmbededFont(Assembly assembly, string resource)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }

        public override SKTypeface GetTypeface(FontStyle fontStyle, int weight)
        {
            if (!(cachedTypeface != null && cachedTypeface.FontSlant == fontStyle.ToSKFontStyleSlant() && cachedTypeface.FontWeight == weight))
            {
                var stream = _assembly.GetManifestResourceStream(_resource);
                cachedTypeface = SKTypeface.FromStream(stream);
            }                

            return cachedTypeface;
        }
    }
}
