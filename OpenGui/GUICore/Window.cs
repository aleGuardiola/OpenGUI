using OpenGui.Controls;
using OpenGui.Core;
using OpenGui.OpenGLHelpers;
using OpenTK;
using OpenTK.Graphics.ES20;
using OpenTK.Platform;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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

        private Thread _uiThread;
        private ConcurrentQueue<Action> _actionsQueue;

        private ViewContainer _root;

        private ConcurrentQueue<FrameRunner> newFrameRunners = new ConcurrentQueue<FrameRunner>();
        private List<FrameRunner> runningFrameRunners = new List<FrameRunner>(); 

        public ViewContainer Root
        {
            get => _root;
            set
            {
                if (Thread.CurrentThread != _uiThread)
                    throw new InvalidOperationException("Trying to change property in a different thread.");

                value.AttachWindow(this);
                _root = value;
            }
        }

        public int Width
        {
            get => _gameWindow.Width;
        }
        
        public int Height
        {
            get => _gameWindow.Width;
        }

        public int X
        {
            get => _gameWindow.X;
        }

        public int Y
        {
            get => _gameWindow.Y;
        }

        public void AddFrameRunner(FrameRunner runner)
        {
            newFrameRunners.Enqueue(runner);
        }

        public Thread UiThread
        {
            get => _uiThread;
        }

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
            _gameWindow.UpdateFrame += _gameWindow_UpdateFrame;
            _gameWindow.Resize += _gameWindow_Resize;
            _actionsQueue = new ConcurrentQueue<Action>();
            _uiThread = Thread.CurrentThread;
        }

        private void _gameWindow_UpdateFrame(object sender, FrameEventArgs e)
        {
            _renderManager.Update(e.Time, _gameWindow, _root);
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
            //execute actions on the uiThread
            while(_actionsQueue.Count > 0)
            {
                Action action;
                if (_actionsQueue.TryDequeue(out action))
                    action.Invoke();
            }
            
            //remove finished runners
            runningFrameRunners.Where((r) => r.IsStop()).ToList().ForEach((r) => runningFrameRunners.Remove(r));

            while (newFrameRunners.Count > 0)
            {
                FrameRunner runner;
                if(newFrameRunners.TryDequeue(out runner))
                {
                    runner.Initialize();
                    runningFrameRunners.Add(runner);
                }
            }

            //execute frame runners
            foreach(var runner in runningFrameRunners)            
                runner.Update((float)e.Time);
                   
            //Print deltatime
            Console.Clear();
            Console.WriteLine("{0} x {1}", _gameWindow.Width, _gameWindow.Height);
            Console.WriteLine("FPS: {0}", 1000 / (e.Time * 1000));
            

            //clear screen with the background color
            GL.ClearColor(_backgroundColor);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            _renderManager.RenderFrame(e.Time, _camera.Projection, _camera.View, _gameWindow, Root, _gameWindow.Width, _gameWindow.Height, _depthZ);

            //swap the buffers after everything is rendered
            _gameWindow.SwapBuffers();
        }


        //setup opengl
        private void setupOpenGL()
        {
            //enable depth test for 3d rendering
            //GL.Enable(EnableCap.DepthTest);
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
            viewPortHeight = viewPortHeight == 0 ? 1 : viewPortHeight;
            float fieldOfAngle = 45f;
            float fieldOfAngleRadians = MathHelper.DegreesToRadians(fieldOfAngle);

            _depthZ = viewPortHeight / (2.0f * (float)Math.Tan(fieldOfAngleRadians / 2.0f));

            return Matrix4.CreatePerspectiveFieldOfView(fieldOfAngleRadians, viewPortWidth / viewPortHeight, 0.1f, viewPortHeight * 2.0f);            
        }

        public void RunInUIThread(Action action)
        {
            if(Thread.CurrentThread == _uiThread)
            {
                action.Invoke();
                return;
            }

            _actionsQueue.Enqueue(action);
        }

    }
}
