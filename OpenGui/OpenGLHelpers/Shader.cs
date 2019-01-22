using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Exceptions;
using OpenTK.Graphics.OpenGL;

namespace OpenGui.OpenGLHelpers
{
    /// <summary>
    /// Represents a shader.
    /// </summary>
    public abstract class Shader
    {
        int shaderId;
        ShaderType type;

        /// <summary>
        /// Id of the shader assigned from OpenGl
        /// </summary>
        public int Id
        {
            get => shaderId;
        }

        /// <summary>
        /// Get the type of the shader
        /// </summary>
        public ShaderType Type { get => type; }

        /// <summary>
        /// Create the shader and compile the code using the code and the type.
        /// </summary>
        /// <param name="code">The code of the shader.</param>
        /// <param name="type">The type of the shader</param>
        protected Shader(string code, ShaderType type)
        {
            //Create the shader and get the id
            shaderId = GL.CreateShader(type);

            //assign the code of the shader
            GL.ShaderSource(shaderId, code);
            //compile the shader
            GL.CompileShader(shaderId);

            //Get compilation status
            int compileStatusResult;
            GL.GetShader(shaderId, ShaderParameter.CompileStatus, out compileStatusResult);

            //error compiling shader
            if(compileStatusResult == 0)
            {
                string infoLog;
                GL.GetShaderInfoLog(shaderId, out infoLog);
                throw new ShaderCompilationException(shaderId, infoLog);
            }
            
        }
    }
}
