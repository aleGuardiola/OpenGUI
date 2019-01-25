using System;
using System.Collections.Generic;
using System.Text;
using ExCSS;
using OpenGui.Core;
using OpenGui.Graphics;
using OpenGui.Values;
using OpenTK;
using SkiaSharp;

namespace OpenGui.Controls
{
    public class View : LowLevelView
    {
        string _id;
        string _class;
        
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

        /// <summary>
        /// Get or Set the background of the view.
        /// </summary>
        public Drawable Background
        {
            get => GetValue<Drawable>();
            set => SetValue<Drawable>(value);
        }

        /// <summary>
        /// Get or Set the aligment of the view
        /// </summary>
        public Align Align
        {
            get => GetValue<Align>();
            set => SetValue<Align>(value);
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

        public View()
        {
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

            SetValue<Align>(nameof(Align), ReactiveObject.LAYOUT_VALUE, Align.Center);
        }

        public override void Initialize(float maxWidth, float maxHeight, float parentX, float parentY)
        {            
            //set x and y based on the relative position
            var x = parentX + RelativeX;
            var y = parentY + RelativeY;
            SetValue<float>(nameof(X), ReactiveObject.LAYOUT_VALUE, x);
            SetValue<float>(nameof(Y), ReactiveObject.LAYOUT_VALUE, y);
        }

        protected override void DrawTexture(SKCanvas canvas, int width, int height)
        {
            if (Background == null)
                base.DrawTexture(canvas, width, height);
            else
                Background.Draw(width, height, canvas);
        }

    }
}
