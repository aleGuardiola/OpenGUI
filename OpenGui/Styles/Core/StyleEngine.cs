using OpenGui.Controls;
using OpenGui.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace OpenGui.Styles.Core
{
    public class StyleEngine
    {
        Dictionary<string, SelectorIndex> _selectors;
        Dictionary<Guid, View> _views;

        public StyleEngine()
        {
            _selectors = new Dictionary<string, SelectorIndex>();
            _views = new Dictionary<Guid, View>();
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

        public IEnumerable<Setter> GetSetters(View view)
        {
            List<Setter> result = new List<Setter>();
            foreach(var selectorIndexKeyValue in _selectors)
            {
                var selectorIndex = selectorIndexKeyValue.Value;
                var keys = selectorIndex.Selector.GetViewKey(view);
                
                foreach(var key in keys)
                {
                    Dictionary<string, Setter> setters;
                    if (selectorIndex.Setters.TryGetValue(key, out setters))
                        result.AddRange(setters.Values);
                }                
            }

            return result;
        }
        
        private static void UpdateSetters(SelectorIndex selectorIndex, SetterContainer setterContainer)
        {
            var setters = GetSetters(selectorIndex, setterContainer);
            bool changed = false;

            foreach(var setter in setterContainer.GetSetters())
            {
                if (UpdateSetter(setters, setter))
                    changed = true;
            }

            if (changed)
                selectorIndex.OnSelectorChange(setterContainer.GetSelector().SelectorKey);

        }

        private static bool UpdateSetter(Dictionary<string, Setter> map, Setter setter)
        {
            Setter mapSetter;
            if(!map.TryGetValue(setter.Property, out mapSetter))
            {
                map.Add(setter.Property, setter);
                return true;
            }

            if(mapSetter.Value != setter.Value)
            {
                mapSetter.Value = setter.Value;
                return true;
            }

            return false;
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
            SelectorIndex result;
            if (_selectors.TryGetValue(selector.SelectorKey, out result))
                return result;

            result = new SelectorIndex(selector);
            _selectors.Add(selector.SelectorKey, result);

            return result;
        }

    }

    internal class SelectorIndex
    {
        public Selector Selector { get; }
        public Dictionary<string, Dictionary<string, Setter> > Setters { get; }

        public event EventHandler<GenericEventArgs<string>> SelectorChange; 
        public void OnSelectorChange(string selector)
        {
            SelectorChange?.Invoke(this, new GenericEventArgs<string>(selector));
        }

        public SelectorIndex(Selector selector)
        {
            Selector = selector;
            Setters = new Dictionary<string, Dictionary<string, Setter>>();
        }
    }

}
