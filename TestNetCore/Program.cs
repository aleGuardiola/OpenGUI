using OpenGui.Animations.Interplators;
using OpenGui.Animations.Xaml;
using OpenGui.Controls;
using OpenGui.Graphics;
using OpenGui.GUICore;
using OpenGui.Values;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using TestNetCore.TestComponent;

namespace TestNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            new TestApp().Run(new GameWindow(800, 600));
        }

    }
}
