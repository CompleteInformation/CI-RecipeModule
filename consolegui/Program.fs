module CompleteInformation.ConsoleGUI.Main

open CompleteInformation.ConsoleGui
open CompleteInformation.Recipes
open CompleteInformation.Recipes.Types
open System

let (|Int|_|) str =
   match System.Int32.TryParse(str) with
   | (true,int) -> Some(int)
   | _ -> None
let (|GreaterZeroInt|_|) x =
    match x with
    | Int x when x > 0 -> Some x
    | _ -> None

let (|GreaterOne|NotGreaterOne|) x = if x > 0 then GreaterOne x else NotGreaterOne x

let list recipeList =
    match List.length recipeList with
    | 0 -> printfn "No recipes."
    | _ ->
        let rec helper recipeList index =
            match recipeList with
            | [] -> ()
            | recipe::tail ->
                Recipe.getName recipe
                |> printfn " %i: %s" index
                helper tail (index+1)
        helper recipeList 1

let rec load recipeList index =
    match index with
    | GreaterZeroInt index ->
        let (recipe, recipeList, _) =
            recipeList
            |> List.fold
                (fun folder recipe ->
                    match folder with
                    | (Some r, prev, _) -> (Some r, recipe::prev, 0)
                    | (None, prev, 0) -> (None, recipe::prev, 0)
                    | (None, prev, 1) -> (Some recipe, prev, 0)
                    | (None, prev, GreaterOne index) -> (None, recipe::prev, (index-1))
                    | (r, prev, _) -> (r, recipe::prev, 0)
                )
                (None, [], index)
        (recipe, recipeList)
    | _ -> (None, recipeList)


let showHelp() =
    printfn "\nCompleteInformation - Recipes\n"
    printfn "Commands:"
    printfn " General:"
    printfn "  help - Show this help page"
    printfn "  save - Saves changes"
    printfn "  quit - Saves changes and exits program"
    printfn ""
    printfn " Recipe organisation:"
    printfn "  list      - Shows a list of all recipes"
    printfn "  load <nr> - Loads the recipe with the given nr from list command"
    printfn "  show      - Shows the loaded recipe"
    printfn "  unload    - Unloads loaded recipe"
    printfn ""
    printfn " Recipe manipulation:"
    printfn "  create <name>      - Creates a new recipe with given name and opens it for further editing"
    printfn "  ingredients"
    printfn "    add <ingredient> - Adds ingredient to loaded recipe"
    printfn "    clear            - Removes all ingredients from recipe"
    printfn "  name set <name>    - Set a name for the loaded recipe"
    printfn "  text"
    printfn "    set        - Opens textmode in which you can enter the text for the recipe"
    printfn "    set <text> - Sets the given text as recipe text"

let unload state =
    let (active, recipeList) = state
    match active with
    | Some recipe ->
        recipe::recipeList
    | None -> recipeList

let handleInput state input =
    let (active, recipeList) = state
    match (Helper.Console.splitCommandAndArguments input, active) with
    | (("create", [name]), _) ->
        let recipeList = unload state
        let recipe = Recipe.create name
        printfn "New recipe created and loaded."
        (Some recipe, recipeList)
    | (("help", []), _) ->
        showHelp()
        state
    | (("ingredients", ["add";ingredient]), Some recipe) ->
        let recipe = Recipe.addIngredient recipe ingredient
        printfn "Ingredient added."
        (Some recipe, recipeList)
    | (("ingredients", ["clear"]), Some recipe) ->
        let recipe = Recipe.clearIngredients recipe
        printfn "Ingredients cleared."
        (Some recipe, recipeList)
    | (("list", []), _) ->
        let recipeList = unload state
        list recipeList
        state
    | (("load", [index]), _) ->
        let recipeList = unload state
        let state = load recipeList index
        match state with
        | (Some recipe, _) ->
            Recipe.getName recipe
            |> printfn "Loaded recipe '%s'"
        | (None, _) -> ()
        state
    | (("name", ["set";name]), Some recipe) ->
        let recipe = Recipe.setName recipe name
        printfn "New name set."
        (Some recipe, recipeList)
    | (("show", []), Some recipe) ->
        Recipe.getName recipe |> printfn "\n%s\n"
        printfn "Ingredients:"
        Recipe.getIngredients recipe |> List.iter (printfn " %s")
        printfn "\nText:"
        Recipe.getText recipe |> printfn "%s"
        state
    | (("save", []), _) ->
        unload state
        |> Saving.save
        state
    | (("text", ["set"]), Some recipe) ->
        let recipe =
            Helper.Console.getText()
            |> Recipe.setText recipe
        printfn "New text set."
        (Some recipe, recipeList)
    | (("text", ["set"; text]), Some recipe) ->
        let recipe = Recipe.setText recipe text
        printfn "New text set."
        (Some recipe, recipeList)
    | (("unload", []), Some recipe) ->
        printfn "Recipe unloaded."
        (None, recipe::recipeList)
    | _ ->
        printfn "Invalid input, use help to get a overview over valid commands. Maybe you did not load a recipe or used a wrong amount of arguments?"
        state

let rec mainLoop state =
    printf "> "
    let input = Console.ReadLine()
    match input with
    | "quit" ->
        unload state
        |> Saving.save
        0
    | input ->
        handleInput state input
        |> mainLoop

[<EntryPoint>]
let main _ =
    let recipeList = Saving.load()
    printfn ""
    printfn "CompleteInformation - Recipes v0.1-alpha.1"
    mainLoop (None, recipeList)
