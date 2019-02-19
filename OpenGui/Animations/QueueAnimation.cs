using OpenGui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGui.Animations
{
    public class QueueAnimation : Animation
    {
        IEnumerable<Animation> _animations;
        IEnumerator<Animation> _enumarator;
        bool _lastOne;

        public QueueAnimation( ReactiveObject obj, IEnumerable<Animation> animations ) : base(obj, animations.Sum((a)=>a.Duration))
        {
            _animations = animations;
        }

        protected override bool Stoped()
        {
            var value = base.Stoped();
            return base.Stoped() || (_lastOne && (_enumarator == null || _enumarator.Current == null || _enumarator.Current.IsStop()) );
        }

        protected override float ChangeFunction(int time)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateValue(float value)
        {
            throw new NotImplementedException();
        }

        public override void Initialize()
        {
            base.Initialize();
            _enumarator = _animations.GetEnumerator();
            Current_Stop(null, null);
        }

        public override void Update(float deltaTime)
        {
            if (_enumarator.Current != null && !_enumarator.Current.IsStop())
                _enumarator.Current.Update(deltaTime);
        }
        
        private void Current_Stop(object sender, EventArgs e)
        {
            if (_enumarator.Current != null)
                _enumarator.Current.Stop -= Current_Stop;

           if( !(_lastOne = !_enumarator.MoveNext()) )
            {
                _enumarator.Current.Initialize();
                _enumarator.Current.Stop += Current_Stop;
            }
        }

    }
}
