using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CompleteInformation.RecipeModule.AvaloniaApp
{
    public class DetailView : UserControl
    {
        public DetailView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
