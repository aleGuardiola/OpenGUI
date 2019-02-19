using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Interplators
{
    public class AccelerateInterpolator : Interpolator
    {
        public float Factor{ get; set; }

        public AccelerateInterpolator()
        {
            Factor = 1f;
        }

        public override float GetValue(float timeFraction)
        {
            return (float)Math.Pow(timeFraction, 2f * Factor);
        }
    }
}
