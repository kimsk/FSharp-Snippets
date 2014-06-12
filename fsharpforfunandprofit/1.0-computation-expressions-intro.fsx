(**
Logging : we wanted to perform some side-effect between each step.
*)
type LoggingBuilder() =
    let log p = printfn "expression is %A" p

    member this.Bind(x, f) = 
        log x
        f x

    member this.Return(x) = 
        x

let logger = new LoggingBuilder()

let loggedWorkflow = 
    logger
        {
        let! x = 42
        let! y = 43
        let! z = x + y
        return z
        }

(**
State: we wanted to keep track of "global" state.
*)

type State<'a, 's> = State of ('s -> 'a * 's)

let runState (State s) a = s a
let getState = State (fun s -> (s,s))
let putState s = State (fun _ -> ((),s))

type StateBuilder() =
    member this.Return(a) = 
        State (fun s -> (a,s))
    member this.Bind(m,k) =
        State (fun s -> 
            let (a,s') = runState m s 
            runState (k a) s')
    member this.ReturnFrom (m) = m

let state = new StateBuilder()

let DoSomething counter = 
    printfn "DoSomething. Counter=%i " counter
    counter + 1

let FinalResult counter = 
    printfn "FinalResult. Counter=%i " counter
    counter

// convert old functions to "state-aware" functions
let lift f = state {
    let! s = getState 
    return! putState (f s)
    }

// new functions
let DoSomething' = lift DoSomething
let FinalResult' = lift FinalResult

let counterWorkflow = 
    let s = state {
        do! DoSomething'
        do! DoSomething'
        do! DoSomething'
        do! FinalResult'
        } 
    runState s 0

(**
Safe division (using maybe) : we wanted to handle errors elegantly so that we could focus on the happy path.
*)
type MaybeBuilder() =

    member this.Bind(x, f) = 
        match x with
        | None -> None
        | Some a -> f a

    member this.Return(x) = 
        Some x
   
let maybe = new MaybeBuilder()

let divideBy bottom top =
    if bottom = 0
    then None
    else Some(top/bottom)

let divideByWorkflow init x y z = 
    maybe 
        {
        let! a = init |> divideBy x
        let! b = a |> divideBy y
        let! c = b |> divideBy z
        return c
        }    
let good = divideByWorkflow 12 3 2 1
let bad = divideByWorkflow 12 3 0 1

(**
Multi-lookup (using orElse) : we wanted to return early with the first success.
*)
type OrElseBuilder() =
    member this.ReturnFrom(x) = x
    member this.Combine (a,b) = 
        match a with
        | Some _ -> a  // a succeeds -- use it
        | None -> b    // a fails -- use b instead
    member this.Delay(f) = f()

let orElse = new OrElseBuilder()

let map1 = [ ("1","One"); ("2","Two") ] |> Map.ofList
let map2 = [ ("A","Alice"); ("B","Bob") ] |> Map.ofList
let map3 = [ ("CA","California"); ("NY","New York") ] |> Map.ofList

let multiLookup key = orElse {
    return! map1.TryFind key
    return! map2.TryFind key
    return! map3.TryFind key
    }

multiLookup "A" |> printfn "Result for A is %A" 
multiLookup "CA" |> printfn "Result for CA is %A" 
multiLookup "X" |> printfn "Result for X is %A" 

(**
Asynchronous calls with callbacks : we wanted to hide the use of callbacks and avoid the "pyramid of doom".
*)

open System.Net
let req1 = HttpWebRequest.Create("http://tryfsharp.org")
let req2 = HttpWebRequest.Create("http://google.com")
let req3 = HttpWebRequest.Create("http://bing.com")

// ugly and hard to follow
req1.BeginGetResponse((fun r1 -> 
    use resp1 = req1.EndGetResponse(r1)
    printfn "Downloaded %O" resp1.ResponseUri

    req2.BeginGetResponse((fun r2 -> 
        use resp2 = req2.EndGetResponse(r2)
        printfn "Downloaded %O" resp2.ResponseUri

        req3.BeginGetResponse((fun r3 -> 
            use resp3 = req3.EndGetResponse(r3)
            printfn "Downloaded %O" resp3.ResponseUri

            ),null) |> ignore
        ),null) |> ignore
    ),null) |> ignore

// better with F# async
async {
    use! resp1 = req1.AsyncGetResponse()  
    printfn "Downloaded %O" resp1.ResponseUri

    use! resp2 = req2.AsyncGetResponse()  
    printfn "Downloaded %O" resp2.ResponseUri

    use! resp3 = req3.AsyncGetResponse()  
    printfn "Downloaded %O" resp3.ResponseUri

    } |> Async.RunSynchronously