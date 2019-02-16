using OpenGui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGui.Animations
{
    public class ParallelAnimation : Animation
    {
        IEnumerable<Animation> _animations;

        public ParallelAnimation(ReactiveObject obj, IEnumerable<Animation> animations) : base(obj, animations.Max((a)=>a.Duration))
        {
            _animations = animations;
        }

        protected override float ChangeFunction(int time)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateValue(float value)
        {
            throw new NotImplementedException();            
        }

        public override void Update(float deltaTime)
        {
            foreach(var anim in _animations)
            {
                if (!anim.Stoped())
                    anim.Update(deltaTime);
            }
        }
    }
}
