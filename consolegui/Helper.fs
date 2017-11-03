namespace CompleteInformation.ConsoleGui.Helper

module String =
    let (^) l r =
        sprintf "%s%s" l r

    /// Converts a string into a list of characters.
    let explode (s:string) =
        [for c in s -> c]

    /// Converts a list of characters into a string.
    let implode (xs:char list) =
        let sb = System.Text.StringBuilder(xs.Length)
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
