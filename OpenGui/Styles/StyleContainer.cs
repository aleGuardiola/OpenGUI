using OpenGui.Controls;
using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    [ContentPropertyAttribute(nameof(StyleContainer.Setters))]
    public abstract class StyleContainer : StyleContainerBase
    {
        public IList<Set> Setters
        {
            get;
        }

        public int Priority
        {
            get;
        }

        public StyleContainer(int priority)
        {
            Setters = new List<Set>();
            Priority = priority;
        } 
    }
}
