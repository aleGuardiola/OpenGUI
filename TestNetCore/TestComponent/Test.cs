using OpenGui.Animations.Interplators;
using OpenGui.Animations.Xaml;
using OpenGui.App;
using OpenGui.Controls;
using OpenGui.Graphics;
using OpenGui.Values;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace TestNetCore.TestComponent
{
    [Component(
       TemplateResource = "TestNetCore.TestComponent.Test.xaml"  
    )]
    public class Test : Component
    {
        Label label;

        public string Name
        {
            get => GetValue<string>();
            set => SetValue<string>(value);            
        }               

        protected override void Initialize()
        {
            Name = "Alejandro";            
        }

        protected override void ViewCreated()
        {
            label = GetViewById<Label>("text");

            AttachedToWindow += Test_AttachedToWindow;
            label.Animation = new ParallelAnimation()
            {
                Animations = {
                new FloatPropertyAnimation()
                {
                     Duration = 1000,
                     StartValue = 0,
                     EndValue = 500,
                     Interpolator = new BounceInterpolator(),
                     Property = "X"
                }
                }
            };

            label.Click += Test_Click;
        }

        private void Test_AttachedToWindow(object sender, EventArgs e)
        {
            label.IsAnimating = true;
        }

        private void Test_Click(object sender, OpenGui.Core.ClickEventArgs e)
        {
            Name = Name == "Alejandro" ? "Tini" : "Alejandro";
        }
    }
}
