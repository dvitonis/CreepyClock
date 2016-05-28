namespace Views

open FsXaml
open System

type MainViewBase = XAML<"MainView.xaml">

type MainView() =
    inherit MainViewBase()
    
    override this.OnInitialize() =
        this.Loaded.Add this.OnLoaded
        
    member this.OnLoaded _ =
        this.ClockLabel.Content <- DateTime.Now.ToString("HH:mm:ss")