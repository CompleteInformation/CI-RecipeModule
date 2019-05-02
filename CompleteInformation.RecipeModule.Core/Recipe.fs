namespace CompleteInformation.RecipeModule.Core.FSharp

open Couchbase.Lite
open Couchbase.Lite.Query

module Recipe =
    type T =
        {
            id: string option;
            name: string;
            ingredients: string list;
            recipetext: string option;
        }

    let create name = {
        id = None;
        name = name;
        ingredients = [];
        recipetext = None;
    }

    let setId id (recipe:T) =
        { recipe with id = Some(id) }
    let getId (recipe:T) = recipe.id

    let setName name (recipe:T) =
        { recipe with name = name }

    let getName recipe = recipe.name

    let addIngredient ingredient recipe =
        { recipe with ingredients = ingredient::recipe.ingredients }

    let clearIngredients recipe =
        { recipe with ingredients = [] }

    let getIngredients recipe = recipe.ingredients |> List.rev
    let setIngredients ingredients recipe =
        { recipe with ingredients = ingredients |> List.rev }

    let setText text (recipe:T) =
        match text with
        | "" -> { recipe with recipetext = None }
        | text -> { recipe with recipetext = Some text }

    let getText recipe =
        match recipe.recipetext with
        | Some text -> text
        | None -> ""

    module SaveLoad =
        type SaveFile =
            {
                version: int;
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

        let saveRecipe (app:RecipeApplication.T) recipe =
            let db = Map.find "recipes" app.core.databases

            let document =
                match recipe.id with
                | None -> new MutableDocument()
                | Some id -> db.GetDocument(id).ToMutable()
            document.SetString ("name", recipe.name) |> ignore
            match recipe.recipetext with
            | Some s -> document.SetString ("text", s) |> ignore
            | None -> ()
            document.SetArray("ingredients", MutableArrayObject(recipe.ingredients |> List.toArray)) |> ignore
            db.Save document

        let save app =
            List.rev
            >> List.iter (fun recipe -> saveRecipe app recipe)

        let load (app:RecipeApplication.T) :T list =
            let db = Map.find "recipes" app.core.databases
            use query = QueryBuilder.Select(SelectResult.All()).From(DataSource.Database(db))

            query.Execute()
            |> List.ofSeq
            |> List.map (fun recipe ->
                let dict = recipe.GetDictionary("recipes")
                create (dict.GetString ("name"))
                |> setId (dict.GetString ("id"))
                |> setText (dict.GetString ("text"))
                |> setIngredients (dict.GetArray("ingredients").ToList() |> Seq.cast<string> |> List.ofSeq)
            )

