﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenGui.Animations.Xaml;
using OpenGui.Core;
using OpenGui.Graphics;
using OpenGui.Helpers;
using OpenGui.MarkupExtensions;
using OpenGui.Styles;
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

        protected OpenTK.Input.KeyboardState LastKeyBoardState;
        protected OpenTK.Input.MouseState LastMouseState;
        protected OpenTK.Input.GamePadState LastGamePadState;
        protected OpenTK.Input.JoystickState LastJostickState;

        //input related variables
        bool _wasMouseDown;
        
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
        
        public IList<Setter> Styles
        {
            get => GetValue<IList<Setter>>();
            set => SetValue(value);
        }

        IEnumerable<Setter> _setters;
        protected IEnumerable<Setter> Setters
        {
            get => _setters;
        }

        public object BindingContext
        {
            get => GetValue<object>();
            set => SetValue<object>(value);
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

        public event EventHandler<MouseEventArgs> Click;

        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;

        private OpenGui.Animations.Animation _animation = null;

        public View()
        {
            
            SubscriptionPool = new SubscriptionPool();
            Parent = null;

            SetValue<float>(nameof(RelativeX), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(RelativeY), ReactiveObject.SYSTEM_VALUE, 0);

            SetValue<float>(nameof(MarginBottom), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(MarginTop), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(MarginRight), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(MarginLeft), ReactiveObject.SYSTEM_VALUE, 0);
            
            SetValue<float>(nameof(PaddingBottom), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(PaddingTop), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(PaddingRight), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(PaddingLeft), ReactiveObject.SYSTEM_VALUE, 0);

            SetValue<float>(nameof(CalculatedWidth), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(CalculatedHeight), ReactiveObject.SYSTEM_VALUE, 0);

            SetValue<Drawable>(nameof(Background), ReactiveObject.SYSTEM_VALUE, new DrawableColor(System.Drawing.Color.Transparent));

            SetValue<Align>(nameof(Align), ReactiveObject.SYSTEM_VALUE, Align.Center);

            SetValue<VerticalAligment>(nameof(VerticalAligment), ReactiveObject.SYSTEM_VALUE,  VerticalAligment.Center );
            SetValue<HorizontalAligment>(nameof(HorizontalAligment), ReactiveObject.SYSTEM_VALUE, HorizontalAligment.Center );

            SetValue<bool>(nameof(IsAnimating), ReactiveObject.SYSTEM_VALUE, false);

            SetValue<IList<Setter>>(nameof(Styles), ReactiveObject.SYSTEM_VALUE, new List<Setter>(0));
            
            AttachedToWindow += View_AttachedToWindow;

            SubscriptionPool.Add(GetObservable<float>(nameof(Width)).Subscribe((v) => Parent?.ForceMeasure()));
            SubscriptionPool.Add(GetObservable<float>(nameof(Height)).Subscribe((v) => Parent?.ForceMeasure()));
            SubscriptionPool.Add(GetObservable<HorizontalAligment>(nameof(HorizontalAligment)).Subscribe((v) => Parent?.ForceMeasure()));
            SubscriptionPool.Add(GetObservable<VerticalAligment>(nameof(VerticalAligment)).Subscribe((v) => Parent?.ForceMeasure()));
            SubscriptionPool.Add(GetObservable<bool>(nameof(IsAnimating)).Subscribe(OnNextIsAnimating));
            SubscriptionPool.Add(GetObservable<Drawable>(nameof(Background)).Subscribe((next) => ForzeDraw()));

            SubscriptionPool.Add(GetObservable<string>(nameof(Id)).Subscribe((next) => applyStyles()));
            SubscriptionPool.Add(GetObservable<string>(nameof(Class)).Subscribe((next) => applyStyles()));

        }

        private void SetStyleValue(string property, string value)
        {
            var type = this.GetType();
            var propertyInfo = type.GetProperty(property);
            if (propertyInfo == null)
                return;

            var methods = type.GetMethods().Where(m=>m.Name == "SetValue").ToList();

            var setValueMethod = type.GetMethod("SetValue");
            setValueMethod = setValueMethod.MakeGenericMethod(propertyInfo.PropertyType);

            setValueMethod.Invoke(this, new object[] { property, ReactiveObject.USER_VALUE, GetValueFromString(propertyInfo.PropertyType, value) });
        }

        private static object GetValueFromString(Type value, string str)
        {
            if (value.IsAssignableFrom(typeof(string)))
                return str;

            var converter = TypeDescriptor.GetConverter(value);
            return converter.ConvertFromString(str);
        }

        protected virtual void ProcessInput(OpenTK.Input.KeyboardState keyboardState, OpenTK.Input.MouseState mouseState, OpenTK.Input.GamePadState gamePadState, OpenTK.Input.JoystickState joystickState)
        {
            ProcessMouseInput(mouseState);
        }

        private void ProcessMouseInput(OpenTK.Input.MouseState mouseState)
        { 
            var mouseX = mouseState.X - Window.X;
            var mouseY = mouseState.Y - Window.Y - 38;

            if (mouseX >= X && mouseX <= X + CalculatedWidth && mouseY >= Y && mouseY <= Y + CalculatedHeight)
            {                
                if ( mouseState.IsButtonDown(OpenTK.Input.MouseButton.Left) )
                {
                    _wasMouseDown = true;
                    OnMouseDown(new MouseEventArgs(mouseX, mouseY));
                }
                else
                {
                    if (LastMouseState.IsButtonDown(OpenTK.Input.MouseButton.Left))
                        OnMouseUp(new MouseEventArgs(mouseX, mouseY));

                    if (_wasMouseDown)
                    {
                        OnClick(new MouseEventArgs(mouseX, mouseY));
                    }
                    _wasMouseDown = false;
                }
            }
        }

        private void OnMouseDown(MouseEventArgs eventArgs)
        {
            MouseDown?.Invoke(this, eventArgs);
            if (eventArgs.Propagate)
                Parent?.OnMouseDown(eventArgs);
        }

        private void OnMouseUp(MouseEventArgs eventArgs)
        {
            MouseUp?.Invoke(this, eventArgs);
            if (eventArgs.Propagate)
                Parent?.OnMouseUp(eventArgs);
        }

        private void OnClick(MouseEventArgs eventArgs)
        {
            Click?.Invoke(this, eventArgs);
            if (eventArgs.Propagate)
                Parent?.OnClick(eventArgs);
        }

        public void UpdateFrame(OpenTK.Input.KeyboardState keyboardState, OpenTK.Input.MouseState mouseState, OpenTK.Input.GamePadState gamePadState, OpenTK.Input.JoystickState joystickState)
        {
            //process the input and handle events
            ProcessInput(keyboardState, mouseState, gamePadState, joystickState);

            LastGamePadState = gamePadState;
            LastJostickState = joystickState;
            LastKeyBoardState = keyboardState;
            LastMouseState = mouseState;
        }

        private void View_AttachedToWindow(object sender, EventArgs e)
        {
            if (IsAnimating && _animation != null && !_animation.IsStop())
                Window.AddFrameRunner(_animation);
                        
            applyStyles();
        }

        private void applyStyles()
        {
            if (Window == null)
                return;

            IEnumerable<Setter> setters = Window.StyleEngine.GetSetters(this).Concat(Styles);

            if (Parent != null && Parent._setters != null)
                setters.Concat(Parent.Setters.Where(s => s.IsInheritable));

            _setters = setters;
            
            foreach (var setter in setters)
            {
                var property = this.GetType().GetProperty(setter.Property);
                if(property == null)
                {
                    return;
                }

                if (property.PropertyType.IsAssignableFrom(typeof(string)))
                    SetValue<string>(setter.Property, ReactiveObject.STYLE_VALUE, setter.Value);
                else
                {
                    var converter = TypeDescriptor.GetConverter(property.PropertyType);
                    if (converter == null)
                    {
                        return;
                    }
                       
                    if (!converter.CanConvertFrom(typeof(string)))
                    {
                        return;
                    }

                    var value = converter.ConvertFrom(setter.Value);
                    if(value == null)
                    {
                        return;
                    }

                    SetValue<object>(setter.Property, ReactiveObject.STYLE_VALUE, setter.Value);                    
                }
            }
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

                    if(Window != null)
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
            AttachedToWindow -= View_AttachedToWindow;
        }

    }
}
