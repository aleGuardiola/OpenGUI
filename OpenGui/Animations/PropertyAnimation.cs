using OpenGui.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations
{
    public abstract class PropertyAnimation<T> : Animation
    {
        string _property;

        public PropertyAnimation(ReactiveObject objectToAnimate, int duration, string property) : base(objectToAnimate, duration)
        {
            _property = property;
        }

        protected abstract float GetScaleValue(float timeScale);

        protected abstract T ConvertScaleValue(float scaleValue);
        
        protected override float ChangeFunction(int time)
        {
            var timeScale = (float)time / (float)Duration;
            var valueScale = GetScaleValue(timeScale);
            return valueScale;
        }
        
        protected override void UpdateValue(float value)
        {
            var valueConverted = ConvertScaleValue(value);
            ReactiveObject.SetValue<T>(_property, ReactiveObject.ANIMATION_VALUE, valueConverted);
        }
    }
}
