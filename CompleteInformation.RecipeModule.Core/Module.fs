namespace CompleteInformation.RecipeModule.Core

open System.Composition

open CompleteInformation.Core.FSharp.Modules

[<Export(typeof<IModule>)>]
type RecipeModule() =
    interface IModule with
        member __.GetDatabaseNames () =
            ["recipes"]
        member __.Initialize dict =
            ()
