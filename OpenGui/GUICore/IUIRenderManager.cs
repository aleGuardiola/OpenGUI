using OpenGui.Controls;
using OpenTK;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.GUICore
{
    public interface IUIRenderManager
    {
        void RenderFrame(double deltaTime, Matrix4 projection, Matrix4 view, IGameWindow gameWindow, ViewContainer rootView, int width, int height, float cameraZ);
        void Update(double deltaTime, IGameWindow gameWindow, ViewContainer rootView);
    }
}
