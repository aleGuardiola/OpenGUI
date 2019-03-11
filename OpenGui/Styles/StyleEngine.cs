using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    public class StyleEngine
    {
        const int MaxNumberOfSelectors = 20;
        Dictionary<string, Dictionary<string, SetterStore>>[] _setters;

        public StyleEngine()
        {
            _setters = new Dictionary<string, Dictionary<string, SetterStore>>[MaxNumberOfSelectors];
        }

        public void AddStyle(StyleDefinition styleDefinition)
        {

        }
    }

    internal class SetterStore
    {
        Set Set{ get; }

        int StyleContainerPriority { get; }

        StyleDefinitionPriority StyleDefinitionPriority { get; }        
    }

}
