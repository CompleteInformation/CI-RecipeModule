using Avalonia;
using CompleteInformation.RecipeModule.AvaloniaApp.Windows;
using System;

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
