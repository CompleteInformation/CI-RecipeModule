using CompleteInformation.RecipeModule.Core;
using System;

namespace CompleteInformation.RecipeModule.AvaloniaApp
{
    public class ViewModel : ObservableObject
    {
        private static ViewModel instance;
        public static ViewModel Instance {
            get {
                if (instance == null) {
                    instance = new ViewModel();
                }
                return instance;
            }
        }

        private ViewModel() {
            this.Recipes = Saving.LoadRecipes();
            if (this.Recipes[0] != null) {
                this.ActiveRecipe = this.Recipes[0];
            }
        }

        public Recipe[] Recipes { get; }
        private Recipe activeRecipe;
        public Recipe ActiveRecipe {
            get => this.activeRecipe;
            set => Update(ref this.activeRecipe, value);
        }
    }
}
