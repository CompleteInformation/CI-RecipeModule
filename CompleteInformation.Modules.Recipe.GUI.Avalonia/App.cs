using Avalonia;
using Avalonia.Markup.Xaml;

namespace CompleteInformation.Modules.Recipe.GUI.Avalonia
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
