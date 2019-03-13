using OpenGui.Styles.Core;
using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    [ContentPropertyAttribute(nameof(StyleDefinition.Containers))]
    public class StyleDefinition
    {
        public IList<SetterContainer> Containers { get; }

        public StyleDefinition()
        {
            Containers = new List<SetterContainer>();
        }
    }
}
