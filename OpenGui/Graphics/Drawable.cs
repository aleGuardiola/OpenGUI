using SkiaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
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

    public class DrawableValueConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType.Equals(typeof(string));
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return base.ConvertFrom(context, culture, value);           
        }

    }

}
