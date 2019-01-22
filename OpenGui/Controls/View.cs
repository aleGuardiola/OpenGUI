using System;
using System.Collections.Generic;
using System.Text;
using ExCSS;
using OpenTK;
using SkiaSharp;

namespace OpenGui.Controls
{
    public class View : LowLevelView
    {
        string _id;
        string _class;
        StyleDeclaration _cssStyle;

        public StyleDeclaration Style
        {
            get => _cssStyle;
        }

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

        public View()
        {
            
        }

        public override void Initialize(int maxWidth, int maxHeight)
        {
           
        }

        protected override void DrawTexture(SKCanvas canvas, int width, int height)
        {
            base.DrawTexture(canvas, width, height);
        }

    }
}
