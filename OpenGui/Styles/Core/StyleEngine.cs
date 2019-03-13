using OpenGui.Controls;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace OpenGui.Styles.Core
{
    public class StyleEngine
    {
        Dictionary<Type, SelectorIndex> _selectors;

        public StyleEngine()
        {
            _selectors = new Dictionary<Type, SelectorIndex>();
        }

        public void AddDefinition(StyleDefinition definition)
        {
            foreach(var container in definition.Containers)
            {
                var selector = container.GetSelector();

                var selectorIndex = GetSelectorIndex(selector);

                UpdateSetters(selectorIndex, container);
            }
        }

        private static void UpdateSetters(SelectorIndex selectorIndex, SetterContainer setterContainer)
        {
            var setters = GetSetters(selectorIndex, setterContainer);

            foreach(var setter in setterContainer.GetSetters())
            {
                UpdateSetter(setters, setter);
            }
        }

        private static void UpdateSetter(Dictionary<string, Setter> map, Setter setter)
        {
            Setter mapSetter;
            if(!map.TryGetValue(setter.Property, out mapSetter))
            {
                map.Add(setter.Property, setter);
                return;
            }

            mapSetter.Value = setter.Value;
        }

        private static Dictionary<string, Setter> GetSetters(SelectorIndex selectorIndex, SetterContainer setterContainer)
        {
            var key = setterContainer.GetSelector().GetContainerKey(setterContainer);
            Dictionary<string, Setter> result;
            if (selectorIndex.Setters.TryGetValue(key, out result))
                return result;

            result = new Dictionary<string, Setter>();
            selectorIndex.Setters.Add(key, result);

            return result;
        }

        private SelectorIndex GetSelectorIndex(Selector selector)
        {
            Type selectorType = selector.GetType();
            SelectorIndex result;
            if (_selectors.TryGetValue(selectorType, out result))
                return result;

            result = new SelectorIndex(selector);
            _selectors.Add(selectorType, result);

            return result;
        }

    }

    internal class SelectorIndex
    {
        public Selector Selector { get; }
        public Dictionary<string, Dictionary<string,  > > Setters { get; }

        public SelectorIndex(Selector selector)
        {
            Selector = selector;
            Setters = new Dictionary<string, Dictionary<string, Setter>>();
        }
    }

}
