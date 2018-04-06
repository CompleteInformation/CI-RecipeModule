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
        public ReactiveList<Recipe> Recipes { get; }

        private int selected = -1;
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
                this.RaiseAndSetIfChanged(ref this.selected, Array.IndexOf(this.Recipes.ToArray(), value));
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

            set
            {
                bool changed = this.editMode != value;
                this.RaiseAndSetIfChanged(ref this.editMode, value);

                if (changed) {
                    if (this.EditMode) {
                        this.CurrentView = this.views["edit"];
                    } else {
                        this.ActiveRecipe.SaveToRecipe(this.Recipes[this.selected]);
                        Saving.SaveRecipes(this.Recipes.ToArray());
                        this.CurrentView = this.views["details"];
                    }
                }
            }
        }

        public ReactiveCommand ToggleEditMode { get; private set; }
        public ReactiveCommand CreateNewRecipe { get; private set; }
        public ReactiveCommand DeleteActiveRecipe { get; private set; }

        protected void InitializeCommands()
        {
            this.ToggleEditMode = ReactiveCommand.Create(() =>
            {
                this.EditMode = !this.EditMode;
            });

            this.CreateNewRecipe = ReactiveCommand.Create(() =>
            {
                Recipe created = new Recipe(""); // TODO: create empty constructor for Recipe
                this.Recipes.Add(created);
                this.EditMode = true;
                this.SelectedRecipe = created;
            });

            this.DeleteActiveRecipe = ReactiveCommand.Create(() =>
            {
                this.EditMode = false;
                this.Recipes.Remove(this.SelectedRecipe);
                this.SelectedRecipe = null;
            });
        }

        protected void InitializeViews()
        {
            this.views = new Dictionary<string, UserControl>();
            this.views.Add("details", new DetailView());
            this.views.Add("edit", new EditView());

            this.CurrentView = this.views["details"];
        }

        public MainWindowViewModel()
        {
            this.ActiveRecipe = new ActiveRecipeViewModel();
            this.Recipes = new ReactiveList<Recipe>(Saving.LoadRecipes());
            if (this.Recipes.Count > 0) {
                this.SelectedRecipe = this.Recipes[0];
            }

            this.InitializeCommands();
            this.InitializeViews();
        }
    }
}
