using OpenGui.Core;
using OpenGui.GUICore;
using OpenGui.Values;
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
        
        public Window Window
        {
            get;
            private set;
        }

        public bool IsAttachedToWindows
        {
            get => GetValue<bool>();
            private set => SetValue<bool>(value);
        }

        /// <summary>
        /// Get or set the width used to draw, if not set Width property will be used
        /// </summary>
        public float CalculatedWidth
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or set the height used to draw, if not set Height property will be used
        /// </summary>
        public float CalculatedHeight
        {
            get;
            private set;
        }

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
        /// get or set the height of the view
        /// </summary>
        public float Height { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// get or set the minimum width of the view.
        /// </summary>
        public float MinWidth { get => GetValue<float>(); set => SetValue<float>(value); }
        
        /// <summary>
        /// get or set the minimum height of the view.
        /// </summary>
        public float MinHeight { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// get or set the tall of the view
        /// </summary>
        public float Depth { get => GetValue<float>(); set => SetValue<float>(value); }

        public event EventHandler AttachedToWindow;

        public LowLevelView()
        {
            IsAttachedToWindows = false;
            modelProvider = GetModelProvider();
            _texture = new Texture();
            _transformableObject = new ViewTransformableObject(modelProvider);
            _uniqueId = Guid.NewGuid();

            SetValue<float>(nameof(Width), ReactiveObject.LAYOUT_VALUE, (int)WidthOptions.Auto);
            SetValue<float>(nameof(Height), ReactiveObject.LAYOUT_VALUE, (int)HeightOptions.Auto);
            SetValue<float>(nameof(Depth), ReactiveObject.LAYOUT_VALUE, 1);
            SetValue<float>(nameof(MinWidth), ReactiveObject.LAYOUT_VALUE, 25);
            SetValue<float>(nameof(MinHeight), ReactiveObject.LAYOUT_VALUE, 25);
            
            SetValue<float>(nameof(X), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(Y), ReactiveObject.LAYOUT_VALUE, 0);
            SetValue<float>(nameof(Z), ReactiveObject.LAYOUT_VALUE, 0);
        }

        public virtual void AttachWindow(Window window)
        {
            CheckThread();
            Window = window;
            IsAttachedToWindows = true;
            AttachedToWindow?.Invoke(this, new EventArgs());
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
        /// Calculate the size of the view.
        /// </summary>
        /// <param name="width">the width specification.</param>
        /// <param name="height">the height specification.</param>
        /// <param name="mode">mode specification.</param>
        /// <returns>return the size measured to render this view.</returns>
        protected virtual (float measuredWidth, float measuredHeight) OnMesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            return (0, 0);
        }

        /// <summary>
        /// Calculate the size of the view.
        /// </summary>
        /// <param name="width">the width specification.</param>
        /// <param name="height">the height specification.</param>
        /// <param name="mode">mode specification.</param>        
        public void Mesure(float widthSpec, float heightSpec, MeasureSpecMode mode)
        {
            var size = OnMesure(widthSpec, heightSpec, mode);
            SetMeasuredSize(size.measuredWidth, size.measuredHeight);
        }

        /// <summary>
        /// Set the measured size of this view.
        /// </summary>
        /// <param name="width">The width to render.</param>
        /// <param name="height">The height to render</param>
        private void SetMeasuredSize(float width, float height)
        {
            CalculatedWidth = width;
            CalculatedHeight = height;
        }

        /// <summary>
        /// Draw using open gl context
        /// </summary>
        /// <param name="perspectiveProjection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        public virtual void GLDraw(Matrix4 perspectiveProjection, Matrix4 view, RectangleF clipRectangle, int windowWidth, int windowHeight, float cameraZ)
        {
            var viewWidth = CalculatedWidth;
            var viewHeight = CalculatedHeight;
            var viewx = X;
            var viewy = -Y;
            var viewz = Z;
            var viewDepth = Depth;

            var x = (viewx - (windowWidth/2)) + (viewWidth/2.0f);
            var y = (viewy + (windowHeight/2) ) - (viewHeight/2.0f);
            var z = (viewz * cameraZ) / 100;

            var model = Matrix4.Identity;
            model *= Matrix4.CreateScale(viewWidth/2.0f, viewHeight/2.0f, viewDepth/2.0f);
            //model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)); 
            model *= Matrix4.CreateTranslation(x, y, z);

            _transformableObject.TransformationMatrix = model;

            _transformableObject.ClipRectangle = clipRectangle;

            GLDraw(perspectiveProjection, view);
        }

        protected void ForzeDraw()
        {
            CheckThread();
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
            var texture = GetTexture(CalculatedWidth, CalculatedHeight);

            texture.StartUsingTexture();
            _transformableObject.Draw(perspectiveProjection, view);
            texture.StopUsingTexture();
        }

        //return the texture to draw
        private Texture GetTexture(float _width, float _height)
        {
            if (!_texture.IsLoaded)
            {
                using (var bitmap = GetBitmap(_width, _height))
                    _texture.LoadTexture(bitmap);
            }
            else
            {
                //if is forzed to draw with same size
                if (forzeDraw && _width == _lastWidth && _height == _lastHeight)
                {
                    using (var bitmap = GetBitmap(_width, _height))
                        _texture.ChangeBitmapSameSize(bitmap);                        
                }
                else
                //check if we have to update the bitmap
                if (_width > _lastWidth || _height > _lastHeight)
                {
                    using (var bitmap = GetBitmap(_width, _height))
                        _texture.ChangeBitmap(bitmap);
                }
                else
                {
                    var newAspectRatio = _width / _height;
                    var lastAspectRatio = _lastWidth / _lastHeight;
                    if (newAspectRatio != lastAspectRatio)
                    {
                        using (var bitmap = GetBitmap(_width, _height))
                            _texture.ChangeBitmap(bitmap);
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
            var bitmap = new SKBitmap(new SKImageInfo((int)_width, (int)_height, SKColorType.Rgba8888));
            using (var canvas = new SKCanvas(bitmap))
            {                
                DrawTexture(canvas, bitmap.Width, bitmap.Height);                
            }

            return bitmap;
        }

        ~LowLevelView()
        {
            if(_texture.IsLoaded)
              _texture.Dispose();
        }

    }
}
