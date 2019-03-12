using OpenGui.Controls;
using OpenGui.Styles.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    public class IdContainer : SingleSetterContainer
    {
        public string Id { get; set; }
        
        public override Selector GetSelector()
        {
            return new IdSelector();
        }

        private class IdSelector : Selector
        {
            public override bool CanBeUsedByView(View view, SetterContainer container)
            {
                var idContainer = (IdContainer)container;
                return view.Id == idContainer.Id;
            }

            public override string GetContainerKey(SetterContainer container)
            {
                var idContainer = (IdContainer)container;
                return "#" + idContainer.Id;
            }

            public override IEnumerable<string> GetViewKey(View view)
            {
                yield return "#" + view.Id;
            }
        }
    }

    public class IdParentContainer : MultipleSetterContainer
    {
        public string Id { get; set; }

        public IdParentContainer() : base(typeof(IdParentContainerSelector))
        {

        }

        public class IdParentContainerSelector : ParentSelector
        {

            public IdParentContainerSelector(Selector viewSelector) : base(viewSelector)
            {

            }

            public override bool CanBeUsedByParent(ViewContainer parent, MultipleSetterContainer container)
            {
                var idContainer = (IdParentContainer)container;
                return idContainer.Id == parent.Class;
            }

            public override string GetParentContainerKey(MultipleSetterContainer container)
            {
                var idContainer = (IdParentContainer)container;
                return "#" + idContainer.Id;
            }

            public override IEnumerable<string> GetParentKey(ViewContainer parent)
            {
                yield return "#" + parent.Id;
            }
        }

    }

}
