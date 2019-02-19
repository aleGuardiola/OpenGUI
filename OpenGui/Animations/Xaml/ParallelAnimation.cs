using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGui.Core;
using Portable.Xaml.Markup;

namespace OpenGui.Animations.Xaml
{
    [ContentPropertyAttribute(nameof(ParallelAnimation.Animations))]
    public class ParallelAnimation : Animation
    {        
        public IList<Animation> Animations
        {
            get;
            private set;
        }

        public ParallelAnimation()
        {
            Animations = new List<Animation>();
        }

        public override OpenGui.Animations.Animation GetAnimation(ReactiveObject objectToAnimate)
        {
            return new OpenGui.Animations.ParallelAnimation(objectToAnimate, Animations.Select(a=>a.GetAnimation(objectToAnimate)));
        }
    }
}
