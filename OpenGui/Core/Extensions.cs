using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Subjects;
using System.Text;

namespace OpenGui.Core
{
    public static class Extensions
    {
        public static IObservable<T> GetObservable<T>( this INotifyPropertyChanged obj, string property )
        {
            var subject = new Subject<T>();

            var prop = obj.GetType().GetProperty(property);
            obj.PropertyChanged += (o, e) =>
            {
                if(e.PropertyName == property)
                {
                    var value = prop.GetValue(obj);
                    subject.OnNext((T)value);
                }
            };

            return subject;
        }

    }
}
