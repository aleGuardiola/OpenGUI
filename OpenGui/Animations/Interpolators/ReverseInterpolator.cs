using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Interplators
{
    public class ReverseInterpolator : Interpolator
    {
        public override float GetValue(float timeFraction)
        {
            return 1f - timeFraction;
        }
    }
}
