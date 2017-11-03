module CompleteInformation.ConsoleGUI.Main

open CompleteInformation.ConsoleGui.Helper.Console
open CompleteInformation.Recipes.Types
open System
open System.IO

let showHelp() =
    printfn "\nCompleteInformation - Recipes\n"
    printfn "Commands:"
    printfn "General:"
    printfn "help - Show this help page"
    printfn "quit - Exit program"
    printfn ""
    printfn "Recipe manipulation:"
    printfn "create                       - Creates a new recipe and opens it for further editing"
    printfn "ingredients add <ingredient> - Adds given ingredient"
    printfn "name set <name>              - Set a name for the loaded recipe"
    printfn "show                         - Shows the actually loaded recipe"
    printfn "unload                       - Unloads loaded recipe"
    printfn ""
    printfn "I/O:"
    printfn "load <file> - Loads a recipe from given file"
    printfn "save <file> - Saves the actually loaded recipe to given file"
    printfn ""

let unload state =
    let (active, recipeList) = state
    match active with
    | Some recipe -> recipe::recipeList
    | None -> recipeList

let handleInput state input =
    let (active, recipeList) = state
    match (splitCommandAndArguments input, active) with
    | (("create", []), _) ->
        let recipe = Recipe.createEmpty()
        printfn "New recipe created and loaded"
        (Some recipe, recipeList)
    | (("name", ["set";name]), Some recipe) ->
        let recipe = Recipe.setName recipe name
        printfn "New name set"
        (Some recipe, recipeList)
    | (("ingredients", ["add";ingredient]), Some recipe) ->
        let recipe = Recipe.addIngredient recipe ingredient
        (Some recipe, recipeList)
    | (("load", [file]), _) ->
        let recipeList = unload state
        let recipe =
            File.ReadAllText file
            |> Recipe.deserialize
        (Some recipe, recipeList)
    | (("show", []), Some recipe) ->
        Recipe.getName recipe |> printfn "\n%s"
        printfn "Ingredients:"
        Recipe.getIngredients recipe |> List.iter (printfn "%s")
        printfn "Text:"
        Recipe.getText recipe |> printfn "%s\n"
        state
    | (("save", [file]), Some recipe) ->
        let text = Recipe.serialize recipe
        File.WriteAllText (file, text)
        state
    | (("unload", []), Some recipe) ->
        printfn "Recipe unloaded"
        (None, recipe::recipeList)
    | (("help", []), _) ->
        showHelp()
        state
    | _ ->
        printfn "Invalid input, use help to get a overview over valid commands. Maybe you did not load a recipe or used a wrong amount of arguments?"
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
