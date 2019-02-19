using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenGui.Core;
using OpenGui.Values;
using SkiaSharp;
using Font = OpenGui.Values.Font;
using FontStyle = OpenGui.Values.FontStyle;

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

        public TextAlign TextAlign
        {
            get => GetValue<TextAlign>();
            set => SetValue<TextAlign>(value);
        }

        public float TextScale
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        public bool IsTextVertical
        {
            get => GetValue<bool>();
            set => SetValue<bool>(value);
        }

        public float TextSkew
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        public Font Font
        {
            get => GetValue<Font>();
            set => SetValue<Font>(value);
        }

        public FontStyle FontStyle
        {
            get => GetValue<FontStyle>();
            set => SetValue<FontStyle>(value);
        }

        public int FontWeight
        {
            get => GetValue<int>();
            set => SetValue<int>(value);
        }

        public Label()
        {
            SetValue<string>(nameof(Text), ReactiveObject.LAYOUT_VALUE, "");
            SetValue<Color>(nameof(TextColor), ReactiveObject.LAYOUT_VALUE, Color.Black);
            SetValue<float>(nameof(TextSize), ReactiveObject.LAYOUT_VALUE, 30f);
            SetValue<TextAlign>(nameof(TextAlign), ReactiveObject.LAYOUT_VALUE, TextAlign.Left);

            SetValue<float>(nameof(TextScale), ReactiveObject.LAYOUT_VALUE, 1);
            SetValue<bool>(nameof(IsTextVertical), ReactiveObject.LAYOUT_VALUE, false);
            SetValue<Font>(nameof(Font), ReactiveObject.LAYOUT_VALUE, new FamilyFont("Normal"));
            SetValue<FontStyle>(nameof(FontStyle), ReactiveObject.LAYOUT_VALUE, FontStyle.Oblique);
            SetValue<int>(nameof(FontWeight), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(TextSkew), ReactiveObject.LAYOUT_VALUE, 0f);

            //force to draw when any of this changed
            SubscriptionPool.Add(GetObservable<string>(nameof(Text)).Subscribe((next) => ForzeDraw()));
            SubscriptionPool.Add(GetObservable<Color>(nameof(TextColor)).Subscribe((next) => ForzeDraw()));
            SubscriptionPool.Add(GetObservable<float>(nameof(TextSize)).Subscribe((next) => ForzeDraw()));
            SubscriptionPool.Add(GetObservable<TextAlign>(nameof(TextAlign)).Subscribe((next) => ForzeDraw()));
            SubscriptionPool.Add(GetObservable<float>(nameof(TextScale)).Subscribe((next) => ForzeDraw()));
            SubscriptionPool.Add(GetObservable<float>(nameof(TextSkew)).Subscribe((next) => ForzeDraw()));
            SubscriptionPool.Add(GetObservable<Font>(nameof(Font)).Subscribe((next) => ForzeDraw()));
            SubscriptionPool.Add(GetObservable<FontStyle>(nameof(FontStyle)).Subscribe((next) => ForzeDraw()));
            SubscriptionPool.Add(GetObservable<int>(nameof(FontWeight)).Subscribe((next) => ForzeDraw()));

        }

        public override void Check()
        {
            base.Check();
            //if()
        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            var minWidth = MinWidth;
            var minHeight = MinHeight;

            float width = 0;
            float height = 0;
            
            switch (mode)
            {
                case MeasureSpecMode.Exactly:
                    width = Math.Max(widthSpec, minWidth);
                    height = Math.Max(heightSpec, minWidth);
                    break;
                case MeasureSpecMode.AtMost:
                    using (var paint = GetTextPaint())
                    {
                        //calculate width                        
                            width = Math.Max(PaddingLeft + PaddingRight + paint.MeasureText(Text), minWidth);
                            if (width > widthSpec)
                                width = Math.Max(widthSpec, minWidth);                        

                        //calculate height                        
                            height = Math.Max(PaddingTop + PaddingBottom + paint.TextSize, minHeight);
                            if (height > heightSpec)
                                height = Math.Max(heightSpec, minHeight);                        
                    }
                    break;
                case MeasureSpecMode.Unspecified:
                    using (var paint = GetTextPaint())
                    {
                        //calculate width
                        width = Math.Max(PaddingLeft + PaddingRight + paint.MeasureText(Text), minWidth);

                        //calculate height
                        height = Math.Max(PaddingTop + PaddingBottom + paint.TextSize, minHeight);
                        break;
                    }
            }

            return (width, height);
        }


        protected override void DrawTexture(SKCanvas canvas, int width, int height)
        {
            base.DrawTexture(canvas, width, height);


            using (var textPaint = GetTextPaint())
            {

                float x = 0;
                float y = (CalculatedHeight / 2) + (textPaint.TextSize) / 2;

                switch (TextAlign)
                {
                    case TextAlign.Left:
                        x = PaddingLeft;
                        break;
                    case TextAlign.Center:
                        x = CalculatedWidth / 2;
                        break;
                    case TextAlign.Right:
                        x = CalculatedWidth - PaddingRight;
                        break;
                }


                canvas.DrawText(Text, x, y, textPaint);
            }

        }

        private SKPaint GetTextPaint()
        {
            var textPaint = new SKPaint();
            var textAlign = TextAlign;

            textPaint.Color = TextColor.ToSkiaColor();
            textPaint.TextSize = TextSize;
            textPaint.IsAntialias = true;
            textPaint.IsStroke = false;
            textPaint.TextAlign = textAlign.ToSkiaTextAlign();
            textPaint.TextScaleX = TextScale;
            textPaint.IsVerticalText = IsTextVertical;
            textPaint.TextSkewX = TextSkew;
            var typeface = Font.GetTypeface(FontStyle, FontWeight);
                        
            return textPaint;
        }

    }
}
