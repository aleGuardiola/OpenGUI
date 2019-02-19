using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Core
{
    public class ClickEventArgs : EventArgs
    {
        /// <summary>
        /// The X position of the pointer.
        /// </summary>
        public float X { get; private set; }
        /// <summary>
        /// The Y position of the pointer.
        /// </summary>
        public float Y { get; private set; }
        /// <summary>
        /// Indicates if this event should propagate back.
        /// </summary>
        public bool Propagate = true;

        public ClickEventArgs(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}
