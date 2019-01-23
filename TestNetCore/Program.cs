using ExCSS;
using OpenGui.Controls;
using OpenGui.GUICore;
using OpenGui.Values;
using OpenTK;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            GameWindow windows = new GameWindow(800, 600);

            Window guiCoreWindow = new Window(windows);
            guiCoreWindow.BackgroundColor = OpenTK.Color.Blue;

            var view = new ViewContainer();

            view.Width = 100f;
            view.Height = 100f;
            
            view.X =  0f;
            view.Y = 0f;
            view.Z = 0f;

            view.Background = new DrawableColor(System.Drawing.Color.Green);

            guiCoreWindow.Root = view;
            windows.Run();

        }
    }
}
