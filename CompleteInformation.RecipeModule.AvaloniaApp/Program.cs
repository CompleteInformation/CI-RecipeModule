using System;
using Avalonia;
using Avalonia.Logging.Serilog;
using CompleteInformation.RecipeModule.AvaloniaApp.ViewModels;
using CompleteInformation.RecipeModule.AvaloniaApp.Views;

namespace CompleteInformation.RecipeModule.AvaloniaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildAvaloniaApp().Start<MainWindow>(() => new MainWindowViewModel());
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}
