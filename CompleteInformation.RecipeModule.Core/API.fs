namespace CompleteInformation.RecipeModule.Core

module FRecipe = CompleteInformation.RecipeModule.Core.FSharp.Types.Recipe
module FSaving = CompleteInformation.RecipeModule.Core.FSharp.Saving

type Recipe(wrapped) =
    let mutable wrapped = wrapped

    new (name) = Recipe(FRecipe.create name)

    member __.Name
        with get () = FRecipe.getName wrapped
        and set value = wrapped <- FRecipe.setName wrapped value
    member __.Ingredients
        with get () = FRecipe.getIngredients wrapped |> List.toArray
    member __.Text
        with get () = FRecipe.getText wrapped
        and set value = wrapped <- FRecipe.setText wrapped value

    member __.Wrapped
        with get () = wrapped

type Saving =
    static member LoadRecipes () = FSaving.load () |> List.map (fun r -> Recipe(r)) |> List.toArray
    static member SaveRecipes (recipes: Recipe[]) = List.ofArray recipes |> List.map (fun r -> r.Wrapped) |> FSaving.save