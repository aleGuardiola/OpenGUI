using OpenGui.Controls;
using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenGui.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(object))]
    public class BindExtension : MarkupExtension
    {
        public string Path
        {
            get;
            set;
        }

        public BindMode Mode
        {
            get;
            set;
        }


        public BindExtension()
        {
            Mode = BindMode.OneWay;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            var view = target.TargetObject as View;
            var property = target.TargetProperty as PropertyInfo;

            view.Bindings.Add((property.Name, Path, Mode, property.PropertyType));

            return null;            
        }
    }
}
