#r @"..\..\..\FsReveal\bin\FsReveal.dll"

open System.IO
open FsReveal

let outDir = @"g:\output"

let fsxFile = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\src\presentations\FsReveal.fsx")
FsReveal.GenerateOutputFromScriptFile outDir "test-fsx.html" fsxFile

let mdFile = Path.Combine(__SOURCE_DIRECTORY__, @"..\..\..\FsReveal\src\presentations\FsReveal.md")
FsReveal.GenerateOutputFromMarkdownFile outDir "test-md.html" mdFile
