using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Values
{
    public class EventEmitter<T> : IObservable<T>
    {
        IObserver<T> observer;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            this.observer = observer;
            return new Subscription(this);
        }

        public void Emit(T value)
        {
            observer?.OnNext(value);
        }

        private class Subscription : IDisposable
        {
            EventEmitter<T> emitter;

            public Subscription(EventEmitter<T> emitter)
            {
                this.emitter = emitter;
            }
            public void Dispose()
            {
                this.emitter.observer = null;
            }
        }
    }



}
