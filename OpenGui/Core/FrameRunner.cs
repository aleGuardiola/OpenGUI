using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Core
{
    public abstract class FrameRunner
    {
        public abstract bool Stoped();
        public abstract void Update(float deltaTime);
    }
}
