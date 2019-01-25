using OpenGui.Collection;
using OpenGui.Core;
using OpenGui.Layout;
using OpenGui.Values;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Controls
{
    public abstract class ViewContainer : View
    {
        IList<View> _children;
        public IList<View> Children
        {
            get => _children;
        }

        public Align ContentAlign
        {
            get => GetValue<Align>();
            set => SetValue<Align>(value);
        }

        public ViewContainer()
        {
            _children = new List<View>();
            SetValue<Align>(nameof(ContentAlign), ReactiveObject.LAYOUT_VALUE, Align.Center);
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

        public override void Initialize(float maxWidth, float maxHeight, float parentX, float parentY)
        {
            //calculate the layout
            var value = _layoutManager.Calculate(this, base.Children, maxWidth, maxHeight);
            CalculatedWidth = value.widthToRender;
            CalculatedHeight = value.heightToRender;
            base.Initialize(maxWidth, maxHeight, parentX, parentY);
        }

        public override void GLDraw(Matrix4 perspectiveProjection, Matrix4 view, RectangleF clipRectangle, int windowWidth, int windowHeight)
        {
            //draw each children
            for (int i = 0; i < base.Children.Count; i++)
            {
                Children[i].GLDraw(perspectiveProjection, view, clipRectangle, windowWidth, windowHeight);
            }

            //draw itself
            base.GLDraw(perspectiveProjection, view, clipRectangle, windowWidth, windowHeight);
        }

    }
}
