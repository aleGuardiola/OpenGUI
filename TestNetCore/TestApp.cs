using OpenGui.App;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestNetCore
{
    public class TestApp : App
    {
        protected override void OnStart()
        {
            MainComponent = new Combination();
        }
    }
}
