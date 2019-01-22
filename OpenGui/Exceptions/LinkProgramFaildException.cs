using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Exceptions
{
    public class LinkProgramFaildException : OpenGLException
    {
        public LinkProgramFaildException(int programId, string infoLog) : base($"Error linkin program {programId}: \"{infoLog}\"")
        {

        }
    }
}
