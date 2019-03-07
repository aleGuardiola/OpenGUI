﻿using OpenGui.App;
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

        public string Text = "";

        protected override void Initialize()
        {
            base.Initialize();

            var component = GetViewById<Test>("test1");
            component.Background = new DrawableColor(System.Drawing.Color.Orange);

            var obs = this.GetObservable(x => x.Text);

            var a = Observable.When(obs.Where(s => s.Length == 8).And(this.GetObservable(x => x.Width)).Then((text, width) => text + width.ToString()));
            


        }
    }
}