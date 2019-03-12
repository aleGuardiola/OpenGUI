using OpenGui.Controls;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles.Core
{
    /// <summary>
    /// Represent a selector that select the views that a set of setters can be applied.
    /// Example class or id
    /// </summary>
    /// <typeparam name="T">The type setter container that this selector use.</typeparam>
    /// <typeparam name="Key">The type of the unique key used to index the container</typeparam>
    public abstract class Selector
    {
        /// <summary>
        /// Check if a view can use the styles in a gived container.
        /// </summary>
        /// <param name="view">The view to check.</param>
        /// <param name="container">The container to check.</param>
        /// <returns>true if the view can use the container or false if not.</returns>
        public abstract bool CanBeUsedByView(View view, SetterContainer container);

        /// <summary>
        /// Get the key used by this container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>Returns the unique key that represents this set of styles.</returns>
        public abstract string GetContainerKey(SetterContainer container);

        /// <summary>
        /// Get the unique key that the view has to match a container.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>Returns the key that a container has to has to be used by the view.</returns>
        public abstract IEnumerable<string> GetViewKey(View view);
    }
}
