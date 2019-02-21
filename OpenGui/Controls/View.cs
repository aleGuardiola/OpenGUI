using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using OpenGui.Animations.Xaml;
using OpenGui.Core;
using OpenGui.Graphics;
using OpenGui.Helpers;
using OpenGui.MarkupExtensions;
using OpenGui.Values;
using OpenTK;
using SkiaSharp;

namespace OpenGui.Controls
{
    public class View : LowLevelView
    {
        string _id;
        string _class;
        protected SubscriptionPool SubscriptionPool;              

        /// <summary>
        /// The id of this view
        /// </summary>
        public string Id
        {
            get => _id;
            set => _id = value;
        }

        /// <summary>
        /// The class of this view
        /// </summary>
        public string Class
        {
            get => _class;
            set => _class = value;
        }

        public object BindingContext
        {
            get => GetValue<object>();
            set => SetValue<object>(value);
        }

        public IList<(string proertyView, string property, BindMode mode, Type type)> Bindings
        {
            get;
            private set;
        }

        public Animation Animation
        {
            get => GetValue<Animation>();
            set => SetValue<Animation>(value);
        }

        public bool IsAnimating
        {
            get => GetValue<bool>();
            set => SetValue<bool>(value);
        }

        public ViewContainer Parent
        {
            get => GetValue<ViewContainer>();
            set => SetValue<ViewContainer>(value);
        }

        /// <summary>
        /// Get or Set the background of the view.
        /// </summary>
        public Drawable Background
        {
            get => GetValue<Drawable>();
            set => SetValue<Drawable>(value);
        }
                
        /// <summary>
        /// Get or Set the padding top of the View
        /// </summary>
        public float PaddingTop
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        /// <summary>
        /// Get or Set the padding right of the View
        /// </summary>
        public float PaddingRight
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        /// <summary>
        /// Get or Set the padding bottom of the View
        /// </summary>
        public float PaddingBottom
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        /// <summary>
        /// Get or Set the padding left of the View
        /// </summary>
        public float PaddingLeft
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }


        /// <summary>
        /// Get or Set the margin top of the View
        /// </summary>
        public float MarginTop
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        /// <summary>
        /// Get or Set the margin right of the View
        /// </summary>
        public float MarginRight
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        /// <summary>
        /// Get or Set the margin bottom of the View
        /// </summary>
        public float MarginBottom
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        /// <summary>
        /// Get or Set the margin left of the View
        /// </summary>
        public float MarginLeft
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        /// <summary>
        /// Get or Set the relative X respect the parent
        /// </summary>
        public float RelativeX
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        /// <summary>
        /// Get or Set the relative Y respect the parent
        /// </summary>
        public float RelativeY
        {
            get => GetValue<float>();
            set => SetValue<float>(value);          
        }

        public VerticalAligment VerticalAligment
        {
            get => GetValue<VerticalAligment>();
            set => SetValue<VerticalAligment>(value);
        }

        public HorizontalAligment HorizontalAligment
        {
            get => GetValue<HorizontalAligment>();
            set => SetValue<HorizontalAligment>(value);
        }

        public event EventHandler<ClickEventArgs> Click;

        private OpenGui.Animations.Animation _animation = null;

        public View()
        {
            Bindings = new List<(string proertyView, string property, BindMode mode, Type type)>();
            SubscriptionPool = new SubscriptionPool();
            Parent = null;

            SetValue<float>(nameof(RelativeX), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(RelativeY), ReactiveObject.LAYOUT_VALUE, 0);

            SetValue<float>(nameof(MarginBottom), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(MarginTop), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(MarginRight), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(MarginLeft), ReactiveObject.LAYOUT_VALUE, 0);
            
            SetValue<float>(nameof(PaddingBottom), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(PaddingTop), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(PaddingRight), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(PaddingLeft), ReactiveObject.LAYOUT_VALUE, 0);

            SetValue<Drawable>(nameof(Background), ReactiveObject.LAYOUT_VALUE, new DrawableColor(System.Drawing.Color.Transparent));

            SetValue<Align>(nameof(Align), ReactiveObject.LAYOUT_VALUE, Align.Center);

            SetValue<VerticalAligment>(nameof(VerticalAligment), ReactiveObject.LAYOUT_VALUE,  VerticalAligment.Center );
            SetValue<HorizontalAligment>(nameof(HorizontalAligment), ReactiveObject.LAYOUT_VALUE, HorizontalAligment.Center );

            SetValue<bool>(nameof(IsAnimating), ReactiveObject.LAYOUT_VALUE, false);

            SubscriptionPool.Add(GetObservable<float>(nameof(Width)).Subscribe((v) => Parent?.ForceMeasure()));
            SubscriptionPool.Add(GetObservable<float>(nameof(Height)).Subscribe((v) => Parent?.ForceMeasure()));
            SubscriptionPool.Add(GetObservable<HorizontalAligment>(nameof(HorizontalAligment)).Subscribe((v) => Parent?.ForceMeasure()));
            SubscriptionPool.Add(GetObservable<VerticalAligment>(nameof(VerticalAligment)).Subscribe((v) => Parent?.ForceMeasure()));
            SubscriptionPool.Add(GetObservable<bool>(nameof(IsAnimating)).Subscribe(OnNextIsAnimating));
            SubscriptionPool.Add(GetObservable<object>(nameof(BindingContext)).Subscribe(OnNextBindingContext));
        }

