namespace CompleteInformation.RecipeModule.GUI.Console.Helper

open System

module String =
    let (^) l r =
        sprintf "%s%s" l r

    /// Converts a string into a list of characters.
    let explode (s:string) =
        [for c in s -> c]

    /// Converts a list of characters into a string.
    let implode (xs:char list) =
        let sb = Text.StringBuilder(xs.Length)
        xs |> List.iter (sb.Append >> ignore)
        sb.ToString()

module Console =
    let splitCommandAndArguments input =
        let partList =
            String.explode input
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
            |> List.map (List.rev >> String.implode)
            |> List.rev

        match partList with
        | [] -> ("", [])
        | command::arguments -> (command, arguments)

    let getText() =
        printfn "Enter text, to finish enter a ';;' line."
        let rec helper text =
            let input = Console.ReadLine()
            match input with
            | ";;" -> text
            | _ ->
                match text with
                | "" -> helper input
                | text ->
                    sprintf "%s\n%s" text input
                    |> helper
        helper ""
