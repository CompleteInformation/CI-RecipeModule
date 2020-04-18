namespace CompleteInformation.RecipeModule.Core

open CompleteInformation.RecipeModule.Core.Types

module Ingredient =
    let mergeAmounts amount1 amount2 =
        match (amount1, amount2) with
        | (Weight w1, Weight w2) -> w1 + w2 |> Weight
        | (Volume v1, Volume v2) -> v1 + v2 |> Volume
        | (Volume v, Weight w)
        | (Weight w, Volume v) -> (v, w) |> Both
        | (Both (v, w), Weight w2)
        | (Weight w2, Both (v, w)) -> (v, w + w2) |> Both
        | (Both (v, w), Volume v2)
        | (Volume v2, Both (v, w)) -> (v + v2, w) |> Both
        | (Both (v1, w1), Both (v2, w2)) -> (v1 + v2, w1 + w2) |> Both

    let mergeIngredientList (ingredientList: Ingredient list) =
        ingredientList
        |> List.groupBy (fun ingredient -> ingredient.name)
        |> List.map (
            fun (name, ingredientList) ->
                let newAmount =
                    ingredientList
                    |> List.map (fun ingredient -> ingredient.amount)
                    |> List.reduce mergeAmounts
                { name = name; amount = newAmount } )

    let getIngredients startList getIngredientsFromElement =
        startList
        |> List.fold (
            fun ingredientList element ->
                ingredientList
                |> List.append (getIngredientsFromElement element) ) []
        |> mergeIngredientList
        |> List.sortBy (fun ingredient -> ingredient.name)

    let getIngredientsBySegment segment =
        getIngredients segment.steps (fun step -> step.ingredients)

    let getIngredientsByRecipe recipe =
        getIngredients recipe.segments getIngredientsBySegment
