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

let fsxLocation = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.fsx")
let fsx = File.ReadAllText (fsxLocation)

let mdLocation = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\presentations\FsReveal.md")
let md = File.ReadAllText (mdLocation)


//let doc, presentation = FsReveal.getPresentationFromMarkdown md
let doc, presentation = FsReveal.getPresentationFromScriptString fsx

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

let paragraphs = FsReveal.getParagraphsFromPresentation presentation

formatParagraphs ctx paragraphs
wr.ToString()




#region temp


// should be true
FsReveal.getPresentationFromMarkdown md = FsReveal.getPresentationFromScriptString fsx


let ps = 
  [Heading (3,[Literal "What is FsReveal?"]);
        ListBlock
          (Unordered,
           [[Span
               [Literal "Generates ";
                DirectLink
                  ([Literal "reveal.js"],
                   ("http://lab.hakim.se/reveal-js/#/", None));
                Literal " presentation from ";
                DirectLink
                  ([Literal "markdown"],
                   ("http://daringfireball.net/projects/markdown/", None))]];
            [Span
               [Literal "Utilizes ";
                DirectLink
                  ([Literal "FSharp.Formatting"],
                   ("https://github.com/tpetricek/FSharp.Formatting", None));
                Literal " for markdown parsing"]]])];

let getParagraphsFromPresentation (presentation:FsReveal.Presentation) =
  let slides = presentation.Slides

  let wrapInSection p = InlineBlock("<section>")::p@[InlineBlock("</section>")]

  let getParagraphsFromSlide = function
    | FsReveal.Simple(p) ->
        wrapInSection p        
    | FsReveal.Nested(l) -> 
        l 
        |> List.map (wrapInSection)
        |> List.collect id
        |> wrapInSection
                
        
  slides 
  |> List.collect (getParagraphsFromSlide)

#endregion