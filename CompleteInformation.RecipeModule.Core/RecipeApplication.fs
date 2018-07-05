namespace CompleteInformation.RecipeModule.Core.FSharp

open CompleteInformation.Core.FSharp

module RecipeApplication =
    type T = {
        core: Application.T;
    }

    let create () =
        Couchbase.Lite.Support.NetDesktop.Activate();
        {
            core =
                Application.createSetup ["recipes"] []
                |> Application.setUpApplication
        }

