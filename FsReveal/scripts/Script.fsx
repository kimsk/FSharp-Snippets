//#I @"..\..\..\FsReveal\src\packages\FSharp.Formatting.2.4.14\lib\net40"
#I @"..\..\..\FSharp.Formatting\bin"

#r "FSharp.Literate.dll"
#r "FSharp.Markdown.dll"
#r "FSharp.CodeFormat.dll"
#r @"..\..\..\FsReveal\src\FsReveal\bin\FsReveal.dll"

open System
open System.IO
open System.Collections.Generic
open FSharp.Literate
open FSharp.Markdown
open FSharp.Markdown.Html

let fsxLocation = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.fsx")
let fsx = File.ReadAllText (fsxLocation)

let mdLocation = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.md")
let md = File.ReadAllText (mdLocation)

let outDir = Path.Combine(__SOURCE_DIRECTORY__, "../reveal.js")

fsx |> FsReveal.processScriptFile outDir "test-fsx.html"
md |> FsReveal.processMarkdownFile outDir "test-md.html"

