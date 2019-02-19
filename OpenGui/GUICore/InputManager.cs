using OpenTK.Input;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.GUICore
{
    public class InputManager
    {
        bool[] _isMouseButtonPressed;
        bool _isMouseOutside;
        int _mouseX;
        int _mouseY;
        int _mouseDeltaX;
        int _mouseDeltaY;

        public int MouseX { get => _mouseX; }
        public int MouseY { get => _mouseY; }
        public int MouseDeltaX { get => _mouseDeltaX; }
        public int MouseDeltaY { get => _mouseDeltaY; }
        
        public InputManager(IGameWindow gameWindows)
        {            
            //13 buttons in the mouse
            _isMouseButtonPressed = new bool[13];

            gameWindows.KeyDown += GameWindows_KeyDown;
            gameWindows.KeyPress += GameWindows_KeyPress;
            gameWindows.KeyUp += GameWindows_KeyUp;
            gameWindows.MouseDown += GameWindows_MouseDown;
            gameWindows.MouseEnter += GameWindows_MouseEnter;
            gameWindows.MouseLeave += GameWindows_MouseLeave;
            gameWindows.MouseMove += GameWindows_MouseMove;
            gameWindows.MouseUp += GameWindows_MouseUp;
            gameWindows.MouseWheel += GameWindows_MouseWheel;            
        }

        public bool GetMouseButtonState(MouseButton button)
        {
            return _isMouseButtonPressed[(int)button];
        }

        private void GameWindows_MouseWheel(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            
        }

        private void GameWindows_MouseUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            _isMouseButtonPressed[(int)e.Button] = e.IsPressed;
        }

        private void GameWindows_MouseMove(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {
            _mouseX = e.X;
            _mouseY = e.Y;
            _mouseDeltaX = e.XDelta;
            _mouseDeltaY = e.YDelta;            
        }

        private void GameWindows_MouseLeave(object sender, EventArgs e)
        {
            _isMouseOutside = true;
        }

        private void GameWindows_MouseEnter(object sender, EventArgs e)
        {
            _isMouseOutside = false;
        }

        private void GameWindows_MouseDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            _isMouseButtonPressed[(int)e.Button] = e.IsPressed; 
        }

        private void GameWindows_KeyUp(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            
        }

        private void GameWindows_KeyPress(object sender, OpenTK.KeyPressEventArgs e)
        {
            
        }

        private void GameWindows_KeyDown(object sender, OpenTK.Input.KeyboardKeyEventArgs e)
        {
            
        }
    }
}
