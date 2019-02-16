using OpenGui.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations
{
    public class FloatPropertyAnimation : PropertyAnimation<float>
    {
        public static Func<float, float> Linear = (time) => time;

        Func<float, float> _changeFunc;
        float _start;
        float _end;

        public FloatPropertyAnimation(ReactiveObject obj, string property, int duration, float start, float end, Func<float, float>changeFunc)
            : base(obj, duration, property)
        {
            _changeFunc = changeFunc;
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
            return _changeFunc(timeScale);
        }
    }
}
