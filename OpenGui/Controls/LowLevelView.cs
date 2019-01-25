using OpenGui.Core;
using OpenGui.GUICore;
using OpenTK;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Controls
{
    /// <summary>
    /// The most low level block view.
    /// </summary>
    public class LowLevelView : ReactiveObject, IDrawableGLObject
    {
        Guid _uniqueId;
        ViewTransformableObject _transformableObject;
        IModelProvider modelProvider;
        Texture _texture;
                
        //last rendered values
        bool forzeDraw = false;
        float _lastWidth;
        float _lastHeight;
        float _lastDepth;

        float _lastZ;
        
        /// <summary>
        /// Get or set the width used to draw, if not set Width property will be used
        /// </summary>
        protected float CalculatedWidth = float.NegativeInfinity;

        /// <summary>
        /// Get or set the height used to draw, if not set Height property will be used
        /// </summary>
        protected float CalculatedHeight = float.NegativeInfinity;

        /// <summary>
        /// Unique id that identify this instance
        /// </summary>
        public Guid UniqueId
        {
            get => _uniqueId;
        }

        /// <summary>
        /// get or set the X position of the view.
        /// </summary>
        public float X { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// Y position of the view.
        /// </summary>
        public float Y { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// get or set the Z position of the view.
        /// </summary>
        public float Z { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// get or set the Width of the view.
        /// </summary>
        public float Width { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// get or set th height of the view
        /// </summary>
        public float Height { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// get or set the tall of the view
        /// </summary>
        public float Depth { get => GetValue<float>(); set => SetValue<float>(value); }

        public LowLevelView()
        {
            modelProvider = GetModelProvider();
            _texture = new Texture();
            _transformableObject = new ViewTransformableObject(modelProvider);
            _uniqueId = Guid.NewGuid();

            SetValue<float>(nameof(Width), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(Height), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(Depth), ReactiveObject.LAYOUT_VALUE, 0);

            SetValue<float>(nameof(X), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(Y), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(Z), ReactiveObject.LAYOUT_VALUE, 0);
        }

        /// <summary>
        /// Provide the model provider that provide the model used by this view
        /// </summary>
        /// <returns></returns>
        protected virtual IModelProvider GetModelProvider()
        {
            return new ViewModelProvider();
        }

        /// <summary>
        /// Call before draw.
        /// </summary>
        public virtual void Initialize(float maxWidth, float maxHeight, float parentX, float parentY)
        {
            
        }

        private float getWidthToRender()
        {
            return CalculatedWidth != float.NegativeInfinity ? CalculatedWidth : Width;
        }

        private float getHeightToRender()
        {
            return CalculatedHeight != float.NegativeInfinity ? CalculatedHeight : Height;
        }

        /// <summary>
        /// Draw using open gl context
        /// </summary>
        /// <param name="perspectiveProjection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        public virtual void GLDraw(Matrix4 perspectiveProjection, Matrix4 view, RectangleF clipRectangle, int windowWidth, int windowHeight)
        {
            var viewWidth = getWidthToRender();
            var viewHeight = getHeightToRender();
            var viewx = X;
            var viewy = -Y;
            var viewz = Z;
            var viewDepth = Depth;

            var x = (viewx - (windowWidth/2)) + (viewWidth/2.0f);
            var y = (viewy + (windowHeight/2) ) - (viewHeight/2.0f);

            var model = Matrix4.Identity;
            model *= Matrix4.CreateScale(viewWidth/2.0f, viewHeight/2.0f, viewDepth/2.0f);
            //model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)); 
            model *= Matrix4.CreateTranslation(x, y, viewz);

            _transformableObject.TransformationMatrix = model;

            _transformableObject.ClipRectangle = clipRectangle;

            GLDraw(perspectiveProjection, view);
        }

        protected void ForzeDraw()
        {
            forzeDraw = true;
        }

        /// <summary>
        /// Draw the texture to show in this view
        /// </summary>
        /// <param name="canvas">The canvas to draw the texture</param>
        protected virtual void DrawTexture(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(SKColors.White);
        }

        public void GLDraw(Matrix4 perspectiveProjection, Matrix4 view)
        {
            var texture = GetTexture(getWidthToRender(), getHeightToRender());

            texture.StartUsingTexture();
            _transformableObject.Draw(perspectiveProjection, view);
            texture.StopUsingTexture();
        }

        //return the texture to draw
        private Texture GetTexture(float _width, float _height)
        {            
            if(!_texture.IsLoaded)
            {
                _texture.LoadTexture(GetBitmap(_width, _height));
            }
            else
            {
                //if is forzed to draw with same size
                if(forzeDraw && _width == _lastWidth && _height == _lastHeight)
                {
                    _texture.ChangeBitmapSameSize(GetBitmap(_width, _height));
                }
                else
                //check if we have to update the bitmap
                if(_width > _lastWidth || _height > _lastHeight)
                {
                    _texture.ChangeBitmap(GetBitmap(_width, _height));
                }
                else
                {
                    var newAspectRatio = _width / _height;
                    var lastAspectRatio = _lastWidth / _lastHeight;
                    if(newAspectRatio != lastAspectRatio)
                    {
                        _texture.ChangeBitmap(GetBitmap(_width, _height));
                    }
                }
            }

            _lastWidth = _width;
            _lastHeight = _height;
            forzeDraw = false;

            return _texture;
        }

        //return the bitmap used by the texture
        private SKBitmap GetBitmap(float _width, float _height)
        {
            var bitmap = new SKBitmap(new SKImageInfo((int)_width, (int)_height, SKColorType.Rgba8888, SKAlphaType.Unpremul));
            using (var canvas = new SKCanvas(bitmap))
            {                
                DrawTexture(canvas, bitmap.Width, bitmap.Height);
            }
            return bitmap;
        }

    }
}
