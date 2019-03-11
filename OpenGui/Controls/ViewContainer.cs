using OpenGui.Collection;
using OpenGui.Core;
using OpenGui.GUICore;
using OpenGui.Styles;
using OpenGui.Values;
using OpenTK;
using OpenTK.Input;
using Portable.Xaml.Markup;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGui.Controls
{
    [ContentPropertyAttribute(nameof(ViewContainer.Children))]
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

        public ViewContainer(int maxItems)
        {
            _children = new ChildrenList(this, maxItems);
            IsForceMeasure = true;
            GetObservable<float>(nameof(X)).Subscribe((val) => IsForceMeasure = true );
            GetObservable<float>(nameof(Y)).Subscribe((val) => IsForceMeasure = true );
        }

        public IEnumerable<Set> GetInheritedStyles()
        {
            if (Setters == null)
                return new Set[0];

            return Setters.Where(s => s.Inheritable);
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

        protected override void ProcessInput(KeyboardState keyboardState, MouseState mouseState, GamePadState gamePadState, JoystickState joystickState)
        {
            for (int i = 0; i < Children.Count; i++)
                Children[i].UpdateFrame(keyboardState, mouseState, gamePadState, joystickState);
        }

        public override bool TryGetViewById(string id, out View view)
        {
            if(!base.TryGetViewById(id, out view))
            {
                for( int i = 0; i < Children.Count; i++ )
                {
                    var child = Children[i];
                    if (child.TryGetViewById(id, out view))
                        return true;
                }
                                
                return false;
            }

            return true;
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

        public override void GLDraw(Matrix4 perspectiveProjection, Matrix4 view, RectangleF clipRectangle, int windowWidth, int windowHeight, float cameraZ, SKBitmap cachedBitmap = null)
        {
            //draw itself
            base.GLDraw(perspectiveProjection, view, clipRectangle, windowWidth, windowHeight, cameraZ, cachedBitmap);

            //draw each children ordered by z
            foreach (var child in Children.OrderBy((c) => c.Z))
            {
                child.GLDraw(perspectiveProjection, view, clipRectangle, windowWidth, windowHeight, cameraZ, cachedBitmap);
            }
        }

    }

    [ContentPropertyAttribute(nameof(ViewContainer.Children))]
    public abstract class ViewContainer<T> : ViewContainer where T : View
    {        
        /// <summary>
        /// Get the children of this container
        /// </summary>
        new public IList<T> Children
        {
            get => (IList<T>)base.Children;
        }

        public ViewContainer(int maxItems) : base(maxItems)
        {
           
        }

        public ViewContainer()
        {
            
        }
    }
}
