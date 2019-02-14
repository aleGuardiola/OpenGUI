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
            guiCoreWindow.BackgroundColor = OpenTK.Color.Purple;

            guiCoreWindow.Root = new CoordinateLayout()
            {
                Background = new DrawableColor(Color.White),
                Children =
                {
                   new Label()
                   {
                       RelativeX = 0,
                       RelativeY = 0,
                       Text = "Mari",
                       TextColor = Color.Black,
                       TextAlign = OpenGui.Values.TextAlign.Center,
                       Background = new DrawableColor(Color.Transparent),
                       HorizontalAligment = OpenGui.Values.HorizontalAligment.Stretch
                       //PaddingLeft = 30f
                   }
                }
               
            };

            windows.Run();
        }
    }
}
