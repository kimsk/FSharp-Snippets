open System.Net
open System.IO
open Microsoft.FSharp.Control.WebExtensions

let tprintfn fmt =
  printf "[.NET thread %d]" System.Threading.Thread.CurrentThread.ManagedThreadId
  printfn fmt

let links = 
  [
    "google", "http://google.com"
    "bing", "http://www.bing.com/"
    "yahoo", "http://www.yahoo.com"
  ]

let fetchAsync(nm, url:string) =
  async{
    do tprintfn "Creating request for %s..." nm
    let req = WebRequest.Create(url)

    let! resp = req.AsyncGetResponse()

    do tprintfn "Getting response stream for %s..." nm
    let stream = resp.GetResponseStream()

    do tprintfn "Reading response for %s..." nm
    let reader = new StreamReader(stream)
    let! html = reader.ReadToEndAsync() |> Async.AwaitTask

    do tprintfn "Read %d characters for %s..." html.Length nm
  }

let fetchAsync' (nm, url:string) =
  async.Delay(fun() ->
      do tprintfn "Creating request for %s..." nm
      let req = WebRequest.Create(url)
      async.Bind(req.AsyncGetResponse(), (fun resp -> 
        do tprintfn "Getting response stream for %s..." nm
        let stream = resp.GetResponseStream()
        do tprintfn "Reading response for %s..." nm
        let reader = new StreamReader(stream)
        async.Bind(reader.ReadToEndAsync() |> Async.AwaitTask, (fun html -> 
          do tprintfn "Read %d characters for %s..." html.Length nm
          async.Return html
          )
        ) 
        ) 
      ) 
    ) 

let fetchAsync''(nm, url:string) =
  async{
    do tprintfn "Creating request for %s..." nm
    let req = WebRequest.Create(url)

    let! resp = req.AsyncGetResponse()

    do tprintfn "Getting response stream for %s..." nm
    let stream = resp.GetResponseStream()

    do tprintfn "Reading response for %s..." nm
    let reader = new StreamReader(stream)
    let! html = reader.ReadToEndAsync() |> Async.AwaitTask

    do tprintfn "Read %d characters for %s..." html.Length nm
    return html
  }
    

for nm, url in links do
  fetchAsync (nm, url) |> Async.Start

links 
|> List.toArray 
|> Array.map (fun (nm, url) ->  fetchAsync (nm, url) |> Async.Start)

fetchAsync''("tachyus", "http://www.tachyus.com") |> Async.RunSynchronously
printfn "hello"

[for nm, url in links -> fetchAsync'(nm, url)]
|> Async.Parallel 
|> Async.Ignore
|> Async.RunSynchronously

[for nm, url in links -> fetchAsync(nm, url)]
|> Async.Parallel
|> Async.RunSynchronously

open System.Threading
ThreadPool.QueueUserWorkItem(fun _ -> printf "Hello") |> ignore


let doSomething something =
  do something
  // or
  //let () = something
  //()

doSomething (printfn "test")