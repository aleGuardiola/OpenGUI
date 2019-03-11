using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public class IdContainer : StyleContainer
    {
        public string Id
        {
            get;
            set;
        }

        public IdContainer() : base(2)
        {

        }

        public override bool CanViewUseStyle(View view)
        {
            return view.Id == Id;
        }
    }
}
