module CompleteInformation.Modules.Recipe.Saving

open Chiron
open System.IO
open Types.Recipe

type SaveFile =
    {
        version: int;
        recipes: T list;
    }
    static member ToJson (x:SaveFile) = json {
        do! Json.write "version" x.version
        do! Json.write "recipes" x.recipes
    }
    static member FromJson (_:SaveFile) = json {
        let! v = Json.read "version"
        let! r = Json.read "recipes"
        return { version = v; recipes = r }
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
        |> wrap
        |> Json.serialize
        |> Json.formatWith JsonFormattingOptions.Pretty
    File.WriteAllText ("recipes.json", text)

let load () :T list =
    // TODO: settings record for file name
    try
        let save :SaveFile =
            File.ReadAllText "recipes.json"
            |> Json.parse
            |> Json.deserialize
        match unwrap save with
        | Some recipes -> recipes
        | None -> []
    with
    | :? FileNotFoundException -> []
