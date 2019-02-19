using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Animations.Interplators;
using OpenGui.Core;

namespace OpenGui.Animations.Xaml
{
    public class FloatPropertyAnimation : Animation
    {
        public Interpolator Interpolator
        {
            get => GetValue<Interpolator>();
            set => SetValue<Interpolator>(value);
        }

        public float StartValue
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        public float EndValue
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        public string Property
        {
            get => GetValue<string>();
            set => SetValue<string>(value);
        }

        public override Animations.Animation GetAnimation(ReactiveObject objectToAnimate)
        {
            return new OpenGui.Animations.FloatPropertyAnimation(objectToAnimate, Property, Duration, StartValue, EndValue, Interpolator);
        }
    }
}
