namespace CompleteInformation.RecipeModule.Core.FSharp

open CompleteInformation.Core.FSharp

module RecipeApplication =
    type T = {
        core: Application.T;
    }

    let create () =
        {
            core =
                Application.createSetup ["recipes"] []
                |> Application.setUpApplication
        }

