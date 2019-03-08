using System;
using System.Collections.Generic;
using System.Text;
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
        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            var label = Children[0] as Label;
            label.Mesure(widthSpec, heightSpec, MeasureSpecMode.AtMost);
            return base.OnMesure(widthSpec, heightSpec, mode);
        }

        protected override void OnLayout()
        {
            var label = Children[0] as Label;
            label.X = (X / 2) + (label.CalculatedWidth / 2);
            label.Y = (Y / 2) + (label.CalculatedHeight / 2);
        }
    }
}
