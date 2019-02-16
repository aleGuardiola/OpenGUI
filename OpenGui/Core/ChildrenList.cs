using OpenGui.Controls;
using OpenGui.GUICore;
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

        public View this[int index] { get => _views[index]; set => _views[index]=value; }

        public int Count => _views.Count;

        public bool IsReadOnly => false;

        public void AttachWindow(Window window)
        {
            CheckThread();            
            for (int i = 0; i < _views.Count; i++)
            {
                _views[i].AttachWindow(window);
            }
        }

        public ChildrenList(ViewContainer container)
        {
            _container = container ?? throw new NullReferenceException(nameof(container));
            _views = new List<View>();
        }

        public void Add(View item)
        {
            CheckThread();
            item.Parent = _container;
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
