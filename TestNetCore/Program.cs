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

            var view = new StackLayout() { Orientation = OpenGui.Values.Orientation.Horizontal };
            view.Children.Add(new View() {
                Background = new DrawableColor(Color.FromArgb(255, 255, 0, 0)),
                Width = 100f,
                Height = 100f,
                MarginLeft = 10f,
                MarginRight = 10f,
                Align = OpenGui.Values.Align.Top
            });

            view.Children.Add(new View()
            {
                Background = new DrawableColor(Color.FromArgb(255, 255, 0, 0)),
                Width = 100f,
                Height = 100f,
                MarginLeft = 10f,
                MarginRight = 10f,
                Align = OpenGui.Values.Align.Bottom
            });

            view.Background = new DrawableColor(Color.FromArgb(255, 255, 255, 255));

            view.Width = float.PositiveInfinity;
            view.Height = float.PositiveInfinity;

            view.PaddingBottom = 10f;
            view.PaddingTop = 10f;
            view.PaddingRight = 10f;
            view.PaddingLeft = 10f;
            view.ContentAlign = OpenGui.Values.Align.Left;

            guiCoreWindow.Root = view;
            windows.Run();

        }
    }
}
