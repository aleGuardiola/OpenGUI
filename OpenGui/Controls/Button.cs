using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Core;
using OpenGui.Graphics;
using OpenGui.Values;

namespace OpenGui.Controls
{
    public class Button : ViewContainer<View>
    {
        Label _label;

        public string Text
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
        
        public Button() : base(1)
        {
            _label = new Label();
            this.Children.Add(_label);
            SubscriptionPool.Add(GetObservable<string>(nameof(Text)).Subscribe((v) => _label.Text = v));
            SetValue<float>(nameof(PaddingBottom), ReactiveObject.LAYOUT_VALUE, 10);
            SetValue<float>(nameof(PaddingTop), ReactiveObject.LAYOUT_VALUE, 10);
            SetValue<float>(nameof(PaddingRight), ReactiveObject.LAYOUT_VALUE, 10);
            SetValue<float>(nameof(PaddingLeft), ReactiveObject.LAYOUT_VALUE, 10);
            Background = new DrawableColor(System.Drawing.Color.Brown);
        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            _label.Mesure(widthSpec, heightSpec, MeasureSpecMode.AtMost);

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
                    //calculate width                    
                    width = Math.Max(PaddingLeft + PaddingRight + _label.CalculatedWidth, minWidth);
                    if (width > widthSpec)
                        width = Math.Max(widthSpec, minWidth);

                    //calculate height
                    height = Math.Max(PaddingTop + PaddingBottom + _label.CalculatedHeight, minHeight);
                    if (height > heightSpec)
                        height = Math.Max(heightSpec, minHeight);
                    break;
                case MeasureSpecMode.Unspecified:

                    //calculate width
                    width = Math.Max(PaddingLeft + PaddingRight + _label.CalculatedWidth, minWidth);

                    //calculate height
                    height = Math.Max(PaddingTop + PaddingBottom + _label.CalculatedHeight, minHeight);
                    break;
            }

            OnLayout();

            return (width, height);
        }

        protected override void OnLayout()
        {
            var label = Children[0] as Label;
            label.X = X + (CalculatedWidth / 2) - (_label.CalculatedWidth / 2);
            label.Y = Y + (CalculatedHeight / 2) - (_label.CalculatedHeight / 2);
        }
    }
}
