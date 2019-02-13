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
            }
            return base.OnMesure(widthSpec, heightSpec, mode);
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
