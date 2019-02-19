using OpenGui.Animations.Interplators;
using OpenGui.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations
{
    public class FloatPropertyAnimation : PropertyAnimation<float>
    {        
        Interpolator _interpolator;
        float _start;
        float _end;

        public FloatPropertyAnimation(ReactiveObject obj, string property, int duration, float start, float end, Interpolator interpolator)
            : base(obj, duration, property)
        {
            _interpolator = interpolator;
            _start = start;
            _end = end;
        }

        protected override float ConvertScaleValue(float scaleValue)
        {
            var diff = _end - _start;
            return _start + (diff * scaleValue);
        }

        protected override float GetScaleValue(float timeScale)
        {
            return _interpolator.GetValue(timeScale);
        }
    }
}
