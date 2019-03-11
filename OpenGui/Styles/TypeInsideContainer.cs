using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;

namespace OpenGui.Styles
{
    public class TypeInsideContainer : InsideContainer
    {
        public string TypeName
        {
            get;
            set;
        }

        public override bool CanChildrenUseStyle(ViewContainer parent)
        {
            var viewType = typeof(View);
            var type = parent.GetType();
            do
            {
                if (type.Name == TypeName)
                    return true;
            } while ((type = type.BaseType) != viewType);

            return false;
        }
    }
}
