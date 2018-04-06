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
        private Recipe[] recipes;
        public Recipe[] Recipes
        {
            get => this.recipes;
            set => this.RaiseAndSetIfChanged(ref this.recipes, value);
        }

        private int selected;
        public Recipe SelectedRecipe
        {
            get
            {
                if (this.selected >= 0 && this.Recipes.Count() > this.selected) {
                    return this.Recipes[selected];
                } else {
                    return null;
                }
            }

            set
            {
                this.RaiseAndSetIfChanged(ref this.selected, Array.IndexOf(this.Recipes, value));
                if (value != null) {
                    this.ActiveRecipe.SetFromRecipe(value);
                }
            }
        }

        public ActiveRecipeViewModel ActiveRecipe { get; set; }

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
        public ReactiveCommand CreateNewRecipe { get; private set; }
        public ReactiveCommand DeleteActiveRecipe { get; private set; }

        protected void InitializeCommands()
        {
            this.ToggleEditMode = ReactiveCommand.Create(() =>
            {
                this.EditMode = !this.EditMode;
                if (this.EditMode) {
                    this.CurrentView = this.views["edit"];
                } else {
                    this.ActiveRecipe.SaveToRecipe(ref this.Recipes[this.selected]);
                    Saving.SaveRecipes(this.recipes);
                    this.CurrentView = this.views["details"];
                }
            });

            this.CreateNewRecipe = ReactiveCommand.Create(() =>
            {
                // TODO:
            });

            this.DeleteActiveRecipe = ReactiveCommand.Create(() =>
            {
                // TODO:
            });
        }

        public MainWindowViewModel()
        {
            this.ActiveRecipe = new ActiveRecipeViewModel();

            this.InitializeCommands();

            this.Recipes = Saving.LoadRecipes();
            if (this.Recipes.Length > 0) {
                this.SelectedRecipe = this.Recipes[0];
            }

            this.views = new Dictionary<string, UserControl>();
            this.views.Add("details", new DetailView());
            this.views.Add("edit", new EditView());

            this.CurrentView = this.views["details"];
        }
    }
}
