using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Helpers
{
    public class GenericEventArgs<T> : EventArgs
    {
        public T Value { get; }
        public GenericEventArgs(T value)
        {
            Value = value;
        }
    }
}
