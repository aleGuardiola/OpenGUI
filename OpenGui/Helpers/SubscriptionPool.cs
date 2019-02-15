using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Helpers
{
    public class SubscriptionPool
    {
        Stack<IDisposable> Subscriptions = new Stack<IDisposable>();

        public void Add(IDisposable subscription)
        {
            Subscriptions.Push(subscription);
        }

        public void UnsubscribeAll()
        {
            while (Subscriptions.Count > 0)
                Subscriptions.Pop().Dispose();
        }

    }
}
