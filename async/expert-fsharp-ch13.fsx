open System.Net
open System.IO
open Microsoft.FSharp.Control.WebExtensions

let links = 
  [
    "google", "http://google.com"
    "bing", "http://www.bing.com/"
    "yahoo", "http://www.yahoo.com"
  ]

let fetchAsync(nm, url:string) =
  async{
    do printfn "Creating request for %s..." nm
    let req = WebRequest.Create(url)

    let! resp = req.AsyncGetResponse()

    do printfn "Getting response stream for %s..." nm
    let stream = resp.GetResponseStream()

    do printfn "Reading response for %s..." nm
    let reader = new StreamReader(stream)
    let! html = reader.ReadToEndAsync() |> Async.AwaitTask

    do printfn "Read %d characters for %s..." html.Length nm
    
  }

for nm, url in links do
  fetchAsync (nm, url) |> Async.Start

links 
|> List.toArray 
|> Array.map (fun (nm, url) ->  fetchAsync (nm, url) |> Async.Start)