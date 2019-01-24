using System;
using System.Collections.Generic;
using System.Text;
using ExCSS;
using OpenGui.Core;
using OpenGui.Graphics;
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
        }

        public override void Initialize(float maxWidth, float maxHeight, float parentX, float parentY)
        {
            //calculate relative position value based on margin
            var actualRelativeX = GetValue<float>(nameof(RelativeX), ReactiveObject.LAYOUT_VALUE);
            var actualRelativeY = GetValue<float>(nameof(RelativeY), ReactiveObject.LAYOUT_VALUE);

            var distanceLeft = actualRelativeX;
            var distanceRight = (parentX + maxWidth) - (actualRelativeX + Width);
            var distanceTop = actualRelativeY;
            var distanceBottom = (parentY + maxHeight) - (actualRelativeY + Height);

            var marginTopToAdd = MarginTop - distanceTop;
            var marginRightToAdd = MarginRight - distanceRight;
            var marginBottomToAdd = MarginBottom - distanceBottom;
            var marginLeftToAdd = MarginLeft - distanceLeft;
            marginLeftToAdd = marginLeftToAdd < 0 ? 0 : marginLeftToAdd;
            marginRightToAdd = marginRightToAdd < 0 ? 0 : marginRightToAdd;
            marginTopToAdd = marginTopToAdd < 0 ? 0 : marginTopToAdd;
            marginBottomToAdd = marginBottomToAdd < 0 ? 0 : marginBottomToAdd;

            actualRelativeX += marginLeftToAdd;
            actualRelativeX -= marginRightToAdd;
            actualRelativeY += marginTopToAdd;
            actualRelativeY -= marginBottomToAdd;

            SetValue<float>(nameof(RelativeX), ReactiveObject.LAYOUT_VALUE, actualRelativeX);
            SetValue<float>(nameof(RelativeY), ReactiveObject.LAYOUT_VALUE, actualRelativeY);

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
