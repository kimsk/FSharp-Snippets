//#I "../packages/FSharp.Formatting.2.4.10/lib/net40/"
#I @"G:\GitHub\FSharp.Formatting\bin"
#I "../packages/RazorEngine.3.3.0/lib/net40"
#I "../packages/FSharp.Compiler.Service.0.0.44/lib/net40"
#r "FSharp.Literate.dll"
#r "FSharp.CodeFormat.dll"
#r "FSharp.MetadataFormat.dll"
#r "FSharp.Markdown.dll"
#r "RazorEngine.dll"
#r "FSharp.Compiler.Service.dll"
#r "CSharpFormat.dll"


open FSharp.Literate
open FSharp.Markdown
open FSharp.CodeFormat
open System.Collections.Generic
open CSharpFormat
open System
open System.IO
open System.Web


let formattingContext templateFile format prefix lineNumbers includeSource replacements layoutRoots =
    { TemplateFile = templateFile 
      Replacements = defaultArg replacements []
      GenerateLineNumbers = defaultArg lineNumbers true
      IncludeSource = defaultArg includeSource false
      Prefix = defaultArg prefix "fs"
      OutputKind = defaultArg format OutputKind.Html
      LayoutRoots = defaultArg layoutRoots [] }

let rec collectCodes par = seq {
    match par with 
    | Matching.LiterateParagraph(HiddenCode(Some id, lines)) -> 
        yield Choice2Of2(id), lines
    | Matching.LiterateParagraph(DoNotEvalCode(lines))
    | Matching.LiterateParagraph(NamedCode(_,lines))
    | Matching.LiterateParagraph(FormattedCode(lines)) ->         
        yield Choice1Of2(lines), lines
    | Matching.ParagraphNested(pn, nested) ->
        yield! Seq.collect (Seq.collect collectCodes) nested
    | _ -> () }


/// Replace all special 'LiterateParagraph' elements recursively using the given lookup dictionary
let rec replaceSpecialCodes ctx (formatted:IDictionary<_, _>) = function
  | Matching.LiterateParagraph(special) -> 
      match special with
      | HiddenCode _ -> None
      | CodeReference ref -> Some (formatted.[Choice2Of2 ref])
      | OutputReference _  
      | ItValueReference _  
      | ValueReference _ -> 
          failwith "Output, it-value and value references should be replaced by FSI evaluator"
      | DoNotEvalCode lines
      | NamedCode(_,lines)
      | FormattedCode lines -> Some (formatted.[Choice1Of2 lines])      
      | LanguageTaggedCode(lang, code) -> 
          let inlined = 
            match ctx.OutputKind with
            | OutputKind.Html ->
                let code = HttpUtility.HtmlEncode code
                let code = SyntaxHighlighter.FormatCode(lang, code)
                sprintf "<pre lang=\"%s\">%s</pre>" lang code
            | OutputKind.Latex ->
                sprintf "\\begin{lstlisting}\n%s\n\\end{lstlisting}" code
          Some(InlineBlock(inlined))
      | StartSlide -> Some(StartSlideBlock)
      | EndSlide -> Some(EndSlideBlock)
  // Traverse all other structures recursively
  | Matching.ParagraphNested(pn, nested) ->
      let nested = List.map (List.choose (replaceSpecialCodes ctx formatted)) nested
      Some(Matching.ParagraphNested(pn, nested))
  | par -> Some par

let replaceLiterateParagraphs ctx (doc:LiterateDocument) = 
    let replacements = Seq.collect collectCodes doc.Paragraphs
    let snippets = [| for _, r in replacements -> Snippet("", r) |]    
    
    // Format all snippets and build lookup dictionary for replacements
    let formatted = CodeFormat.FormatHtml(snippets, ctx.Prefix, ctx.GenerateLineNumbers, false)
      
    let lookup = 
      [ for (key, _), fmtd in Seq.zip replacements formatted.Snippets -> 
          key, InlineBlock(fmtd.Content) ] |> dict 
   
    
    // Replace original snippets with formatted HTML and return document
    let newParagraphs = List.choose (replaceSpecialCodes ctx lookup) doc.Paragraphs
    doc.With(paragraphs = newParagraphs, formattedTips = formatted.ToolTip)

let ctx = formattingContext None (Some OutputKind.Html) None None None None None


let fsx ="""
(*** slide-start ***)
(**
## F# syntax in 60 seconds

### The two major differences between F# syntax and a standard C-like syntax are:

- Curly braces are not used to delimit blocks of code. Instead, indentation is used (Python is similar this way).
- Whitespace is used to separate parameters rather than commas.
*)
// The "let" keyword defines an (immutable) value
let myInt = 5
let myFloat = 3.14
let myString = "hello"   //note that no types needed

// ======== Lists ============
let twoToFive = [2;3;4;5]        // Square brackets create a list with
                                 // semicolon delimiters.
let oneToFive = 1 :: twoToFive   // :: creates list with new 1st element
// The result is [1;2;3;4;5]
let zeroToFive = [0;1] @ twoToFive   // @ concats two lists
(*** slide-end ***)
(*** slide-start ***)
(**
## F# collections
*)
let list = [1;2;3;4]
let array = [|"A";"B";"C";"D"|]
let sequence = seq {1..10}
(**
- very simple **F#** code
- tooltip should work
  - show all `types`
  - and so on
*)
(*** slide-end ***)
"""
let slides = fsx  |> Literate.ParseScriptString


let replacements = Seq.collect collectCodes slides.Paragraphs
let snippets = [| for _, r in replacements -> Snippet("", r) |]       
let formatted = CodeFormat.FormatHtml(snippets, ctx.Prefix, ctx.GenerateLineNumbers, false)    

let tips = formatted.ToolTip

let slides' = replaceLiterateParagraphs ctx slides 
Markdown.WriteHtml(MarkdownDocument(slides'.Paragraphs, slides'.DefinedLinks))

let html = slides |> Literate.WriteHtmlWithoutFormattedTips


fsx 
|> Literate.ParseScriptString
|> Literate.WriteHtmlWithoutFormattedTips



