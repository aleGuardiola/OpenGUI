using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    [ContentPropertyAttribute(nameof(StyleContainer.Setters))]
    public class ViewStyles
    {
        public IList<Set> Setters
        {
            get;
        }

        public ViewStyles(int priority)
        {
            Setters = new List<Set>();            
        }
    }
}
