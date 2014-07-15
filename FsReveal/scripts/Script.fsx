#I @"..\..\..\FsReveal\src\packages\FSharp.Formatting.2.4.14\lib\net40"

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

let fsxLocation = @"C:\Users\Karlkim\Documents\GitHub\FsReveal\presentations\FsReveal.fsx"
let fsx = File.ReadAllText (fsxLocation)

let mdLocation = @"C:\Users\Karlkim\Documents\GitHub\FsReveal\presentations\FsReveal.md"
let md = File.ReadAllText (mdLocation)
let presentation = FsReveal.getPresentationFromMarkdown md


// should be true
FsReveal.getPresentationFromMarkdown md = FsReveal.getPresentationFromScriptString fsx

presentation.Properties

let sb = new System.Text.StringBuilder()
let wr = new StringWriter(sb)


let ctx = 
  {
    Writer = wr
    Links = Dictionary<_, _>()
    Newline = Environment.NewLine
    LineBreak = ignore
    ParagraphIndent = ignore
  }

let paragraphs = match presentation.Slides.Head with FsReveal.Simple(p) -> p | _ -> failwith "FAIL"

formatParagraphs ctx paragraphs
wr.ToString()