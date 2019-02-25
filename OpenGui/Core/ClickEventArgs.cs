using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Core
{
    public class MouseEventArgs : InputHandlerEventArgs
    {
        /// <summary>
        /// The X position of the pointer.
        /// </summary>
        public float X { get; private set; }
        /// <summary>
        /// The Y position of the pointer.
        /// </summary>
        public float Y { get; private set; }
        
        public MouseEventArgs(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
