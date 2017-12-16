using System;
using Avalonia;

namespace CompleteInformation.Modules.Recipe.GUI.Avalonia
{
    class Program
    {
        static void Main(string[] args)
        {
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .Start<MainWindow>();
        }
    }
}
