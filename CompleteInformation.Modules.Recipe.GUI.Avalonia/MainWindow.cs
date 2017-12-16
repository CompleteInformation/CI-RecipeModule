using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CompleteInformation.Modules.Recipe.GUI.Avalonia
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
