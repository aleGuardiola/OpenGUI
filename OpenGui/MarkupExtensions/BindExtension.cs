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
        Core.Binder _binder = null;

        IObservable<object> targetObservable;
        View target;
        PropertyInfo targetPropertyInfo;
        IDisposable bindingContextSubscription;

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
            target = view;
            targetPropertyInfo = property;

            if(Mode == BindMode.OneWay)
            {
                bindingContextSubscription = view.GetObservable(x => x.BindingContext).Subscribe(onNextBindingContextOneWay);
            }
            else
            {
                targetObservable = view.GetObservable<object>(property.Name);
                bindingContextSubscription = view.GetObservable(x => x.BindingContext).Subscribe(onNextBindingContextTwoWay);
            }

            if (property.PropertyType.IsValueType)
            {
                return Activator.CreateInstance(property.PropertyType);
            }

            return null;
        }

        void onNextBindingContextOneWay(object bindingContext)
        {

        }
        
        void onNextBindingContextTwoWay(object bindingContext)
        {

        }

        (PropertyInfo Property, object Source)getSourceProperty(object bindingContext)
        {
            object source;
            var pathSplit = Path.Split('.');
            source = bindingContext;
            PropertyInfo property = null;

            for(int i = 0; i < pathSplit.Length; i++)
            {
                property = source.GetType().GetProperty(pathSplit[i]);

                if(i < pathSplit.Length - 1)
                {
                    source = property.GetValue(source);
                }
            }

            return (property, source);
        }

        IObservable<object> getSourceObservable(object source, PropertyInfo property)
        {
             
        }

        ~BindExtension()
        {
            bindingContextSubscription?.Dispose();
            _binder?.Dispose();
        }
    }
}
