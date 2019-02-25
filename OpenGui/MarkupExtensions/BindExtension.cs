using OpenGui.Controls;
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
                return bindProperty(view, property);
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

        object bindProperty(View view, object propertyInfo)
        {
            var property = propertyInfo as PropertyInfo;

            view.GetObservable<object>(nameof(View.BindingContext)).Subscribe((next) =>
            {
                if (next == null)
                    return;

                if (next == null)
                    return;

                var bindMethod = typeof(View).GetMethod("Bind", new[] { typeof(string), typeof(string), });
                var twoWayBindMethod = typeof(View).GetMethod("BindTwoWay", new[] { typeof(string), typeof(string), });

                MethodInfo genericMethod;

                switch (Mode)
                {
                    case BindMode.OneWay:
                        genericMethod = bindMethod.MakeGenericMethod(property.PropertyType);
                        genericMethod.Invoke(view, new object[] { property.Name, Path });
                        break;

                    case BindMode.TwoWay:
                        genericMethod = twoWayBindMethod.MakeGenericMethod(property.PropertyType);
                        genericMethod.Invoke(view, new object[] { property.Name, Path });
                        break;
                }
            });
            
            if (property.PropertyType.IsValueType)
            {
                return Activator.CreateInstance(property.PropertyType);
            }

            return null;
        }
    }
}
