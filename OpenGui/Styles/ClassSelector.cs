using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    public class ClassSelector : Selector
    {
        public string Class
        {
            get;
            private set;
        }

        public ClassSelector(string clas)
        {
            Class = clas;
        }

    }
}
