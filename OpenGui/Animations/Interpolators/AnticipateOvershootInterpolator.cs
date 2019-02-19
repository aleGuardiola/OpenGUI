using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Interplators
{
    public class AnticipateOvershootInterpolator : Interpolator
    {
        public float Tension { get; set; }

        public AnticipateOvershootInterpolator()
        {
            Tension = 1f;
        }

        public override float GetValue(float timeFraction)
        {
            if (timeFraction < 0.5f)
                return 0.5f * ((Tension + 1) * (float)Math.Pow(2 * timeFraction, 3) - Tension * (float)Math.Pow(2 * timeFraction, 2));

            return 0.5f * ((Tension + 1) * (float)Math.Pow(2 * timeFraction - 2, 3) + Tension * (float)Math.Pow(2 * timeFraction - 2, 2)) + 1;
        }
    }
}
