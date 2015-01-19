open System.IO
open System.Net
open Microsoft.FSharp.Control

let doWebRequest (url:string) methd transformer =
  async {
    let request = WebRequest.Create(url, Method = methd)
    use! response = request.AsyncGetResponse()
    return! transformer (response.GetResponseStream())
  }

let readStreamAsString (stream:Stream) =
  async {
    use streamReader = new StreamReader(stream)
    return! streamReader.ReadToEndAsync() |> Async.AwaitTask
  }
  
type Agent<'a> = MailboxProcessor<'a>

type Request = | Get of string * AsyncReplyChannel<string>

let downloadAgent =
  Agent<_>.Start(fun inbox ->
    let rec loop (cache: Map<string, string>) =
      async{
        let! msg = inbox.Receive()
        match msg with
        | Get(url, reply) ->
          match cache.TryFind(url) with
          | Some (result) ->
              reply.Reply(result)
              return! loop cache
          | None ->
              let! result = doWebRequest url "GET" readStreamAsString
              reply.Reply(result)
              return! loop (Map.add url result cache)
      }

    loop Map.empty
  )