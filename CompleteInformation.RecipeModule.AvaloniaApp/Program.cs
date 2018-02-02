using System;
using Avalonia;

namespace CompleteInformation.RecipeModule.AvaloniaApp
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
