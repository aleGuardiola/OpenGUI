using OpenGui.App;
using OpenGui.Styles;
using System;
using System.Collections.Generic;
using System.Text;

namespace TestNetCore
{
    public class TestApp : App
    {
        protected override void OnStart()
        {
            Window.StyleEngine.AddDefinition(new StyleDefinition()
            {
                Containers =
                {
                    new IdContainer("text")
                    {
                        Setters = 
                        {
                            new Setter("Text", "Hola")
                        }
                    }
                }
            });

            MainComponent = new Combination();
        }
    }
}
