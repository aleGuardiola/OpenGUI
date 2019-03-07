using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Core
{
    public class SyncronicValueObservable<T> : IObservable<T>
    {
        T _value;

        public SyncronicValueObservable(T value)
        {
            _value = value; 
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            observer.OnNext(_value);
            observer.OnCompleted();
            return new EmptyDisposable();
        }

        private class EmptyDisposable : IDisposable
        {
            public void Dispose() { }
        }
    }
}
