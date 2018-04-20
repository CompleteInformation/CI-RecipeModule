using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CompleteInformation.RecipeModule.AvaloniaApp.Views
{
    public class EditView : UserControl
    {
        public EditView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
