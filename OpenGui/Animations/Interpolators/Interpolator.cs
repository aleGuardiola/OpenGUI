using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Interplators
{
    public abstract class Interpolator
    {
        public abstract float GetValue(float timeFraction);
    }
}
