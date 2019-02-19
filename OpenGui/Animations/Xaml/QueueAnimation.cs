using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGui.Core;
using Portable.Xaml.Markup;

namespace OpenGui.Animations.Xaml
{
    [ContentPropertyAttribute(nameof(QueueAnimation.Animations))]
    public class QueueAnimation : Animation
    {
        public IList<Animation> Animations
        {
            get;
            private set;
        }

        public QueueAnimation()
        {
            Animations = new List<Animation>();
        }

        public override Animations.Animation GetAnimation(ReactiveObject objectToAnimate)
        {
            return new OpenGui.Animations.QueueAnimation(objectToAnimate, Animations.Select(a => a.GetAnimation(objectToAnimate)));
        }
    }
}
