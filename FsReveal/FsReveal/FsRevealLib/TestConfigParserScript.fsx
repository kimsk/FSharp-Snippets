//#I "../packages/FSharp.Formatting.2.4.10/lib/net40/"
//#I @"C:\Users\Karlkim\Documents\GitHub\FSharp.Formatting\bin"
#I @"G:\GitHub\FSharp.Formatting\bin"
#r "FSharp.Literate.dll"
#r "FSharp.CodeFormat.dll"
#r "FSharp.MetadataFormat.dll"
#r "FSharp.Markdown.dll"
#r "RazorEngine.dll"
#r "FSharp.Compiler.Service.dll"
#r "CSharpFormat.dll"

#load "FsReveal.fs"

open FSharp.Literate
open FSharp.Markdown
open FSharp.CodeFormat
open System.Collections.Generic
open CSharpFormat
open System
open System.IO
open System.Web
open FsReveal
open System.Text.RegularExpressions

let fsx = """
(*@ title = F#Reveal @*)
(*@ description = Introduction to F#Reveal @*)
(*@ author = Karlkim Suwanmongkol @*)
(*@ theme = default @*)
(*** slide-start ***)
(**
***
# F#Reveal
***
#### Karlkim Suwanmongkol
#### @kimsk
#### [http://karlk.im](http://karlk.im)
#### to@karlk.im
*)
let title = "F#Reveal"
(*** slide-end ***)
"""

let processConfigs (fsx:string) =
  let regex =     
    Regex("^\(\*\@(?<name>[^=]*)=(?<value>[^\@]*)\@\*\)$", RegexOptions.Singleline)

  let lines = fsx.Split([|'\r';'\n'|], StringSplitOptions.RemoveEmptyEntries)

  let configs, processedLines =  
    lines
    |> List.ofArray
    |> List.fold (fun acc l -> 
      let configs, processedLines = acc
      let m = regex.Match l
      if m.Success then
        let name = m.Groups.["name"].Value.Trim()
        let value = m.Groups.["value"].Value.Trim()
        ((name,value)::configs, processedLines)
      else
        (configs, l::processedLines)
    ) ([],[])

  let newFsx = (processedLines |> List.fold (fun acc l -> l + "\r\n" + acc) "")
  (configs, newFsx )
      
let headers, processedLines = fsx |> processConfigs
