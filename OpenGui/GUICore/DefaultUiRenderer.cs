using System;
using System.Collections.Generic;
using System.Text;
using OpenGui.Controls;
using OpenTK;
using OpenTK.Graphics.ES20;
using OpenTK.Platform;
using SkiaSharp;

namespace OpenGui.GUICore
{
    public class DefaultUiRenderer : IUIRenderManager
    {
        //last values to know if re measure is needed
        float lastWidth;
        float lastHeight;

        OpenTK.Input.KeyboardState _lastKeyBoardState;
        OpenTK.Input.MouseState _lastMouseState;
        OpenTK.Input.GamePadState _lastGamePadState;
        OpenTK.Input.JoystickState _lastJostickState;

        public void RenderFrame(double deltaTime, Matrix4 projection, Matrix4 view, IGameWindow gameWindow, ViewContainer rootView, int width, int height, float cameraZ)
        {
            //if there is no root view just no render anything
            if (rootView == null)
                return;

            //handle input
            if(gameWindow.Focused)
            {
                var mouseState = OpenTK.Input.Mouse.GetCursorState();
                //clicked
                if (mouseState.IsButtonUp(OpenTK.Input.MouseButton.Left) && _lastMouseState.IsButtonDown(OpenTK.Input.MouseButton.Left))
                {
                    rootView.OnClick(new Core.ClickEventArgs(mouseState.X - gameWindow.X, mouseState.Y - gameWindow.Y));
                }

                //update states
                _lastMouseState = mouseState;
            }            

            //measure only if size has changed
            if(lastWidth != width || lastHeight != height)
              rootView.Mesure(width, height, Values.MeasureSpecMode.Exactly);
            else            
              rootView.Check();            

            //render view
            rootView.GLDraw(projection, view, new RectangleF(0, 0, 1000, 1000), width, height, cameraZ); 
                        

            lastWidth = width;
            lastHeight = height;
        }
    }
}
