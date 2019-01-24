using OpenGui.Collection;
using OpenGui.Layout;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Controls
{
    public class ViewContainer : View
    {
        IList<View> _children;
        public IList<View> Children
        {
            get => _children;
        }

        public ViewContainer()
        {
            _children = new List<View>();       
        }
    }

    public abstract class ViewContainer<T> : ViewContainer where T : View
    {
        ILayoutManager<T> _layoutManager;

        /// <summary>
        /// Get the children of this container
        /// </summary>
        new public IList<T> Children
        {
            get => (IList<T>)base.Children;
        }
               
        public ViewContainer()
        {
            _layoutManager = GetLayoutManager();
        }
               
        /// <summary>
        /// Provide the layout manager that position the children.
        /// </summary>
        /// <returns>The layout manager.</returns>
        protected abstract ILayoutManager<T> GetLayoutManager();

    }
}
