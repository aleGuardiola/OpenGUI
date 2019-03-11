using OpenGui.Controls;
using OpenGui.Controls.Containers;
using OpenGui.GUICore;
using OpenTK.Platform;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.App
{
    public abstract class App
    {
        private Window _window;
        
        public Window Window
        {
            get => _window;
        }

        private Component _mainComponent;
        public Component MainComponent
        {
            get => _mainComponent;
            set
            {
                _mainComponent = value;
                _mainComponent.HorizontalAligment = Values.HorizontalAligment.Stretch;
                _mainComponent.VerticalAligment = Values.VerticalAligment.Stretch;
                _window.Root.Children.Add(value);
            }
        }

        protected App()
        {
            
        }
        
        protected abstract void OnStart();

        public void Run(IGameWindow window)
        {
            _window = new Window(window);            
            _window.Root = new CoordinateLayout();

            _window.BackgroundColor = OpenTK.Color.Wheat;

            OnStart();
            if (MainComponent == null)
                throw new Exception("Main component not set");
                       
            window.Run();            
        }


    }
}
