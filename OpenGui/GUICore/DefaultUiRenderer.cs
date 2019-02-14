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
        //last values to know if re measure is needed
        float lastWidth;
        float lastHeight;

        public void RenderFrame(double deltaTime, Matrix4 projection, Matrix4 view, InputManager inputManager, ViewContainer rootView, int width, int height, float cameraZ)
        {
            //if there is no root view just no render anything
            if (rootView == null)
                return;

            //handle input

            //measure only if size has changed
            if(lastWidth != width || lastHeight != height)
              rootView.Mesure(width, height, Values.MeasureSpecMode.Exactly);
            
            //render view
            rootView.GLDraw(projection, view, new RectangleF(0, 0, 1000, 1000 ), width, height, cameraZ);

            lastWidth = width;
            lastHeight = height;
        }
    }
}
