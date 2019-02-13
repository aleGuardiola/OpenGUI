using OpenGui.Collection;
using OpenGui.Core;
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
        /// <summary>
        /// Get the children of this container
        /// </summary>
        new public IList<T> Children
        {
            get => (IList<T>)base.Children;
        }
                
        public ViewContainer()
        {
            
        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            OnLayout();
            return base.OnMesure(widthSpec, heightSpec, mode);
        }

        protected abstract void OnLayout();
        
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
