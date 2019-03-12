using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles.Core
{
    /// <summary>
    /// A single setter container is a container of setters that only has the setters and no other containers.
    /// </summary>
    /// <typeparam name="T">The type of the container</typeparam>
    /// <typeparam name="Key">The kery used in the selector.</typeparam>
    [ContentPropertyAttribute("Setters")]
    public abstract class SingleSetterContainer : SetterContainer
    {
        public IList<Setter> Setters { get; }

        public SingleSetterContainer()
        {
            Setters = new List<Setter>();
        }

        public override IList<Setter> GetSetters()
        {
            return Setters;
        }
    }
}
