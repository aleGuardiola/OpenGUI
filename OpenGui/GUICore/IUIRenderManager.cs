using OpenGui.Controls;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.GUICore
{
    public interface IUIRenderManager
    {
        void RenderFrame(double deltaTime, Matrix4 projection, Matrix4 view, InputManager inputManager, ViewContainer rootView, int width, int height, float cameraZ);
    }
}
