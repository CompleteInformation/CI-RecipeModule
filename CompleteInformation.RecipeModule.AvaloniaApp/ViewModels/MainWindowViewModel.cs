using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CompleteInformation.RecipeModule.AvaloniaApp.Helper;
using CompleteInformation.RecipeModule.AvaloniaApp.Views;
using CompleteInformation.RecipeModule.Core;

namespace CompleteInformation.RecipeModule.AvaloniaApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IReactiveList<Recipe> recipes;
        private Dictionary<string, UserControl> views;
        private bool editMode = false;
        private int selected = -1;
        private UserControl currentView;

        // Properties
        public ActiveRecipeViewModel ActiveRecipe { get; set; }

        public IReactiveList<Recipe> Recipes
        {
            get => this.recipes;
            private set { this.RaiseAndSetIfChanged(ref this.recipes, value); }
        }

        public Recipe SelectedRecipe
        {
            get
            {
                if (this.selected >= 0 && this.Recipes.Count() > this.selected) {
                    return this.Recipes[selected];
                }
                else {
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

        public UserControl[] Views
        {
            get => this.views.Values.ToArray();
        }

        public UserControl CurrentView
        {
            get => this.currentView;
            set => this.RaiseAndSetIfChanged(ref this.currentView, value);
        }

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
                    }
                    else {
                        Recipe changedRecipe = this.ActiveRecipe.GetAsRecipe();
                        this.Recipes[this.selected] = changedRecipe;
                        this.SelectedRecipe = changedRecipe;
                        this.Save();
                        this.CurrentView = this.views["details"];
                    }
                }
            }
        }

        public void ToggleEditMode()
        {
            this.EditMode = !this.EditMode;
        }

        public void CreateNewRecipe()
        {
            Recipe created = new Recipe();
            this.EditMode = true;
            this.Recipes.Add(created);
            this.SelectedRecipe = created;
        }

        public void DeleteActiveRecipe()
        {
            this.EditMode = false;
            this.Recipes.Remove(this.SelectedRecipe);
            this.SelectedRecipe = null;
            this.Save();
        }

        public void AddIngredientToRecipe()
        {
            this.ActiveRecipe.Ingredients.Add(new SimpleObservable<string>(""));
        }

        // TODO:
        /*public void DeleteIngredientFromRecipe(string ingredient)
        {
            this.ActiveRecipe.Ingredients.Remove(ingredient);
        }*/

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

            this.InitializeViews();
        }

        public void Save()
        {
            Saving.SaveRecipes(this.Recipes.ToArray());
        }
    }
}
