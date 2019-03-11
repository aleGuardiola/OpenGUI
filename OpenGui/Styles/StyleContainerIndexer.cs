using OpenGui.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    public abstract class StyleContainerIndexer<Key, S> where S : StyleContainer
    {
        Dictionary<Key, S> _indexes = new Dictionary<Key, S>();
          
        protected abstract Key GetKey(S style);

        protected abstract Key GetKey(View view);

    }
}
