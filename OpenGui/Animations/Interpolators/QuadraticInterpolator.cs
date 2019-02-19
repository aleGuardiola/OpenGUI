using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Interplators
{
    public class QuadraticInterpolator : Interpolator
    {
        public float Exponent
        {
            get;
            set;
        }

        public QuadraticInterpolator()
        {
            Exponent = 2f;
        }

        public override float GetValue(float timeFraction)
        {
            return (float)Math.Pow(timeFraction, Exponent);
        }
    }
}
