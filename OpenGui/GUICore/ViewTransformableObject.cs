using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.ES30;

namespace OpenGui.GUICore
{
    /// <summary>
    /// Transformable object used by views
    /// </summary>
    public class ViewTransformableObject : TransformableObject
    {
        IModelProvider _modelProvider;

        public ViewTransformableObject(IModelProvider modelProvider)
        {
            _modelProvider = modelProvider;            
        }

        protected override void DrawVertex()
        {
            _modelProvider.Draw();
        }
    }
}
