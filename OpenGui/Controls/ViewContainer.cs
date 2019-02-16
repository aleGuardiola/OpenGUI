using OpenGui.Collection;
using OpenGui.Core;
using OpenGui.GUICore;
using OpenGui.Values;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGui.Controls
{
    public abstract class ViewContainer : View
    {
        public bool IsForceMeasure
        {
            get;
            private set;
        }

        IList<View> _children;
        public IList<View> Children
        {
            get => _children;
        }

        public ViewContainer()
        {
            _children = new ChildrenList(this);
            IsForceMeasure = true;
        }

        public override void Check()
        {
            for (int i = 0; i < _children.Count; i++)
                _children[i].Check();

            if (IsForceMeasure)
            {
                var width = CalculatedWidth;
                var height = CalculatedHeight;

                Mesure(width, height, MeasureSpecMode.Unspecified);
                if (CalculatedWidth > width || CalculatedHeight > height)
                    Parent.ForceMeasure();
                else
                    Mesure(width, height, MeasureSpecMode.Exactly);
            }
            IsForceMeasure = false;
        }
        
        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            OnLayout();
            return base.OnMesure(widthSpec, heightSpec, mode);
        }

        public override void AttachWindow(Window window)
        {
            base.AttachWindow(window);
            ((ChildrenList)Children).AttachWindow(window);
        }

        protected abstract void OnLayout();
        
        public void ForceMeasure()
        {
            CheckThread();
            IsForceMeasure = true;
        }

        public override void GLDraw(Matrix4 perspectiveProjection, Matrix4 view, RectangleF clipRectangle, int windowWidth, int windowHeight, float cameraZ)
        {
            //draw itself
            base.GLDraw(perspectiveProjection, view, clipRectangle, windowWidth, windowHeight, cameraZ);

            //draw each children ordered by z
            foreach (var child in Children.OrderBy((c) => c.Z))
            {
                child.GLDraw(perspectiveProjection, view, clipRectangle, windowWidth, windowHeight, cameraZ);
            }
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

    }
}
