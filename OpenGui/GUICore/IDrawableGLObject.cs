using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.GUICore
{
    public interface IDrawableGLObject
    {
        /// <summary>
        /// Draw the object using opengl
        /// </summary>
        /// <param name="projectionMatrix">the projection matrix using</param>
        void GLDraw(Matrix4 perspectiveProjection, Matrix4 view);
    }
}
