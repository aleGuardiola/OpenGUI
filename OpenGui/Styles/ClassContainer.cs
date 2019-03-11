using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public class ClassContainer : StyleContainer
    {
        public string Class
        {
            get;
            set;
        }

        public ClassContainer() : base(3)
        {

        }

        public override bool CanViewUseStyle(View view)
        {
            return view.Class == Class;
        }
    }
}
