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
    public class LowLevelView : IDrawableGLObject
    {
        Guid _uniqueId;
        ViewTransformableObject _transformableObject;
        IModelProvider modelProvider;
        Texture _texture;

        float _x;
        float _y;
        float _z;

        float _width;
        float _height;
        float _depth;

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
        public float X { get => _x; set => _x = value; }

        /// <summary>
        /// Y position of the view.
        /// </summary>
        public float Y { get => _y; set => _y = value; }

        /// <summary>
        /// get or set the Z position of the view.
        /// </summary>
        public float Z { get => _z; set => _z = value; }

        /// <summary>
        /// get or set the Width of the view.
        /// </summary>
        public float Width { get => _width; set => _width = value; }

        /// <summary>
        /// get or set th height of the view
        /// </summary>
        public float Height { get => _height; set => _height = value; }

        /// <summary>
        /// get or set the tall of the view
        /// </summary>
        public float Depth { get => _depth; set => _depth = value; }

        public LowLevelView()
        {
            modelProvider = GetModelProvider();
            _texture = new Texture();
            _transformableObject = new ViewTransformableObject(modelProvider);
            _uniqueId = Guid.NewGuid();
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
        public virtual void Initialize(int maxWidth, int maxHeight)
        {
            
        }

        /// <summary>
        /// Draw using open gl context
        /// </summary>
        /// <param name="perspectiveProjection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        public virtual void GLDraw(Matrix4 perspectiveProjection, Matrix4 view, RectangleF clipRectangle, int windowWidth, int windowHeight)
        {
            var x = (_x - (windowWidth/2)) + (_width/2.0f);
            var y = (_y + (windowHeight/2)) - (_height/2.0f);

            var model = Matrix4.Identity;
            model *= Matrix4.CreateScale(_width/2.0f, _height/2.0f, _depth/2.0f);
            //model *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(45)); 
            model *= Matrix4.CreateTranslation(x, y, _z);

            _transformableObject.TransformationMatrix = model;

            _transformableObject.ClipRectangle = clipRectangle;

            GLDraw(perspectiveProjection, view);
        }

        /// <summary>
        /// Draw the texture to show in this view
        /// </summary>
        /// <param name="canvas">The canvas to draw the texture</param>
        protected virtual void DrawTexture(SKCanvas canvas, int width, int height)
        {
            canvas.Clear(SKColors.Transparent);
        }

        public void GLDraw(Matrix4 perspectiveProjection, Matrix4 view)
        {
            var texture = GetTexture();

            texture.StartUsingTexture();
            _transformableObject.Draw(perspectiveProjection, view);
            texture.StopUsingTexture();
        }

        //return the texture to draw
        private Texture GetTexture()
        {
            if(!_texture.IsLoaded)
            {
                _texture.LoadTexture(GetBitmap());
            }

            return _texture;
        }

        //return the bitmap used by the texture
        private SKBitmap GetBitmap()
        {
            var bitmap = new SKBitmap(new SKImageInfo((int)_width, (int)_height, SKColorType.Rgba8888));
            using (var canvas = new SKCanvas(bitmap))
            {
                DrawTexture(canvas, bitmap.Width, bitmap.Height);
            }
            return bitmap;
        }

    }
}
