namespace CompleteInformation.RecipeModule.Core

module Types =
    [<Measure>] type g
    [<Measure>] type ml

    type Amount =
        | Volume of float<ml>
        | Weight of float<g>
        | Both of float<ml> * float<g>

    type Ingredient = {
        name: string
        amount: Amount
    }

    type Step = {
        ingredients: Ingredient list
        text: string
    }

    type Segment = {
        steps: Step list
    }

    type Recipe = {
        name: string
        segments: Segment list
    }
