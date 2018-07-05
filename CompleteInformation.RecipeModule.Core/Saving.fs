module CompleteInformation.RecipeModule.Core.FSharp.Saving

open CompleteInformation.Core.FSharp
open Couchbase.Lite
open Newtonsoft.Json
open System.Collections.Generic
open System.IO
open System.Runtime.Serialization

open Types.Recipe

let mutable databaseDict: Dictionary<string, Database> = null

[<DataContract>]
type SaveFile =
    {
        [<field: DataMember>]
        version: int;
        [<field: DataMember>]
        recipes: T list;
    }

let wrap recipes =
    {
        version = 1;
        recipes = recipes
    }

let unwrap save =
    match save.version with
    | 1 -> Some save.recipes
    | _ -> None

let save (recipes :T list) =
    let text =
        recipes
        |> List.rev
        |> wrap
        |> JsonConvert.SerializeObject
    File.WriteAllText ("recipes.json", text)

let load () :T list =
    // TODO: settings record for file name
    try
        let save :SaveFile =
            File.ReadAllText "recipes.json"
            |> JsonConvert.DeserializeObject<SaveFile>
        match unwrap save with
        | Some recipes -> List.rev recipes
        | None -> []
    with
    | :? FileNotFoundException -> []
