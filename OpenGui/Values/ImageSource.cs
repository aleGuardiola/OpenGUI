using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace OpenGui.Values
{
    [TypeConverter(typeof(ImageSourceTypeConverter))]
    public abstract class ImageSource
    {
        /// <summary>
        /// Get the image bitmap to render.
        /// </summary>
        /// <param name="width">The width where the image is going to render.</param>
        /// <param name="height">The height where the image is going to render.</param>
        /// <returns></returns>
        public abstract Task<SKBitmap> GetImage(int width, int height);

        /// <summary>
        /// Return true if the bitmap is cached in the memory.
        /// </summary>
        public abstract bool IsCached(int width, int height);
        
    }

    public class ImageSourceTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if(value is string)
            {
                var strValue = value as string;
                var uri = new Uri(strValue);
                if(uri != null)
                {
                    if(uri.Scheme == "http" || uri.Scheme == "https")
                    {
                        return new WebImageSource(strValue);
                    }
                }
            }

            return null;
        }

    }

}
