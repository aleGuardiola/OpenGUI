using OpenGui.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Layout
{
    public interface ILayoutManager<T> where T : View
    {
        (float widthToRender, float heightToRender) Calculate(ViewContainer viewContainer, IList<View> children, float parentWidth, float parentHeight);
    }
}
