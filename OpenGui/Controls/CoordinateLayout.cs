using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Values;

namespace OpenGui.Controls
{
    public class CoordinateLayout : ViewContainer<View>
    {
        public CoordinateLayout() : base()
        {

        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                child.Mesure(widthSpec, heightSpec, MeasureSpecMode.AtMost);

                if(child.HorizontalAligment == HorizontalAligment.Stretch)
                    child.Mesure(widthSpec, child.CalculatedHeight, MeasureSpecMode.Exactly);
                else if(child.Width != (int)WidthOptions.Auto)
                    child.Mesure(child.Width, child.CalculatedHeight, MeasureSpecMode.Exactly);

                if(child.VerticalAligment == VerticalAligment.Stretch )
                    child.Mesure(child.CalculatedWidth, heightSpec, MeasureSpecMode.Exactly);
                else if(child.Height != (int)HeightOptions.Auto)
                    child.Mesure(child.CalculatedWidth, child.Height, MeasureSpecMode.Exactly);

            }
            return base.OnMesure(widthSpec, heightSpec, MeasureSpecMode.Exactly);//(widthSpec, heightSpec);
        }

        protected override void OnLayout()
        {
            for(int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];
                child.X = X + child.RelativeX;
                child.Y = Y + child.RelativeY;                
            }
        }
    }
}
