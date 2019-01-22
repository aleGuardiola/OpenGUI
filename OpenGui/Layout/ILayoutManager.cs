using OpenGui.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Layout
{
    public interface ILayoutManager<T> where T : View
    {
        void Calculate(ViewContainer viewContainer);
    }
}
