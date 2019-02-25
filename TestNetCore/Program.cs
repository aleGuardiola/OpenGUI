using OpenTK;

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
