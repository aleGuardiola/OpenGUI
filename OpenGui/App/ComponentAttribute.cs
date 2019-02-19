using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenGui.App
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {        
        public string Template
        {
            get;
            set;
        }

        public string TemplateResource
        {
            get;
            set;
        }

        public string TemplateFile
        {
            get;
            set;
        }

    }
}
