using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public abstract class InsideContainer : StyleContainerBase
    {
        public StyleContainer Container
        {
            get;
            set;
        }

        public abstract bool CanChildrenUseStyle(ViewContainer parent);
        
        public override bool CanViewUseStyle(View view)
        {
            if (view.Parent == null)
                return false;

            if (!CanChildrenUseStyle(view.Parent))
                return false;

            return Container.CanViewUseStyle(view);
        }
    }
}
