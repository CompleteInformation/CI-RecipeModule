using CompleteInformation.RecipeModule;

namespace CompleteInformation.RecipeModule.AvaloniaApp
{
    public class ViewModel
    {
        public string Name
        {
            get
            {
                return "Lilu";
            }
        }

        public Recipe[] Recipes
        {
            get
            {
                var recipe = new Recipe("Beispiel");
                return new Recipe[] {
                    recipe
                };
            }
        }
    }
}
