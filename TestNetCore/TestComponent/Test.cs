using OpenGui.Animations.Interplators;
using OpenGui.Animations.Xaml;
using OpenGui.App;
using OpenGui.Controls;
using OpenGui.Graphics;
using OpenGui.Styles;
using OpenGui.Values;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace TestNetCore
{
    [Component(
       TemplateResource = "TestNetCore.TestComponent.Test.xaml"        
    )]
    public class Test : Component
    {
        public string Name { get => GetValueOrDefault<string>(); set => SetValue<string>(value); }         
        
        public Color Color { get => GetValueOrDefault<Color>(); set => SetValue<Color>(value); }

        public string Status { get => GetValueOrDefault<string>(); set => SetValue<string>(value); }

        public Test()
        {
            Name = "Alejandro";
        }

        protected override void Initialize()
        {
            Name = "Alejandro";

            Status = "Good";
            Color = Color.Red;

            Background = new DrawableColor(Color.Blue);

            var label = GetViewById<Label>("text");
            //var setters = Window.StyleEngine.GetSetters(label).ToList();

            //label.Styles = new OpenGui.Styles.ViewStyleContainer()
            //{
            //    Setters =
            //    {
            //        new OpenGui.Styles.Set()
            //        {
            //            Property = "Text",
            //            Value = "Hola"
            //        }
            //    }
            //};

        }

        public void Test_Click(object sender, OpenGui.Core.MouseEventArgs e)
        {
            Name = Name == "Alejandro" ? "Tini" : "Alejandro";
            Color = Color == Color.Red ? Color.Green : Color.Red;
        }
    }
}
