#I "../packages/FSharp.Formatting.2.4.10/lib/net40/"
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



let md = """
# Title

    let sqr x = x * x

- very simple **F#** code
- tooltip should work
  - show all `types`
  - and so on

"""

let codeMd = """
    let x = 10
    let y = 20
"""

let doc = 
  codeMd 
  |> Literate.ParseMarkdownString
  |> Literate.WriteHtml

let doc = (md |> Literate.ParseMarkdownString)
let paragraphs = (md |> Literate.ParseMarkdownString).Paragraphs



let doc = codeMd |> Literate.ParseMarkdownString
let doc = md |> Literate.ParseMarkdownString
let doc' = replaceLiterateParagraphs ctx doc
printfn "%A" doc.Paragraphs
printfn "%A" doc'.Paragraphs

paragraphs.[1]
paragraphs.[2]

Markdown.WriteHtml(doc.MarkdownDocument)

open System
open System.IO
open System.Web

let sb = new System.Text.StringBuilder()
let wr = new StringWriter(sb)

let mdd = MarkdownDocument(doc.Paragraphs, doc.DefinedLinks)
Markdown.WriteHtml(mdd, wr, Environment.NewLine)
//Markdown.WriteHtml(MarkdownDocument(doc.Paragraphs @ [InlineBlock doc.FormattedTips], doc.DefinedLinks))

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


let doc = 
  codeMd 
  |> Literate.ParseMarkdownString

collectCodes doc.Paragraphs.[0]

let doc = replaceLiterateParagraphs ctx doc
printfn "%A" doc.Paragraphs
Markdown.WriteHtml(MarkdownDocument(doc.Paragraphs @ [InlineBlock doc.FormattedTips], doc.DefinedLinks))


let outputElement (output:TextWriter) tag attributes body =
    let attrString = 
        [ for k, v in attributes -> k + "=\"" + v + "\""]
        |> String.concat " "
    output.Write("<" + tag + attrString + ">")
    body()
    output.Write("</" + tag + ">")


let rec formatSpan (output:TextWriter) span = 
    let out = outputElement output
    let iter spans = (fun () -> spans |> List.iter (formatSpan output))
    match span with
    | Literal(str) ->
        output.Write(str)
    | Strong(spans) ->
        out "strong" [] (spans |> iter)
    | Emphasis(spans) ->
        out "em" [] (spans |> iter)    
    | InlineCode(code) ->
        output.Write("<code>" + code + "</code>")
    | _ -> output.Write("N/A")

let rec formatBlock (output:TextWriter) block = 
    let out = outputElement output
    let iter spans = (fun () -> spans |> List.iter (formatSpan output))
    match block with
    | Heading(size, spans) ->
        out ("h" + size.ToString()) [] (spans |> iter)    
    | Paragraph(spans) ->
        out "p" [] (spans |> iter)    
    | CodeBlock(lines) -> ()
    | EmbedParagraphs (x) -> out "code" [] (fun () -> ())
    | _ -> ()

let sb = System.Text.StringBuilder()
let output = new StringWriter(sb)
paragraphs.[0] |> formatBlock output
output.ToString()

paragraphs.[1] |> formatBlock output

