using OpenGui.Controls;
using OpenGui.Styles.Core;
using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGui.Styles
{
    public class ClassContainer : SingleSetterContainer
    {
        public string Class { get; set; }

        public override Selector GetSelector()
        {
            return new ClassSelector();
        }

        private class ClassSelector : Selector
        {
            public override bool CanBeUsedByView(View view, SetterContainer container)
            {
                var classContainer = (ClassContainer)container;
                return view.Class == classContainer.Class;
            }

            public override string GetContainerKey(SetterContainer container)
            {
                var classContainer = (ClassContainer)container;
                return "." + classContainer.Class;
            }

            public override IEnumerable<string> GetViewKey(View view)
            {
                return view.Class.Split(' ').Select(s => "." + s);
            }
        }
    }

    public class ClassParentContainer : MultipleSetterContainer
    {
        public string Class { get; set; }

        public ClassParentContainer() : base(typeof(ClassParentContainerSelector))
        {

        }

        public class ClassParentContainerSelector : ParentSelector
        {

            public ClassParentContainerSelector(Selector viewSelector) : base(viewSelector)
            {

            }

            public override bool CanBeUsedByParent(ViewContainer parent, MultipleSetterContainer container)
            {
                var classContainer = (ClassParentContainer)container;
                return classContainer.Class == parent.Class;
            }

            public override string GetParentContainerKey(MultipleSetterContainer container)
            {
                var classContainer = (ClassParentContainer)container;
                return "." + classContainer.Class;
            }

            public override IEnumerable<string> GetParentKey(ViewContainer parent)
            {
                return parent.Class.Split(' ').Select(s => "." + s);
            }
        }

    }

}
