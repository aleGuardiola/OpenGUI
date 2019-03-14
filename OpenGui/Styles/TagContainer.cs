using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;
using OpenGui.Styles.Core;

namespace OpenGui.Styles
{
    public class TagContainer : SingleSetterContainer
    {
        public string Tag { get; set; }

        public TagContainer()
        {
            Tag = "";
        }

        public TagContainer(string tag)
        {
            Tag = tag;
        }

        public override Selector GetSelector()
        {
            return new TagSelector();
        }

        private class TagSelector : Selector
        {
            public override string SelectorKey => "TagSelector";

            public override bool CanBeUsedByView(View view, SetterContainer container)
            {
                return view.GetType().Name == (container as TagContainer).Tag;
            }

            public override string GetContainerKey(SetterContainer container)
            {
                return (container as TagContainer).Tag;
            }

            public override IEnumerable<string> GetViewKey(View view)
            {
                yield return view.GetType().Name;
            }
        }
    }

    public class TagParentContainer : MultipleSetterContainer
    {
        public string Tag { get; set; }

        public TagParentContainer() : base(typeof(TagParentContainerSelector))
        {

        }

        public class TagParentContainerSelector : ParentSelector
        {

            public TagParentContainerSelector(Selector viewSelector) : base(viewSelector)
            {

            }

            public override string ParentSelectorKey => "ParentClassSelector";

            public override bool CanBeUsedByParent(ViewContainer parent, MultipleSetterContainer container)
            {
                var tagContainer = (TagParentContainer)container;
                return tagContainer.Tag == parent.GetType().Name;
            }

            public override string GetParentContainerKey(MultipleSetterContainer container)
            {
                return (container as TagParentContainer).Tag;
            }

            public override IEnumerable<string> GetParentKey(ViewContainer parent)
            {
                yield return parent.GetType().Name;
            }
        }
    }

}
