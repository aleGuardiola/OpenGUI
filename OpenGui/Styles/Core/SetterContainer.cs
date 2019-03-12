using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles.Core
{
    public abstract class SetterContainer
    {
        /// <summary>
        /// Get the selector used by this setter container.
        /// </summary>
        /// <returns>The selector used by the container.</returns>
        public abstract Selector GetSelector();

        /// <summary>
        /// get the setters that this container contains.
        /// </summary>
        /// <returns>The list of setters.</returns>
        public abstract IList<Setter> GetSetters();
    }
}
