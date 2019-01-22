using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.OpenGLHelpers
{
    public class VertexShader : Shader
    {
        /// <summary>
        /// Represent a vertex shader.
        /// </summary>
        /// <param name="code">The code of the vertex shader</param>
        public VertexShader(string code) : base(code, OpenTK.Graphics.OpenGL.ShaderType.VertexShader)
        {
           
        }
    }
}
