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
open FsRevealLib

let fsx ="""
(*** slide-start ***)
(*** slide-start ***)
(**
## F# syntax in 60 seconds

### The two major differences between F# syntax and a standard C-like syntax are:
*)
(*** slide-end ***)
(*** slide-start ***)
(**
- Curly braces are not used to delimit blocks of code. Instead, indentation is used (Python is similar this way).
- Whitespace is used to separate parameters rather than commas.
*)
(*** slide-end ***)
(*** slide-start ***)
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

let slides,tooltips = fsx |> FsReveal.GetHtmlWithoutFormattedTips

let relative subdir = Path.Combine(__SOURCE_DIRECTORY__, subdir)

let template = File.ReadAllText (relative "template.html")

let output = 
  template.Replace("{tooltips}", tooltips).Replace("{slides}", slides)