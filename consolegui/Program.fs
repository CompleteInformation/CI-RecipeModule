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
    printfn "\nCompleteInformation - Recipes"
    printfn "Commands:"
    printfn "help - Show this help page"
    printfn "quit - Exit program\n"

let handleInput recipeList input =
    match (splitCommandAndArguments input) with
    | ("help", []) ->
        showHelp()
        recipeList
    | (command, args) ->
        printfn "Invalid input"
        printfn "Command: %s" command
        printfn "Arguments:"
        args |> List.iter (printfn "%s")
        recipeList

let rec mainLoop recipeList =
    let input = Console.ReadLine()
    match input with
    | "quit" -> 0
    | input ->
        handleInput recipeList input
        |> mainLoop

[<EntryPoint>]
let main argv =
    mainLoop []
