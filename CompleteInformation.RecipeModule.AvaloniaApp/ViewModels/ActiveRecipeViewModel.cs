using ReactiveUI;

using CompleteInformation.RecipeModule.Core;

namespace CompleteInformation.RecipeModule.AvaloniaApp.ViewModels
{
    public class ActiveRecipeViewModel : ReactiveObject
    {
        public bool Set { get; private set; }

        private string name;
        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        private string[] ingredients;
        public string[] Ingredients
        {
            get => this.ingredients;

            // TODO: implement setter for ingredients array
        }

        private string text;
        public string Text
        {
            get => this.text;
            set => this.RaiseAndSetIfChanged(ref this.text, value);
        }

        public void SetFromRecipe(Recipe recipe)
        {
            this.Set = true;
            this.Name = recipe.Name;
            this.ingredients = new string[0]; // TODO:
            this.Text = recipe.Text;
        }

        public void SaveToRecipe(ref Recipe recipe)
        {
            if (this.Set) {
                recipe.Name = this.Name;
                //recipe.Ingredients = this.Ingredients; // TODO:
                recipe.Text = this.Text;
            }
        }
    }
}
