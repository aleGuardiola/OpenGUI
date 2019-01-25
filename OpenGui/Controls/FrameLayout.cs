using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Layout;

namespace OpenGui.Controls
{
    /// <summary>
    ///Frame Layout is a layout where children can be posisioned freely.
    /// </summary>
    public class FrameLayout : ViewContainer<View>
    {
        protected override ILayoutManager<View> GetLayoutManager()
        {
            return new FrameLayoutManager();
        }

        private class FrameLayoutManager : ILayoutManager<View>
        {
            public (float widthToRender, float heightToRender) Calculate(ViewContainer viewContainer, IList<View> children, float parentWidth, float parentHeight)
            {
                for(int i = 0; i < children.Count; i++)
                {
                    var child = children[i];
                    
                    //initialize each children with the limits of the view
                    child.Initialize(
                        viewContainer.Width - (viewContainer.PaddingLeft + viewContainer.PaddingRight),
                        viewContainer.Height - (viewContainer.PaddingTop + viewContainer.PaddingBottom),
                        viewContainer.X + viewContainer.PaddingLeft, 
                        viewContainer.Y + viewContainer.PaddingTop);
                }

                return (100, 100);
            }
        }

    }

    

}
