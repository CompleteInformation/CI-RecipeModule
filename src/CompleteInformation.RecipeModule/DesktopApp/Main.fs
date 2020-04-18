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
        choosenRecipe: int option
        choosenSegment: int option
    }

    type Msg =
        | UpdateChoosenRecipe of int
        | UpdateChoosenSegment of int

    let init() =
        let state = {
            choosenRecipe = None
            choosenSegment = None
            recipeList = Persistence.loadRecipeList ()
        }

        state, Cmd.none

    let update (msg: Msg) (state: State) =
        match msg with
        | UpdateChoosenRecipe index ->
            { state with choosenRecipe = Some index }, Cmd.none
        | UpdateChoosenSegment index ->
            { state with choosenSegment = Some index }, Cmd.none

    let recipeItemView recipe =
        TextBlock.create [
            TextBlock.text recipe.name
        ]

    let view (state: State) (dispatch: Msg -> unit) =
        StackPanel.create [
            StackPanel.orientation Orientation.Horizontal
            StackPanel.children [
                ListBox.create [
                    ListBox.dataItems state.recipeList
                    ListBox.itemTemplate (DataTemplateView<Recipe>.create recipeItemView)
                    ListBox.onSelectedIndexChanged (UpdateChoosenRecipe >> dispatch, Always)
                ]
                match state.choosenRecipe with
                | None -> ()
                | Some index ->
                    let currentRecipe = state.recipeList.Item index
                    ListBox.create [
                        ListBox.dataItems currentRecipe.segments
                        ListBox.onSelectedIndexChanged (UpdateChoosenSegment >> dispatch, Always)
                    ]
                    match state.choosenSegment with
                    | None -> ()
                    | Some index ->
                        let currentSegment = currentRecipe.segments.Item index
                        ListBox.create [
                            ListBox.dataItems currentSegment.steps
                        ]
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
