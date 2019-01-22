using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Exceptions
{
    public class UniformLocationNotFound : OpenGLException
    {
        public UniformLocationNotFound(string name, int programId) : base($"The uniform value {name} cannot be found in program with id {programId}")
        {

        }
    }
}
