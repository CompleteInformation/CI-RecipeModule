using CompleteInformation.RecipeModule.Core;
using CompleteInformation.RecipeModule.AvaloniaApp.Views;
using Avalonia.Controls;
using System;
using System.Collections.Generic;

namespace CompleteInformation.RecipeModule.AvaloniaApp.ViewModels
{
    public class ViewModel : ObservableObject
    {
        private static ViewModel instance;
        public static ViewModel Instance
        {
            get
            {
                if (instance == null) {
                    instance = new ViewModel();
                }
                return instance;
            }
        }

        public Recipe[] Recipes { get; }
        private Recipe activeRecipe;
        public Recipe ActiveRecipe
        {
            get => this.activeRecipe;
            set => Update(ref this.activeRecipe, value);
        }

        public Dictionary<string, UserControl> Views { get; }
        private UserControl currentView;
        public UserControl CurrentView
        {
            get => this.currentView;
            set => Update(ref this.currentView, value);
        }

        private ViewModel()
        {
            this.Recipes = Saving.LoadRecipes();
            if (this.Recipes.Length > 0) {
                this.ActiveRecipe = this.Recipes[0];
            }

            this.Views = new Dictionary<string, UserControl>();
            this.Views.Add("details", new DetailView());

            this.CurrentView = this.Views["details"];
        }
    }
}
