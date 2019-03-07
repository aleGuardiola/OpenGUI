using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenGui.Core
{
    public class TwoWayBinder<T> : Binder
    {
        object source;

        IDisposable targetSubscription;
        IDisposable sourceSubscription;
        PropertyInfo targetPropertyInfo;
        PropertyInfo sourcePropertyInfo;

        public TwoWayBinder(
            object target, 
            string targetProperty, 
            IObservable<T> targetObservable,
            object source,
            string sourceProperty,
            IObservable<T> sourceObservable
            ) : base(target, targetProperty)
        {
            this.source = source;

            targetPropertyInfo = target.GetType().GetProperty(targetProperty);
            sourcePropertyInfo = target.GetType().GetProperty(targetProperty);

            sourceSubscription = sourceObservable.Subscribe(nextSourceValue);
            targetSubscription = targetObservable.Subscribe(nextTargetValue);
        }

        bool targetChanged = false;
        private void nextSourceValue(T next)
        {
            targetPropertyInfo.SetValue(Target, next);
        }

        private void nextTargetValue(T next)
        {
            if(targetChanged)
            {
                targetChanged = false;
                return;
            }

            targetChanged = true;
            sourcePropertyInfo.SetValue(source, next);
        }

        public override void Dispose()
        {
            targetSubscription.Dispose();
            sourceSubscription.Dispose();
        }
    }
}
