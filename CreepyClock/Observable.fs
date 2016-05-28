module CreepyClock.Observable

open System
open RateLimitAgent

let private observable f = 
    { new IObservable<_> with
        member x.Subscribe(obs) = f obs }
  
/// Limits the rate of emitted messages to at most one per the specified number of milliseconds
let limitRate milliseconds (source:IObservable<_>) = 
    observable (fun obs ->
        let rate = RateLimitAgent(milliseconds)
        rate.EventOccurred.Add(obs.OnNext)
        rate.ErrorOccurred.Add(obs.OnError)
        { new IObserver<_> with
            member x.OnCompleted() = obs.OnCompleted()
            member x.OnError(e) = obs.OnError(e)
            member x.OnNext(v) = rate.AddEvent(v) }
        |> source.Subscribe )