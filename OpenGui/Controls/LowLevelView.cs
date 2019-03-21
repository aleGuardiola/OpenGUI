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
            get => GetValue<float>();
            set => SetValue<float>(value);
        }

        /// <summary>
        /// Get or set the height used to draw, if not set Height property will be used
        /// </summary>
        public float CalculatedHeight
        {
            get => GetValue<float>();
            set => SetValue<float>(value);
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
        /// get or set the rotation of the view.
        /// </summary>
        public float Rotation { get => GetValue<float>(); set => SetValue<float>(value); }


        /// <summary>
        /// get or set the height of the view
        /// </summary>
        public float ScaleX { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// get or set the rotation of the view.
        /// </summary>
        public float ScaleY { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// get or set the rotationY of the view.
        /// </summary>
        public float RotationY { get => GetValue<float>(); set => SetValue<float>(value); }

        /// <summary>
        /// get or set the rotationZ of the view.
        /// </summary>
        public float RotationZ { get => GetValue<float>(); set => SetValue<float>(value); }

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

            SetValue<float>(nameof(Width), ReactiveObject.SYSTEM_VALUE, (int)WidthOptions.Auto);
            SetValue<float>(nameof(Height), ReactiveObject.SYSTEM_VALUE, (int)HeightOptions.Auto);
            SetValue<float>(nameof(Depth), ReactiveObject.SYSTEM_VALUE, 1);
            SetValue<float>(nameof(MinWidth), ReactiveObject.SYSTEM_VALUE, 25);
            SetValue<float>(nameof(MinHeight), ReactiveObject.SYSTEM_VALUE, 25);

            SetValue<float>(nameof(X), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(Y), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(Z), ReactiveObject.SYSTEM_VALUE, 0);

            SetValue<float>(nameof(Rotation), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(RotationY), ReactiveObject.SYSTEM_VALUE, 0);
            SetValue<float>(nameof(RotationZ), ReactiveObject.SYSTEM_VALUE, 0);

            SetValue<float>(nameof(ScaleX), ReactiveObject.SYSTEM_VALUE, 1f);
            SetValue<float>(nameof(ScaleY), ReactiveObject.SYSTEM_VALUE, 1f);
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
            SetValue<float>(nameof(CalculatedWidth), ReactiveObject.LAYOUT_VALUE, width);
            SetValue<float>(nameof(CalculatedHeight), ReactiveObject.LAYOUT_VALUE, height);            
        }

        /// <summary>
        /// Draw using open gl context
        /// </summary>
        /// <param name="perspectiveProjection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        public virtual void GLDraw(Matrix4 perspectiveProjection, Matrix4 view, RectangleF clipRectangle, int windowWidth, int windowHeight, float cameraZ, SKBitmap cachedBitmap = null)
        {
            var viewWidth = CalculatedWidth;
            var viewHeight = CalculatedHeight;
            var viewx = X;
            var viewy = -Y;
            var viewz = Z;
            var viewDepth = Depth;

            var x = (viewx - (windowWidth / 2)) + (viewWidth / 2.0f);
            var y = (viewy + (windowHeight / 2)) - (viewHeight / 2.0f);
            var z = (viewz * cameraZ) / 100;                       

            var model = Matrix4.Identity;
            model *= Matrix4.CreateScale((viewWidth / 2.0f) * ScaleX, (viewHeight / 2.0f) * ScaleY, (viewDepth / 2.0f));
            model *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Rotation));
            model *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(RotationY));
            model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(RotationZ));
            model *= Matrix4.CreateTranslation(x, y, z);

            _transformableObject.TransformationMatrix = model;

            _transformableObject.ClipRectangle = clipRectangle;

            GLDraw(perspectiveProjection, view, cachedBitmap);
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

        public void GLDraw(Matrix4 perspectiveProjection, Matrix4 view, SKBitmap cachedBitmap = null)
        {
            var texture = GetTexture(CalculatedWidth, CalculatedHeight, cachedBitmap);

            texture.StartUsingTexture();
            _transformableObject.Draw(perspectiveProjection, view);
            texture.StopUsingTexture();
        }

        public void GLDraw(Matrix4 perspectiveProjection, Matrix4 view)
        {
            var texture = GetTexture(CalculatedWidth, CalculatedHeight);

            texture.StartUsingTexture();
            _transformableObject.Draw(perspectiveProjection, view);
            texture.StopUsingTexture();

        }

        //return the texture to draw
        private Texture GetTexture(float width, float height, SKBitmap cachedBitmap = null)
        {
            if (!_texture.IsLoaded)
            {
                if(cachedBitmap == null)
                {
                    using (var bitmap = GetBitmap(width, height))
                        _texture.LoadTexture(bitmap, (int)width, (int)height);
                }
                else
                {
                    var bitmap = GetBitmap(width, height, cachedBitmap);
                    _texture.LoadTexture(bitmap, (int)width, (int)height);
                }
            }
            else
            {
                //if is forzed to draw with same size
                if (forzeDraw && width == _lastWidth && height == _lastHeight)
                {
                    if (cachedBitmap == null)
                    {
                        using (var bitmap = GetBitmap(width, height))
                            _texture.ChangeBitmapSameSize(bitmap, (int)width, (int)height);
                    }
                    else
                    {
                        var bitmap = GetBitmap(width, height, cachedBitmap);
                        _texture.ChangeBitmapSameSize(bitmap, (int)width, (int)height);
                    }
                }
                else
                //check if we have to update the bitmap
                if (width > _lastWidth || height > _lastHeight)
                {
                    if (cachedBitmap == null)
                    {
                        using (var bitmap = GetBitmap(width, height))
                            _texture.ChangeBitmap(bitmap, (int)width, (int)height);
                    }
                    else
                    {
                        var bitmap = GetBitmap(width, height, cachedBitmap);
                        _texture.ChangeBitmap(bitmap, (int)width, (int)height);
                    }
                }
                else
                {
                    var newAspectRatio = width / height;
                    var lastAspectRatio = _lastWidth / _lastHeight;
                    if (newAspectRatio != lastAspectRatio)
                    {
                        if (cachedBitmap == null)
                        {
                            using (var bitmap = GetBitmap(width, height))
                                _texture.ChangeBitmap(bitmap, (int)width, (int)height);
                        }
                        else
                        {
                            var bitmap = GetBitmap(width, height, cachedBitmap);
                            _texture.ChangeBitmap(bitmap, (int)width, (int)height);
                        }
                    }
                }
            }

            _lastWidth = width;
            _lastHeight = height;
            forzeDraw = false;

            return _texture;
        }

        //return the bitmap used by the texture
        private SKBitmap GetBitmap(float _width, float _height, SKBitmap cachedBitmap = null)
        {
            var bitmap = cachedBitmap ?? new SKBitmap(new SKImageInfo((int)_width, (int)_height, SKColorType.Rgba8888));
            
            using (var canvas = new SKCanvas(bitmap))
            {
                DrawTexture(canvas, bitmap.Width, bitmap.Height);
            }

            return bitmap;
        }

        ~LowLevelView()
        {
            if (_texture.IsLoaded)
                _texture.Dispose();
        }

    }
}
