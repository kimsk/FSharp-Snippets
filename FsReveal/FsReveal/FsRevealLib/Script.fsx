//#I "../packages/FSharp.Formatting.2.4.10/lib/net40/"
//#I @"C:\Users\Karlkim\Documents\GitHub\FSharp.Formatting\bin"
#I @"G:\GitHub\FSharp-Snippets\FsReveal\FsReveal"
#load "packages/FsLab.0.0.14-beta/FsLab.fsx"

#I @"G:\GitHub\FSharp.Formatting\bin"
#r "FSharp.Literate.dll"
#r "FSharp.CodeFormat.dll"
#r "FSharp.MetadataFormat.dll"
#r "FSharp.Markdown.dll"
#r "RazorEngine.dll"
#r "FSharp.Compiler.Service.dll"
#r "CSharpFormat.dll"

#load "Formatters.fs"
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

File.ReadAllText(Path.Combine(__SOURCE_DIRECTORY__, "fsreveal.fsx"))
|> FsReveal.ProcessScriptFile @"G:\GitHub\FSharp-Snippets\FsReveal\reveal.js"