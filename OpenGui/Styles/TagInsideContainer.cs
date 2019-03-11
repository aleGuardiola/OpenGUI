using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public class TagInsideContainer : InsideContainer
    {

        public string Tag
        {
            get;
            set;
        }

        public override bool CanChildrenUseStyle(ViewContainer parent)
        {
            return parent.GetType().Name == Tag;
        }
    }
}
