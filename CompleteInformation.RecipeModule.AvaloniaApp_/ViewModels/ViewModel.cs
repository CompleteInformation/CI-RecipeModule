using CompleteInformation.RecipeModule.Core;
using CompleteInformation.RecipeModule.AvaloniaApp.Views;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private Dictionary<string, UserControl> views;
        public UserControl[] Views
        {
            get => this.views.Values.ToArray();
        }

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

            this.views = new Dictionary<string, UserControl>();
            this.views.Add("details", new DetailView());
            this.views.Add("edit", new EditView());

            this.CurrentView = this.views["details"];
        }
    }
}
