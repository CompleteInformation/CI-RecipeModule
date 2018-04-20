using Avalonia;
using Avalonia.Markup.Xaml;

namespace CompleteInformation.RecipeModule.AvaloniaApp
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
