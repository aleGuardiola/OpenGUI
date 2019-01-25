using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using OpenGui.Core;
using OpenGui.Layout;
using OpenGui.Values;

namespace OpenGui.Controls
{
    /// <summary>
    /// A layout that put the children one after the other.
    /// </summary>
    public class StackLayout : ViewContainer<View>
    {
        /// <summary>
        /// Get or Set the orientation of the Children
        /// </summary>
        public Orientation Orientation
        {
            get => GetValue<Orientation>();
            set => SetValue<Orientation>(value);
        }

        public StackLayout()
        {
            SetValue<Orientation>(nameof(Orientation), ReactiveObject.LAYOUT_VALUE, Orientation.Vertical);
        }

        protected override ILayoutManager<View> GetLayoutManager()
        {
            return new StackLayoutManager();
        }

        private class StackLayoutManager : ILayoutManager<View>
        {

            public (float widthToRender, float heightToRender) Calculate(ViewContainer viewContainer, IList<View> children, float parentWidth, float parentHeight)
            {
                var width = viewContainer.Width == float.PositiveInfinity ? parentWidth : viewContainer.Width;
                var height = viewContainer.Height == float.PositiveInfinity ? parentHeight : viewContainer.Height;
                
                var orientation = ((StackLayout)viewContainer).Orientation;

                switch (orientation)
                {
                    case Orientation.Horizontal:
                        return CalcHorizontal(viewContainer, children, width, height);
                        
                    case Orientation.Vertical:
                        CalcVertical(viewContainer, children);
                        break;
                }

                return (0, 0);

            }

            //calculate for horizontal orientation
            private static (float widthToRender, float heightToRender) CalcHorizontal(ViewContainer viewContainer, IList<View> children, float width, float height)
            {
                //if there is not children return
                if (children.Count == 0)
                    return (width, height);

                //calculate first children
                var lastChild = children[0];
                var marginToSum = Math.Max(viewContainer.PaddingLeft, lastChild.MarginLeft);

                float maxHeight = 0;
                if (height == float.NegativeInfinity)
                {
                    maxHeight = Math.Max(viewContainer.PaddingTop, lastChild.MarginTop)
                              + Math.Max(viewContainer.PaddingBottom, lastChild.MarginBottom)
                              + lastChild.Height;
                }

                var lastX = 0f;

                lastChild.SetValue<float>(nameof(lastChild.RelativeX), ReactiveObject.LAYOUT_VALUE, lastX);

                for(int i = 1; i < children.Count; i++)
                {
                    var child = children[i];

                    var newX = lastX + lastChild.Width + Math.Max(lastChild.MarginRight, child.MarginLeft);

                    child.SetValue<float>(nameof(child.RelativeX), ReactiveObject.LAYOUT_VALUE, newX);
                                       

                    if (height == float.NegativeInfinity)
                    {
                        var newHeight = Math.Max(viewContainer.PaddingTop, child.MarginTop)
                                      + Math.Max(viewContainer.PaddingBottom, child.MarginBottom)
                                      + child.Height;

                        maxHeight = Math.Max(newHeight, maxHeight);
                    }

                    lastX = newX;
                    lastChild = child;
                }

                width = width == float.NegativeInfinity
                        ? lastX + lastChild.Width
                                + Math.Max(children[0].MarginLeft, viewContainer.PaddingLeft)
                                + Math.Max(lastChild.MarginRight, viewContainer.PaddingRight)
                        : width;

                height = height == float.NegativeInfinity ? maxHeight : height;

                float leftSpaceToAdd;

                switch(viewContainer.ContentAlign)
                {
                    case Align.Left:
                        leftSpaceToAdd = Math.Max(children[0].MarginLeft, viewContainer.PaddingLeft);
                        break;

                    case Align.Right:
                        leftSpaceToAdd = width - lastX - Math.Max(lastChild.MarginRight, viewContainer.PaddingRight);
                        break;

                    case Align.Center:
                        leftSpaceToAdd = (width / 2) - ( (lastX+lastChild.Width) / 2);
                        break;

                    default:
                        leftSpaceToAdd = Math.Max(children[0].MarginLeft, viewContainer.PaddingLeft);
                        break;
                }

                for(int i = 0; i < children.Count; i++)
                {
                    var child = children[i];
                    float topSpaceToAdd;

                    switch(child.Align)
                    {
                        case Align.Bottom:
                            topSpaceToAdd = height - child.Height - Math.Max(viewContainer.PaddingBottom, child.MarginBottom);
                            break;

                        case Align.Top:
                            topSpaceToAdd = Math.Max(viewContainer.PaddingTop, child.MarginTop);
                            break;

                        case Align.Center:
                            topSpaceToAdd = (height / 2) - (child.Height / 2);
                            break;

                        default:
                            topSpaceToAdd = Math.Max(viewContainer.PaddingTop, child.MarginTop);
                            break;
                    }

                    child.SetValue<float>(nameof(child.RelativeX), ReactiveObject.LAYOUT_VALUE, child.GetValue<float>(nameof(child.RelativeX), ReactiveObject.LAYOUT_VALUE) + leftSpaceToAdd);
                    child.SetValue<float>(nameof(child.RelativeY), ReactiveObject.LAYOUT_VALUE, topSpaceToAdd);

                    child.Initialize(width, height, viewContainer.X, viewContainer.Y);
                }

                return (width, height);

            }

            //calculate for vertical orientation
            private static void CalcVertical(ViewContainer viewContainer, IList<View> children)
            {
                //if there is not children return
                if (children.Count == 0)
                    return;



            }

        }

    }
}
