using System;
using System.Collections.Generic;
using System.Text;
using ExCSS;
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

        public View()
        {
            
        }

        public override void Initialize(int maxWidth, int maxHeight)
        {
           
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
