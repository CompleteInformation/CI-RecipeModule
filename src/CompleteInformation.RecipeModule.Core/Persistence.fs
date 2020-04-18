namespace CompleteInformation.RecipeModule.Core

open CompleteInformation.RecipeModule.Core.Types
open FSharp.Json
open System.IO

module Persistence =
    let private filePath = "recipes.json"

    let saveRecipeList (recipeList: Recipe list) =
        let json =
            recipeList
            |> Json.serialize
        File.WriteAllText (filePath, json)

    let loadRecipeList () =
        File.ReadAllText filePath
        |> Json.deserialize<Recipe list>
