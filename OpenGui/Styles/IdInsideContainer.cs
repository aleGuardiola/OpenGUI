using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public class IdInsideContainer : InsideContainer
    {
        public string Id
        {
            get;
            set;
        }

        public override bool CanChildrenUseStyle(ViewContainer parent)
        {
            return parent.Id == Id;
        }
    }
}
