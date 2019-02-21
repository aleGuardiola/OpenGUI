using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    public abstract class StyleProvider
    {
        private Dictionary<string, SelectorStyle> _classStyles;
        private Dictionary<string, SelectorStyle> _idStyles;
        private Dictionary<string, SelectorStyle> _tagStyles;

        public StyleProvider()
        {
            _classStyles = new Dictionary<string, SelectorStyle>();
            _idStyles = new Dictionary<string, SelectorStyle>();
            _tagStyles = new Dictionary<string, SelectorStyle>();
        }

        protected void AddClassSelector( string clas, SelectorStyle style )
        {
            _classStyles.Add(clas, style);
        }
        
        protected void AddIdSelector(string id, SelectorStyle style)
        {
            _classStyles.Add(id, style);
        }

        protected void AddTagSelector(string tag, SelectorStyle style)
        {
            _classStyles.Add(tag, style);
        }

        public bool TryGetSelectorStyle(Selector selector, out SelectorStyle result)
        {
            if (selector is ClassSelector)
            {
                var classSelector = selector as ClassSelector;
                return _classStyles.TryGetValue(classSelector.Class, out result);
            }

            if (selector is IdSelector)
            {
                var idSelector = selector as IdSelector;
                return _idStyles.TryGetValue(idSelector.Id, out result);
            }

            if (selector is TagSelector)
            {
                var tagSelector = selector as TagSelector;
                return _tagStyles.TryGetValue(tagSelector.Tag, out result);
            }

            result = null;
            return false;
        }        

    }
}
