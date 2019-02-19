using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Interplators
{
    public class CycleInterpolator : Interpolator
    {

        public float Cycles { get; set; }

        public CycleInterpolator()
        {
            Cycles = 1f;
        }

        public override float GetValue(float timeFraction)
        {
            return (float)Math.Sin(2f * Math.PI * Cycles * timeFraction);
        }
    }
}
