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

            guiCoreWindow.Root = new CoordinateLayout()
            {
                Background = new DrawableColor(Color.Cyan),
                Children =
                {
                   new Label()
                   {
                       Width = 500,
                       Height = 500,
                       RelativeX = 20,
                       RelativeY = 20,
                       Text = "Alejo"
                   }
                }
               
            };

            windows.Run();
        }
    }
}
