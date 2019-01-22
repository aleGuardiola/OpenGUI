using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.GUICore
{
    /// <summary>
    /// Provide vertex data to draw a model
    /// </summary>
    public interface IModelProvider
    {
        /// <summary>
        /// Draw the model using GL
        /// </summary>
        void Draw();
    }
}
