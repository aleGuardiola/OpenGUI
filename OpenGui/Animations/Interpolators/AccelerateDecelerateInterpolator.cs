using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Interplators
{
    public class AccelerateDecelerateInterpolator : Interpolator
    {
        //cos((t+1)π)/2+0.5
        public override float GetValue(float timeFraction)
        {
            return (float)Math.Cos((timeFraction + 1f) * Math.PI) / 2f + 0.5f;
        }
    }
}
