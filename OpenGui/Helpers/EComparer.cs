using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Helpers
{
    public class EComparer<T> : EqualityComparer<T>
    {
        Func<T, T, bool> _equals;
        Func<T, int> _hashcode;
        
        public EComparer(Func<T, T, bool> equals)
        {
            _equals = equals ?? throw new ArgumentNullException(nameof(equals));            
        }

        public EComparer(Func<T, T, bool> equals, Func<T, int> hashcode)
        {
            _equals = equals ?? throw new ArgumentNullException(nameof(equals));
            _hashcode = hashcode ?? throw new ArgumentNullException(nameof(hashcode));
        }

        public override bool Equals(T x, T y)
        {
            return _equals(x, y);
        }

        public override int GetHashCode(T obj)
        {
            if (_hashcode == null)
                return obj.GetHashCode();

            return _hashcode(obj);
        }
    }
}
