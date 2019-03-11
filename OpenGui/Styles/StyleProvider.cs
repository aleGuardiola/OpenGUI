using OpenGui.Controls;
using OpenGui.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGui.Styles
{
    public class StyleProvider
    {
        List<StyleDefinition> _definitions;

        public StyleProvider()
        {
            _definitions = new List<StyleContainer>();
        }

        public void AddDefinition(StyleDefinition definition)
        {
            _containers.Add(container);
        }

        public IEnumerable<Set> GetStyles(View view)
        {
            return view.Styles.Setters.Concat(
                _containers.Where(c => c.CanViewUseStyle(view))
                .OrderBy(c => c.Priority).SelectMany(c => c.Setters))
                .Concat(view.Parent.GetInheritedStyles())
                .Distinct(new EComparer<Set>((x, y) => x.Property == y.Property));
        }
    }
}
