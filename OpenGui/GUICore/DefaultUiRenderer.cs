using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;
using OpenTK;
using OpenTK.Graphics.ES20;

namespace OpenGui.GUICore
{
    public class DefaultUiRenderer : IUIRenderManager
    {

        public void RenderFrame(double deltaTime, Matrix4 projection, Matrix4 view, InputManager inputManager, ViewContainer rootView, int width, int height)
        {
            //if there is no root view just no render anything
            if (rootView == null)
                return;

            //handle input

            rootView.Initialize(width, height, 0, 0);
            //render view
            rootView.GLDraw(projection, view, new RectangleF(0, 0, 1000, 1000 ), width, height);
        }
    }
}
