using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Core
{
    public abstract class Binder : IDisposable
    {
        protected object Target;
        protected string TargetProperty;

        public Binder(object target, string targetProperty)
        {
            Target = target;            
        }

        public abstract void Dispose();        
    }
}
