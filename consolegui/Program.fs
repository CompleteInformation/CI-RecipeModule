module CompleteInformation.ConsoleGUI.Main

open CompleteInformation.ConsoleGui.Helper.String
open CompleteInformation.Recipes.Types.Recipe
open System

let splitCommandAndArguments input =
    let partList =
        explode input
        |> List.fold
            (fun args a ->
                match (args, a) with
                | ((list, false), '"') ->
                    (list, true)
                | ((list, true), '"') ->
                    (list, false)
                | ((list, false), ' ') ->
                    ([]::list, false)
                | ((head::tail, flag), a) ->
                    ((a::head)::tail, flag)
                | (([], flag), a) ->
                    ([[a]], flag)
            )
            ([[]], false)
        |> fst
        |> List.map (List.rev >> implode)
        |> List.rev

    match partList with
    | [] -> ("", [])
    | command::arguments -> (command, arguments)

let showHelp() =
    printfn "\nCompleteInformation - Recipes\n"
    printfn "Commands:"
    printfn "create                       - Creates a new recipe and opens it for further editing"
    printfn "help                         - Show this help page"
    printfn "ingredients add <ingredient> - Adds given ingredient"
    printfn "name set <name>              - Set a name for the loaded recipe"
    printfn "quit                         - Exit program\n"
    printfn "show                         - Shows the actually loaded recipe"
    printfn "unload                       - Unloads loaded recipe"

let handleInput state input =
    let (active, recipeList) = state
    match (splitCommandAndArguments input, active) with
    | (("create", []), _) ->
        let recipe = createEmptyRecipe()
        printfn "New recipe created and loaded"
        (Some recipe, recipeList)
    | (("name", ["set";name]), Some recipe) ->
        let recipe = setName recipe name
        printfn "New name set"
        (Some recipe, recipeList)
    | (("ingredients", ["add";ingredient]), Some recipe) ->
        let recipe = addIngredient recipe ingredient
        (Some recipe, recipeList)
    | (("show", []), Some recipe) ->
        getName recipe |> printfn "\n%s"
        printfn "Ingredients:"
        getIngredients recipe |> List.iter (printfn "%s")
        printfn "Text:"
        getText recipe |> printfn "%s\n"
        state
    | (("unload", []), Some recipe) ->
        printfn "Recipe unloaded"
        (None, recipe::recipeList)
    | (("help", []), _) ->
        showHelp()
        state
    | _ ->
        printfn "Invalid input, use help to get a overview over valid commands"
        state

let rec mainLoop state =
    let input = Console.ReadLine()
    match input with
    | "quit" -> 0
    | input ->
        handleInput state input
        |> mainLoop

[<EntryPoint>]
let main _ =
    mainLoop (None, [])
