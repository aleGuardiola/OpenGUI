using OpenGui.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OpenGui.Core
{
    public class ViewBinder : IDisposable
    {
        Binder binder = null;
        IDisposable bindingContextSubscription;

        bool _isTwoWay;
        View _view;
        PropertyInfo _viewPropertyInfo;       

        string[] _propertyPath;        
        
        public ViewBinder(View view, string viewProperty, string sourcePropertyPath, bool isTwoWay = false)
        {
            if (string.IsNullOrEmpty(sourcePropertyPath))
                throw new ArgumentNullException(nameof(sourcePropertyPath));

            _propertyPath = sourcePropertyPath.Split('.');
            
            _isTwoWay = isTwoWay;

            _view = view;
            _viewPropertyInfo = view.GetType().GetProperty(viewProperty);

            if ( view.GetValueOrDefault<object>(nameof(view.BindingContext)) != null)
                createBinder(view.BindingContext);

            bindingContextSubscription =  view.GetObservable(x => x.BindingContext).Subscribe(nextBindingContext);
        }

        void nextBindingContext(object next)
        {
            binder?.Dispose();
            binder = null;
            createBinder(next);
        }

        void createBinder(object bindingContext)
        {
            if (bindingContext == null)
                return;

            var tuple = getSourceObjectAndPropertyInfo(bindingContext);
            var source = tuple.Item2;
            var sourcePropertyInfo = tuple.Item1;

            var type = _viewPropertyInfo.PropertyType;

            object sourceObservable;
                                    
            sourceObservable = getPropObservable(source, sourcePropertyInfo, _viewPropertyInfo.PropertyType);

            var sourceObservableType = sourceObservable.GetType().GetInterfaces().First(i => i.GetGenericTypeDefinition() == typeof(IObservable<>)).GetGenericArguments()[0];
              
            if (!_viewPropertyInfo.PropertyType.IsAssignableFrom( sourceObservableType ) )
            {                
                throw new Exception("Types not match.");
            }
                

            if (!_isTwoWay)
            {
                binder = newOneWayBinder(_view, _viewPropertyInfo.Name, sourceObservable, _viewPropertyInfo.PropertyType);
                return;
            }

            var targetObservable = getPropObservable(_view, _viewPropertyInfo, _viewPropertyInfo.PropertyType);
            binder = newTwoWayBinder(_view, _viewPropertyInfo.Name, targetObservable, source, sourcePropertyInfo.Name, sourceObservable, _viewPropertyInfo.PropertyType);
        }

        static Binder newTwoWayBinder(object target, string targetProperty, object targetObservable, object source, string sourceProperty, object sourceObservable, Type propertyType)
        {
            var type = typeof(TwoWayBinder<>).MakeGenericType(propertyType);
            return Activator.CreateInstance(type, target, targetProperty, targetObservable, source, sourceProperty, sourceObservable) as Binder;
        }

        static Binder newOneWayBinder(object target, string targetProperty, object sourceObservable, Type propertyType)
        {
            var type = typeof(OneWayBinder<>).MakeGenericType(propertyType);
            return Activator.CreateInstance(type, target, targetProperty, sourceObservable) as Binder;
        }

        static object getPropObservable(object obj, PropertyInfo propertyInfo, Type type)
        {
            var method = typeof(ViewBinder).GetMethod(nameof(getPropertyObservable), BindingFlags.Static | BindingFlags.NonPublic)
                                           .MakeGenericMethod(type);

            return method.Invoke(null, new object[] { obj, propertyInfo });
        }

        static IObservable<T> getPropertyObservable<T>(object obj, PropertyInfo propertyInfo)
        {
            if(propertyInfo.PropertyType == typeof(IObservable<T>))
            {
                return propertyInfo.GetValue(obj) as IObservable<T>;
            }
            if(obj is IPropertyChangeObservable)
            {
                return (obj as IPropertyChangeObservable).GetObservable<T>(propertyInfo.Name);
            }
            else if(obj is INotifyPropertyChanged)
            {
                return (obj as INotifyPropertyChanged).GetObservable<T>(propertyInfo.Name);
            }

            return new SyncronicValueObservable<T>((T)propertyInfo.GetValue(obj));
        }
               
        Tuple<PropertyInfo, object> getSourceObjectAndPropertyInfo(object bindingContext)
        {
            var objectResult = bindingContext;
            var propertyInfoResult = bindingContext.GetType().GetProperty(_propertyPath[0]);

            for(int i = 1; 1 < _propertyPath.Length; i++)
            {
                objectResult = propertyInfoResult.GetValue(objectResult);
                propertyInfoResult = objectResult.GetType().GetProperty(_propertyPath[i]);
            }

            return new Tuple<PropertyInfo, object>(propertyInfoResult, objectResult);
        }

        public void Dispose()
        {            
            binder?.Dispose();
            bindingContextSubscription.Dispose();
        }
    }
}
