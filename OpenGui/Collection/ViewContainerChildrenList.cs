using OpenGui.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Collection
{
    internal class ViewContainerChildrenList<T> : IList<T> where T : View
    {
        IList<LowLevelView> children;
        public ViewContainerChildrenList(IList<LowLevelView> lowLevelViewList)
        {
            children = lowLevelViewList;
        }

        public T this[int index] { get => (T)children[index]; set => children[index] = value; }

        public int Count => children.Count;

        public bool IsReadOnly => children.IsReadOnly;

        public void Add(T item)
        {
            children.Add(item);
        }

        public void Clear()
        {
            children.Clear();
        }

        public bool Contains(T item)
        {
            return children.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            children.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)children.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return children.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            children.Insert(index, item);
        }

        public bool Remove(T item)
        {
            return children.Remove(item);
        }

        public void RemoveAt(int index)
        {
            children.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }
    }
}
