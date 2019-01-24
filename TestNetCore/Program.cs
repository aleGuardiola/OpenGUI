using OpenGui.Controls;
using OpenGui.Graphics;
using OpenGui.GUICore;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            OpenTK.GameWindow windows = new OpenTK.GameWindow(800, 600);

            Window guiCoreWindow = new Window(windows);
            guiCoreWindow.BackgroundColor = OpenTK.Color.Gray;

            var view = new ViewContainer();                     

            view.Width = 100f;
            view.Height = 100f;
            
            view.Background = new DrawableColor(Color.FromArgb(255, 255, 0, 0));

            view.MarginTop = 20f;
            view.MarginLeft = 20f;
            
            guiCoreWindow.Root = view;
            windows.Run();

        }
    }
}
