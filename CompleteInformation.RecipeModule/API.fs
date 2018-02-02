namespace CompleteInformation.RecipeModule

module FRecipe = CompleteInformation.RecipeModule.FSharp.Types.Recipe;

type Recipe(name) =
    member private __.Wrapped = FRecipe.create name
    member this.Name = FRecipe.getName this.Wrapped
    member this.Ingredient = FRecipe.getIngredients this.Wrapped |> List.toArray
