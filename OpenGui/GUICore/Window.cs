using OpenGui.Controls;
using OpenGui.OpenGLHelpers;
using OpenTK;
using OpenTK.Graphics.ES20;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.GUICore
{
    public class Window
    {
        //Camera default position: x = 0, y = 0, z = 100
        static Vector3 CameraDefaultPosition = new Vector3(0.0f, 0.0f, 0.0f);
        //World up used for the camera
        static Vector3 CameraWorldUp = Vector3.UnitY;
        //Center of the world where the ui is rendered
        static Vector3 WorldCenter = Vector3.Zero;

        private InputManager _inputManager;
        //background color
        private Color _backgroundColor;
        //camera used;
        private Camera _camera;
        //Render manager that render all views
        private IUIRenderManager _renderManager;

        //z position where objects dimensions are seen as pixels in the screen
        private float _depthZ;

        public ViewContainer Root { get; set; }

        /// <summary>
        /// Get the camera used.
        /// </summary>
        public Camera Camera
        {
            get => _camera;
        }

        /// <summary>
        /// Background color of the window
        /// </summary>
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        IGameWindow _gameWindow;
        public Window(IGameWindow gameWindow)
        {
            //handle all events
            _gameWindow = gameWindow;
            _gameWindow.Load += _gameWindow_Load;
            _gameWindow.RenderFrame += _gameWindow_RenderFrame;
            _gameWindow.Resize += _gameWindow_Resize;            
        }

        //this function is called when the window finish load
        private void _gameWindow_Load(object sender, EventArgs e)
        {
            //configurations in opengl
            setupOpenGL();

            //change the openGL viewport
            GL.Viewport(0, 0, _gameWindow.Width, _gameWindow.Height);

            //Setup camera
            setupCamera(_gameWindow.ClientRectangle);

            //initialize the render manager with the default one
            _renderManager = new DefaultUiRenderer();
        }
        
        //called when the window resize
        private void _gameWindow_Resize(object sender, EventArgs e)
        {
            var rect = _gameWindow.ClientRectangle;
            //change the openGL viewport
            GL.Viewport(rect);

            //setup the camera
            setupCamera(rect);
        }

        //this function is called every time a frame has to be draw
        private void _gameWindow_RenderFrame(object sender, FrameEventArgs e)
        {
            //Print deltatime
            Console.Clear();
            Console.WriteLine("{0} x {1}", _gameWindow.Width, _gameWindow.Height);
            Console.WriteLine("FPS: {0}", 1000 / (e.Time * 1000));
            

            //clear screen with the background color
            GL.ClearColor(_backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _renderManager.RenderFrame(e.Time, _camera.Projection, _camera.View, _inputManager, Root, _gameWindow.Width, _gameWindow.Height);

            //swap the buffers after everything is rendered
            _gameWindow.SwapBuffers();
        }


        //setup opengl
        private void setupOpenGL()
        {
            //enable depth test for 3d rendering
            GL.Enable(EnableCap.DepthTest);
            //enable blend for alpha texture
            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
        }

        //Setup the camera
        private void setupCamera(Rectangle rect)
        {
            var projection = GetProjection(rect);
            var position = new Vector3(0.0f, 0.0f, _depthZ);

            _camera = new Camera(position, CameraWorldUp, WorldCenter, projection);
        }

        //Get the projection of the world
        private Matrix4 GetProjection(Rectangle rect)
        {
            float viewPortWidth = (float)rect.Width;
            float viewPortHeight = (float)rect.Height;
            float fieldOfAngle = 45f;
            float fieldOfAngleRadians = MathHelper.DegreesToRadians(fieldOfAngle);

            _depthZ = viewPortHeight / (2.0f * (float)Math.Tan(fieldOfAngleRadians / 2.0f));

            return Matrix4.CreatePerspectiveFieldOfView(fieldOfAngleRadians, viewPortWidth / viewPortHeight, 0.1f, viewPortHeight * 2.0f);            
        }

    }
}