        public virtual bool TryGetViewById(string id, out View view)
        {
            if(Id == id)
            {
                view = this;
                return true;
            }

            view = null;

            return false;
        }

        public T GetViewById<T>(string id) where T : View
        {
            View result;

            if (!TryGetViewById(id, out result ))            
                throw new Exception($"View with id {id} cannot be found.");

            return result as T;
        }

        public virtual void OnClick(ClickEventArgs e)
        {
            Click?.Invoke(this, e);
        }

        private void OnNextBindingContext(object next)
        {
            if (next == null)
                return;

            var bindMethod = typeof(View).GetMethod("Bind", new[] { typeof(string), typeof(string), });
            var twoWayBindMethod = typeof(View).GetMethod("BindTwoWay", new[] { typeof(string), typeof(string), });

            MethodInfo genericMethod;

            foreach (var binding in Bindings)
            {
                switch(binding.mode)
                {
                    case BindMode.OneWay:
                        genericMethod = bindMethod.MakeGenericMethod(binding.type);
                        genericMethod.Invoke(this, new object[] { binding.proertyView, binding.property } );    
                        break;

                    case BindMode.TwoWay:
                        genericMethod = twoWayBindMethod.MakeGenericMethod(binding.type);
                        genericMethod.Invoke(this, new object[] { binding.proertyView, binding.property });
                        break;
                }
            }
        }

        private object GetBindingContext()
        {
            var reactiveObj = this;
            while (!reactiveObj.Exist(nameof(BindingContext))) {

                reactiveObj = reactiveObj.Parent;

                if (reactiveObj == null)
                    return new object();               
            }

            return reactiveObj.BindingContext;            
        }

        public void Bind<T>(string viewProperty, string bindingContextProperty)
        {
            var bindingContext = GetBindingContext();

            var prop = bindingContext.GetType().GetProperty(bindingContextProperty);
            try
            {
                SetValue<T>((T)prop.GetValue(bindingContext), viewProperty);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error getting value: {e.Message}");
            }            

            if(bindingContext is INotifyPropertyChanged)
            {
                var notifier = (INotifyPropertyChanged)bindingContext;
                var observable = notifier.GetObservable<T>(bindingContextProperty);
                Bind(viewProperty, observable);
            }
            else if(bindingContext is ReactiveObject )
            {
                var reactiveObj = (ReactiveObject)bindingContext;
                var observable = reactiveObj.GetObservable<T>(bindingContextProperty);
                Bind(viewProperty, observable);
            }
        }

        public void BindTwoWay<T>(string viewProperty, string bindingContextProperty)
        {
            var bindingContext = GetBindingContext();
            
            var prop = bindingContext.GetType().GetProperty(bindingContextProperty);            
            try
            { 
                SetValue<T>((T)prop.GetValue(bindingContext), viewProperty);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error getting value: {e.Message}");
            }

            if (bindingContext is INotifyPropertyChanged)
            {
                var notifier = (INotifyPropertyChanged)bindingContext;
                var observable = notifier.GetObservable<T>(bindingContextProperty);
                BindTwoWay(viewProperty, observable, bindingContext, bindingContextProperty);
            }
            else if (bindingContext is ReactiveObject)
            {
                var reactiveObj = (ReactiveObject)bindingContext;
                var observable = reactiveObj.GetObservable<T>(bindingContextProperty);
                BindTwoWay(viewProperty, observable, bindingContext, bindingContextProperty);
            }
        }

        private void OnNextIsAnimating(bool value)
        {
            if (!Exist(nameof(Animation)))
                return;

            if(value)
            {
                if(_animation == null || _animation.IsStop())
                {
                    _animation = Animation.GetAnimation(this);

                    _animation.Stop += _animation_Stop;

                    Window.AddFrameRunner(_animation);
                }
            }
            else
            {
                if(_animation != null && (_animation.IsRunning() || !_animation.IsStop()))
                {
                    _animation.ForceStop();
                }
            }
        }

        private void _animation_Stop(object sender, EventArgs e)
        {
            _animation = null;
            IsAnimating = false;
        }

        public virtual void Check()
        {
            
        }

        protected override (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            var minWidth = MinWidth;
            var minHeight = MinHeight;

            float width = 0;
            float height = 0;

            switch (mode)
            {
                case MeasureSpecMode.Exactly:
                    width = Math.Max(widthSpec, minWidth);
                    height = Math.Max(heightSpec, minWidth);
                    break;                 
                case MeasureSpecMode.AtMost:
                    //calculate width                    
                        width = Math.Max(PaddingLeft + PaddingRight, minWidth );
                        if (width > widthSpec)
                            width = Math.Max(widthSpec, minWidth);                    

                    //calculate height
                        height = Math.Max(PaddingTop + PaddingBottom, minHeight);
                        if (height > heightSpec)
                            height = Math.Max(heightSpec, minHeight);                    
                    break;
                case MeasureSpecMode.Unspecified:

                    //calculate width
                    width = Math.Max(PaddingLeft + PaddingRight, minWidth);                   

                    //calculate height
                    height = Math.Max(PaddingTop + PaddingBottom, minHeight);
                    break;
            }

            return (width, height);
        }

        protected override void DrawTexture(SKCanvas canvas, int width, int height)
        {
            if (Background == null)
                base.DrawTexture(canvas, width, height);
            else
                Background.Draw(width, height, canvas);
        }

        ~View()
        {
            SubscriptionPool.UnsubscribeAll();
        }

    }
}
