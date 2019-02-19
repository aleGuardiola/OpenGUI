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
            Children[0].Click += Test_Click;
        }

        private void Test_Click(object sender, OpenGui.Core.ClickEventArgs e)
        {
            Name = Name == "Alejandro" ? "Mu" : "Alejandro";
        }
    }
}
