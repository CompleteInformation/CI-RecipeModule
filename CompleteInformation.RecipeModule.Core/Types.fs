namespace CompleteInformation.RecipeModule.Core.FSharp.Types

open CompleteInformation.Core.FSharp

module RecipeApplication =
    type T = {
        coreApplication: Application.T;
    }

    let create core =
        {
            coreApplication = core;
        }

    let createTmp databases =
        {
            coreApplication =
                Application.createSetup databases []
                |> Application.setUpApplication
        }

module Recipe =
    type T =
        {
            name: string;
            ingredients: string list;
            recipetext: string option;
        }

    let create name = {
        name = name;
        ingredients = [];
        recipetext = None;
    }

    let setName (recipe:T) name =
        { recipe with name = name }

    let getName recipe = recipe.name

    let addIngredient recipe ingredient =
        { recipe with ingredients = ingredient::recipe.ingredients }

    let clearIngredients recipe =
        { recipe with ingredients = [] }

    let getIngredients recipe = recipe.ingredients |> List.rev
    let setIngredients recipe ingredients =
        { recipe with ingredients = ingredients |> List.rev }

    let setText (recipe:T) text =
        match text with
        | "" -> { recipe with recipetext = None }
        | text -> { recipe with recipetext = Some text }

    let getText recipe =
        match recipe.recipetext with
        | Some text -> text
        | None -> ""
