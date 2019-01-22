using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.OpenGLHelpers
{
    public class Camera
    {
        Vector3 _position;
        Vector3 _up;
        Vector3 _target;
        Vector3 _cameraUp;
        Matrix4 _viewMatrix;

        /// <summary>
        /// Get or Set the position of the camera
        /// </summary>
        public Vector3 Position
        {
            get => _position;
            set { _position = value; update(); }
        }
        
        /// <summary>
        /// Get or set the world up of the camera
        /// </summary>
        public Vector3 Up
        {
            get => _up;
            set {  _up = value; update(); }
        }
        
        /// <summary>
        /// Get or Set the target to wich the camera is pointing to
        /// </summary>
        public Vector3 Target
        {
            get => _target;
            set { _target = value; update(); }
        }

        /// <summary>
        /// Get or Set the projection matrix 
        /// </summary>
        public Matrix4 Projection { get; set; }

        /// <summary>
        /// Create a new Camera that can provide a view matrix
        /// </summary>
        /// <param name="position">Start position of the camera.</param>
        /// <param name="up">Start Up world.</param>
        /// <param name="target">Start target to wich the camera is pointing to.</param>
        /// <param name="projection">The projection used by the camera</param>
        public Camera(Vector3 position, Vector3 up, Vector3 target, Matrix4 projection)
        {
            _position = position;
            _up = up;
            _target = target;
            Projection = projection;
            update();
        }

        void update()
        {
             var direction = Vector3.Normalize(_position - _target);
             var right = Vector3.Normalize(Vector3.Cross(_up, direction));
             _cameraUp = Vector3.Cross(direction, right);
             _viewMatrix = Matrix4.LookAt(_position, _target, _cameraUp);
        }       

        /// <summary>
        /// Get the view matrix
        /// </summary>
        public Matrix4 View
        {
            get => _viewMatrix;
        }

    }
}
