using OpenGui.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Animations.Xaml
{
    public abstract class Animation : ReactiveObject
    {
        public int Duration
        {
            get => GetValue<int>();
            set => SetValue<int>(value);
        }

        public Animation()
        {
            SetValue<int>(nameof(Duration), ReactiveObject.LAYOUT_VALUE, 1000);
        }

        public abstract OpenGui.Animations.Animation GetAnimation(ReactiveObject objectToAnimate);
    }
}
