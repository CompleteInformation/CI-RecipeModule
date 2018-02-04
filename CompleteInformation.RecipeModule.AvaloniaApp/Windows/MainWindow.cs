using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CompleteInformation.RecipeModule.AvaloniaApp.ViewModels;
using System;

namespace CompleteInformation.RecipeModule.AvaloniaApp.Windows
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel.Instance;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
