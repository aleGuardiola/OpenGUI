using OpenGui.Controls;
using OpenGui.Core;
using Portable.Xaml.Markup;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace OpenGui.MarkupExtensions
{
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
            var property = target.TargetProperty;

            if (property is PropertyInfo)
            {
                return bindProperty(view, property as PropertyInfo);
            }
            else
            {
                bindMethod(view, property);
            }

            return null;
        }

        void bindMethod(View view, object eventHandler)
        {
            var eventInfo = eventHandler as EventInfo;
            Delegate lastDelegate = null;
            view.GetObservable<object>(nameof(View.BindingContext)).Subscribe((bindingContext) =>
            {

                if (bindingContext == null)
                    return;

                var methodInfo = bindingContext.GetType().GetMethod(Path);
                if (methodInfo == null)
                    throw new InvalidCastException($"Cannot find any public method with name {Path}.");

                var deleg = Delegate.CreateDelegate(eventInfo.EventHandlerType, bindingContext, methodInfo);

                if (lastDelegate != null)
                    eventInfo.RemoveEventHandler(view, lastDelegate);

                lastDelegate = deleg;
                eventInfo.AddEventHandler(view, deleg);

            });
        }

        object bindProperty(View view, PropertyInfo property)
        {
            new ViewBinder(view, property.Name, Path, Mode == BindMode.OneWay ? false : true);
                        
            if (property.PropertyType.IsValueType)
            {
                return Activator.CreateInstance(property.PropertyType);
            }

            return null;
        }

    }
}
