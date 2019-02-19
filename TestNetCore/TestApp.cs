using OpenGui.App;
using System;
using System.Collections.Generic;
using System.Text;
using TestNetCore.TestComponent;

namespace TestNetCore
{
    public class TestApp : App
    {
        protected override void OnStart()
        {
            MainComponent = new Test();
        }
    }
}
