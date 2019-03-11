using OpenGui.Controls;
using OpenGui.GUICore;
using OpenGui.Styles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OpenGui.Core
{
    public class ChildrenList : IList<View>
    {
        Window _window = null;
        List<View> _views;
        ViewContainer _container;
        int _maxItems;

        public View this[int index] { get => _views[index]; set => _views[index]=value; }

        public int Count => _views.Count;

        public bool IsReadOnly => false;
        
        public ChildrenList(ViewContainer container, int maxItems = -1)
        {
            _container = container ?? throw new NullReferenceException(nameof(container));
            _views = new List<View>();
            _maxItems = maxItems;
            _container.GetObservable<object>(nameof(View.BindingContext)).Subscribe(BindingContextChanged);           
        }

        public void BindingContextChanged(object bindingContext)
        { 
            CheckThread();
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].SetValue<object>(nameof(View.BindingContext), ReactiveObject.LAYOUT_VALUE, bindingContext);
            }
        }

        public void AttachWindow(Window window)
        {
            CheckThread();
            _window = window;
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].AttachWindow(window);
            }
        }

        public void Add(View item)
        {
            CheckThread();
            if ( _maxItems > -1 && _views.Count >= _maxItems)
                throw new InvalidOperationException($"This container only admits {_maxItems} number of children.");

            item.Parent = _container;
            if(_container.Exist(nameof(_container.BindingContext)))            
                item.SetValue<object>(nameof(_container.BindingContext), ReactiveObject.LAYOUT_VALUE, _container.BindingContext);
             
            _views.Add(item);
            if (_window != null)
                item.AttachWindow(_window);
        }

        public void Clear()
        {
            CheckThread();
            _views.Clear();            
        }

        public bool Contains(View item)
        {
            return _views.Contains(item);
        }

        public void CopyTo(View[] array, int arrayIndex)
        {
            _views.CopyTo(array, arrayIndex);
        }

        public IEnumerator<View> GetEnumerator()
        {
           return _views.GetEnumerator();
        }

        public int IndexOf(View item)
        {
            return _views.IndexOf(item);
        }

        public void Insert(int index, View item)
        {
            CheckThread();
            _views.Insert(index, item);
        }

        public bool Remove(View item)
        {
            CheckThread();
            return _views.Remove(item);
        }

        public void RemoveAt(int index)
        {
            CheckThread();
            _views.RemoveAt(index);            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _views.GetEnumerator();
        }
        
        private void CheckThread()
        {
            _container.CheckThread();
        }

    }
}
