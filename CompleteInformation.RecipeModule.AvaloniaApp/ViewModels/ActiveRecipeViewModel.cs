using ReactiveUI;
using System;
using System.Collections.Generic;

using CompleteInformation.RecipeModule.Core;
using CompleteInformation.RecipeModule.AvaloniaApp.Helper;

namespace CompleteInformation.RecipeModule.AvaloniaApp.ViewModels
{
    public class ActiveRecipeViewModel : ReactiveObject
    {
        private string name = "";
        private IReactiveList<string> ingredients;
        private string text = "";

        // Observables
        private readonly ObservableAsPropertyHelper<string> error;
        private readonly ObservableAsPropertyHelper<bool> valid;
        private readonly ObservableAsPropertyHelper<bool> invalid;

        // Properties
        public string Error
        {
            get => this.error.Value;
        }

        public bool Valid
        {
            get => this.valid.Value;
        }

        public bool Invalid
        {
            get => this.invalid.Value;
        }

        public bool Set { get; private set; }

        public string Name
        {
            get => this.name;
            set => this.RaiseAndSetIfChanged(ref this.name, value);
        }

        public IReactiveList<string> Ingredients
        {
            get => this.ingredients;
            set => this.RaiseAndSetIfChanged(ref this.ingredients, value);
        }

        public string Text
        {
            get => this.text;
            set => this.RaiseAndSetIfChanged(ref this.text, value);
        }

        // Functions
        public ActiveRecipeViewModel()
        {
            // Initialize Observables
            this.WhenAnyValue(x => x.Name, x => x.Text, (name, text) =>
            {
                if (name.Length == 0) {
                    return "Bitte gib einen Namen ein!";
                }
                // No error
                else {
                    return "";
                }
            }).ToProperty(this, x => x.Error, out error);

            this.WhenAnyValue(x => x.Error, error => error.Length == 0).ToProperty(this, x => x.Valid, out valid);

            this.WhenAnyValue(x => x.Valid, valid => !valid).ToProperty(this, x => x.Invalid, out invalid);
        }

        public void SetFromRecipe(Recipe recipe)
        {
            if (recipe != null) {
                this.Set = true;
                this.Name = recipe.Name;
                this.Ingredients = new ReactiveList<string>(recipe.Ingredients);
                this.Text = recipe.Text;
            }
            else {
                this.Set = false;
            }
        }

        public Recipe GetAsRecipe()
        {
            Recipe recipe = new Recipe();
            recipe.Name = this.Name;
            recipe.Ingredients = ReactiveHelper.Instance.ReactiveListToArray(this.Ingredients, x => x.Length > 0);
            recipe.Text = this.Text;
            return recipe;
        }
    }
}
