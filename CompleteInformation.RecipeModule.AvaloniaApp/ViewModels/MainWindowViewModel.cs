using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CompleteInformation.RecipeModule.AvaloniaApp.Views;
using CompleteInformation.RecipeModule.Core;

namespace CompleteInformation.RecipeModule.AvaloniaApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public Recipe[] Recipes { get; }
        private Recipe activeRecipe;
        public Recipe ActiveRecipe
        {
            get => this.activeRecipe;
            set => this.RaiseAndSetIfChanged(ref this.activeRecipe, value);
        }

        private Dictionary<string, UserControl> views;
        public UserControl[] Views
        {
            get => this.views.Values.ToArray();
        }

        private UserControl currentView;
        public UserControl CurrentView
        {
            get => this.currentView;
            set => this.RaiseAndSetIfChanged(ref this.currentView, value);
        }

        private bool editMode = false;
        public bool EditMode
        {
            get => this.editMode;
            set => this.RaiseAndSetIfChanged(ref this.editMode, value);
        }

        public ReactiveCommand ToggleEditMode { get; private set; }

        protected void InitializeCommands()
        {
            ToggleEditMode = ReactiveCommand.Create(() =>
            {
                this.EditMode = !this.EditMode;
                if (this.EditMode) {
                    this.CurrentView = this.views["edit"];
                } else {
                    this.CurrentView = this.views["details"];
                }
            });
        }

        public MainWindowViewModel()
        {
            this.InitializeCommands();

            this.Recipes = Saving.LoadRecipes();
            if (this.Recipes.Length > 0) {
                this.ActiveRecipe = this.Recipes[0];
            }

            this.views = new Dictionary<string, UserControl>();
            this.views.Add("details", new DetailView());
            this.views.Add("edit", new EditView());

            this.CurrentView = this.views["details"];
        }
    }
}
