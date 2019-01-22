using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Exceptions
{
    public class ShaderCompilationException : OpenGLException
    {
        public ShaderCompilationException(int shaderId, string infoLog) : base($"Error compiling shader {shaderId} \"{infoLog}\"")
        {

        }
    }
}
