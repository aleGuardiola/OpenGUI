using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public class ClassInsideContainer : InsideContainer
    {
        public string Class
        {
            get;
            set;
        }

        public override bool CanChildrenUseStyle(ViewContainer parent)
        {
            return parent.Class == Class;
        }
    }
}
