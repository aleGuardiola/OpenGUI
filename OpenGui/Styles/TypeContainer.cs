using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public class TypeContainer : StyleContainer
    {
        public string TypeName
        {
            get;
            set;
        }

        public TypeContainer() : base(5)
        {

        }

        public override bool CanViewUseStyle(View view)
        {
            var viewType = typeof(View);
            var type = view.GetType();
            do
            {
                if (type.Name == TypeName)
                    return true;                
            } while ((type = type.BaseType) != viewType);

            return false;
        }
    }
}
