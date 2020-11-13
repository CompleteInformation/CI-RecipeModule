namespace CompleteInformation.RecipeModule.DesktopApp

open Avalonia
open Avalonia.Controls
open Avalonia.FuncUI.Components
open Avalonia.FuncUI.Components.Hosts
open Avalonia.FuncUI.Elmish
open Avalonia.FuncUI.DSL
open Avalonia.Input
open Avalonia.Layout
open Avalonia.Threading
open CompleteInformation.RecipeModule.Core
open CompleteInformation.RecipeModule.Core.Types
open Elmish

module Main =
    type State = {
        recipeList: Recipe list
        chosenRecipe: int option
        chosenSegment: int option
        chosenStep: int option
    }

    type Msg =
        | UpdateChosenRecipe of int
        | UpdateChosenSegment of int
        | UpdateChosenStep of int

    let init() =
        let state = {
            chosenRecipe = None
            chosenSegment = None
            chosenStep = None
            recipeList = Persistence.loadRecipeList ()
        }

        state, Cmd.none

    let update (msg: Msg) (state: State) =
        match msg with
        | UpdateChosenRecipe index ->
            let index =
                match index with
                | index when index >= 0 -> Some index
                | _ -> None
            let state = { state with chosenRecipe = index; chosenSegment = None; chosenStep = None }

            state, Cmd.none
        | UpdateChosenSegment index ->
            let index =
                match index with
                | index when index >= 0 -> Some index
                | _ -> None
            let state = { state with chosenSegment = index; chosenStep = None }

            state, Cmd.none
        | UpdateChosenStep index ->
            let index =
                match index with
                | index when index >= 0 -> Some index
                | _ -> None
            let state = { state with chosenStep = index }

            state, Cmd.none

    let basicItemView text =
        TextBlock.create [
            TextBlock.text text
        ]

    let recipeItemView (recipe: Recipe) =
        basicItemView recipe.name

    let segmentItemView (segment: Segment) =
        basicItemView segment.title

    let stepItemView (step: Step) =
        match step.title with
        | Some title -> title
        | None -> step.text // TODO: limit
        |> basicItemView

    let getChosenElement index list =
        match index with
        | Some index ->
            List.tryItem index list
        | None -> None

    let getChosenRecipe state =
        getChosenElement state.chosenRecipe state.recipeList

    let getChosenSegment state recipe =
        getChosenElement state.chosenSegment recipe.segments

    let getChosenStep state segment =
        getChosenElement state.chosenStep segment.steps

    let listView (state: State) (dispatch: Msg -> unit) =
        Grid.create [
            Grid.column 0
            Grid.columnDefinitions "1*, 1*, 1*"
            Grid.children [
                ListBox.create [
                    ListBox.column 0
                    ListBox.selectedIndex (state.chosenRecipe |> Option.defaultValue -1)
                    ListBox.dataItems state.recipeList
                    ListBox.itemTemplate (DataTemplateView<Recipe>.create recipeItemView)
                    ListBox.onSelectedIndexChanged (UpdateChosenRecipe >> dispatch, Never)
                ]
                match getChosenRecipe state with
                | None -> ()
                | Some currentRecipe ->
                    ListBox.create [
                        ListBox.column 1
                        ListBox.selectedIndex (state.chosenSegment |> Option.defaultValue -1)
                        ListBox.dataItems currentRecipe.segments
                        ListBox.itemTemplate (DataTemplateView<Segment>.create segmentItemView)
                        ListBox.onSelectedIndexChanged (UpdateChosenSegment >> dispatch, Never)
                    ]
                    match getChosenSegment state currentRecipe with
                    | None -> ()
                    | Some currentSegment ->
                        ListBox.create [
                            ListBox.column 2
                            ListBox.selectedIndex (state.chosenStep |> Option.defaultValue -1)
                            ListBox.dataItems currentSegment.steps
                            ListBox.itemTemplate (DataTemplateView<Step>.create stepItemView)
                            ListBox.onSelectedIndexChanged (UpdateChosenStep >> dispatch, Never)
                        ]
            ]
        ]

    let detailViewWithStep (state: State) (step: Step) (dispatch: Msg -> unit) =
        [
            TextBlock.create [
                TextBlock.text $"""%i{state.chosenSegment.Value + 1}.%i{state.chosenStep.Value + 1}. %s{step.title |> Option.defaultValue ""}"""
            ]
            // TODO: Ingredients
            TextBlock.create [
                TextBlock.text step.text
            ]
        ]

    let detailViewWithSegment (state: State) (segment: Segment) (dispatch: Msg -> unit) =
        [
            TextBlock.create [
                TextBlock.text $"%i{state.chosenSegment.Value + 1}. %s{segment.title}"
            ]
            match getChosenStep state segment with
            | None -> () // TODO: ingredients, if no step is chosen
            | Some step -> yield! detailViewWithStep state step dispatch
        ]

    let detailViewWithRecipe (state: State) (recipe: Recipe) (dispatch: Msg -> unit) =
        [
            TextBlock.create [
                TextBlock.text recipe.name
            ]
            match getChosenSegment state recipe with
            | None -> () // TODO: ingredients, if no segment is chosen
            | Some segment -> yield! detailViewWithSegment state segment dispatch
        ]
        |> Seq.cast<FuncUI.Types.IView>
        |> Seq.toList

    let detailView (state: State) (dispatch: Msg -> unit) =
        StackPanel.create [
            StackPanel.column 2
            StackPanel.orientation Orientation.Vertical
            StackPanel.margin 10.0
            StackPanel.spacing 5.0
            StackPanel.children
                (match getChosenRecipe state with
                | None -> []
                | Some recipe -> detailViewWithRecipe state recipe dispatch)
        ]

    let view (state: State) (dispatch: Msg -> unit) =
        Grid.create [
            Grid.columnDefinitions "1*, Auto, 1*"
            Grid.children [
                listView state dispatch
                GridSplitter.create [
                    GridSplitter.classes ["vertical"]
                    Grid.column 1
                ]
                detailView state dispatch
            ]
        ]

    type MainWindow() as this =
        inherit HostWindow()
        do
            base.Title <- "KP"
            //base.Icon <- WindowIcon (AvaloniaHelper.loadAssetPath "avares://Andromeda.AvaloniaApp.FSharp/Assets/logo.ico")
            base.Width <- 1024.0
            base.Height <- 660.0

#if DEBUG
            this.AttachDevTools(KeyGesture(Key.F12))
#endif

            let syncDispatch (dispatch: Dispatch<'msg>): Dispatch<'msg> =
                match Dispatcher.UIThread.CheckAccess() with
                | true -> fun msg -> Dispatcher.UIThread.Post(fun () -> dispatch msg)
                | false -> dispatch

            Program.mkProgram init update view
            |> Program.withHost this
            |> Program.withSyncDispatch syncDispatch
#if DEBUG
            |> Program.withConsoleTrace
#endif
            |> Program.run
