using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Styles
{
    /// <summary>
    /// Represent a setter for a property of a view.
    /// </summary>
    [ContentPropertyAttribute("Value")]
    public class Setter
    {

        /// <summary>
        /// The property to set the value.
        /// </summary>
        public string Property { get; set; }
        
        /// <summary>
        /// The value to be set. If the Type is different to string a TypeConverter will be used.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// If set true this setter will be applied to children of the ViewContainer. It is false by default
        /// </summary>
        public bool IsInheritable { get; set; }

        public Setter()
        {
            IsInheritable = false;
            Property = "";
            Value = "";
        }
        
        public Setter(string property, string value, bool isInheritable = false)
        {
            Property = property;
            Value = value;
            IsInheritable = isInheritable;
        }

    }
}
