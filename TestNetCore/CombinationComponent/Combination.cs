using OpenGui.App;
using OpenGui.Core;
using OpenGui.Graphics;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace TestNetCore
{
    [Component(
        TemplateResource = "TestNetCore.CombinationComponent.Combination.xaml"
        )]
    public class Combination : Component
    {
        int clickCount { get => GetValue<int>(); set => SetValue<int>(value); }

        public IObservable<string> ButtonText { get; set; }

        public override void Constructor()
        {
            ButtonText = this.GetObservable(x => x.clickCount).Select(x => $"Clicks: {x}");            
        }

        protected override void Initialize()
        {
            base.Initialize();

            var component = GetViewById<Test>("test1");
            component.Background = new DrawableColor(System.Drawing.Color.Orange);
            clickCount = 0;
        }
        
        public void onButtonClick(object sender, OpenGui.Core.MouseEventArgs e)
        {
            clickCount++;
        }

    }
}
