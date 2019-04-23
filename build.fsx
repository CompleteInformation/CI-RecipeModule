#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.DotNet.Cli
nuget Fake.Core.Target //"
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.DotNet
open Fake.IO
open Fake.IO.Globbing.Operators

Target.create "Build" (fun _ ->
    DotNet.exec id "build" "CompleteInformation.RecipeModule.AvaloniaApp/CompleteInformation.RecipeModule.AvaloniaApp.csproj"
    |> ignore
)

// start build
Target.runOrDefault "Build"
