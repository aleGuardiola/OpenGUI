using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    public class IdSelector : Selector
    {
        public string Id
        {
            get;
            private set;
        }

        public IdSelector(string id)
        {
            Id = id;
        }

    }
}
