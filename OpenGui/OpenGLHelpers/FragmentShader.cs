using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.OpenGLHelpers
{
    public class FragmentShader : Shader
    {
        /// <summary>
        /// Represent a fragment shader.
        /// </summary>
        /// <param name="code">The code of the fragment shader.</param>
        public FragmentShader(string code) : base(code, OpenTK.Graphics.OpenGL.ShaderType.FragmentShader)
        {

        }
    }
}
