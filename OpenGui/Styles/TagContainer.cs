using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public class TagContainer : StyleContainer
    {
        public string Tag
        {
            get;
            set;
        }

        public TagContainer() : base(4)
        {

        }

        public override bool CanViewUseStyle(View view)
        {
            return view.GetType().Name == Tag;
        }
    }
}
