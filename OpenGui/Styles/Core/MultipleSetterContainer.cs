﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGui.Controls;
using Portable.Xaml.Markup;

namespace OpenGui.Styles.Core
{
    /// <summary>
    /// Multiple Setter Container is a container that contains another container and filter by parent rather than by view.
    /// </summary>
    /// <typeparam name="T">The type of the container.</typeparam>
    /// <typeparam name="C">The type of the container that this container contains.</typeparam>
    /// <typeparam name="C">The type of the key of the container that this container contains.</typeparam>
    [ContentPropertyAttribute("Container")]
    public abstract class MultipleSetterContainer : SetterContainer       
    {
        Type _parentSelectorType;

        public SetterContainer Container
        {
            get;
            set;
        }

        /// <summary>
        /// The parent selector to be used in this container.
        /// </summary>
        /// <param name="parnetSelectorType"></param>
        public MultipleSetterContainer(Type parentSelectorType)
        {
            _parentSelectorType = parentSelectorType ?? throw new ArgumentNullException(nameof(parentSelectorType));
        }

        public override Selector GetSelector()
        {
            return (Selector)Activator.CreateInstance(_parentSelectorType, (object)Container.GetSelector() );
        }

        public override IList<Setter> GetSetters()
        {
            return Container.GetSetters();
        }
    }

    /// <summary>
    /// Special selector used for MultipleSetterContainer.
    /// </summary>
    /// <typeparam name="T">The type of the MultipleSetterContainer.</typeparam>
    /// <typeparam name="C">The type of the container that contains the MultipleSetterContainer.</typeparam>
    /// <typeparam name="Key">The type of the key of the container that contains MultipleSetterContainer.</typeparam>
    public abstract class ParentSelector : Selector
    {
        Selector _viewSelector;

        public ParentSelector(Selector viewSelector)
        {
            _viewSelector = viewSelector;
        }

        public override string SelectorKey
        {
            get
            {
               return ParentSelectorKey + _viewSelector.SelectorKey;
            }
        }

        public abstract string ParentSelectorKey { get; }

        public abstract bool CanBeUsedByParent(ViewContainer parent, MultipleSetterContainer container);

        public abstract string GetParentContainerKey(MultipleSetterContainer container);

        public abstract IEnumerable<string> GetParentKey(ViewContainer parent);

        public override bool CanBeUsedByView(View view, SetterContainer container)
        {
            var multipleSetterContainer = (MultipleSetterContainer)container;
            if (view.Parent == null)
                return false;

            return CanBeUsedByParent(view.Parent, multipleSetterContainer) && _viewSelector.CanBeUsedByView(view, multipleSetterContainer.Container);
        }

        public override string GetContainerKey(SetterContainer container)
        {
            var multipleSetterContainer = (MultipleSetterContainer)container;
            return GetParentContainerKey(multipleSetterContainer) + _viewSelector.GetContainerKey(multipleSetterContainer.Container);
        }

        public override IEnumerable<string> GetViewKey(View view)
        {
            if (view.Parent == null)
                return new string[0];

            var parentKeys = GetParentKey(view.Parent).ToArray();
            var viewKeys = _viewSelector.GetViewKey(view).ToArray();

            var result = new string[parentKeys.Length * viewKeys.Length];

            int index = 0;
            for (int i = 0; i < parentKeys.Length; i++)
                for (int j = 0; j < viewKeys.Length; j++, index++)
                    result[index] = parentKeys[i] + viewKeys[j];

            return result;
        }
    }

}
