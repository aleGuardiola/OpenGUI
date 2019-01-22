using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Exceptions
{
    public class CantCreateProgramException : OpenGLException
    {
        public CantCreateProgramException() : base("Error creating shader program.")
        {

        }
    }
}
