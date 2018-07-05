namespace CompleteInformation.RecipeModule.Core

module FRecipe = CompleteInformation.RecipeModule.Core.FSharp.Recipe
module FRecipeApplication = CompleteInformation.RecipeModule.Core.FSharp.RecipeApplication

type Recipe(wrapped) =
    let mutable wrapped = wrapped

    new () = Recipe(FRecipe.create "")

    member __.Name
        with get () = FRecipe.getName wrapped
        and set value = wrapped <- FRecipe.setName value wrapped
    member __.Ingredients
        with get () = FRecipe.getIngredients wrapped |> List.toArray
        and set value = wrapped <- FRecipe.setIngredients (List.ofArray value) wrapped
    member __.Text
        with get () = FRecipe.getText wrapped
        and set value = wrapped <- FRecipe.setText value wrapped

    member __.Wrapped
        with get () = wrapped

type RecipeApplication() =
    let mutable wrapped = FRecipeApplication.create ()

    member __.Core
        with get () = wrapped.core

    member __.LoadRecipes () =
        FRecipe.SaveLoad.load wrapped
        |> List.map (fun r -> Recipe(r))
        |> List.toArray

    member __.SaveRecipes (recipes: Recipe[]) =
        List.ofArray recipes
        |> List.map (fun r -> r.Wrapped)
        |> FRecipe.SaveLoad.save wrapped
