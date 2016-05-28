namespace Views

open FsXaml
open System
open CreepyClock
open Decision

type MainViewBase = XAML<"MainView.xaml">

type MainView() =
    inherit MainViewBase()

    let lookingDecider = CompositionRoot.lookingDecider
    
    override this.OnInitialize() =
        this.Loaded.Add this.OnLoaded
        this.Dispatcher.ShutdownStarted.Add this.OnUnloaded
        
    member this.OnLoaded _ =
        let showTime decision =
            this.Dispatcher.Invoke( fun () ->
                match decision with
                | Looking    -> this.ClockLabel.Content <- DateTime.Now.ToString("HH:mm:ss")
                | NotLooking -> this.ClockLabel.Content <- "" )

        lookingDecider.DecisionOccurred
        |> Observable.subscribe showTime
        |> ignore

        lookingDecider.Start ()

    member this.OnUnloaded _ =
        lookingDecider.Stop ()
