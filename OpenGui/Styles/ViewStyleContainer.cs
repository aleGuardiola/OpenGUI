using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public class ViewStyleContainer : StyleContainer
    {
        public ViewStyleContainer() : base(1)
        {

        }

        public override bool CanViewUseStyle(View view)
        {
            return true;
        }
    }
}
