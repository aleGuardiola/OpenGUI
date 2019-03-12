using OpenGui.Controls;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace OpenGui.Styles.Core
{
    public class StyleEngine
    {
        Dictionary<Type, SelectorStyleContainer> _selectors;
        List<View> views;

        public StyleEngine()
        {
            _selectors = new Dictionary<Type, SelectorStyleContainer>();
        }
        
        public Add

        /// <summary>
        /// Observe to style changes in a view.
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        IObservable<IEnumerable<Setter>> GetStyles(View view)
        {

        }

//----------------------------------------------------------------------------------------------------------------------------------------------
        private class SelectorStyleContainer
        {
            Selector Selector { get; }
            Dictionary<string, IList<Setter>> StylesIndexed { get; }

            List<(Subject<IEnumerable<Setter>> subject, View view)> Views { get; }

            public SelectorStyleContainer(Selector selector)
            {
                Selector = selector;
                StylesIndexed = new Dictionary<string, IList<Setter>>();
                Views = new List<(Subject<IEnumerable<Setter>> subject, View view)>();
            }

        }

    }
}
