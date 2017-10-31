namespace CompleteInformation.Recipes.Types

module Recipe =
    type T = {
        ingredients: string list;
        recipetext: string option
    }

    let createEmptyRecipe () = {
        ingredients = [];
        recipetext = None
    }

    let addIngredient recipe ingredient =
        { recipe with ingredients = ingredient::recipe.ingredients }

    let getIngredients recipe = recipe.ingredients |> List.rev

    let setText (recipe:T) text =
        match text with
        | "" -> { recipe with recipetext = None }
        | text -> { recipe with recipetext = Some text }

    let getText recipe =
        match recipe.recipetext with
        | Some text -> text
        | None -> ""
