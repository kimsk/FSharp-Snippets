#I @"..\..\..\FsReveal\src\FsReveal\bin"

#r "FSharp.Literate.dll"
#r "FSharp.Markdown.dll"

#load @"..\..\..\FsReveal\src\FsReveal\FsReveal.fs"

open System.IO
open FsReveal
open FSharp.Literate
open FSharp.Markdown

[1;2;3;0;1;2;3] |> splitBy 0

let fsxLocation = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.fsx")
let fsx = File.ReadAllText (fsxLocation)
getPresentationFromScriptString fsx

let mdLocation = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.md")
let md = File.ReadAllText (mdLocation)
let presentation = getPresentationFromMarkdown md


// should be true
getPresentationFromMarkdown md = getPresentationFromScriptString fsx




