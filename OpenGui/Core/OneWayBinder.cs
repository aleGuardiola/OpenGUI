using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenGui.Core
{
    public class OneWayBinder<T> : Binder
    {
        IDisposable subscripton;
        PropertyInfo propertyInfo;

        public OneWayBinder(object target, string targetProperty, IObservable<T> sourceObservable) : base(target, targetProperty)
        {
            propertyInfo = target.GetType().GetProperty(targetProperty);
            subscripton = sourceObservable.Subscribe(nextValue);            
        }

        private void nextValue(T next)
        {
            propertyInfo.SetValue(Target, next);
        }

        public override void Dispose()
        {
            subscripton.Dispose();
        }
    }
}
