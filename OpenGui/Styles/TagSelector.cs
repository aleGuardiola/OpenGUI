using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    public class TagSelector : Selector
    {
        public string Tag
        {
            get; private set;
        }

        public TagSelector(string tag)
        {
            Tag = tag;
        }

    }
}
