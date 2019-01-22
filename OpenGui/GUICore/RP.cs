using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;

namespace OpenGui.GUICore
{
    public class RP<V> : IObservable<V>
    {
        List<RPSubscription> subscriptions = new List<RPSubscription>();
        V _value;

        public V Value
        {
            get => _value;
            set => SetValue(value);
        }

        void SetValue(V value)
        {
            _value = value;
            for(int i = 0; i < subscriptions.Count; i++)
            {
                var subs = subscriptions[i];
                if (subs == null)
                    continue;
                subs.Observer.OnNext(value);
            }

        }

        public IDisposable Subscribe(IObserver<V> observer)
        {
            var subs = new RPSubscription(subscriptions.Count, observer);
            subscriptions.Add(subs);

            subs.Unsubscribe += Subs_Unsubscribe;
            return subs;
        }

        private void Subs_Unsubscribe(object sender, EventArgs e)
        {
            var subs = (RPSubscription)sender;
            subscriptions.RemoveAt(subs.ID);
        }

        private class RPSubscription : IDisposable
        {
            public int ID { get; private set; }
            public IObserver<V> Observer { get; private set; }

            public RPSubscription(int id, IObserver<V> observer)
            {
                ID = id;
                Observer = observer;
            }

            public event EventHandler Unsubscribe;

            public void Dispose()
            {
                Unsubscribe?.Invoke(this, null);
            }
        }

    }
}
