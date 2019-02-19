using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Interplators
{
    public class LinearInterplator : Interpolator
    {
        public override float GetValue(float timeFraction)
        {
            return timeFraction;
        }
    }
}
