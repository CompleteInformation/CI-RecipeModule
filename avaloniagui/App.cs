using Avalonia;
using Avalonia.Markup.Xaml;

namespace avaloniagui
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
