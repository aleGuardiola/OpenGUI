using OpenGui.Animations;
using OpenGui.Controls;
using OpenGui.Graphics;
using OpenGui.GUICore;
using OpenGui.Values;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TestNetCore
{
    class Program
    {
        static void Main(string[] args)
        {

            OpenGui.Controls.Image image = null;
            OpenTK.GameWindow windows;
            Window guiCoreWindow = null;


            windows = new OpenTK.GameWindow(800, 600);
            guiCoreWindow = new Window(windows);

            guiCoreWindow.BackgroundColor = OpenTK.Color.Purple;

            image = new OpenGui.Controls.Image()
            {
                Width = 700,
                Height = 500,
                ImageMode = ImageMode.Fit,
                Source = new WebImageSource(@"http://www.bestprintingonline.com/help_resources/Image/Ducky_Head_Web_Low-Res.jpg"),
                Background = new DrawableColor(Color.Wheat)
            };

            guiCoreWindow.Root = new CoordinateLayout()
            {
                Background = new DrawableColor(Color.White),
                Children =
                {
                   //new Label()
                   //{
                   //    RelativeX = 0,
                   //    RelativeY = 0,
                   //    Text = "Mari",
                   //    TextColor = Color.Black,
                   //    TextAlign = OpenGui.Values.TextAlign.Center,
                   //    Background = new DrawableColor(Color.Transparent),
                   //    HorizontalAligment = OpenGui.Values.HorizontalAligment.Stretch,
                   //    PaddingLeft = 30f
                   //},
                   image
                }
            };
                        
            var anim = new ParallelAnimation(image, new List<Animation>()
            {
                new FloatPropertyAnimation(image, "Width", 5000, 0f, 500f, FloatPropertyAnimation.Linear),
                new FloatPropertyAnimation(image, "Height", 5000, 0f, 500f, FloatPropertyAnimation.Linear)
            });

            guiCoreWindow.AddFrameRunner(anim);
            

            windows.Run();

        }
    }
}
