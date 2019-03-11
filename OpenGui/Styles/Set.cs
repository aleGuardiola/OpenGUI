using Portable.Xaml.Markup;

namespace OpenGui.Styles
{
    [ContentPropertyAttribute(nameof(Set.Value))]
    public class Set
    {
        public bool Inheritable
        {
            get;
            set;
        }

        public string Property
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public Set()
        {
            Inheritable = false;
        }

    }
}
