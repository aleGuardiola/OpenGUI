using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Interplators
{
    public class BounceInterpolator : Interpolator
    {
        public override float GetValue(float timeFraction)
        {
            if(timeFraction < 0.31489)
            {
                return 8f * (1.1226f * timeFraction) * (1.1226f * timeFraction);
            }

            if (timeFraction < 0.65990)
            {
                return 8f * (1.1226f * timeFraction - 0.54719f) * (1.1226f * timeFraction - 0.54719f) + 0.7f;
            }

            if (timeFraction < 0.85908)
            {
                return 8f * (1.1226f * timeFraction - 0.8526f) * (1.1226f * timeFraction - 0.8526f) + 0.9f;
            }

            return 8f * (1.1226f * timeFraction - 1.0435f) * (1.1226f * timeFraction - 1.0435f) + 0.95f;
        }
    }
}
