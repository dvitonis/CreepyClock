module CreepyClock.Decision

open System.Collections.Generic
open AForge.Video.DirectShow
open System.Drawing

type LookingDecision = Looking | NotLooking

type FixedSizeQueue<'T> (size) =
    let queue = Queue<'T>()

    member this.Add(element) =
        queue.Enqueue element
        if queue.Count > size then queue.Dequeue () |> ignore
    member this.Contains(element) =
        queue.Contains element
        
type LowPassDecisionFilter(windowSize) =
    let window = FixedSizeQueue(windowSize)
        
    member this.Apply(data) =
        let filtered =
            match data with
            | Looking    -> Looking
            | NotLooking ->
                if window.Contains Looking
                then Looking
                else NotLooking
        window.Add data
        filtered

type LookingDecider (camera : VideoCaptureDevice, findFaces : Bitmap -> Rectangle [], filterDecision) =
    let decision = Event<LookingDecision>()

    let findAnyFace image =
        try
            let faces = findFaces image
            match faces with
            | [||] -> NotLooking
            | _    -> Looking
        with e -> NotLooking

    let decide (image : Bitmap) =
        findAnyFace image
        |> filterDecision
        |> decision.Trigger

    let subscription =
        camera.NewFrame
        |> Observable.map (fun f -> f.Frame.Clone() :?> Bitmap)
        |> Observable.limitRate 100
        |> Observable.subscribe decide

    member this.Start() = camera.Start()
    member this.Stop() = camera.SignalToStop()
    member this.DecisionOccurred = decision.Publish