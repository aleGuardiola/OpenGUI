using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenGui.Core;
using OpenGui.Values;
using SkiaSharp;

namespace OpenGui.Controls
{
    public class Label : View
    {
        public string Text
        {
            get => GetValue<string>();
            set => SetValue<string>(value);
        }
        
        public Color TextColor
        {
            get => GetValue<Color>();
            set => SetValue<Color>(value);
        }

        public float TextSize
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        public Label()
        {
            SetValue<string>(nameof(Text), ReactiveObject.LAYOUT_VALUE, "");
            SetValue<Color>(nameof(TextColor), ReactiveObject.LAYOUT_VALUE, Color.Black);
            SetValue<float>(nameof(TextSize), ReactiveObject.LAYOUT_VALUE, 30f);
        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            return base.OnMesure(widthSpec, heightSpec, mode);
        }

        protected override void DrawTexture(SKCanvas canvas, int width, int height)
        {
            //base.DrawTexture(canvas, width, height);
            canvas.Clear(SKColors.White);
            using (var textPaint = new SKPaint())
            {
                textPaint.Color = TextColor.ToSkiaColor();
                textPaint.TextSize = TextSize;
                textPaint.IsAntialias = true;
                textPaint.IsStroke = false;

                canvas.DrawText(Text, width/2, height/2, textPaint);
            }

        }
    }
}
