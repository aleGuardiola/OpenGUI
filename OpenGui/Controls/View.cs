using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Core;
using OpenGui.Graphics;
using OpenGui.Helpers;
using OpenGui.Values;
using OpenTK;
using SkiaSharp;

namespace OpenGui.Controls
{
    public class View : LowLevelView
    {
        string _id;
        string _class;
        SubscriptionPool _subscriptionPool;

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

        public View()
        {
            _subscriptionPool = new SubscriptionPool();
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

            _subscriptionPool.Add(GetObservable<float>(nameof(Width)).Subscribe((v) => Parent?.ForceMeasure()));
            _subscriptionPool.Add(GetObservable<float>(nameof(Height)).Subscribe((v) => Parent?.ForceMeasure()));
            _subscriptionPool.Add(GetObservable<HorizontalAligment>(nameof(HorizontalAligment)).Subscribe((v) => Parent?.ForceMeasure()));
            _subscriptionPool.Add(GetObservable<VerticalAligment>(nameof(VerticalAligment)).Subscribe((v) => Parent?.ForceMeasure()));
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
            _subscriptionPool.UnsubscribeAll();
        }

    }
}
