using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Core
{
    public interface IPropertyChangeObservable
    {
        /// <summary>
        /// Get observable of the values of specific property.
        /// </summary>
        /// <typeparam name="T">Type of the value.</typeparam>
        /// <param name="propertyName">Property Name.</param>
        /// <returns>The observable of the property.</returns>
        IObservable<T> GetObservable<T>(string propertyName);
    }
}
