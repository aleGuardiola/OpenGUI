using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    [ContentPropertyAttribute(nameof(StyleDefinition.Containers))]
    public class StyleDefinition
    {
        public StyleDefinitionPriority Priority
        {
            get;
            set;
        }

        public bool IsInheritable
        {
            get;
            set;
        }

        public IList<StyleContainer> Containers
        {
            get;           
        }

        public StyleDefinition()
        {
            Containers = new List<StyleContainer>();
            IsInheritable = false;
            Priority = StyleDefinitionPriority.Low;
        }
    }

    public enum StyleDefinitionPriority
    {
        High = 0,
        Medium,
        Low
    }

}
