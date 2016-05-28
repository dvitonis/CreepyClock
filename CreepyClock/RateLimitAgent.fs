module CreepyClock.RateLimitAgent

open System

type RateLimitAgent<'T>(milliseconds:int) = 
  let event = Event<'T>()
  let error = Event<_>()
  let agent = MailboxProcessor.Start(fun inbox -> 
    let rec loop (lastMessageTime:DateTime) = async {
      let! e = inbox.Receive()
      let now = DateTime.UtcNow
      if (now - lastMessageTime).TotalMilliseconds > float milliseconds then
        try event.Trigger(e)
        with e -> error.Trigger(e)
        return! loop now
      else 
        return! loop lastMessageTime }
    loop DateTime.MinValue )

  member x.EventOccurred = event.Publish
  member x.AddEvent(event) = agent.Post(event)
  member x.ErrorOccurred = Event.merge agent.Error error.Publish