#I @"..\..\..\FsReveal\src\packages\FSharp.Formatting.2.4.18\lib\net40"
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

let outDir = Path.Combine(__SOURCE_DIRECTORY__, "../reveal.js")

let fsxFile = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.fsx")
FsReveal.generateOutputFromScriptFile outDir "test-fsx.html" fsxFile

let mdFile = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.md")
FsReveal.generateOutputFromMarkdownFile outDir "test-md.html" mdFile
