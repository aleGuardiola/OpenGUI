using OpenGui.Exceptions;
using OpenGui.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace OpenGui.Core
{
    public class ReactiveObject
    {
        public const int SYSTEM_VALUE = 0;
        public const int LAYOUT_VALUE = 1;
        public const int USER_VALUE = 2;
        public const int ANIMATION_VALUE = 3;

        private SubscriptionPool _subscriptionPool = new SubscriptionPool();

        Dictionary<string, Property> _properties;
        private Thread _threadWhereCreated;

        public ReactiveObject()
        {
            _properties = new Dictionary<string, Property>();
            _threadWhereCreated = Thread.CurrentThread;
        }

        public void CheckThread()
        {
            var runningThread = Thread.CurrentThread;
            if (runningThread != _threadWhereCreated)
                throw new InvalidOperationException("Trying to change property in a different thread.");            
        }

        public void Bind<T>(string propertyName, IObservable<T> observable)
        {
            _subscriptionPool.Add(observable.Subscribe((next) =>
            {
                SetValue<T>(next, propertyName);
            }));
        }

        public void BindTwoWay<T>( string propertyName, IObservable<T> observable, object obj, string property )
        {
            {
                var prop = obj.GetType().GetProperty(property);

                bool changed  = false;
                _subscriptionPool.Add(observable.Subscribe((next) => {
                    if (!changed)
                    {
                        SetValue<T>(next, propertyName);
                        changed = true;
                    }
                    else
                        changed = false;
                }));

                _subscriptionPool.Add(GetObservable<T>(propertyName).Subscribe((next) =>
                {
                    if (!changed)
                    {
                        prop.SetValue(obj, next);
                        changed = true;
                    }
                    else
                        changed = false; 
                }));
            }           
        }

        /// <summary>
        /// Try to delete a value by its priority.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="priority">The priority value to delete</param>
        /// <returns>Return true if the property exist</returns>
        public bool TryDeleteValueByPriority(string propertyName, int priority)
        {
            Property prop;
            if (_properties.TryGetValue(propertyName, out prop))
            {
                prop.values[priority] = null;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Verify if the specific property exist.
        /// </summary>
        /// <param name="propertyName">The name of the property</param>
        /// <returns>true if the property exist or false if not.</returns>
        public bool Exist(string propertyName)
        {
            return _properties.ContainsKey(propertyName);
        }

        /// <summary>
        /// Set the value of the property with a priority.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="priority">The priority of the value.</param>
        public void SetValue<T>(string propertyName, int priority, T value)
        {
            CheckThread();
            var type = typeof(T);
            var property = GetOrCreateProperty(propertyName, type);

            property.values[priority] = value;
            property.NotifyChange(priority);
        }
        
        /// <summary>
        /// Set user value of the property.
        /// </summary>
        /// <param name="memberName">property name</param>
        protected void SetValue<T>(T value, [CallerMemberName] string memberName = "")
        {
            CheckThread();
            var type = typeof(T);
            var property = GetOrCreateProperty(memberName, type);

            property.values[USER_VALUE] = value;
            property.NotifyChange(USER_VALUE);
        }

        /// <summary>
        /// Get value of the property.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="memberName">Property name</param>
        /// <returns>The value of the property.</returns>
        public T GetValue<T>([CallerMemberName] string memberName = "")
        {
            Property property;
            var exist = _properties.TryGetValue(memberName, out property);

            if (!exist)
                throw new MemberNotFoundException(memberName);

            return (T)property.GetPriorityValue();
        }

        /// <summary>
        /// Get value of the property with specific priority.
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="priority">The priority of the property.</param>
        /// <returns>Return the value of the property</returns>
        public T GetValue<T>(string propertyName, int priority)
        {
            Property property;
            var exist = _properties.TryGetValue(propertyName, out property);

            if (!exist)
                throw new MemberNotFoundException(propertyName);

            return (T)property.values[priority];
        }

        /// <summary>
        /// Try to get the value of a property.
        /// </summary>
        /// <typeparam name="T">The property type.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <param name="priority">The priority of the value.</param>
        /// <param name="value">The value to try to get.</param>
        /// <returns>return true if the property exist or false if not.</returns>
        public bool TryGetValue<T>(string propertyName, int priority, out T value)
        {
            Property property;
            var exist = _properties.TryGetValue(propertyName, out property);

            value = default(T);

            if (!exist || property.values[priority] == null)
                return false;

            value = (T)property.values[priority];

            return true;
        }

        /// <summary>
        /// Get observable of the values of specific property.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="propertyName">Property Name.</param>
        /// <returns>The observable of the property.</returns>
        public IObservable<T> GetObservable<T>(string propertyName)
        {
            var property = GetOrCreateProperty(propertyName, typeof(T));

            return property.GetObservable<T>();
        }

        private Property GetOrCreateProperty(string propertyName, Type type)
        {
            Property result;
            var exist = _properties.TryGetValue(propertyName, out result);

            if(!exist)
            {
                result = new Property(propertyName, type);
                _properties.Add(propertyName, result);
            }

            return result;
        }

        private class Property
        {
            public string Name;
            public Type Type;

            public Property(string name, Type type)
            {
                Name = name;
                Type = type;
            }

            public event EventHandler Change;

            public void NotifyChange(int priority)
            {
                Change?.Invoke(this, null);
            }

            public object[] values = new object[4];

            public object GetPriorityValue()
            {
                for(int i = 3; i >= 0; i--)
                {
                    var val = values[i];
                    if (val != null)
                        return val;
                }

                return null;
            }

            public IObservable<T> GetObservable<T>()
            {
                if (typeof(T) != Type)
                    throw new Exception("Different Type.");

                return new Observable<T>(this);
            }

            private class Observable<T> : IObservable<T>
            {
                Property _property;
                ConcurrentDictionary<Guid, Subscription> _subscriptions;

                public Observable(Property property)
                {
                    _subscriptions = new ConcurrentDictionary<Guid, Subscription>();
                    _property = property;
                    property.Change += Property_Change;
                }

                private void Property_Change(object sender, EventArgs e)
                {
                    var value = (T)_property.GetPriorityValue();
                    foreach(var subscription in _subscriptions)
                    {
                        subscription.Value.Next(value);
                    }
                }

                public IDisposable Subscribe(IObserver<T> observer)
                {
                    var id = Guid.NewGuid();
                    var subscription = new Subscription(id, this, observer);

                    _subscriptions[id] = subscription;

                    return subscription;
                }

                ~Observable()
                {
                    _property.Change -= Property_Change;
                }

                private class Subscription : IDisposable
                {
                    Guid _id;
                    IObserver<T> _observer;
                    Observable<T> _observable;

                    public Subscription(Guid id, Observable<T> observable, IObserver<T> observer)
                    {
                        _id = id;
                        _observable = observable;
                        _observer = observer;
                    }

                    public void Next(T value)
                    {
                        _observer.OnNext(value);
                    }

                    public void Dispose()
                    {
                        Subscription sub;
                        _observable._subscriptions.TryRemove(_id, out sub);
                    }
                }


            }

        }

        ~ReactiveObject()
        {
            _subscriptionPool.UnsubscribeAll();
        }

    }
}
